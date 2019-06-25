using System;

using Beregnungs.App.Models;

namespace Beregnungs.App.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public BeregnungsDaten BeregnungsDaten { get; set; }
        public ItemDetailViewModel(BeregnungsDaten beregnungsDaten = null)
        {
            Title = "Beregnungsdaten";
            BeregnungsDaten = beregnungsDaten;
        }
    }
}
