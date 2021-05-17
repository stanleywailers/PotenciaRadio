using System;

using Xamarin.Forms;

namespace PotenciaRadio.Views.views
{
    public class AdMobView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
                  nameof(AdUnitId),
                  typeof(string),
                  typeof(AdMobView),
                  string.Empty);


       public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }
    }
}

