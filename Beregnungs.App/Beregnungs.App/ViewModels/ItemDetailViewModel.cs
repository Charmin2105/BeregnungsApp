using System;

using Beregnungs.App.Models;

namespace Beregnungs.App.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = "Item";
            Item = item;
        }
    }
}
