using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class CheckMemberInOutModel
    {
        public List<User> Users { get; set; }
        public User ToBeChecked { get; set; }
    }
}
