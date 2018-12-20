using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.DAL
{
    public interface IVisitDAL
    {
        bool CheckIn(int memberId);
        bool HasOpenVisit(int memberId);
        bool CheckOut(int memberId);
        void DeleteVisit(Visit trip);
        Visit GetVisit(int id);
        List<Visit> GetVisits(int memberId);
    }
}
