using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;

using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PotenciaRadio.Dependencies;
using Xamarin.Essentials;
using Android.Media;
using Android.Support.V4.App;


namespace PotenciaRadio.Droid.Dependencies
{
    // [IntentFilter(new [] { "com.devstroyers.Potencia.Radio" })]
    [Service]
    public class DroidStreamingService : Service, IStreaming, Android.Media.AudioManager.IOnAudioFocusChangeListener
    {
        const int SERVICE_RUNNING_NOTIFICATION_ID = 123;
        const string NOTIFICATION_CHANNEL_ID = "com.devstroyers.Potencia.Radio";
        public static Android.Media.AudioManager am = (Android.Media.AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);


        public static MediaPlayer player;
        string dataSource = "http://max.miradio.in:8300/stream?type=.mp3";

        bool IsPrepared = false;

        public void Play()
        {
            if (!IsPrepared)
            {
                if (player == null)
                    player = new MediaPlayer();

                else
                    player.Reset();

                player.SetDataSource(dataSource);
                player.PrepareAsync();
            }

            player.Error += Player_Error;
            player.Prepared += (sender, args) =>
            {
                player.Start();
                IsPrepared = true;
            };


        }

        private void Player_Error(object sender, MediaPlayer.ErrorEventArgs e)
        {

        }

        public void Pause()
        {
            player.Pause();
        }

        public void Stop()
        {

            try
            {
                player.Stop();
                IsPrepared = false;
            }
            catch (Exception a)
            {
                System.Diagnostics.Debug.WriteLine(a);
            }

        }

        public override void OnCreate()
        {
            base.OnCreate();
            am.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
        }


        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            // Code not directly related to publishing the notification has been omitted for clarity.
            // Normally, this method would hold the code to be run when the service is started.

            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                var notification = new Notification.Builder(this)
               .SetContentTitle(AppInfo.Name.ToString())
               .SetContentText(Prism.PrismApplicationBase.Current.Resources["notification_text"].ToString())
               .SetSmallIcon(Resource.Drawable.ic_launcher)

               .Build();
                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }
            else
            {
                var notification = new Notification.Builder(this, NOTIFICATION_CHANNEL_ID)
              .SetContentTitle(AppInfo.Name.ToString())
              .SetContentText(Prism.PrismApplicationBase.Current.Resources["notification_text"].ToString())
              .SetSmallIcon(Resource.Drawable.ic_launcher)
              .Build();

                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }

            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
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
            var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "Potencia-Radio", NotificationImportance.Default);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);


        }



        public override void OnTaskRemoved(Intent rootIntent)
        {
            Stop();

            base.OnTaskRemoved(rootIntent);
        }

        public override void OnDestroy()
        {


            am.AbandonAudioFocus(this);
            base.OnDestroy();
        }

        void AudioManager.IOnAudioFocusChangeListener.OnAudioFocusChange(AudioFocus focusChange)
        {
            switch (focusChange)
            {

                case Android.Media.AudioFocus.Gain:
                case Android.Media.AudioFocus.GainTransient:
                    Play();
                    break;
                case Android.Media.AudioFocus.LossTransient:
                case Android.Media.AudioFocus.Loss:
                    Stop();
                    break;
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


    }
}