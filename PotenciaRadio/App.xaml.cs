using Prism;
using Prism.Ioc;
using PotenciaRadio.ViewModels;
using PotenciaRadio.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PotenciaRadio.Dependencies;
using PotenciaRadio.Models;
using PotenciaRadio.Services;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PotenciaRadio
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer)
        {

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }
        
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {

                MainPage.DisplayAlert("Potencia Radio", "No tienes acceso a internet", "OK");
            }
            else if (current == NetworkAccess.ConstrainedInternet)
            {
                MainPage.DisplayAlert("Potencia Radio", "Tu acceso a internet es debil", "OK");
            }
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("app:///MainPage/NavigationPage/HomePage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<RadioPage, RadioPageViewModel>();
            containerRegistry.RegisterForNavigation<DatePage, DatePageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.Register<IAppService<RootShow>, ShowService>();
            containerRegistry.Register<IAppService<Settings>, RadioService>();
            
        }



    }
}
