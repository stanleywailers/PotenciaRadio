using System;
using System.Collections.Generic;
using System.Text;

namespace PotenciaRadio.Dependencies
{
    public interface IStreaming
    {
        void Play();
        void Pause();
        void Stop();
    }
}
