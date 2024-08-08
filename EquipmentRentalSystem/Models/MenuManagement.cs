using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRentalSystem.Models
{
    public static class MenuManagement
    {
        public static Boolean IsAdmin { get; set; } = true;
        public static Boolean IsShowLogin { get; set; } = true;
        public static string User { get; set; } = "Not Login";
        
        public static string account;
        public static string password;
    }
}
