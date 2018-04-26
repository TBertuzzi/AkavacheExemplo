using AkavacheExemplo.Helpers;
using AkavacheExemplo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AkavacheExemplo.Services
{
    public class NintendoService
    {
        HttpClient httpClient;

        public NintendoService()
        {
            httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<IEnumerable<Game>> GetGamesAsync(int index)
        {
            var response = await httpClient.GetAsync($"{Constantes.NintendoURL}&offset={index}").ConfigureAwait(false);


            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {

                    return JsonConvert.DeserializeObject<Nintendo>(
                        await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false)).Games.Game;

                }
            }

            return new List<Game>();
        }

    }
}
