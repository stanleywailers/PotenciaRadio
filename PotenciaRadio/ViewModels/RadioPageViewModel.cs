using PotenciaRadio.Dependencies;
using PotenciaRadio.Events;
using PotenciaRadio.Models;
using PotenciaRadio.Services;
using Prism.AppModel;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PotenciaRadio.ViewModels
{
    public class RadioPageViewModel : ViewModelBase
    {
        const string isRunningKey = "runningKey";
        private IStreaming _streaming;
        private IPageDialogService _dialogService;
        private IAppService<Settings> _radioService;
        private IAppService<RootShow> _showService;
        public DelegateCommand PlayCommand { get; set; }
        public DelegateCommand StopCommand { get; set; }
        private IEventAggregator _eventAggregator;

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                SetProperty(ref _isRunning, value);
                Preferences.Set(isRunningKey, value);
            }
        }

        private RootShow _currentShow;
        public RootShow CurrentShow
        {
            get { return _currentShow; }
            set
            {
                SetProperty(ref _currentShow, value);

            }
        }
        private string _adUnitId;
        public string AdUnitId
        {
            get { return _adUnitId; }
            set { SetProperty(ref _adUnitId, value); }
        }

        public RadioPageViewModel(INavigationService navigationService, IStreaming streaming,
            IPageDialogService dialogService, IAppService<Settings> radioService, IAppService<RootShow> showService, IEventAggregator eventAggregator)
              : base(navigationService)
        {
            if (Device.RuntimePlatform == Device.iOS)
                AdUnitId = "";
            else if (Device.RuntimePlatform == Device.Android)
                AdUnitId = "ca-app-pub-8582719280960685/3307949149";

            _dialogService = dialogService;
            _radioService = radioService;
            _showService = showService;
            _eventAggregator = eventAggregator;

            _streaming = streaming;
            PlayCommand = new DelegateCommand(Play);
            StopCommand = new DelegateCommand(Stop);
            _eventAggregator.GetEvent<StopEvent>().Subscribe(Stop);
        }



        private async void Play()
        {
            _streaming.Play();
            IsRunning = true;
        }

        private async void Stop()
        {
            IsRunning = false;
            _streaming.Stop();
        }



        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            CurrentShow = await _showService?.Read();
            if (CurrentShow == null)
            {
                CurrentShow = new RootShow
                {
                    Show = new Show
                    {
                        Image = "logo_cuadrado.png",
                        Description = "Potencia Radio"
                    }
                };
            }

            Preferences.Set("streamUri", "http://max.miradio.in:8300/stream?type=.mp3");

            Play();

            IsRunning = Preferences.Get(isRunningKey, false);


        }

    }
}

