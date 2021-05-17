using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVFoundation;
using Foundation;
using PotenciaRadio.Dependencies;
using UIKit;
using Xamarin.Essentials;

namespace PotenciaRadio.iOS.Dependencies
{
    class IosStreamingService : IStreaming
    {
        AVPlayer player;
        bool isPrepared;


        public void Play()
        {
            try
            {
                var dataSource = Preferences.Get("streamUri", string.Empty);

                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                if (!isPrepared || player == null)

                    player = AVPlayer.FromUrl(NSUrl.FromString(dataSource));

                player.AutomaticallyWaitsToMinimizeStalling = false;
                isPrepared = true;

                player.Play();

            }
            catch (Exception a)
            {
                System.Diagnostics.Debug.WriteLine("ERROR de reproduccion" + a);
            }

        }

        public void Pause()
        {
            player.Pause();
        }

        public void Stop()
        {
            if (player != null)
            {
                player.Dispose();
                isPrepared = false;
            }
        }

        public double SetVolume(double value)
        {
            return player.Volume = (float)value;
        }
    }
}
