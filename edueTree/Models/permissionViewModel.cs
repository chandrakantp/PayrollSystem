using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edueTree.Models
{
    public class permissionViewModel
    {
        public List<AspNetRole> Rolelist { get; set; }
        public List<MenuItem> MenuItemslist { get; set; }
        public List<MenuItem> PermissionsList { get; set; }
        public List<AllMenulist> Menulist { get; set; }

        public List<Menu> MainManuList { get; set; }

        public List<MenuItem> PermissionsList1 { get; set; }

        public int roleId { get; set; }
    }

    public class AllMenulist
    {
        public int menuItemId { get; set; }
        public string itemName { get; set; }
        public Nullable<int> menuId { get; set; }
        public bool IsActive { get; set; }
    }
}