using LoginForms.Shared;
using LoginForms.Utils;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Models;

namespace LoginForms
{
    class SocketIOClient
    {
        public static Prueba prueba;
        private static readonly string baseUrl = ConfigurationManager.AppSettings["IpSocketIO"];
        static RestHelper rh = new RestHelper();
        public static bool Monitoreando = false;

        public static async Task ClienteSocketIO()
        {
            var uri = new Uri(baseUrl);
            //var uri = new Uri("http://201.149.34.171:3025");
            var socket = new SocketIO(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string>
                {
                    {"token", "V3"}
                },
                Reconnection = true,
                ReconnectionDelay = 600

            });

            GlobalSocket.GlobalVarible = socket;

            socket .OnConnected += async (sender, e) =>
            {
                Console.WriteLine("Socket_OnConnected");
                var client  = sender as SocketIO;
                Console.WriteLine($"SocketId: {socket.Id}");
                //await treatNotificationAsync(socket);
                await socket.EmitAsync("hi", DateTime.Now.ToString());
                //await socket.EmitAsync("agent-data", new Models.Message
                //{
                //    messagePlatformId = GlobalSocket.message.messagePlatformId,
                //    text = GlobalSocket.message.text,
                //    transmitter = GlobalSocket.message.transmitter,
                //    statusId = GlobalSocket.message.statusId,
                //    chatId = GlobalSocket.message.chatId,
                //    clientPlatformIdentifier = GlobalSocket.message.clientPlatformIdentifier,
                //    platformIdentifier = GlobalSocket.message.agentPlatformIdentifier,
                //    agentPlatformIdentifier = GlobalSocket.message.agentPlatformIdentifier
                //});
            };
            //socket.OnConnected += Socket_OnConnected;
            socket.OnPing += Socket_OnPing;
            socket.OnPong += Socket_OnPong;
            socket.OnDisconnected += Socket_OnDisconnected;
            socket.OnReconnectAttempt += Socket_OnReconnecting;
            socket.OnReconnected += Socket_OnReconnected;
            socket.OnError += Socket_OnError;
            socket.OnAny((name, response) =>
            {
                Console.WriteLine("Evento que se escucha desde el server");
                Console.WriteLine($"Event Name:{name} \t Response:{response}\n");
            });

            socket.On("serverEvent", response =>
            {
                Console.WriteLine($"Server response:{response.GetValue<string>()}");

            });

            socket.On("serverNotification", async response =>
            {

                Console.WriteLine($"Server Notification:{response}");
                await treatNotificationAsync(response.ToString());
            });

            socket.On("port", async response => //async
            {
                await treatNotificationAsync(response.ToString());
                //await SetSocketPort(response.ToString());
            });

            //socket.EmitAsync("server-data", SendData());

            await socket.ConnectAsync();
        }

        private static void Socket_OnError(object sender, string e)
        {
            Console.WriteLine($"Error en Socket: {e}");
        }

        private static void Socket_OnReconnected(object sender, int e)
        {
            var temp = GlobalSocket.currentUser.ID.ToString();
            prueba.recoverActiveChats(temp);
            throw new NotImplementedException(); /// esté metodo cuando se renecta el socket, por ejemplo cuandp hay reinicio de API
        }

        //private static async void Socket_OnConnected(object sender, EventArgs e)
        //{
        //    Console.WriteLine("Socket_OnConnected");
        //    var socket = sender as SocketIO;
        //    Console.WriteLine($"SocketId: {socket.Id}");
        //    //await treatNotificationAsync(socket);
        //    await socket.EmitAsync("hi", DateTime.Now.ToString());
        //    await socket.EmitAsync("agent-data", new Models.Message
        //    {
        //        messagePlatformId = GlobalSocket.message.messagePlatformId,
        //        text = GlobalSocket.message.text,
        //        transmitter = GlobalSocket.message.transmitter,
        //        statusId = GlobalSocket.message.statusId,
        //        chatId = GlobalSocket.message.chatId,
        //        clientPlatformIdentifier = GlobalSocket.message.clientPlatformIdentifier,
        //        platformIdentifier = GlobalSocket.message.agentPlatformIdentifier,
        //        agentPlatformIdentifier = GlobalSocket.message.agentPlatformIdentifier
        //    });

        //    //await socket.EmitAsync("agent-data", SendData());
        //}

        private static void Socket_OnReconnecting(object sender, int e)
        {
            Console.WriteLine($"{DateTime.Now}:Reconnecting attemp = {e}");
        }

        public static void Socket_OnDisconnected(object sender, string e)
        {
            Console.WriteLine($"Disconnected:{e}");
        }

        private static void Socket_OnPong(object sender, TimeSpan e)
        {
            Console.WriteLine($"Pong:{e.TotalMilliseconds}");
            //Console.WriteLine($"Pong en segundos:{e.TotalSeconds}");
            Console.WriteLine($"Pong en milisegundos:{e.TotalMilliseconds}");
            var client = sender as SocketIO;
            Console.WriteLine($"Estado socket:{client.Connected}");
            Console.WriteLine($"Socket Id: {client.Id}");
        }

        private static void Socket_OnPing(object sender, EventArgs e)
        {
            Console.WriteLine("Ping");
        }

        public static async Task treatNotificationAsync(string socketNotification)
        {
            try
            {

                #region Metodos con notificacion
                Console.WriteLine("\n************** \nSe recibe lo siguiente:" + socketNotification);

                var temp3 = socketNotification.Substring(1).ToString();
                var temp4 = temp3.ToString().Replace(']', ' ');
                Console.WriteLine("Limpiado" + temp4.ToString());

                var jobjectP = JsonConvert.DeserializeObject(temp4);

                var jobject = JsonConvert.DeserializeObject<JObject>(temp4);
                //var jobject = JsonConvert.DeserializeObject<JObject>(socketNotification);


                //socketNotification.GetValue<string>();
                // Console.WriteLine(socketNotification);
                //[{ "socket": "dasdasdasdads"}]

                if (jobject.ContainsKey("Agent"))
                {
                    try
                    {
                        string Agent = jobject.Value<string>("Agent");
                        string Message = jobject.Value<string>("message");
                        var time24 = DateTime.Now.ToString("HH:mm:ss");

                        new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", Agent + time24)
                        .AddText("Agente " + Agent)
                        .AddText(Message)
                        .AddAttributionText("Hora: " + time24)
                        //.SetBackgroundActivation()
                        .Show(toast =>
                        {
                            toast.Tag = "18365";
                            toast.Group = "wallPosts";
                            toast.ExpirationTime = DateTime.Now.AddSeconds(1);
                            toast.Dismissed += (senderT, args) => ToastNotificationManagerCompat.History.Clear();
                        });
                    }
                    catch(System.Runtime.InteropServices.COMException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                //else if (jobject.ContainsKey("CloseChat"))
                //{
                //    Console.WriteLine("\nEl id del chat es: " + jobject.Value<string>("chatId"));
                //    TabPageChat chat = prueba.getTabChatByChatId(jobject.Value<string>("chatId"));
                //    MessageBox.Show("El cliente ha abandonado la conversacion, \nPor favor, cierre el chat.");
                //    chat.txtSendMessage.Visible = false;
                //    chat.btnSendMessage.Visible = false;
                //    //chat.btnCloseButton.PerformClick();
                //}
                else if (jobject.ContainsKey("closeTransferChat"))
                {
                    Console.WriteLine("\nEl id del chat es: " + jobject.Value<string>("chatId"));


                    Prueba prue = (Prueba)Application.OpenForms["Prueba"];
                    TabPageChat chat = prue.getTabChatByChatId(jobject.Value<string>("chatId"));

                    //TabPageChat chat = prueba.getTabChatByChatId(jobject.Value<string>("chatId"));
                    chat.removeTabChat();

                }
                else if (jobject.ContainsKey("openTransferChat"))
                {

                    Console.WriteLine("La respuesta de la transferencia de chats cayo al supervisor");

                    string chatId = jobject.Value<string>("chatId");
                    var chatData = await rh.getChatById(chatId);
                    var cleanData = (JObject)JsonConvert.DeserializeObject(chatData);
                    var algo = cleanData["data"];

                    Models.Message Object = new Models.Message();
                    Object.chatId = algo["id"].Value<string>();
                    Object.platformIdentifier = algo["platformIdentifier"].Value<string>();
                    Object.clientPlatformIdentifier = algo["clientPlatformIdentifier"].Value<string>();

                    Console.WriteLine("El objeto chat creado es este:" + JsonConvert.SerializeObject(Object).ToString());



                    Prueba prue = (Prueba)Application.OpenForms["Prueba"];
                    prue.treatNotification(Object);

                    //SocketIOClient.prueba.treatNotification(Object);
                }
                else if (jobject.ContainsKey("startMonitoring"))
                {
                    string idSupervisor = jobject.Value<string>("idSupervisor");

                    //*******
                    //Metodo alternativo para obtener la captura de pantalla del agente
                    //*******

                    // Determine the size of the "virtual screen", including all monitors.
                    int screenLeft = SystemInformation.VirtualScreen.Left;
                    int screenTop = SystemInformation.VirtualScreen.Top;
                    int screenWidth = SystemInformation.VirtualScreen.Width;
                    int screenHeight = SystemInformation.VirtualScreen.Height;

                    // Create a bitmap of the appropriate size to receive the screenshot.
                    using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
                    {
                        // Draw the screenshot into our bitmap.
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                            await rh.shareScreenshot(bmp, idSupervisor);

                        }
                    }

                    //******
                    //Metodo para guardar mapa de bits como imagen y mostrarlo
                    //******

                    //string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AgentScreenshots\";
                    //if (Directory.Exists(appPath) == false)
                    //{
                    //    Directory.CreateDirectory(appPath);
                    //}
                    //string path = appPath + "image.jpeg";

                    //bitmap.Save(path, ImageFormat.Jpeg);
                    //pictureBox1.ImageLocation = path;

                }
                else if (jobject.ContainsKey("getMonitoring"))
                {

                    screenMonitor scrM = (screenMonitor)Application.OpenForms["screenMonitor"];
                    PictureBox pic = (PictureBox)scrM.Controls["pictureBox1"];

                    var temp = jobject;
                    var imageTemp = jobject.Value<string>("Image");
                    var idSupervisor = jobject.Value<int>("idSupervisor");
                    var idAgente = jobject.Value<int>("idAgente");

                    ImageCodecInfo myImageCodecInfo;
                    System.Drawing.Imaging.Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters = new EncoderParameters();

                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    Console.WriteLine("\nEl id del supervisor es:" + idSupervisor + "\nEl id del Agente es:" + idAgente + "\nEl estatus de monitoreo es:" + Monitoreando);

                    if (Monitoreando)
                    {
                        await rh.startMonitoring(idAgente, idSupervisor);
                    }

                    //*******
                    //Este metodo obtiene la ruta desde el servidor y la pinta 
                    //*******

                    //string baseUrl = ConfigurationManager.AppSettings["IpServidor"];
                    //string webPath = baseUrl + "monitoreo/uploads/" + imageTemp;
                    //screenMonitor scrM = (screenMonitor)Application.OpenForms["screenMonitor"];
                    //scrM.setImagePath(webPath);

                    //******
                    //Este metodo obtiene el nombe de la imagen en el servidor y hace una peticion para obtener toda la imagen
                    //******

                    var dataTemp = await rh.getMonitoring(imageTemp);
                    using (var ms = new MemoryStream(dataTemp))
                    {
                        Image imagen = Image.FromStream(ms);
                        Bitmap bits = new Bitmap(imagen);

                        string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AgentScreenshots\";
                        if (Directory.Exists(appPath) == false)
                        {
                            Directory.CreateDirectory(appPath);
                        }
                        string path = appPath + "image.jpeg";


                        myEncoderParameter = new EncoderParameter(myEncoder, 25L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bits.Save(path, myImageCodecInfo, myEncoderParameters);

                        pic.Image = bits;

                        if (!Monitoreando)
                        {
                            pic.Image = null;
                        }

                    }
                }
                else if (jobject.ContainsKey("socketPort"))
                {
                    var port = jobject.Value<string>("socketPort");
                    Console.WriteLine("\nHola agente, tu puerto asignado por la API es:" + port);
                    var temp = await rh.updateAgentActiveIp(GlobalSocket.currentUser.email, port.ToString());
                    if (temp == "OK")
                    {
                        GlobalSocket.currentUser.activeIp = port;
                    }
                    else
                    {
                        Console.WriteLine("No se pudo actualizar el puerto en base correctamente:" + temp);
                    }

                    FormPrincipal ActivePrincipal = (FormPrincipal)Application.OpenForms["FormPrincipal"];
                    ActivePrincipal.BeginInvoke(new MethodInvoker(() =>
                    {
                        ActivePrincipal.TextSocket = "Socket:" + port;

                    }));


                    //if (conexionPerdidaMonitoreo == true && temp == "OK")
                    //{
                    //    screenMonitor scrM = (screenMonitor)Application.OpenForms["screenMonitor"];
                    //    screenMonitor.Reconectado = true;
                    //    scrM.ReconexionServidor();
                    //}
                }
                else
                {

                    var tempFirst = jobject.First;

                    //Models.Message notification = JsonConvert.DeserializeObject<Models.Message>(jobjectP.ToString())
                    Models.Message notification = JsonConvert.DeserializeObject<Models.Message>(jobject.ToString());
                    //if(jobject.Value<string>("platformIdentifier") == "t")
                    //{
                    //    GlobalSocket.numberToClose = jobject.Value<string>("clientPlatformIdentifier");
                    //    Console.WriteLine($"clientPlatformIdentifier: {GlobalSocket.numberToClose}");
                    //}
                    //else if(jobject.Value<string>("platformIdentifier") == "w")
                    //{
                    //    GlobalSocket.numberToClose = jobject.Value<string>("clientPlatformIdentifier");
                    //    Console.WriteLine($"clientPlatformIdentifier: {GlobalSocket.numberToClose}");
                    //}

                    Console.WriteLine("notificacion:" + notification);

                    //Cuando ocurria una intermitencia en la red y se reconectaba, la instancia de prueba venia nula, por eso el otro metodo
                    //prueba.treatNotification(notification);

                    Prueba Activeprueba = (Prueba)Application.OpenForms["Prueba"];
                    Activeprueba.treatNotification(notification);

                }

                //if (!prueba.tabChatExits(notification.chatId))
                //    prueba.buildNewTabChat(notification.chatId);

                #region Proceso para disparar el método de agregado dinámico
                //using (Prueba fprueba = new Prueba())
                //{
                //    //Thread threadAddLabelMessages = new Thread(new ThreadStart(fprueba.invokeAddLabelMessages));
                //    //threadAddLabelMessages.Start();
                //    fprueba.throwThread();
                //    //Intentar metodo => metodo delegado => hilo  
                //}
                #endregion

                ////Comentada para intentar disparar la petición desde los métodos internos del Form
                //var instancia = new RestHelper();
                //instancia.llamarGet(notification.chatId, notification.id, container, whatsapp, prueba, fPrincipal);
                ////string str = instancia.llamarGet(chatId, id, container, whatsapp, prueba, fPrincipal);
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[treatNotification]: " + ex.ToString());


            }
        }

        //public static string SendData(string data = "")
        //{
        //    return data;
        //}
        
        public static void GetData(Dictionary<string, string> json)
        {
            var jsonData = JsonConvert.SerializeObject(json);
            
            foreach (var item in jsonData)
            {
                new JsonSocket().AgentPlatformIdentifier = jsonData;
            }
        }
        public static async Task SetSocketPort(string port)
        {
            var jobject = JsonConvert.DeserializeObject<JObject>(port);
            Console.WriteLine(jobject);
            Console.WriteLine("\nHola agente, tu puerto asignado por la API es:" + port);
            var temp = await rh.updateAgentActiveIp(GlobalSocket.currentUser.email, port.ToString());
            if (temp == "OK")
            {
                GlobalSocket.currentUser.activeIp = port;
            }
            else
            {
                Console.WriteLine("No se pudo actualizar el puerto en base correctamente:" + temp);
            }

            FormPrincipal ActivePrincipal = (FormPrincipal)Application.OpenForms["FormPrincipal"];
            ActivePrincipal.BeginInvoke(new MethodInvoker(() =>
            {
                ActivePrincipal.TextSocket = "Socket:" + port;

            }));
        }

        //Se comenta el constructor por si se llegara a utilizar<s
        //public SocketIOClient()
        //{
        //    //ConnectSocketIOClient();
        //}

        //public async void ConnectSocketIOClient()
        //{
        //    //SocketIO client = new SocketIO("http://201.149.34.171:3020", new SocketIOOptions
        //    //{
        //    //    Reconnection = true,
        //    //});
        //    SocketIO client = new SocketIO("http://192.168.1.108:3001", new SocketIOOptions
        //    {
        //        Reconnection = true,
        //    });
        //    Console.WriteLine($"Application Connected to: {client.ServerUri}");
        //    SocketPort(client);
        //    await client.ConnectAsync();
        //}

        //public async void SendMessage(string data, SocketIO client = null)
        //{
        //    await treatNotificationAsync(data);
        //}

        //private void ReceiveMessage(SocketIO client)
        //{

        //}

        //public async Task treatNotificationAsync(string socketNotification)
        //{
        //    try
        //    {
        //        Console.WriteLine("\n************** \nSe recibe lo siguiente:" + socketNotification);
        //        var jobject = JsonConvert.DeserializeObject<JObject>(socketNotification);

        //        if (jobject.ContainsKey("Agent"))
        //        {

        //            string Agent = jobject.Value<string>("Agent");
        //            string Message = jobject.Value<string>("message");
        //            var time24 = DateTime.Now.ToString("HH:mm:ss");

        //            new ToastContentBuilder()
        //            .AddArgument("action", "viewConversation")
        //            .AddArgument("conversationId", Agent + time24)
        //            .AddText("Agente " + Agent)
        //            .AddText(Message)
        //            .AddAttributionText("Hora: " + time24)
        //            //.SetBackgroundActivation()
        //            .Show(toast =>
        //            {
        //                toast.ExpirationTime = DateTime.Now.AddSeconds(1);
        //                toast.Dismissed += (senderT, args) => ToastNotificationManagerCompat.History.Clear();
        //            });
        //        }
        //        //else if (jobject.ContainsKey("CloseChat"))
        //        //{
        //        //    Console.WriteLine("\nEl id del chat es: " + jobject.Value<string>("chatId"));
        //        //    TabPageChat chat = prueba.getTabChatByChatId(jobject.Value<string>("chatId"));
        //        //    MessageBox.Show("El cliente ha abandonado la conversacion, \nPor favor, cierre el chat.");
        //        //    chat.txtSendMessage.Visible = false;
        //        //    chat.btnSendMessage.Visible = false;
        //        //    //chat.btnCloseButton.PerformClick();
        //        //}
        //        else if (jobject.ContainsKey("closeTransferChat"))
        //        {
        //            Console.WriteLine("\nEl id del chat es: " + jobject.Value<string>("chatId"));

        //            TabPageChat chat = prueba.getTabChatByChatId(jobject.Value<string>("chatId"));
        //            chat.removeTabChat();

        //        }
        //        else if (jobject.ContainsKey("openTransferChat"))
        //        {

        //            Console.WriteLine("La respuesta de la transferencia de chats cayo al supervisor");

        //            string chatId = jobject.Value<string>("chatId");
        //            var chatData = await rh.getChatById(chatId);
        //            var cleanData = (JObject)JsonConvert.DeserializeObject(chatData);
        //            var algo = cleanData["data"];

        //            Models.Message Object = new Models.Message();
        //            Object.chatId = algo["id"].Value<string>();
        //            Object.platformIdentifier = algo["platformIdentifier"].Value<string>();
        //            Object.clientPlatformIdentifier = algo["clientPlatformIdentifier"].Value<string>();

        //            Console.WriteLine("El objeto chat creado es este:" + JsonConvert.SerializeObject(Object).ToString());

        //            prueba.treatNotification(Object);
        //        }
        //        else if (jobject.ContainsKey("startMonitoring"))
        //        {
        //            string idSupervisor = jobject.Value<string>("idSupervisor");

        //            //*******
        //            //Metodo alternativo para obtener la captura de pantalla del agente
        //            //*******

        //            // Determine the size of the "virtual screen", including all monitors.
        //            int screenLeft = SystemInformation.VirtualScreen.Left;
        //            int screenTop = SystemInformation.VirtualScreen.Top;
        //            int screenWidth = SystemInformation.VirtualScreen.Width;
        //            int screenHeight = SystemInformation.VirtualScreen.Height;

        //            // Create a bitmap of the appropriate size to receive the screenshot.
        //            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
        //            {
        //                // Draw the screenshot into our bitmap.
        //                using (Graphics g = Graphics.FromImage(bmp))
        //                {
        //                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
        //                    await rh.shareScreenshot(bmp, idSupervisor);

        //                }
        //            }

        //            //******
        //            //Metodo para guardar mapa de bits como imagen y mostrarlo
        //            //******

        //            //string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AgentScreenshots\";
        //            //if (Directory.Exists(appPath) == false)
        //            //{
        //            //    Directory.CreateDirectory(appPath);
        //            //}
        //            //string path = appPath + "image.jpeg";

        //            //bitmap.Save(path, ImageFormat.Jpeg);
        //            //pictureBox1.ImageLocation = path;

        //        }
        //        else if (jobject.ContainsKey("getMonitoring"))
        //        {

        //            screenMonitor scrM = (screenMonitor)Application.OpenForms["screenMonitor"];
        //            PictureBox pic = (PictureBox)scrM.Controls["pictureBox1"];

        //            var temp = jobject;
        //            var imageTemp = jobject.Value<string>("Image");
        //            var idSupervisor = jobject.Value<int>("idSupervisor");
        //            var idAgente = jobject.Value<int>("idAgente");

        //            ImageCodecInfo myImageCodecInfo;
        //            System.Drawing.Imaging.Encoder myEncoder;
        //            EncoderParameter myEncoderParameter;
        //            EncoderParameters myEncoderParameters = new EncoderParameters();

        //            myImageCodecInfo = GetEncoderInfo("image/jpeg");
        //            myEncoder = System.Drawing.Imaging.Encoder.Quality;

        //            Console.WriteLine("\nEl id del supervisor es:" + idSupervisor + "\nEl id del Agente es:" + idAgente + "\nEl estatus de monitoreo es:" + Monitoreando);

        //            if (Monitoreando)
        //            {
        //                await rh.startMonitoring(idAgente, idSupervisor);
        //            }

        //            //*******
        //            //Este metodo obtiene la ruta desde el servidor y la pinta 
        //            //*******

        //            //string baseUrl = ConfigurationManager.AppSettings["IpServidor"];
        //            //string webPath = baseUrl + "monitoreo/uploads/" + imageTemp;
        //            //screenMonitor scrM = (screenMonitor)Application.OpenForms["screenMonitor"];
        //            //scrM.setImagePath(webPath);

        //            //******
        //            //Este metodo obtiene el nombe de la imagen en el servidor y hace una peticion para obtener toda la imagen
        //            //******

        //            var dataTemp = await rh.getMonitoring(imageTemp);
        //            using (var ms = new MemoryStream(dataTemp))
        //            {
        //                Image imagen = Image.FromStream(ms);
        //                Bitmap bits = new Bitmap(imagen);

        //                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AgentScreenshots\";
        //                if (Directory.Exists(appPath) == false)
        //                {
        //                    Directory.CreateDirectory(appPath);
        //                }
        //                string path = appPath + "image.jpeg";


        //                myEncoderParameter = new EncoderParameter(myEncoder, 25L);
        //                myEncoderParameters.Param[0] = myEncoderParameter;
        //                bits.Save(path, myImageCodecInfo, myEncoderParameters);

        //                pic.Image = bits;

        //                if (!Monitoreando)
        //                {
        //                    pic.Image = null;
        //                }

        //            }
        //        }
        //        else if (jobject.ContainsKey("socketPort"))
        //        {
        //            var port = jobject.Value<string>("socketPort");
        //            Console.WriteLine("\nHola agente, tu puerto asignado por la API es:" + port);
        //            var temp = await rh.updateAgentActiveIp(GlobalSocket.currentUser.email, port.ToString());
        //            if (temp == "OK")
        //            {
        //                GlobalSocket.currentUser.activeIp = port;
        //            }
        //            else
        //            {
        //                Console.WriteLine("No se pudo actualizar el puerto en base correctamente:" + temp);
        //            }

        //            FormPrincipal ActivePrincipal = (FormPrincipal)Application.OpenForms["FormPrincipal"];
        //            ActivePrincipal.BeginInvoke(new MethodInvoker(() => {
        //                ActivePrincipal.TextSocket = "Socket:" + port;

        //            }));


        //            //if (conexionPerdidaMonitoreo == true && temp == "OK")
        //            //{
        //            //    screenMonitor scrM = (screenMonitor)Application.OpenForms["screenMonitor"];
        //            //    screenMonitor.Reconectado = true;
        //            //    scrM.ReconexionServidor();
        //            //}
        //        }
        //        else
        //        {
        //            Models.Message notification = JsonConvert.DeserializeObject<Models.Message>(socketNotification);

        //            Console.WriteLine("notificacion:" + notification);

        //            //Cuando ocurria una intermitencia en la red y se reconectaba, la instancia de prueba venia nula, por eso el otro metodo
        //            //prueba.treatNotification(notification);

        //            Prueba Activeprueba = (Prueba)Application.OpenForms["Prueba"];
        //            Activeprueba.treatNotification(notification);

        //        }

        //        //if (!prueba.tabChatExits(notification.chatId))
        //        //    prueba.buildNewTabChat(notification.chatId);

        //        #region Proceso para disparar el método de agregado dinámico
        //        //using (Prueba fprueba = new Prueba())
        //        //{
        //        //    //Thread threadAddLabelMessages = new Thread(new ThreadStart(fprueba.invokeAddLabelMessages));
        //        //    //threadAddLabelMessages.Start();
        //        //    fprueba.throwThread();
        //        //    //Intentar metodo => metodo delegado => hilo  
        //        //}
        //        #endregion

        //        ////Comentada para intentar disparar la petición desde los métodos internos del Form
        //        //var instancia = new RestHelper();
        //        //instancia.llamarGet(notification.chatId, notification.id, container, whatsapp, prueba, fPrincipal);
        //        ////string str = instancia.llamarGet(chatId, id, container, whatsapp, prueba, fPrincipal);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error[treatNotification]: " + ex.ToString());
        //    }
        //}

        //public async SocketIO SocketPort(SocketIO client)
        //{
        //    var data = null;
        //    await client.On("socketPort", response => {
        //        Console.WriteLine(response);
        //        data = response;
        //    });
        //    return data;
        //}
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }

    public class JsonSocket
    {
        [JsonPropertyName("messagePlatformId")]
        public string MessagePlatformId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("transmitter")]
        public string Transmitter { get; set; }

        [JsonPropertyName("statusId")]
        public string StatusId { get; set; }

        [JsonPropertyName("chatId")]
        public string ChatId { get; set; }

        [JsonPropertyName("clientPlatformIdentifier")]
        public string ClientPlatformIdentifier { get; set; }

        [JsonPropertyName("platformIdentifier")]
        public string PlatformIdentifier { get; set; }

        [JsonPropertyName("agentPlatformIdentifier")]
        public string AgentPlatformIdentifier { get; set; }

        [JsonPropertyName("numberToSend")]
        public string NumberToSend { get; set; }
    }
}
