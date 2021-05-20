using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginForms.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LoginForms.Shared
{
    public class RestHelper
    {
        //private string baseUrl = "http://192.168.1.102:3000/api/";

        private string baseUrl = "http://192.168.1.102:3000/api/";

        //private string baseUrl = "http://192.168.1.158:3000/api/";

        //private string baseUrl = "http://192.168.100.13:3000/api/";

        public async void llamarGet(string chatId, string id, RichTextBox rtx, Form whatsapp, Form telegram, Form fPrincipal)
        {
            try
            {
                var resultadoGet = await GetAllMessage(chatId, id);
                Json json = JsonConvert.DeserializeObject<Json>(resultadoGet);
                rtx.Text = resultadoGet; 
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Error [LlamarGet]: " + ex.Message);
            }
        }
        //GET
        public async Task<string> GetAllMessage(string chatId, string id)
        {
            var inputData = new Dictionary<string, string>
            {
                {"chatId", chatId},
                {"id", id}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger", input);
            HttpContent content = response.Content;

            string data = await content.ReadAsStringAsync();
            //if (data != null)
            //{
            //    return data.ToString();
            //}
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                return data;
            else
                return response.StatusCode.ToString();

            return string.Empty;
        }

        //public async Task<string> GetMessage(string chatId, string id)
        //{
        //    var inputData = new MessageJson
        //    {
        //        chatId = chatId,
        //        id = id
        //    };

        //    List<MessageJson> lst = new List<MessageJson>
        //    {
        //        inputData
        //    };

        //    var input = new FormUrlEncodedContent(inputData);
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger", input);
        //    HttpContent content = response.Content;
        //    string data = await content.ReadAsStringAsync();
        //    if (data != null)
        //    {
        //        return data;
        //    }

        //    return string.Empty;

        //}

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
        public async Task<string> Login(string mail, string password, string ipAddress)
        {
            try
            {
                var inputData = new Dictionary<string, string>
            {
                { "email", mail },
                { "password", password },
                { "ipAddress", ipAddress }
            };

                var input = new FormUrlEncodedContent(inputData);

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.PostAsync(baseUrl + "auth", input))
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error [Login]: " + ex.Message);

            }
            return string.Empty;

        }


        public string BeautifyJson(string strJson)
        {
            JToken parseJson = JToken.Parse(strJson);
            return parseJson.ToString(Formatting.Indented);
        }

        //Metodo para deserializar las respuestas del servidor
        //decirle a Diego que si se puede hacer un metodo para que se encargue de las deserializaciones
        public string ResponseMessage(string strJson)
        {
            Json json = JsonConvert.DeserializeObject<Json>(strJson);
            return json.ToString();
        }

        public User GetUser(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
            User user = JsonConvert.DeserializeObject<User>(json);
            return user; 
        }


        public async Task<string> SendMessage(string text, string chatId, string clientPlatformIdentifier, string platformIdentifier)
        {
            var inputData = new Dictionary<string, string>
            {
                //whatsapp:+5214621929111 , w
                { "messagePlatformId", ""},
                { "text", text},
                { "transmitter",  "a"},
                { "statusId", "1"},
                { "chatId", chatId},
                { "clientPlatformIdentifier", clientPlatformIdentifier},
                { "platformIdentifier", platformIdentifier},
                { "agentPlatformIdentifier", "192.168.1.153" }
            };

            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger/outMessage", input);
            HttpContent content = response.Content;
            //string data = await content.ReadAsStringAsync();
            string data = response.StatusCode.ToString();
            if (data != null)
            {
                return data.ToString();
            } 

            return string.Empty;
            //cuando se contruya form}
            //se identifique de cual chat viene el mensaje
            //tambien el chatPlatformIdentifer
            //sacarlo del json
        }

        public void createChatForm(string chatId)
        {
            int usedHeight = 0;
            Form formChat = new Form();

            formChat.Location = new System.Drawing.Point(18, 17);
            formChat.Text = "Chat: " + chatId;

            formChat.Width = 400;
            formChat.Height = 400;
            formChat.SuspendLayout();


            Label label = new Label();
            label.Text = "Etiqueta creada dinámicamente";
            label.Top = usedHeight + 7;
            label.Left = 5;
            formChat.Controls.Add(label);

            formChat.ResumeLayout();

            //formChat.MdiParent = FormPrincipal.ActiveForm;
            formChat.Show();

            formChat.Activate();
        }

        public void addLabel(Form fPrincipal)
        {
            int usedHeight = 0;
            try
            {
                Form myForm = new Form();

                Label nuevaLabel = new Label();
                nuevaLabel.Text = "Etiqueta dinámica";
                nuevaLabel.Width = 100;
                nuevaLabel.Height = 100;

                fPrincipal.Controls.Add(nuevaLabel);
                //myForm.MdiParent = fPrincipal;
                //myForm.Show();   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en addLabel: " + ex.Message);
            }
        }


        public string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }

                    return address.Address.ToString();
                }
            }

            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";

        }
    }
    public class MsjApi
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

}

