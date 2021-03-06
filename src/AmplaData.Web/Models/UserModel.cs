﻿using System;

namespace AmplaData.Web.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Session { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastActivity { get; set; }
        public string LoginType { get; set; }
    }
}