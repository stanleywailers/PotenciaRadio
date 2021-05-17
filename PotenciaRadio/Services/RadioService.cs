using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PotenciaRadio.Dependencies;
using PotenciaRadio.Models;
using Xamarin.Essentials;

namespace PotenciaRadio.Services
{

    public class RadioService : IAppService<Settings>
    {
        private static HttpClient _client;

        public RadioService()
        {
            _client = new HttpClient();

        }

        public Task<Settings> Read()
        {
            return null;
        }

        public async Task<IEnumerable<Settings>> ReadAll()
        {
            var apiUrl = Prism.PrismApplicationBase.Current.Resources["api_url"].ToString() + "/settings";

            try
            {
                var response = await _client.GetAsync(apiUrl);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ObservableCollection<Settings>>(stringResponse);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception a)
            {
                System.Diagnostics.Debug.WriteLine("error en servicio " + a);
                return null;
            }

        }
    }
}
