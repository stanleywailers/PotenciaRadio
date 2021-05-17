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
using Org.Videolan.Libvlc;
using PotenciaRadio.Dependencies;
using Xamarin.Essentials;
using Android.Media;
using MediaPlayer = Org.Videolan.Libvlc.MediaPlayer;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;


namespace PotenciaRadio.Droid.Dependencies
{
    // [IntentFilter(new [] { "com.devstroyers.Potencia.Radio" })]
    [Service]
    public class DroidStreamingService : Service, IStreaming, Android.Media.AudioManager.IOnAudioFocusChangeListener
    {
        const int SERVICE_RUNNING_NOTIFICATION_ID = 123;
        const string NOTIFICATION_CHANNEL_ID = "com.devstroyers.Potencia.Radio";
    

        public static LibVLCLibVLC mLibVLC = null;
        public static MediaPlayer mMediaPlayer = null;
        public static MediaLibVLC m;
        public static Android.Media.AudioManager am = (Android.Media.AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);

        
        public async void Play()
        {

            if (mLibVLC == null)
            {
                mLibVLC = new LibVLCLibVLC(Android.App.Application.Context);
                mMediaPlayer = new MediaPlayer(mLibVLC);
            }
            var streamUri = Preferences.Get("streamUri", string.Empty);
            m = new MediaLibVLC(mLibVLC, Android.Net.Uri.Parse(streamUri));

            // Tell the media player to play the new Media.
            mMediaPlayer.Media = m;

            if (mMediaPlayer.IsPlaying == false)
            {
                // Finally, play it!
               mMediaPlayer.Play();
            }
        }

        public void Pause()
        {
            mMediaPlayer.Pause();
            StopForeground(true);

        }

        public void Stop()
        {
            try
            {
                // Tell the media player to play the new Media.
                if (mMediaPlayer != null)
                {
                    mMediaPlayer.Stop();
                    
                }

            }
            catch (Exception a)
            {
                System.Diagnostics.Debug.WriteLine("NO SE DETIENE" + a);
            }
        }

        public double SetVolume(double value)
        {
            var a = am.GetStreamVolume(Android.Media.Stream.Music);
            mMediaPlayer.SetVolume((int)value);

            return mMediaPlayer.Volume;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CreateNotificationChannel();
            am.RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Stop();
            am.AbandonAudioFocus(this);
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
                var notification = new Notification.Builder(this,NOTIFICATION_CHANNEL_ID)
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

     
    }
}