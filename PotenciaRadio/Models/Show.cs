using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace PotenciaRadio.Models
{
    public class Show:BindableBase
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _description;
        [JsonProperty("description")]
        public string Description {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _image;
        [JsonProperty("image")]
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

     
    }

    
}
