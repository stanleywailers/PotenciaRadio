using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace PotenciaRadio.Models
{
    public class Settings : BindableBase
    {
        private string _streamUri;
        [JsonProperty("value")]
        public string StreamUri
        {
            get { return _streamUri; }
            set { SetProperty(ref _streamUri, value); }
        }
    }
}
