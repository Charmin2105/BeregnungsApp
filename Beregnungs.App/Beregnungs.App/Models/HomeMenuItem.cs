using System;
using System.Collections.Generic;
using System.Text;

namespace Beregnungs.App.Models
{
    public enum MenuItemType
    {
        Beregnungsdaten,
        Schlage,
        Betrieb,
        Mitarbeiter,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
