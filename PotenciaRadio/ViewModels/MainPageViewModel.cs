using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PotenciaRadio.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace PotenciaRadio.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;

        private ObservableCollection<MenuItems> _items;
        public ObservableCollection<MenuItems> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public DelegateCommand<MenuItems> NavigateCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            NavigateCommand = new DelegateCommand<MenuItems>(ItemTapped);
            Items = new ObservableCollection<MenuItems>()
            {
                new MenuItems
                {
                     Image="home.png",
                    Title="Inicio",
                    Uri="NavigationPage/HomePage"
                }
               // },
               //new MenuItems
               //{
               //     Image="premium.png",
               //     Title="Premium",
               //     Uri="NavigationPage/SuscriptionPage"

               //}

            };
        }



        private async void ItemTapped(MenuItems Item)
        {
            if (Item.Title != "Inicio")
            {
                await _dialogService.DisplayAlertAsync("Potencia Radio", "Pronto podras suscribirte", "ok");
                return;
            }
            await NavigationService.NavigateAsync(Item.Uri);

        }


    }
}