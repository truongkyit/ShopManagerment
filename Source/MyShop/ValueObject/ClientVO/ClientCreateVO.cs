using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ValueObject.ClientVO
{
    public class ClientCreateVO : INotifyPropertyChanged
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ClientCreateVO()
        {
            PhoneNumber = "";
            Name = "";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
