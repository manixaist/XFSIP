using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFSIP.Models
{
    /// <summary>
    /// Model for the signin page.  Wraps a 'user:password' pair
    /// </summary>
    class UserSigninInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
