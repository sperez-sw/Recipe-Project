using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class LoginModel
    {
        public LoginModel()
        {
            
        }
        public string email { get; set; }
        public string password { get; set; }
        public bool returnSecureToken { get; set; }
    }
