using MyShop.Business.Interface;
using MyShop.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Business
{
    class UserBusiness : IUserBusiness
    {
        private UserDAO _userDAO;

        public UserBusiness()
        {
            _userDAO = new UserDAO();
        }
        public User getUser(string username)
        {
            return _userDAO.getByUserName(username);
        }
    }
}
