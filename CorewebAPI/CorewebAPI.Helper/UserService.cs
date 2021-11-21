using CorewebAPI.Entities.Authorization;
using CorewebAPI.Entities.Model;
using Microsoft.Extensions.Options;
using System;
using System.DirectoryServices;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;
namespace CorewebAPI.Helper
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(LoginModel model, string ipAddress);

    }
    public class UserService : IUserService
    {
        private AppDBContext _context;
        private IJwtUtils _jwtUtils;
        private IEmail _emails;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, AppDBContext context, IJwtUtils jwtUtils, IEmail emails)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _emails = emails;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(LoginModel  requestModel, string ipAddress)
        {
            if (requestModel.Email.Length < 5)
                throw new AppException("Email is not valid");

            var user = _context.Users.SingleOrDefault(x => x.Email == requestModel.Email && x.IsValide == true);

            // validate
            if (user == null)
                throw new AppException("Email is incorrect");

            if (user.UsrType == "SALES")
            {
                if (!BCryptNet.Verify(requestModel.Password, user.PasswordHash))
                    throw new AppException("Password is incorrect");
            }
            else
            {
                bool Vlaidate = LogonValid("10.240.10.1:389/OU=Softlogic Group,DC=softlogic,DC=lk", requestModel.Email.Trim().Replace("@softlogiclife.lk", ""), requestModel.Password.Trim());

                if (Vlaidate)
                {
                    Vlaidate = LogonValid("softlogic.lk:389/OU=Softlogic Group,DC=softlogic,DC=lk", requestModel.Email.Trim().Replace("@softlogiclife.lk", ""), requestModel.Password.Trim());
                }
                if (Vlaidate)
                    throw new AppException("Password is incorrect");
            }

            // authentication successful so generate jwt and refresh tokens
            var token = _jwtUtils.GenerateToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            // save changes to db
            _context.Update(user);
            _context.SaveChanges();

            return new AuthenticateResponse(user, token, refreshToken.Token);
        }

        private bool LogonValid(string ldapDomain, string userName, string password)
        {
            DirectoryEntry de = new DirectoryEntry(@"LDAP://" + ldapDomain, userName, password);
            try
            {
                object o = de.NativeObject;
                DirectorySearcher search = new DirectorySearcher(de);
                search.PropertiesToLoad.Add("description");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("employeeNumber");
                search.Filter = "(SAMAccountName=" + userName + ")";
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                string attribute = "NUll";
                try
                {
                    attribute = (string)result.Properties["employeeNumber"][0];
                    attribute = attribute.Substring(2);
                }
                catch (Exception)
                {
                    attribute = (string)result.Properties["description"][0];
                }

                if (attribute == "NULL")
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void removeOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }
    }
}
