using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PotenciaRadio.Dependencies;
using PotenciaRadio.Models;

namespace PotenciaRadio.Services
{
    public class ShowService : IAppService<RootShow>
    {
        private static HttpClient _client;

        public ShowService()
        {
            _client = new HttpClient();
        }

        public async Task<RootShow> Read()
        {
            var apiUrl = Prism.PrismApplicationBase.Current.Resources["api_url"].ToString() + "/show";

            try
            {
                var response = await _client.GetAsync(apiUrl);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootShow>(stringResponse);
                    return model;
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

       

        public async Task<IEnumerable<RootShow>> ReadAll()
        {
            return null;
        }
    }
}
