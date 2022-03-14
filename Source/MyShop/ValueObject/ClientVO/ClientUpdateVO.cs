using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.ClientVO
{
    public class ClientUpdateVO : INotifyPropertyChanged
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ClientUpdateVO(ClientCreateVO client)
        {
            PhoneNumber = client.PhoneNumber;
            Name = client.Name;
            UpdatedAt = DateTime.Now;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
