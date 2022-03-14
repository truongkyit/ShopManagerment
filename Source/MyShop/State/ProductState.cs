using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.State
{
    public static class ProductState
    {
        public static BindingList<Product> productsState = new BindingList<Product>();
        public static int categoryId = -1;
        public static string name = "";
        public static Pagination pagination = new Pagination();

    }

    public class Pagination:INotifyPropertyChanged
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int skip { get; set; }
        public bool next { get; set; }
        public bool previus { get; set; }

        public Pagination() {
            this.skip = 0;
            this.limit = 6;
            this.next = false;
            this.previus = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
