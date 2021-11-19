
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

        /*
         * Hasta ahorita la clase RestHelper.cs ya es dinamica ya no se le tiene que poner los datos
         * estaticos, pero necesito probar que todos los métodos con los nuevos cambios este funcionando
         * como deberian.
         */

        private static readonly string baseUrl = ConfigurationManager.AppSettings["IpServidor"];
        /*
         * Lo mismo preguntar a Juan Carlos que pasa también con este método
         */
        #region ver que pasa con el método de llamarGet
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
        #endregion
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

            //return string.Empty;
        }

        //public async Task<string> RecoverActiveChats(string agentId, string agentPlatformIdentifier)
        public async Task<string> RecoverActiveChats(string agentId)
        {
            //string activeIp = GlobalSocket.currentUser.activeIp;
            //agentPlatformIdentifier = GlobalSocket.currentUser.activeIp;
            try
            {
                //var agentId = GlobalSocket.currentUser.activeIp;
                var inputData = new Dictionary<string, string>
                {
                    {"userId", agentId}
                    //, { "agentPlatformIdentifier", agentPlatformIdentifier} 
                };
                var input = new FormUrlEncodedContent(inputData);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger/recoverActiveChats", input);
                HttpContent content = response.Content;

                string data = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                    return data;
                else
                    return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[RecoverActiveChats][RestHelper]: " + ex.ToString());
            }
            return string.Empty;
        }

        public async Task<string> RecoverAllActiveChats(string agentId)
        {
            try
            {
                //var inputData = new Dictionary<string, string>
                //{
                //    {"userId", agentId}
                //};
                //var input = new FormUrlEncodedContent(inputData);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(baseUrl + "chat/getAllActiveChats");
                HttpContent content = response.Content;

                string data = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    Console.WriteLine("Se obtuvieron lo siguientes chats:" +data.ToString());
                    return data;
                }
                    
                else
                    return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[RecoverAllActiveChats][RestHelper]: " + ex.ToString());
            }
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

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(baseUrl + "auth", input);
                HttpContent content = response.Content;
                string data = await content.ReadAsStringAsync();
                if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    return data;
                }
                else
                {
                    return response.StatusCode.ToString();
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
        public string DeserializarJson(string strJson)
        {
            Json json = JsonConvert.DeserializeObject<Json>(strJson);
            return json.ToString();
        }

        public User GetUser(string strJson)
        {
            MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(strJson);
            //Json response = JsonConvert.DeserializeObject<Json>(strJson);
            //Json resp = JsonConvert.DeserializeObject<Json>(strJson);
             //data = resp.dataLogin;
            var data = resp.data;
            string json = BeautifyJson(data.ToString());
           //string json = JsonConvert.SerializeObject(data, Formatting.Indented);
           User user = JsonConvert.DeserializeObject<User>(json);
           return user; 
        }

        public async Task<string> getPermissions(string rolId)

        {
            var inputData = new Dictionary<string, string>
            {
                {"rolId", rolId}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "permission", input);
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

            //return string.Empty;
        }

        public async Task<string> getMenu(string id)
        {
            var inputData = new Dictionary<string, string>
            {
                {"id", id}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "menu", input);
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

            //return string.Empty;
        }

        public async Task<string> getMyAgents(string id)
        {
            var inputData = new Dictionary<string, string>
            {
                {"leaderId", id}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "user/myAgents", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            
            if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getSupervisorAgents(string rolId)
        {
            //var inputData = new Dictionary<string, string>
            //{
            //    {"rolID", rolId }
            //};
            //var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "user/supervisorAgents");
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getAgentsDetails(string userId)
        {
            var inputData = new Dictionary<string, string>
            {
                {"userId", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "user/agentInfo", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();

            if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getCloseChat(string chatId)
        {
            var inputData = new Dictionary<string, string>
            {
                { "chatId", chatId }
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/closeChat", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();

            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> getSubstactActiveChat(string userId)
        {
            var inputData = new Dictionary<string, string>
            {
                { "userId", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/subtractActiveChat", input);
            Console.WriteLine(response.StatusCode);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getNetworkCategories()
        {
            //var inputData = new Dictionary<string, string> 
            //{
            //    { "id", id }
            //};
            //var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "network/networks/");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> updateNetworkCategories(string chatId, string networkCategoryId)
        {
            var inputData = new Dictionary<string, string>
            {
                {"chatId", chatId },
                {"networkId", networkCategoryId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/updateNetworkCategory", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();

            if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> AppParameters(string twilioAccountSID, string twilioAuthToken, string whatsappAccount, string botTokenTelegram)
        {
            var inputData = new Dictionary<string, string>
            {
                {"twilioAccountSID", twilioAccountSID },
                { "twilioAuthToken", twilioAuthToken},
                { "whatsappAccount", whatsappAccount},
                {"botTokenTelegram", botTokenTelegram }
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/appParameters", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> SoftphoneParameters(string userId)
        {
            var inputData = new Dictionary<string, string>
            {
                {"userId", userId }
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/getCredentials", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            } 
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getAppParameters()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "parameters/getAppParameters");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getUsers()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "parameters/getUsers");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> updateSoftphoneParameters(string id, string userName, string displayName, string domain, string server, string password, string authName, string port)
        {
            var inputData = new Dictionary<string, string>
            {
                {"id", id },
                { "userName", userName },
                { "password", password },
                { "domain", domain },
                { "displayName", displayName },
                { "authName", authName },
                { "server", server },
                { "port", port }

            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/softphoneParameters", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine(response.StatusCode.ToString());
                return response.StatusCode.ToString();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> updateUserStatus(string statusId, string userId)
        {
            var inputData = new Dictionary<string, string>
            {
                {"status", statusId},
                { "id", userId}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/updateUserStatus", input);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getUserStatus()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "status");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> getLogOut(string userId)
        {
            var inputData = new Dictionary<string, string>
            {
                {"Id", userId}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "auth/logOut", input);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> validateTransferAgent(string userId) {

            var inputData = new Dictionary<string, string>
            {
                { "id", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "user/validateTransferAgent", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK" )
            {
                Console.WriteLine("\nEste es el estado del agente:" +data.ToString());
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> getAllAgents()
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "user/getAllAgents");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine("Se obtuvieron lo siguientes chats:" + data.ToString());
                return data;
            }

            else
                return response.StatusCode.ToString();

        }

        public async Task<string> updateAgentActiveIp(string mail, string ipAddress)
        {
            var inputData = new Dictionary<string, string> 
            {
                {"email", mail},
                {"ipAddress", ipAddress}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "user/agentUpdateActiveIp", input);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> updateAgentMaxActiveChats(string id, string maxActiveChats)
        {
            var inputData = new Dictionary<string, string>
            {
                {"id", id},
                {"maxActiveChats", maxActiveChats}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "user/agentUpdateMaxActiveChats", input);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine("Se actualizo el numero de chats simultaneos");
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> SendMessage(string text, string chatId, string clientPlatformIdentifier, string platformIdentifier, string agentPlatformIdentifier)
        {
            var numberToSend = "";

            if (string.IsNullOrEmpty(chatId) && string.IsNullOrEmpty(clientPlatformIdentifier)) 
            { 
                numberToSend = GlobalSocket.numberToSend;
            }

            var inputData = new Dictionary<string, string>
            {
                /*
                 * En este porcion de codigo se necesita poner los parametros estaticos fijos
                 * agentPlatformIdentifier
                 * messagePlatformId
                 * transmitter
                 * statusId
                 * agentPlatformIdentifier
                 */
                //whatsapp:+5214621929111 , w
                { "messagePlatformId", ""},
                { "text", text},
                { "transmitter",  "a"},
                { "statusId", "1"},
                { "chatId", chatId},
                { "clientPlatformIdentifier", clientPlatformIdentifier},
                { "platformIdentifier", platformIdentifier},
                { "agentPlatformIdentifier", agentPlatformIdentifier },
                { "numberToSend", numberToSend}
            };

            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger/outMessage", input);
            HttpContent content = response.Content;
            //string data = await content.ReadAsStringAsync();

            if (platformIdentifier == "c")
            {
                string socketData = JsonConvert.SerializeObject(inputData);
                Console.WriteLine("\n\nSe intentara enviar por socket la data directamente al servidor a continuacion:\n");
                new AsynchronousClient().Send(GlobalSocket.GlobalVarible, socketData);
            }

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

        public async Task<string> newEmptyChat(string text, string chatId, string clientPlatformIdentifier, string platformIdentifier, string agentPlatformIdentifier)
        {
            var numberToSend = "";

            if (string.IsNullOrEmpty(chatId) && string.IsNullOrEmpty(clientPlatformIdentifier))
            {
                numberToSend = GlobalSocket.numberToSend;
            }

            var inputData = new Dictionary<string, string>
            {
                /*
                 * En este porcion de codigo se necesita poner los parametros estaticos fijos
                 * agentPlatformIdentifier
                 * messagePlatformId
                 * transmitter
                 * statusId
                 * agentPlatformIdentifier
                 */
                //whatsapp:+5214621929111 , w
                { "messagePlatformId", ""},
                { "text", "..."},
                { "clientPhoneNumber", GlobalSocket.numberToSend},
                { "transmitter",  "a"},
                { "statusId", ""},
                { "chatId", "0"},
                { "clientPlatformIdentifier","whatsapp:+521"+GlobalSocket.numberToSend}, 
                { "platformIdentifier", "w"},
                { "agentPlatformIdentifier", GlobalSocket.currentUser.activeIp},
                { "userId", GlobalSocket.currentUser.ID }
                
            };

            Console.WriteLine(inputData.Keys.ToString());
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger/newEmptyChat", input);
            HttpContent content = response.Content;
            //string data = await content.ReadAsStringAsync();
            string data = response.StatusCode.ToString();
            if (data != null)
            {
                return data.ToString();
            }

            return string.Empty;
            //cuando se contruya form
            //se identifique de cual chat viene el mensaje
            //tambien el chatPlatformIdentifer
            //sacarlo del json
        }
        /*
         * Preguntar a Juan Carlos que se va a a hacer con estos métodos
         * Para analizar si se peuden borrar o no
         */
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

        public string GetPublicIpAddress()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.ipify.org/");
            request.UserAgent = "curl";
            string publicIpAddress;
            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    publicIpAddress = reader.ReadToEnd();
                }
            }
            return publicIpAddress.Replace("\n", "");
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

