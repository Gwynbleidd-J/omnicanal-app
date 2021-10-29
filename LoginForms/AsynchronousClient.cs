using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using LoginForms.Shared;
using Newtonsoft.Json;
using System.Drawing;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Toolkit.Uwp.Notifications;
using LoginForms.Utils;

namespace LoginForms
{

    /*CAMBIOS PROPUESTOS PARA MEJORAR LA CLASE AsynchronousClient.cs
     * A la aplicacion de sockets se le haran pequeños cambios para solucionar algunos errores al momento de desconectarse.
     * de la API y cerrar comunicacion con el socket.
     * Solo le agregaran métodos a la clase.
     * El código comentado es el código original.
     */
    class AsynchronousClient
    {
        private RichTextBox container;
        private Form whatsapp;
        public Prueba prueba;
        public WebChat webChat;
        private Form fPrincipal;
        //private TabControl tbControlContainer;
        //private ChatWindow chatWindow;

        //Constructores
        public AsynchronousClient(RichTextBox container, Form whatsapp, Prueba prueba, Form fPrincipal, WebChat webChat)
        {
            try
            {
                this.container = container;
                this.whatsapp = whatsapp;
                this.prueba = prueba;
                this.fPrincipal = fPrincipal;
                //this.tbControlContainer = tabControlFromForm;
                //inicializarChatWindow();
                //recoverActiveChats();

                //Se agrega referencia al nuevo form para probar funcionalidad de las construcción dinámica en otra ventana
                this.webChat = webChat;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[construct AsynchronousClient]: " + ex.ToString());
            }
        }
        public AsynchronousClient()
        {
        }
        
        public RestHelper rh = new RestHelper();
        private readonly int port = Convert.ToInt32(ConfigurationManager.AppSettings["serverTCPPort"]);

        
        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static string response = string.Empty;

        public void Connect()
        {
            try
            {
                //Establish the remote endpoint for the socket.
                IPAddress ipAddress = IPAddress.Parse("192.168.1.103");
                //IPAddress ipAddress = IPAddress.Parse("201.149.34.171");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                
                
                // Create a TCP/IP socket.  
                GlobalSocket.GlobalVarible = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //Connect to the remote endpoint
                GlobalSocket.GlobalVarible.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), GlobalSocket.GlobalVarible);
                connectDone.WaitOne();
                Receive(GlobalSocket.GlobalVarible);//Receive();
                receiveDone.WaitOne();

                //Send(GlobalSocket.GlobalVarible, "This is a test");
                sendDone.WaitOne();

                Receive(GlobalSocket.GlobalVarible);
                receiveDone.WaitOne();

                Console.WriteLine($"Response received:{response}");

                GlobalSocket.GlobalVarible.Shutdown(SocketShutdown.Both);
                GlobalSocket.GlobalVarible.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //public void EscucharMesdsdnsajes()
        //{
        //    try
        //    {
        //        // Receive the response from the remote device.
        //        Receive();
        //        //receiveDone.WaitOne();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("Error [EscucharMensajes]: " + ex);
        //    }
        //}

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine($"Socket connected to:{client.RemoteEndPoint}");
                Console.WriteLine($"Desde el socket:{client.LocalEndPoint}");
                

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Receive(Socket client) //public void Receive()
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client; //state.workSocket = GlobalSocket.GlobalVariable
                                           // Begin receiving the data from the remote device.

                client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                //GlobalSocket.GlobalVarible.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                //    new AsyncCallback(ReceiveCallback), state);


            }
            catch (Exception e)
            {
                Console.WriteLine("Error[Receive]: " + e.ToString());
            }
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                string socketNotification;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.stringBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);

                    socketNotification = Encoding.UTF8.GetString(state.buffer, 0, bytesRead).ToString();
                    Console.WriteLine("Desde string es: " + socketNotification);

                    treatNotification(socketNotification);
                }
                else
                {
                    Console.WriteLine("Ya no se está recibiendo datos");
                    // All the data has arrived; put it in response.  
                    if (state.stringBuilder.Length > 1)
                    {
                        response = state.stringBuilder.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Error[ReceiveCallback]: " + e.ToString());
            }
        }

        public void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CloseSocketConnection()
        {
            try
            {
                GlobalSocket.GlobalVarible.Shutdown(SocketShutdown.Both);
                GlobalSocket.GlobalVarible.Disconnect(true);
                if (GlobalSocket.GlobalVarible.Connected)
                {
                    Console.WriteLine($"Socket Connected");
                }
                else
                {
                    Console.WriteLine($"Socket Disconnected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[CloseSocketConnection][AsynchronousClient] {ex.Message}");
            }
            finally
            {
                GlobalSocket.GlobalVarible.Close();
            }
        }

        public void treatNotification(string socketNotification)
        {
            try
            {
                var jobject = JsonConvert.DeserializeObject<JObject>(socketNotification);

                if (jobject.ContainsKey("Agent"))
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
                        toast.ExpirationTime = DateTime.Now.AddSeconds(1);
                        toast.Dismissed += (senderT, args) => ToastNotificationManagerCompat.History.Clear();
                    });
                }
                else if (jobject.ContainsKey("CloseChat"))
                {
                    Console.WriteLine("\nEl id del chat es: " + jobject.Value<string>("chatId"));
                    TabPageChat chat = prueba.getTabChatByChatId(jobject.Value<string>("chatId"));
                    MessageBox.Show("El cliente ha abandonado la conversacion, \nSe cerrara el chat.");
                    chat.txtSendMessage.Visible = false;
                    chat.btnSendMessage.Visible = false;
                    //chat.btnCloseButton.PerformClick();
                }
                else if (jobject.ContainsKey("socketPort")) {
                    var port = jobject.Value<string>("socketPort");
                    Console.WriteLine("\nHola agente, tu puerto asignado por la API es:"+port);
                    new RestHelper().updateAgentActiveIp(GlobalSocket.currentUser.email, port.ToString());
                }
                else
                {
                    Models.Message notification = JsonConvert.DeserializeObject<Models.Message>(socketNotification);

                    Console.WriteLine("notificacion:" + notification);
                    //if (notification.platformIdentifier != "c")
                    //{
                    //    prueba.treatNotification(notification);
                    //}
                    //else
                    //{
                    //    webChat.treatNotification(notification);
                    //}

                    prueba.treatNotification(notification);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[treatNotification]: " + ex.ToString());
            }
        }

        #region Estos métodos no tienen ninguna referencias en el código considerar borrarlos
        //Metodos que no tienen ninguna referencia en el codigo
        public bool tabChatExits(string chatId)
        {
            bool resultado = false;
            try
            {
                #region Proceso original comentado
                ////Proceso original: buscaba por nombre en los controles de la referencia al Form 'Prueba'
                ////Control ctn = prueba.Controls["tabControlChats"].Controls[$"tabPageChat_{chatId}"];
                //Control ctn = prueba.Controls["tabControlChats"].Controls[$"tabPageChat_{chatId}"];//.Controls[$"panelControls_{chatId}"];//.Controls[$"txtSendMessage_{chatId}"];
                //if (ctn != null)
                //{
                //    Control ctnTxt = ctn.Controls[$"panelControls_{chatId}"].Controls[$"txtSendMessage_{chatId}"];
                //    var arrgeglo = chatWindow.tbControlChats.TabPages[1].Controls[5].Controls[0];
                //    var arrgeglotb = chatWindow.arrTabPageChat;
                //    resultado = true;
                //}
                #endregion

                //Proceso desde la instancia de chatWindow
                //for (int position = 0; position < chatWindow.arrTabPage.Length; position++)
                //{
                //    if (chatWindow.arrTabPage[position] != null && chatWindow.arrTabPage[position].Name == "tabPageChat_" + chatId)
                //        resultado = true;
                //}


                //////Proceso desde el chatWindowLocal dentro de Prueba()
                for (int position = 0; position < prueba.chatWindowLocal.arrTabPage.Count; position++)
                {
                    if (prueba.chatWindowLocal.arrTabPage[position] != null && prueba.chatWindowLocal.arrTabPage[position].Name == "tabPageChat_" + chatId)
                        resultado = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[validateActiveTabChat]: " + ex.ToString());
                resultado = false;
            }
            return resultado;
        }

        public void buildNewTabChat(string notification)
        {
            try
            {
                #region Proceso original comentado
                //Proceso original: Asignaba el chatId a la instancia del chatWindow y después disparaba el método de contrucción de controles en un hilo
                //chatWindow.chatId = notification;
                //Thread t = new Thread(chatWindow.addTabPage);
                //t.Start();
                #endregion

                //////Nuevo: disparar el evento desde la instancia del form
                //prueba.chatWindowLocal.tbControlChats = tbControlContainer;
                prueba.chatWindowLocal.chatId = notification;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[sendNewTabChatParams]: " + ex.ToString());
            }
        }

        public void inicializarChatWindow()
        {
            try
            {
                //Intentar creando un tabControls propio
                TabControl tabControlChats_2 = new TabControl();
                //prueba.Controls.Add(tabControlChats_2);
                tabControlChats_2.Name = "tabControlChats_2";
                tabControlChats_2.Tag = "tabControlChats_2";
                tabControlChats_2.Size = new Size(683, 488);
                tabControlChats_2.Location = new Point(913, 13);

                //chatWindow = new ChatWindow(tabControlChats_2);
                //chatWindow = new ChatWindow(this.tbControlContainer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[inicializarChatWindow]: " + ex.ToString());
            }
        }

        public void recoverActiveChats()
        {
            try
            {

                //prueba.recoverActiveChats();
                //prueba.treatNotification();

                Models.Message fakeNotification2 = new Models.Message();
                fakeNotification2.chatId = "238";
                fakeNotification2.platformIdentifier = "w";
                fakeNotification2.clientPlatformIdentifier = "whatsapp:+5214625950962";
                prueba.treatNotification(fakeNotification2);

                Models.Message fakeNotification1 = new Models.Message();
                fakeNotification1.chatId = "236";
                fakeNotification1.platformIdentifier = "w";
                fakeNotification1.clientPlatformIdentifier = "whatsapp:+5214621257826";
                prueba.treatNotification(fakeNotification1);
                //Thread.Sleep(700);

                // {"chatId": "236", "platformIdentifier": "w", "clientPlatformIdentifier": "whatsapp:+5214621257826"}
                // {"chatId": "236", "platformIdentifier": "w", "clientPlatformIdentifier": "whatsapp:+5214621257826"}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[AsynchronousClient_recoverActiveChats]: " + ex.ToString());
            }
        }
        #endregion

    }
}
