using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginForms.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Configuration;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Drawing;
using System.Net.Http.Headers;
using LoginForms.Utils;

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
        private static string date;
        private static string statusDate;
        private static string endDate;
        private static int llamadaId;
        public static string idStatus;
        private string folio;
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ApplicationLogs\";
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
            catch (Exception ex)
            {
                Console.WriteLine("Error [LlamarGet]: " + ex.Message);
            }
        }
        #endregion
        //GET
        public async Task<string> GetAllMessage(string chatId, string id)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"chatId", chatId},
                {"id", id}
            };
            log.Add($"[RestHelper][GetAllMessage]: id chat:{chatId}, id tipo mensajes:{id}");
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
            Log log = new Log(appPath);
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
                log.Add($"[RestHelper][RecoverActiveChats]: Id de agente:{agentId}");
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
                    Console.WriteLine("Se obtuvieron lo siguientes chats:" + data.ToString());
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
            Log log = new Log(appPath);
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
            Log log = new Log(appPath);
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
                //Aquí cuando la contraseña o el correo son incorrectos manda un error 401, Unauthorized
                //Abra que mandarle una advertencia al usuario
                //de que la contraseña o el correo están mal
                log.Add($"[RestHelper][Login]: Intentando Login: {mail}");
                HttpResponseMessage response = await client.PostAsync(baseUrl + "auth", input);
                HttpContent content = response.Content;
                string data = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    return data;
                }
                else
                {
                    //puede ser aquí donde se le mande al usuario la advertencia
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Add($"Error [RestHelper][Login]:{ex.Message}");
                Console.WriteLine("Error [Login]: " + ex.Message);

            }
            return string.Empty;

        }

        public async Task<string> UpdateActiveColumn(string email)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"email", email}
            };
            log.Add($"[RestHelper][UpdateActiveColumn]:{email}");
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "auth/activeColumn", input);
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

        public async Task<string> UpdateActiveColumnOnClose(string userId)
        {
            Log log = new Log(appPath);
            log.Add($"[RestHelper][UpdateActiveColumnOnClose]:{userId}");
            var inputData = new Dictionary<string, string>
            {
                {"userId", userId}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/updateColmnActiveOnClose", input);
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

        public string BeautifyJson(string strJson)
        {
            JToken parseJson = JToken.Parse(strJson);
            return parseJson.ToString(Formatting.Indented);
        }

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

        #region Metodos sin usar
        public async Task<string> getPermissions(string rolId)
        {
            Log log = new Log(appPath);
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
            Log log = new Log(appPath);
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
        #endregion


        public async Task<string> getMyAgents(string id)
        {
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][getmyAgents] del supervisor con id:{id}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
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
            Log log = new Log(appPath);
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

        public async Task<string> GetActiveUserStatus()
        {
            Log log = new Log(appPath);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "status/timer");
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

        public async Task<string> obtenerDatosChatDiarios(string userId)
        {
            try
            {
                Log log = new Log(appPath);
                var inputData = new Dictionary<string, string> {
                    {"userId", userId }
                };
                var input = new FormUrlEncodedContent(inputData);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/obtenerDatosChatDiarios", input);
                Console.WriteLine(response);
                HttpContent content = response.Content;
                log.Add($"[RestHelper][obtenerDatosChatDiarios]:{userId}");
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
            catch (Exception _e)
            {
                throw _e;
            }
        }

        public async Task<string> getUserChats(string userId)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"userId", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/getUserChats", input);
            Console.WriteLine("Respuesta de RestHelper.getUserChats:" + response);
            log.Add($"[RestHelper][getUserChats] id del agente:{userId} Respuesta del servidor:{response.StatusCode}");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> getAgentsDetails(string userId)
        {
            Log log = new Log(appPath);
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

            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
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
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][getCloseChat]:id chat que se va a cerrar{chatId} respuesta del servidor {response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> transferChat(string idAgenteAnterior, string idAgenteNuevo, string idChat) {
            try
            {
                Log log = new Log(appPath);
                var inputData = new Dictionary<string, string>
                {
                    { "idAntiguo", idAgenteAnterior},
                    { "idNuevo", idAgenteNuevo},
                    { "idChat", idChat},
                    { "idSupervisor", GlobalSocket.currentUser.ID }
                };

                var input = new FormUrlEncodedContent(inputData);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/transferChat", input);
                Console.WriteLine(response);
                HttpContent content = response.Content;
                string data = response.StatusCode.ToString();
                log.Add($"[RestHelper][transferChat]:id Agente que pasa chat:{idAgenteAnterior} id agente que recibe chat:{idAgenteNuevo} id del chat:{idChat} respuesta servidor:{response.StatusCode}");
                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    return data;
                }
                else {
                    return response.StatusCode.ToString();
                }


            }
            catch (Exception _e)
            {
                throw _e;
            }
        }

        public async Task<string> startMonitoring(int idAgente, int idSupervisor)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"idAgente", idAgente.ToString() },
                {"idSupervisor", idSupervisor.ToString() }
            };
            var input = new FormUrlEncodedContent(inputData);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "record/startMonitoring", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            log.Add($"[RestHelper][startMonitoring]:Id Agente a monitorear:{idAgente} id Supervisor monitoreando:{idSupervisor} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> shareScreenshot(Bitmap bitImage, string idSupervisor)
        {
            Log log = new Log(appPath);
            HttpContent stringContent = new StringContent(idSupervisor); // a que chat
            ImageConverter converter = new ImageConverter();
            byte[] BArray = (byte[])converter.ConvertTo(bitImage, typeof(byte[]));

            string idAgente = GlobalSocket.currentUser.ID;
            HttpContent agentContent = new StringContent(idAgente);

            //HttpContent fileStreamContent = new StreamContent(fileStream);
            HttpContent bytesContent = new ByteArrayContent(BArray);

            //Setting type of file
            bytesContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {

                // <input type="text" name="filename" />
                formData.Add(bytesContent, "campo1", "campo1");
                formData.Add(stringContent, "campo2", idSupervisor);
                formData.Add(agentContent, "campo3", idAgente);

                var response = await client.PostAsync(baseUrl + "record", formData);
                HttpContent content = response.Content;
                var data = await content.ReadAsStringAsync();
                log.Add($"[RestHelper][shareScreenshot]:id del supervidor:{idSupervisor} respuesta servidor:{response.StatusCode}");
                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    return data;
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
        }

        public async Task<byte[]> getMonitoring(string imageName)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string> {
                {"image",imageName }
            };
            var input = new FormUrlEncodedContent(inputData);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "record/screen/", input);
            HttpContent content = response.Content;
            var data = await content.ReadAsByteArrayAsync();
            log.Add($"[RestHelper][getMonitoring]: respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> getSubstactActiveChat(string userId)
        {
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][getSubstactActiveChat]: id agente:{userId} respuesta servidor:{response.StatusCode}");
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
            Log log = new Log(appPath);
            //var inputData = new Dictionary<string, string> 
            //{
            //    { "id", id }
            //};
            //var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "network/networks/");
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

        public async Task<string> updateNetworkCategories(string chatId, string networkCategoryId)
        {
            Log log = new Log(appPath);
            string clientPlatformIdentifier = TabPageChat.clientPlatformIdentifierClose;
            string platformIdentifier = TabPageChat.platformIdentifierClose;
            var inputData = new Dictionary<string, string>
            {
                {"chatId", chatId },
                {"networkId", networkCategoryId },
                {"clientPlatformIdentifier", clientPlatformIdentifier },
                {"platformIdentifier", platformIdentifier}

            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/updateNetworkCategory", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            log.Add($"[RestHelper][updateNetworkCategories]: id chat:{chatId} id de la red:{networkCategoryId} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        //public async Task<string> AppParameters(string twilioAccountSID, string twilioAuthToken, string whatsappAccount, string botTokenTelegram)
        //{
        //    var inputData = new Dictionary<string, string>
        //    {
        //        {"twilioAccountSID", twilioAccountSID },
        //        { "twilioAuthToken", twilioAuthToken},
        //        { "whatsappAccount", whatsappAccount},
        //        {"botTokenTelegram", botTokenTelegram }
        //    };

        //    var input = new FormUrlEncodedContent(inputData);
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/appParameters", input);
        //    Console.WriteLine(response);
        //    HttpContent content = response.Content;
        //    string data = await content.ReadAsStringAsync();
        //    if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
        //    {
        //        return data;
        //    }
        //    else
        //    {
        //        return response.StatusCode.ToString();
        //    }
        //}

        public async Task<string> SoftphoneParameters(string userId)
        {
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][startMonitoring]:Id agente:{userId} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> GetUserData(string userId)
        {
            var inputData = new Dictionary<string, string>
            {
                { "userId", userId}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/getUserData", input);
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
            Log log = new Log(appPath);
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
            Log log = new Log(appPath);
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
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][updateSoftphoneParameters]: {response.StatusCode}");
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
            Log log = new Log(appPath);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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
            log.Add($"[RestHelper][updateUserStatus]: id Usuario:{userId}, status a cambiar:{statusId} fecha:{date} respuesta servidor:{response.StatusCode}");
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
            Log log = new Log(appPath);
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
            Log log = new Log(appPath);
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

        public async Task<string> getChatById(string chatId)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>{
                { "chatId", chatId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/getChatByIdRequest", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"[RestHelper][getChatById]: id del chat:{chatId} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine("Chat obtenido:" + data);
                return data;
            }
            else
            {
                return data;
            }
        }

        public async Task<string> SaveUser(string nombre, string apPaterno, string apMaterno, string email, string contrasena, string siglasUser, string tipoUsuario)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                { "nombre", nombre },
                { "apPaterno", apPaterno},
                { "apMaterno", apMaterno },
                { "email", email },
                { "contrasena", contrasena },
                { "siglasUsuario", siglasUser },
                { "tipoUsuario", tipoUsuario }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/saveUser", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"[RestHelper][SaveUser]: datos del nuevo agente:{nombre}, {apPaterno}, {apMaterno}, {email}, {contrasena} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine($"Estatus de la petición[SaveUser]:{response.StatusCode.ToString()}");
                return response.StatusCode.ToString();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> UpdateUser(string userId, string nombre, string apPaterno, string apMaterno, string email, string contrasena, string siglasUser, string tipoUsuario)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                { "userId", userId },
                { "nombre", nombre },
                { "apPaterno", apPaterno},
                { "apMaterno", apMaterno },
                { "email", email },
                { "contrasena", contrasena },
                { "siglasUsuario", siglasUser },
                { "tipoUsuario", tipoUsuario }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/updateUser", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"[RestHelper][UpdateUser]: UserId del agente que se va a modificar:{userId} respuesta servidor:{response.StatusCode}");
            if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return response.StatusCode.ToString();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> DeleteUser(string userId)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                { "userId", userId },
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "parameters/deleteUser", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"[RestHelper][DeleteUser]: UserId del agente que se va a borrar:{userId} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return response.StatusCode.ToString();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> UserHangUp(string colgo)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"IdLlamada", llamadaId.ToString()},
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/user", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"");
            if(!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> VendorHangUp(string colgo)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"IdLlamada", llamadaId.ToString()},
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/vendor", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> SetUserNumber(string number)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"IdLlamada", llamadaId.ToString()},
                {"Number", number},
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/phoneUser", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }


        public async Task<string> getLastChatByUserId(string userId)
        {

            var inputData = new Dictionary<string, string>{
                { "userId", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "chat/getLastChatByUserId", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine("Chat obtenido:" + data);
                return data;
            }
            else
            {
                return data;
            }


        }

        public async Task<string> validateTransferAgent(string userId) 
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                { "id", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "user/validateTransferAgent", input);
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"[RestHelper][validateTransferAgent]: id de usuario:{userId} respuesta servidor:{response.StatusCode}");
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
            Log log = new Log(appPath);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(baseUrl + "user/getAllAgents");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            log.Add($"[RestHelper][getAllAgents]: respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine("Se obtuvieron lo siguientes chats:" + data.ToString());
                return data;
            }

            else
                return response.StatusCode.ToString();

        }

        public async Task<string> getTotalCalls(string userId) 
        {
            Log log = new Log(appPath);
            try
            {
                var inputData = new Dictionary<string, string>
                {
                    {"userId", userId }
                };
                var input = new FormUrlEncodedContent(inputData);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/getTotalCalls", input);
                HttpContent content = response.Content;
                string data = await content.ReadAsStringAsync();
                log.Add($"[RestHelper][getTotalCalls]: id Usuario:{userId} respuesta servidor:{response.StatusCode}");
                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    return data;
                }
                else {
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception _e)
            {
                throw _e;
            }
        }

        public async Task<string> updateAgentActiveIp(string mail, string ipAddress)
        {
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][updateAgentActiveIp]: Active Socket:{ipAddress} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> GenerateExcel(string userId = "" , string fechaInicial = "", string fechaFinal = "")
        {
            var inputData = new Dictionary<string, string>
            {
                { "userId", userId },
                { "date", date },
                { "fechaInicial", fechaInicial },
                { "fechaFinal", fechaFinal }
            };

            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/getCallsUser", input);
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

        public async Task<string> updateAgentMaxActiveChats(string id, string maxActiveChats)
        {
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][updateAgentMaxActiveChats]: id Usuario:{id} chat maximos:{maxActiveChats} respuesta servidor:{response.StatusCode}");
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
            Log log = new Log(appPath);
            var numberToSend = "";

            if (string.IsNullOrEmpty(chatId) && string.IsNullOrEmpty(clientPlatformIdentifier)) 
            { 
                numberToSend = GlobalSocket.numberToSend;
            }

            var inputData = new Dictionary<string, string>
            {
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


            await GlobalSocket.GlobalVarible.EmitAsync("agent-data",inputData);

            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "messenger/outMessage", input);
            HttpContent content = response.Content;
            //string data = await content.ReadAsStringAsync();

            string data = response.StatusCode.ToString();
            log.Add($"[RestHelper][sendMessage]: mensaje enviado al chat:{chatId} respuesta servidor:{response.StatusCode}");
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

        public async Task<string> SendImage(Bitmap bitImage, string chatId, string clientPlatformIdentifier, string platformIdentifier, string agentPlatformIdentifier, string imageLocation)//string text
        {
            Log log = new Log(appPath);
            ImageConverter converter = new ImageConverter();
            byte[] BArray = (byte[])converter.ConvertTo(bitImage, typeof(byte[]));

            HttpContent bytesContent = new ByteArrayContent(BArray);

            bytesContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("chatId", chatId);
            client.DefaultRequestHeaders.Add("clientPlatformIdentifier", clientPlatformIdentifier);
            client.DefaultRequestHeaders.Add("platformIdentifier", platformIdentifier);
            client.DefaultRequestHeaders.Add("agentPlatformIdentifier", agentPlatformIdentifier);
            client.DefaultRequestHeaders.Add("imagelocation", imageLocation);

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(bytesContent, "imagen", "imagen");
                //formData.Add(stringContent, "chatId", chatId);
                //formData.Add(stringContent1, "clientPlatformIdentifier", clientPlatformIdentifier);
                //formData.Add(stringContent2, "platformIdentifier", platformIdentifier);
                //formData.Add(stringContent3, "agentPlatformIdentifier", agentPlatformIdentifier);

                var response = await client.PostAsync(baseUrl + "messenger/sendImage", formData);
                response.EnsureSuccessStatusCode();
                client.Dispose();
                var sd = response.Content.ReadAsStringAsync().Result;
                log.Add($"[RestHelper][sendImage]: imagen enviada al chat:{chatId} respuesta servidor:{response.StatusCode}");
                return sd;
            }
        }

        //Tiempo de cada estado del agente
        public async Task<string> SetStatusTime( string statusId)
        {
            Log log = new Log(appPath);
            statusDate = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var inputData = new Dictionary<string, string>
            {
                { "startingTime", statusDate},
                { "userId", GlobalSocket.currentUser.ID},
                { "statusId", statusId},
                { "startDate", date},
                {"idStatus", idStatus}

            };
            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/setStarTime", input);
            HttpContent content = response.Content;
            string data = response.Content.ReadAsStringAsync().Result;
            log.Add($"[RestHelper][SetStatusTime]: userId:{GlobalSocket.currentUser.ID} id del status:{statusId} tiempo inicio:{statusDate} fecha:{date} idstatus:{idStatus} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                var values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(data);
                idStatus = values["data"]["statusId"];
                Console.WriteLine("StatusId:" + idStatus);
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> ChangeStatus(string userId, string statusId)
        {
            Log log = new Log(appPath);
            Console.WriteLine("idStatus" + idStatus);
            endDate = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var inputData = new Dictionary<string, string>
            {
                {"endingTime", endDate},
                {"startingTime", endDate },
                {"userId", userId},
                {"statusId", statusId },
                {"startDate", date},
                {"idStatus", idStatus}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/changeStatus", input);
            HttpContent content = response.Content;
            string data = await response.Content.ReadAsStringAsync();
            log.Add($"[RestHelper][ChangeStatus]: userId:{GlobalSocket.currentUser.ID} id del status:{statusId} fecha de inicio:{date} inicio status:{endDate} cierre status:{endDate} fecha:{date} idStatus:{idStatus} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString())&& response.StatusCode.ToString() == "OK")
            {
                var values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(data);
                idStatus = values["data"]["idStatus"];
                Console.WriteLine("StatusId:" + idStatus);
                return idStatus;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> TotalTimeStatus(string userId)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"startingTime", endDate},
                {"userId", userId }
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/totalTime", input);
            HttpContent content = response.Content;
            string data = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                Console.WriteLine(data);
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> UpdateOnClosing(string userId, string statusId)
        {
            Log log = new Log(appPath);
            string endDate = DateTime.Now.ToString("HH:mm:ss");
            var inputData = new Dictionary<string, string>
            {
                {"endingTime", endDate },
                {"userId", userId },
                {"statusId", statusId },
                {"idStatus", idStatus}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/updateOnClosing", input);
            HttpContent content = response.Content;
            string data = await response.Content.ReadAsStringAsync();
            log.Add($"[RestHelper][UpdateOnClosing]: userId:{userId} endingTime:{endDate} statusId:{statusId} idStatus:{idStatus} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

            //updateOnClosing
        }

        public async Task<string> SendCall(string tipo, string clientPhoneNumber, string agentPhoneNumber, string transfer = "0")
        {
            Log log = new Log(appPath);
            date = DateTime.Now.ToString("HH:mm:ss");
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var inputData = new Dictionary<string, string>
            {
                {"startTime", date},
                {"userId", GlobalSocket.currentUser.ID },
                {"tipoLlamada", tipo },
                { "llamadaTransferida", transfer },
                {"startingDate", dateTime},
                {"clientPhoneNumber", clientPhoneNumber },
                {"agentPhoneNumber", agentPhoneNumber },

            };

            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls", input);
            HttpContent content = response.Content;
            string data = response.Content.ReadAsStringAsync().Result;
            log.Add($"[RestHelper][SendCall]: userId:{GlobalSocket.currentUser.ID} startTime:{date} startingDate:{dateTime} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                var folio = GetFolioFromApi(data);
                return folio;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> UpdateNetworkCategoryCalls(string networkCategoryId = "", string score = "", string comments = "")
        {
            Log log = new Log(appPath);
            string endingTime = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"Inicio de la llamada:{date}");
            Console.WriteLine($"Termino de la llamada:{endingTime}");
            var inputData = new Dictionary<string, string>
            {
                {"startTime", date },
                {"endingTime", endingTime},
                {"networkCategoryId", networkCategoryId },
                {"score", score },
                {"comments", comments },
                {"idLlamada", llamadaId.ToString()}
            };
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/updateNetworkCall", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = response.StatusCode.ToString();
            log.Add($"[RestHelper][UpdateNetworkCategoryCalls]:idLlamada:{llamadaId} startTime:{date} endingTime:{endingTime} networkCategoryId:{networkCategoryId} espuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> GetIdCall()
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                {"idLlamada", llamadaId.ToString()}
            };
            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "calls/getIdCall", input);
            Console.WriteLine(response);
            HttpContent content = response.Content;
            string data = await response.Content.ReadAsStringAsync();
            log.Add($"[RestHelper][GetIdCall]: idLlamada:{llamadaId} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }

        }

        public async Task<string> GetUserStates(string id)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                { "id", id}
            };
            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/GetUserStates", input);
            HttpContent content = response.Content;
            string data = response.Content.ReadAsStringAsync().Result;
            log.Add($"[RestHelper][GetUserStates]: userId:{id} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> GetUserStatesSupervisor(string id)
        {
            Log log = new Log(appPath);
            var inputData = new Dictionary<string, string>
            {
                { "id", id},
            };
            Console.WriteLine(inputData);
            var input = new FormUrlEncodedContent(inputData);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(baseUrl + "status/GetUserStatesSupervisor", input);
            HttpContent content = response.Content;
            string data = response.Content.ReadAsStringAsync().Result;
            log.Add($"[RestHelper][GetUserStatesSupervisor]: idSupervisor:{id} respuesta servidor:{response.StatusCode}");
            if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
            {
                return data;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

        public async Task<string> newEmptyChat(string text, string chatId, string clientPlatformIdentifier, string platformIdentifier, string agentPlatformIdentifier)
        {
            Log log = new Log(appPath);
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
            log.Add($"[RestHelper][newEmptyChat]: clientPlatformIdentifier:whatsapp:+521{GlobalSocket.numberToSend} userId:{GlobalSocket.currentUser.ID} agentPlatformIdentifier:{GlobalSocket.currentUser.activeIp} respuesta servidor:{response.StatusCode}");
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


        //***** MÉTODO QUE SIRVE PARA SACAR EL FOLIO DE LA PETICIÓN HTTP *********//
        public string GetFolioFromApi(string response)
        {
            Log log = new Log(appPath);
            var values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(response);
            folio = values["data"]["folio"];
            llamadaId = values["data"]["idLlamada"];
            log.Add($"[RestHelper][GetFolioFromAPI]: folio:{folio} llamadaId:{llamadaId}");
            Console.WriteLine(folio);
            Console.WriteLine(llamadaId);
            
            return folio;
        }

    }
    public class MsjApi
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

}

