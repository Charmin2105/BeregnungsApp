using Beregnungs.App.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Beregnungsdaten, Title="Beregnungsdaten" },
                new HomeMenuItem {Id = MenuItemType.Schlage, Title="Schläge" },
                new HomeMenuItem {Id = MenuItemType.Betrieb, Title="Betrieb" },
                new HomeMenuItem {Id = MenuItemType.Mitarbeiter, Title="Mitarbeiter" },
                new HomeMenuItem {Id = MenuItemType.Wasserverbrauch, Title="Wasserverbrauch" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}