using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Business.Interface
{
    interface IUserBusiness
    {
        User getUser(string username);
    }
}
