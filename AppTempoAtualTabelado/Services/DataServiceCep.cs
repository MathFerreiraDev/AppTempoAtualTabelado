using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTempoAtualTabelado.Model;
using Newtonsoft.Json;

namespace AppTempoAtualTabelado.Services
{
    class DataServiceCep
    {
        public static async Task<DadosEndereco?> GetEnderecoByCep(string cep)
        {
            DadosEndereco? end;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(
                    "http://localhost:8000/endereco/by-cep?cep=" + cep);

                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    end = JsonConvert.DeserializeObject<DadosEndereco?>(json);
                }
                else
                {
                    throw new Exception(response.RequestMessage.Content.ToString());
                }



            }

            return end;

        }
    }
}
