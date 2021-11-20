﻿namespace CorewebAPI.Entities.Model
{
    public class AppSettings
    {
        public string Secret { get; set; }
        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }
        public string FtpUrl { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string SmtpHost { get; set; }
        public int PortNumber { get; set; }
        public string EmailSender { get; set; }
        public string EmailPassword { get; set; }
        public string reportServer { get; set; }
        public string reportUserName { get; set; }
        public string reportPassword { get; set; }
        public bool IsSSL { get; set; }

    }
}
