using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Common
{
    enum ActionPagination
    {
        Previus,
        Next,
        Load,
        Create,
        Delete,
        Update,
        Import,
        FindProduct,
        TabSwitch
    }

    enum ActionProduct
    {
        GetAll,
        Reload,
        FilterByCategory,
        FilterByCategoryAndName,
        FilterByName,
        Create,
        Update,
        Delete
    }

    enum ActionCategory
    {
        Update,
        Delete
    }

    enum ActionOrder
    {
        Create,
        Update,
        Delete,
        Reload
    }

    enum RoleId
    {
       SupperAdmin = 1,
       Manager = 2,
       Client = 3
    }

    enum OrderStatus
    {
        New = 1,
        Completed = 2,
        Cancelled = 3,
        Shipping = 4
    }

    enum Option
    {
        TopQuantity = 1,
        BelowQuantity = 2
    }
}
