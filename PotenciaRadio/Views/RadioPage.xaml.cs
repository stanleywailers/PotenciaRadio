using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PotenciaRadio.ViewModels;
using Xamarin.Forms;


namespace PotenciaRadio.Views
{
    public partial class RadioPage : ContentPage
    {
        public RadioPage()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            (BindingContext as RadioPageViewModel).SetVolume(e.NewValue);
        }


    }
}