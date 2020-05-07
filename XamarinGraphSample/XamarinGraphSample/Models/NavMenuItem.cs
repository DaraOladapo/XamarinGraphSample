using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinGraphSample.Models
{
    public enum MenuItemType
    {
        Welcome,
        Calendar
    }

    public class NavMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
