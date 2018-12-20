using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.DAL
{
    public interface IEquipmentDAL
    {
        Equipment GetEquipment(int id);
        List<Equipment> GetAllEquipment();
        void CreateEquipment(Equipment machine);
        void UpdateEquipment(Equipment machine);

        void DeleteEquipment(Equipment machine);
    }
}
