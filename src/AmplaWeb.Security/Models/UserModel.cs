using System;

namespace AmplaWeb.Security.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Session { get; set; }
        public DateTime Started { get; set; }
        public DateTime Expires { get; set; }
    }
}