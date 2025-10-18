using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Models.IDentityModel
{
    public class Application_User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }

        public string? RoleName = "User";
    }
}
