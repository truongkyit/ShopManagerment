using MyShop.Business.Interface;
using MyShop.DAO;
using MyShop.ValueObject.ClientVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Business
{
    class ClientBusiness : IClientBusiness
    {
        private ClientDAO _clientDAO;

        public ClientBusiness()
        {
            _clientDAO = new ClientDAO();
        }

        public Client getByPhoneNumber(string PhoneNumber)
        {
            return _clientDAO.findByPhoneNumber(PhoneNumber);
        }

        public Client insertData(ClientCreateVO data)
        {
            var client = new Client()
            {
                PhoneNumber = data.PhoneNumber,
                Name = data.Name,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt
            };

            return _clientDAO.insert(client);
        }
    }
}
