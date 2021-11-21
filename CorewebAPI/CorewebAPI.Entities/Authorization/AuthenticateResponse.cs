using CorewebAPI.Entities.Model;
using System.Text.Json.Serialization;

namespace CorewebAPI.Entities.Authorization
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UsrType { get; set; }
        public int? EPF { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }


        public AuthenticateResponse(User user, string token, string refreshToken)
        {
            UserId = user.UserID;
            FirstName = user.Fname;
            LastName = user.Lname;
            UsrType = user.UsrType;
            EPF = user.EPF;
            Email = user.Email;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
