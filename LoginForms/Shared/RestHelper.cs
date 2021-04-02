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
        private string baseUrl = "http://192.168.0.9:3000/api/";

        public async Task<string> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(baseUrl))
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
        public async Task<string> RegistrerUser(string userName, string mail, string password)
        {
            var inputData = new Dictionary<string, string>
            {
                {"username", userName },
                {"email", mail},
                {"password", password}
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(baseUrl, input))
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
        public async Task<string> Login(string mail, string password)
        {
            User user = new User();
            var inputData = new Dictionary<string, string>
            {
                {user.email, mail},
                {user.password, password}
            };

            var input = new FormUrlEncodedContent(inputData);

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(baseUrl+ "auth", input))
                {
                    using (HttpContent content = response.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        Console.WriteLine(data);
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

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strToken);
                using (HttpResponseMessage respuesta = await client.PostAsync(baseUrl + "saver", input))
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

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(strToken);
                using (HttpResponseMessage respuesta = await client.GetAsync(baseUrl+ idUser))
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

        public User GetUser(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
            User user = JsonConvert.DeserializeObject<User>(json);
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

