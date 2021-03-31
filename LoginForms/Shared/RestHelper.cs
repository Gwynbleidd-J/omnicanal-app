using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginForms.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace LoginForms.Shared
{
    public class RestHelper
    {
        private string baseUrl = ConfigurationManager.AppSettings["ipServidor"];

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

        //POST
        public async Task<string> RegistrerUser(string mail, string password)
        {
            var inputData = new Dictionary<string, string>
            {
                {"email", mail},
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

        //POST
        public async Task<string> Login(string email, string password)
        {
            var inputData = new Dictionary<string, string>
            {
                { "email", email},
                {"password", password}
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient cliente = new HttpClient())
            {
                using (HttpResponseMessage response = await cliente.PostAsync(baseUrl + "auth", input))
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

        public async Task<string> SaverDatos(string idUser, string strDatos, string strToken)
        {
            var inputData = new Dictionary<string, string>
            {
                {"idUser", idUser},
                {"data", strDatos}
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient cliente = new HttpClient())
            {
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strToken);
                using (HttpResponseMessage respuesta = await cliente.PostAsync(baseUrl + "saver", input))
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

        //GET
        public async Task<string> GetSaverDatos(string idUser, string strToken)
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

        public string ResponseMessage(string strJson)
        {

            MsjApi msj = JsonConvert.DeserializeObject<MsjApi>(strJson);

            string message = msj.message;

            return message;
        }

        public Usuario GetUser(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
            Usuario user = JsonConvert.DeserializeObject<Usuario>(json);
            return user;
        }

        public List<Text> GetTextos(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
            List<Text> lst = JsonConvert.DeserializeObject<List<Text>>(json);
            return lst;
        }
    }
    public class MsjApi
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class Text
    {
        public int idSaver { get; set; }
        public string data { get; set; }
        public DateTime lastUpdate { get; set; }
    }
}

