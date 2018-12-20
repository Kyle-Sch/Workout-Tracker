using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Message
    {
       
        public string MyMessage { get; set; }
        public Message()
        {
            MyMessage = "";
        }
        public Message(string message)
        {
            MyMessage = message;
        }
    }
}
