using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class MemberToBeCheckInOut
    {
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public bool IsCheckedIn { get; set; }
    }
}
