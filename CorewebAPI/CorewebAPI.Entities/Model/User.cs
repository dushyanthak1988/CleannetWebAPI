using CorewebAPI.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CorewebAPI.Entities.Model
{
    [Table("UserInfo", Schema = "GLS")]
    public class User
    {
        public int UserID { get; set; }

        public string Fname { get; set; }

        public string Lname { get; set; }

        public string UsrType { get; set; }

        public int? EPF { get; set; }

        public string Email { get; set; }

        public int? MobileNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsValide { get; set; }

        public string InvalidateUser { get; set; }

        public DateTime? InvalidateDate { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }


        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }

    }
}
