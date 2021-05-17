using System;
using Prism.Mvvm;

namespace PotenciaRadio.Models
{
    public class MenuItems : BindableBase
    {

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private string _uri;
        public string Uri
        {
            get { return _uri; }
            set { SetProperty(ref _uri, value); }
        }
    }
}
