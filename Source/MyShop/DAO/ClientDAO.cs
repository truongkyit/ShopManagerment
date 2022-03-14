using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    public class ClientDAO
    {
        private MyShopEntities conn;

        public ClientDAO()
        {
            conn = dbConnection.getInstance();
        }

        public Client insert(Client client)
        {
            var query = conn.Clients.Add(client);
            conn.SaveChanges();
            return query;
        }

        public Client findByPhoneNumber(string PhoneNumber)
        {
            var query = conn.Clients.Where(item => item.PhoneNumber.Equals(PhoneNumber)).FirstOrDefault();
            return query;
        }

    }
}
