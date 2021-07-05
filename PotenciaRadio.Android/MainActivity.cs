using Android.App;
using Android.Content.PM;
using Android.OS;
using PotenciaRadio.Dependencies;
using PotenciaRadio.Droid.Dependencies;
using Prism;
using Prism.Ioc;
using Xamarin.Forms.Platform.Android;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;
using Android.Runtime;
using Android.Content;
using System.Threading.Tasks;
using Prism.Events;
using Unity;
using Prism.Unity;
using PotenciaRadio.Events;

namespace PotenciaRadio.Droid
{
    [Activity(Label = "PotenciaRadio", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        // MyFirebaseIIDService myFirebaseIIDService = new MyFirebaseIIDService();
        Intent serviceStreamPlayer;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            serviceStreamPlayer = new Intent(this, typeof(DroidStreamingService));


            base.OnCreate(bundle);


            Xamarin.Essentials.Platform.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            IsPlayServicesAvailable();
            // Android.Gms.Ads.MobileAds.Initialize(this);

            CreateNotificationChannel();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App(new AndroidInitializer()));
            LoadService().ConfigureAwait(false);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    System.Diagnostics.Debug.WriteLine("ERROR FROM RESULT CODE " + GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    System.Diagnostics.Debug.WriteLine("This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Google Play Services is available.");
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async Task LoadService()
        {

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                StartForegroundService(serviceStreamPlayer);
            }
            else
            {
                StartService(serviceStreamPlayer);
            }
        }

        protected override void OnDestroy()
        {
           StopService(serviceStreamPlayer);
           base.OnDestroy();

        }
    }




    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<IStreaming, DroidStreamingService>();
        }
    }
}

