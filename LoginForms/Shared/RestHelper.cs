using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginForms.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LoginForms.Shared
{
    public class RestHelper
    {
        private string baseUrl = "http://201.149.90.195:3001/";

        public async Task<string> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(baseUrl + "user"))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            return data;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> RegisterUser(string username, string mail, string password)
        {
            var inputData = new Dictionary<string, string>
            {
                { "userName", username},
                { "email", mail},
                {"password", password}
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(baseUrl + "user", input))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            string json = BeautifyJson(data);
                            return json;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> Login(string username, string password )
        {
            var inputData = new Dictionary<string, string>
            {
                {"user", username },
                {"password", password }
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(baseUrl + "auth", input))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            string json = BeautifyJson(data);
                            return json;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> Data(string idUser, string strDatos, string strToken)
        {
            var inputData = new Dictionary<string, string>
            {
                {"idUser", idUser },
                {"data", strDatos }
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strToken);
                using (HttpResponseMessage response = await client.PostAsync(baseUrl + "datos", input))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if(data != null)
                        {
                            string json = BeautifyJson(data);
                            return json;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> GetData(string idUser, string strToken)
        {
            var inputData = new Dictionary<string, string>
            {
                {"idUser", idUser}
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient cliente = new HttpClient())
            {
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strToken);
                using (HttpResponseMessage respuesta = await cliente.GetAsync(baseUrl + "saver/" + idUser))
                {
                    using (HttpContent contenido = respuesta.Content)
                    {
                        string data = await contenido.ReadAsStringAsync();
                        if (data != null)
                        {
                            string json = BeautifyJson(data);
                            return json;
                        }
                    }

                }
            }
            return string.Empty;
        }

        public string BeautifyJson(string strJson)
        {
            JToken parseJson = JToken.Parse(strJson);
            return parseJson.ToString(Formatting.Indented);
        }

        public string MessageResponse(string strJson)
        {
            MsjApi msj = JsonConvert.DeserializeObject<MsjApi>(strJson);
            string mensaje = msj.message;
            return mensaje;
        }

        public Usuario GetUsuario(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
            Usuario user = JsonConvert.DeserializeObject<Usuario>(json);
            return user;
        }

        public List<Texto> GetTextos(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
            List<Texto> lst = JsonConvert.DeserializeObject<List<Texto>>(json);
            return lst;
        }
    }
    public class MsjApi
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
    public class Texto
    {
        public int idSaver { get; set; }
        public string data { get; set; }
        public DateTime lastUpdate { get; set; }
    }


}

