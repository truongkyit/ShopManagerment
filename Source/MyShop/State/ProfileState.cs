using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.State
{
    public static class ProfileState
    {
        public static Profile profile = new Profile();
    }

    public class Profile: INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int RoleId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
