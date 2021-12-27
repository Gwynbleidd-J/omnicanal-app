using LoginForms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Shared;
using LoginForms.Utils;
using RestSharp;
using System.Net.NetworkInformation;

namespace LoginForms
{
    public partial class Prueba : Form
    {
        //public int contPfrueba = 1;
        public Json jsonRecoveredChats { get; set; }
        public TabControl tbCtrl = new TabControl();
        RestHelper restHelper = new RestHelper();
        TabPageChat pageChat = new TabPageChat();
        public Label lblLastMessageId { get; set; }
        public Label lblChatId { get; set; }
        public Label lblClientPlatformIdentifier { get; set; }
        public Label lblLastHeighUsed { get; set; }
        public Label lblPlatformIdentifier { get; set; }
        public Panel pnlMessages { get; set; }
        public Label lastLabel { get; set; }
        public TextBox txtSendMessage { get; set; }
        public Button btnSendMessage { get; set; }
        public Button btnCloseButton { get; set; }
        public TextBox txtNumber { get; set; }
        public Form frmNumberToSend { get; set; }
        public ChatWindow chatWindowLocal { get; set; }
        public TabPage tbPage { get; set; }
        AsynchronousClient client = new AsynchronousClient();


        public Prueba()
        {

            InitializeComponent();
            chatWindowLocal = new ChatWindow(this.tabControlChats);
            tbPage = new TabPage();
            
        }


        private void Prueba_Load(object sender, EventArgs e)
        {
            try
            {
                string agentId = GlobalSocket.currentUser.ID;
                recoverActiveChats(agentId);

                NetworkChange.NetworkAvailabilityChanged += new
                NetworkAvailabilityChangedEventHandler(AddressChangedCallback);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[Prueba_Load]: " + ex.ToString());
            }
        }

        public void AddressChangedCallback(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable == true)
            {
                label1.Text = "Se ha recuperado la conexion a internet";
                client.Connect();
            }
            else
            {
                label1.Text = "Se ha perdido la conexion a internet";
            }
        }

            /*
             * Este botón ya no se utiliza, necesito preguntarle a Juan Carlos que pasa con este metodo
             */
            public async void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnSendMessageTemporal = sender as Button;
                String chatId = btnSendMessageTemporal.Name.Split('_')[1];
                AsynchronousClient client = new AsynchronousClient(this.richTextBox1, this, this, this);

                var arr = this.tabControlChats.TabPages;
                var arreglo = chatWindowLocal.arrTabPageChat;
                //await restHelper.SendMessage("Mensage de prueba desde el chat: " + chatId, "188", "whatsapp:+5214621257826", "w");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[btnSend_Click]: " + ex.Message);
            }
        }

        #region Métodos comentados, comprobar que ya no se usan en esta versión estable y quitarlos
        //private void txtNewTabChat_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Thread threadCreateNewTabChat = new Thread(invokeCreateNewTabChat);
        //        threadCreateNewTabChat.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error[txtNewTabChat_TextChanged]: " + ex.Message);
        //        MessageBox.Show("Error[txtNewTabChat_TextChanged]: " + ex.Message);
        //    }
        //}
        //public void invokeCreateNewTabChat()
        //{
        //    try
        //    {
        //        if (InvokeRequired)
        //            Invoke(new Action(() => createNewTabChat()));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("[invokeCreateNewTabChat]" + ex.Message);
        //    }
        //}
        //public void createNewTabChat()
        //{
        //    try
        //    {

        //        //MessageBox.Show("El contenido de textNuevoTabChat cambió! Hay que construir un nuevo Tab para el chat: " + txtNewTabChat.Text);
        //        //Json jsonChat = JsonConvert.DeserializeObject<Json>(textNuevoTabChat.Text.ToString());    ////Ya no es necesario usar Json´s 

        //        //Si no existe, entonces construir en el tabControlChats un nuevo tabPageChat_[chatId] 
        //        //con un panel panelMessages_[chatId] y sus controles correspondientes
        //        TabPage newTabPageChat = new TabPage();
        //        newTabPageChat.Name = "tabPageChat_" + txtNewTabChat.Text.ToString();
        //        newTabPageChat.Text = "Chat: " + txtNewTabChat.Text.ToString();
        //        newTabPageChat.Size = new Size(675, 462);
        //        tabControlChats.Controls.Add(newTabPageChat);

        //        //Agregar el panel para las etiquetas de los mensajes
        //        Panel newPanelMessage = new Panel();
        //        newPanelMessage.Name = $"panelMessages_{txtNewTabChat.Text.ToString()}";
        //        newPanelMessage.BackColor = Color.Gray;
        //        newPanelMessage.Size = new Size(640, 335);
        //        newPanelMessage.Location = new Point(18, 8);
        //        //newTabPageChat.Controls.Add(newPanelMessage); //al parecer no deja manipular controles nuevos
        //        Control ctnNewTabPageChat = this.Controls["tabControlChats"].Controls[$"tabPageChat_{txtNewTabChat.Text.ToString()}"];
        //        ctnNewTabPageChat.Controls.Add(newPanelMessage);

        //        //Agregar el panel de los controles generales del chat, para envío de mensajes 
        //        Panel newPanelControls = new Panel();
        //        newPanelControls.Name = $"panelControls_{txtNewTabChat.Text.ToString()}";
        //        newPanelControls.Size = new Size(640, 81);
        //        newPanelControls.Location = new Point(18, 375);
        //        newTabPageChat.Controls.Add(newPanelControls);

        //        //Agregar al textbox para el envío de mensajes
        //        TextBox newTxtSendMessage = new TextBox();
        //        newTxtSendMessage.Name = $"txtSendMessage_{txtNewTabChat.Text.ToString()}";
        //        newTxtSendMessage.Size = new Size(523, 20);
        //        newTxtSendMessage.Location = new Point(3, 55);
        //        newPanelControls.Controls.Add(newTxtSendMessage);

        //        //Agregar al textbox para la recepción de mensajes del chat
        //        TextBox newtxtChatHistoric = new TextBox();
        //        newtxtChatHistoric.Name = $"txtSendMessage_{txtNewTabChat.Text.ToString()}";
        //        newtxtChatHistoric.Size = new Size(197, 20);
        //        newtxtChatHistoric.Location = new Point(238, 13);
        //        newPanelControls.Controls.Add(newtxtChatHistoric);

        //        //Agregar una etiqueta en la que se estará guardando el registro del último id de mensage cargado en el panel
        //        //(Este dato ayuda a mandarlo en las siguientes peticiones del chat y solo traer los msg que falten de cargar)
        //        Label newLblLastMessageId = new Label();
        //        newLblLastMessageId.Name = $"lblLastMessageId{txtNewTabChat.Text.ToString()}";
        //        newPanelControls.Controls.Add(newLblLastMessageId);

        //        //Agregar al boton para el envío de mensajes
        //        Button newBtnSend = new Button();
        //        newBtnSend.Name = $"newBtnSend_{txtNewTabChat.Text.ToString()}";
        //        newBtnSend.Text = "Enviar";
        //        newBtnSend.Size = new Size(75, 23);
        //        newBtnSend.Location = new Point(549, 55);
        //        newPanelControls.Controls.Add(newBtnSend);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error[createNewTabChat]: " + ex.Message);
        //        MessageBox.Show("Error[createNewTabChat]: " + ex.Message);
        //    }
        //}

        //public delegate void throwThreadDelegate(string chatId, string chatMessagestHistoric);
        //public void throwThread()
        //{
        //    try
        //    {
        //        Thread threadAddLabelMessages = new Thread(invokeAddLabelMessages);
        //        threadAddLabelMessages.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public void invokeAddLabelMessages()
        //{
        //    try
        //    {
        //        //Cuando se dispara dentro del form InvokeRequired es true, desde otra clase es false
        //        //if (InvokeRequired)
        //        //    Invoke(new Action(() => addLabelMessages(chatId, chatMessagestHistoric)));

        //        if (InvokeRequired)
        //            Invoke(new Action(() => addLabelMessages("3", "Hola desde invokeAddLabelMessages")));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("[invokeAddLabelMessages]" + ex.Message);
        //    }
        //}

        ////NOTA: este proceso està preparado para recibir un històrico del chat y pintarlo en el panel del chat especìfico,
        ////para efectos de probar la funcionalidad de los threads desde otra clase sin escribir en un textBox, se comentan los generales del ciclo For.
        ////{Por ahora solo imprimirà una etiqueta}
        ////Una vez logrado el llamado del proceso desde otra clase, poder enviar en string o Json el històrico de forma normal
        //public void addLabelMessages(string chatId, string chatMessagestHistoric)
        //{
        //    try
        //    {
        //        //private void addMessageLabel(string messageText, string messageId)
        //        Json jsonChatMessagestHistoric = JsonConvert.DeserializeObject<Json>(chatMessagestHistoric);


        //        //for (int i = jsonChatMessagestHistoric.data.chat.Count - 1; i >= 0; i--)
        //        //{
        //        //Iniciio de construcción de la etiqueta dinámica
        //        Label newLabelMessage = new Label();
        //        newLabelMessage.Width = 140;
        //        newLabelMessage.Height = 20;
        //        //newLabelMessage.Top = usedHeight + 20;
        //        //newLabelMessage.Name = jsonChatMessagestHistoric.data.chat[i].chatId + "_" + jsonChatMessagestHistoric.data.chat[i].id;
        //        newLabelMessage.BackColor = Color.LightGray;
        //        //lastLabel = newLabelMessage.Name;
        //        newLabelMessage.Text = "Prueba " + chatMessagestHistoric;

        //        ////Validación de la alineación de la etiqueta según el Transmitter  <-- cliente / agente -->
        //        //if (jsonChatMessagestHistoric.data.chat[i].transmitter == "c")
        //        //{
        //        //    newLabelMessage.Text = "Cliente: " + jsonChatMessagestHistoric.data.chat[i].text;
        //        //    newLabelMessage.Left = 1;
        //        //}
        //        //else
        //        //{
        //        //    newLabelMessage.Text = "Tú: " + jsonChatMessagestHistoric.data.chat[i].text;
        //        //    newLabelMessage.Left = 350;
        //        //}

        //        //usedHeight += newLabelMessage.Height;
        //        Control ctnPanelMessages = tabControlChats.Controls["tabPageChat_" + chatId].Controls["panelMessages_" + chatId];
        //        ctnPanelMessages.Controls.Add(newLabelMessage);
        //        //ctnPanelMessages.ScrollControlIntoView(newLabelMessage);
        //        //}


        //        //tabPageChat_1.Text = "Chat: " + jsonChatMessagestHistoric.data.chat[0].chatId;
        //        //Control ctn = panelMessages.Controls[lastLabel];

        //        //MessageBox.Show("Último msg recibido en: " + ctn.Name + ", decía: " + ctn.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error[addLabelMessages]: " + ex.Message);
        //    }
        //}

        //public void procesoNuevoTab()
        //{
        //    try
        //    {
        //        Thread threadCreateNewTabChat = new Thread((invokeAddNewTabPage));
        //        threadCreateNewTabChat.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error[procesoTabChatExits]: " + ex.ToString());
        //    }
        //}
        //public void invokeAddNewTabPage()
        //{
        //    try
        //    {
        //        //if (this.tabControlChats.InvokeRequired)
        //        //    this.tabControlChats.Invoke(new Action(() => validarNuevoTab()));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("[invokeCreateNewTabChat]" + ex.Message);
        //    }
        //} 
        #endregion

        #region Métodos que si estoy usando ya

        public void treatNotification(Models.Message newNotification) 
        {
            try
            {
                if (!tabChatExits(newNotification.chatId))
                {
                    buildNewTabChat(newNotification);
                    //Thread.Sleep(100);
                    Thread.CurrentThread.Join(50);
                    buildNewMessagesLabels(newNotification);
                }
                else
                {
                    buildNewMessagesLabels(newNotification);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[validarNuevoTab]: " + ex.ToString());
            }
        }

        //public void buildExistingTabChat(Models.Message data) {
        //    try
        //    {
        //        //buildNewTabChat(data);
        //        //buildNewMessagesLabels(data);

        //        treatNotification(data);

        //    }
        //    catch (Exception _e)
        //    {
        //        throw _e;
        //    }
        //}
         
        public bool tabChatExits(string chatId)
        {
            bool resultado = false;
            try
            {
                for (int position = 0; position < chatWindowLocal.arrTabPageChat.Count; position++)
                    if (chatWindowLocal.arrTabPageChat[position] != null && chatWindowLocal.arrTabPageChat[position].tbPage.Name == "tabPageChat_" + chatId)
                        resultado = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[tabChatExits]: " + ex.ToString());
                resultado = false;
            }
            return resultado;
        }

        public void buildNewTabChat(Models.Message chatGenerals)
        {
            try
            { 
                //chatWindowLocal.chatGenerals = chatGenerals;
                Thread tBuildNewTabChat = new Thread(chatWindowLocal.addTabPage);
                tBuildNewTabChat.Start(chatGenerals);

                //Comprobar funcionalidad de la siguiente linea
                tBuildNewTabChat.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[buildNewTabChat]: " + ex.ToString());
            }
        }
         

        public void buildNewMessagesLabels(Models.Message chatGenerals)
        {
            try
            {
                
                //chatWindowLocal.chatGenerals = chatGenerals;
                Thread tBuildNewMessagesLabels = new Thread(chatWindowLocal.threadAddNewMessages);
                
                tBuildNewMessagesLabels.Start(chatGenerals);

                //Comprobar funcionalidad de la siguiente linea
                tBuildNewMessagesLabels.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[buildNewMessagesLabels]: " + ex.ToString());
            }
        }

        /* Checar para mañana:
         * Método del cual se van a cargar los mensajes que se tienen pendientes, si por alguna razón
         * la aplicacion se llegara a cerrar de manera inesperada, preguntarle a Juan Carlos como planeaba
         * utilizar este método.
         * 
         * 
         */
        public async void recoverActiveChats(string agentId)
        {
            //preguntarle a Juan Carlos que pasa con el chatId
            //y tambien como pensaba implementar este metodo
            //el problema está en como mandar llamar el chatId en esta clase
            //Ver si está implementacion del chatId funciona
            //sino pensar como cambiarla
            try
            {
                //agentId = GlobalSocket.currentUser.activeIp;
                //string recoveredChatsFromAPI = await restHelper.RecoverActiveChats(chat.chatId, agentId);
                string recoveredChatsFromAPI = await restHelper.RecoverActiveChats(agentId);
                
                jsonRecoveredChats = JsonConvert.DeserializeObject<Json>(recoveredChatsFromAPI);
                //Models.Message fakeNotification = new Models.Message();                
                //MessageBox.Show("Mensajes activos recuperados: " + recoveredChats);

                foreach(LoginForms.Models.Chat chatGenerals in jsonRecoveredChats.data.chats)
                {
                    Models.Message fakeNotification = new Models.Message();
                    fakeNotification.chatId = chatGenerals.id;
                    fakeNotification.platformIdentifier = chatGenerals.platformIdentifier;
                    fakeNotification.clientPlatformIdentifier = chatGenerals.clientPlatformIdentifier;
                    treatNotification(fakeNotification);

                    chatWindowLocal.firstRecoveredChatsLoading = true;

                    //Thread.Sleep(9000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[recoverActiveChats]: " + ex.ToString());
            }
        }
        #endregion

        private void btnBeginChat_Click(object sender, EventArgs e)
        {
            try
            {
                frmNumberToSend = new Form
                {
                    Name = $"numberToSend",
                    ControlBox = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Tag = $"NumberToSend",
                    Size = new Size(400, 280),
                    Text = $"Ingresa un número",
                    AutoSize = false,
                    AutoSizeMode = AutoSizeMode.GrowOnly
                };


                Label lblInfo = new Label
                {
                    Name = $"lblInfo",
                    AutoSize = true,
                    Text = $"Escribe el número al cual quieres mandar el mensaje",
                    Font = new Font("Microsoft Sans Serif", 9f),
                    Location = new Point(15, 25)  
                };

                txtNumber = new TextBox
                {
                    Name = $"txtNumber",
                    Size = new Size(170, 30),
                    Location = new Point(105, 70),
                    Text = "4621929111"
                };

                Button btnSaveNumber = new Button
                {
                    Name = $"btnSaveNumber",
                    Text = $"Aceptar",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Microsoft Sans Serif", 9f),
                    Size = new Size(85, 39),
                    Location = new Point(150, 110)
                };
                frmNumberToSend.Controls.AddRange(new Control[] { lblInfo, txtNumber, btnSaveNumber });

                txtNumber.KeyPress += (w, q) =>
                {
                    if (!char.IsControl(q.KeyChar) && !char.IsDigit(q.KeyChar))
                    {
                        q.Handled = true;
                        MessageBox.Show("Sólo puedes introducir números", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                    if(q.KeyChar == (int)Keys.Enter)
                    {
                        setNumber();
                        buildChatForAgent();
                    }
                };

                btnSaveNumber.Click += (s, a) =>
                {
                    setNumber();
                    buildChatForAgent();
                };

                frmNumberToSend.ShowDialog();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error [btnBeginChat][Prueba] {ex.Message}");
            }
        }

        private void setNumber()
        {
            if (!string.IsNullOrEmpty(txtNumber.Text))
            {
                string number =$"{txtNumber.Text}";
                GlobalSocket.numberToSend = number;
                Console.WriteLine($"Número teléfonico: {GlobalSocket.numberToSend}");
                frmNumberToSend.Dispose();
            }
            else
            {
                MessageBox.Show("Introduce un número", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public TabPageChat getTabChatByChatId(string chatId)
        {
            try
            {
                TabPageChat chat = new TabPageChat();
                string name = "";
                string searchedName = "tabPageChat_" + chatId;

                for (int position = 0; position < chatWindowLocal.arrTabPageChat.Count; position++)
                {
                    name = chatWindowLocal.arrTabPageChat[position].tbPage.Name.ToString();
                    var sTabPAge = chatWindowLocal.arrTabPageChat[position];
                    Console.WriteLine("Comparando "+name +" y " +searchedName);

                    if (sTabPAge != null && name == searchedName)
                    {
                        chat = chatWindowLocal.arrTabPageChat[position];
                    }
                }

                return chat;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[tabChatExits]: " + ex.ToString());
                return null;
            }
        }

        private async void buildChatForAgent()
        {
            #region Proceso de prueba
            //Llamar
            //try
            //{
            //    lblLastMessageId = new Label();
            //    lblClientPlatformIdentifier = new Label();
            //    lblLastHeighUsed = new Label();
            //    lblPlatformIdentifier = new Label();
            //    lblChatId = new Label();
            //    pnlMessages = new Panel();
            //    txtSendMessage = new TextBox();
            //    btnSendMessage = new Button();
            //    btnCloseButton = new Button();

            //    TabPage tbPage = new TabPage
            //    {
            //        Name = $"tabPageChat_Prueba",//{chatId}
            //        Tag = $"tabPageChat_ Prueba",//{chatId}
            //        Text = $"Chat Agente: #",
            //        Size = new Size(675, 462)
            //    };
            //    //tabControlChats.Controls.Add(tbPage);
            //    tabControlChats.TabPages.Add(tbPage);
            //    //Agregar al textbox para el envío de mensajes
            //    lblLastHeighUsed.Name = $"lblLastHeighUsed_";//{chatId}
            //    lblLastHeighUsed.Tag = $"lblLastHeighUsed_";//{chatId}
            //    lblLastHeighUsed.Text = "0";
            //    lblLastHeighUsed.Visible = false;
            //    //tbPage.Controls.Add(lblLastHeighUsed); 

            //    //Agregar las etiquetas que nos ayudarán a almacenar información gral del chat para la recepción de mensajes del chat 
            //    lblLastMessageId.Name = $"lblLastMessageId_";//{chatId}
            //    lblLastMessageId.Tag = $"lblLastMessageId_";//{chatId}
            //    lblLastMessageId.Text = "1";
            //    lblLastMessageId.Visible = false;
            //    //tbPage.Controls.Add(lblLastMessageId); 

            //    lblChatId.Name = $"lblChatId_";//{chatId}
            //    lblChatId.Tag = $"lblChatId_";//{chatId}
            //    lblChatId.Text = ""; //chatId
            //    lblChatId.Visible = false;

            //    lblClientPlatformIdentifier.Name = $"lblClientPlatformIdentifier_";//{chatId}
            //    lblClientPlatformIdentifier.Tag = $"lblClientPlatformIdentifier_";//{chatId}
            //    lblClientPlatformIdentifier.Text = "clientPlatformIdentifier";
            //    lblClientPlatformIdentifier.Visible = false;
            //    //lblNombreCliente.Text = "etiqueta de cliente";
            //    //tbPage.Controls.Add(lblNombreCliente); 

            //    lblPlatformIdentifier.Name = $"lblPlatformIdentifier_";//{chatId}
            //    lblPlatformIdentifier.Tag = $"lblPlatformIdentifier_";//{chatId}
            //    lblPlatformIdentifier.Text = "platformIdentifier";
            //    lblPlatformIdentifier.Visible = false;
            //    //tbPage.Controls.Add(lblPlatformIdentifier); 

            //    pnlMessages.Name = $"lblPlatformIdentifier_";//{chatId}
            //    pnlMessages.Tag = $"lblPlatformIdentifier_";//{chatId}
            //    pnlMessages.Size = new Size(640, 335);
            //    pnlMessages.Location = new Point(18, 8);
            //    pnlMessages.BackColor = Color.FromArgb(236, 229, 221);
            //    pnlMessages.AutoScroll = true;
            //    //tbPage.Controls.Add(pnlMessages);

            //    //panelControl.chatId = chatId;
            //    //panelControl.buildPanel(); 


            //    //prueba agregando los controles a nivel TabPage
            //    //Agregar al textbox para el envío de mensajes 
            //    txtSendMessage.Name = $"txtSendMessage_";//{chatId}
            //    txtSendMessage.Tag = $"txtSendMessage_";//{chatId}
            //    txtSendMessage.Size = new Size(480, 20);
            //    txtSendMessage.Location = new Point(10, 432);
            //    txtSendMessage.Text = "Buen día, soy su agente a cargo, ¿En qué le puedo ayudar?";

            //    txtSendMessage.KeyPress += async (s, e) => {
            //        try
            //        {
            //            if ((int)e.KeyChar == (int)Keys.Enter)
            //                await pageChat.sendMessageFromPanelControl();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("Error[txtSendMessage.KeyPress]: " + ex.Message);
            //        }

            //    };

            //    //Agregar al boton para el envío de mensajes 
            //    btnSendMessage.Name = $"btnSendMessage_";//{chatId}
            //    btnSendMessage.Tag = $"btnSendMessage_";//{chatId}
            //    btnSendMessage.Text = "Enviar";
            //    btnSendMessage.Size = new Size(82, 23);
            //    btnSendMessage.Location = new Point(498, 429);

            //    btnSendMessage.Click += async (s, e) => {
            //        try
            //        {
            //            if (!string.IsNullOrEmpty(txtSendMessage.Text.ToString()))
            //            {
            //                await pageChat.sendMessageFromPanelControl();
            //            }

            //            //if (await sendMessageFromPanelControl())
            //            //    askForNewMessages();

            //            //MessageBox.Show("Mensages nuevos cargados correctamente!");
            //            //addLabelMessages();
            //            //if(!string.IsNullOrEmpty(resulAskForNewMessages))

            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("Error[btnSendMessage.Click]: " + ex.Message);
            //        }
            //    };


            //    btnCloseButton.Name = $"btnCloseButton_";//{chatId}
            //    btnCloseButton.Tag = $"btnCloseButton_";//{chatId}
            //    btnCloseButton.Text = $"Cerrar Chat";
            //    btnCloseButton.Size = new Size(86, 23);
            //    btnCloseButton.Location = new Point(586, 429);

            //    btnCloseButton.Click += async (s, e) =>
            //    {
            //        if (await pageChat.closeChat())
            //        {
            //            NetworkCategories networkCategories = new NetworkCategories("string");//chatId
            //            networkCategories.ShowDialog();
            //            pageChat.removeTabChat();

            //        }
            //    };

            //    tbPage.Controls.Add(lblLastHeighUsed);
            //    tbPage.Controls.Add(lblLastMessageId);
            //    tbPage.Controls.Add(lblClientPlatformIdentifier);
            //    tbPage.Controls.Add(lblPlatformIdentifier);
            //    tbPage.Controls.Add(lblChatId);
            //    tbPage.Controls.Add(pnlMessages);
            //    //tbPage.Controls.Add(panelControl.pnlControls);
            //    tbPage.Controls.Add(txtSendMessage);
            //    tbPage.Controls.Add(btnSendMessage);
            //    tbPage.Controls.Add(btnCloseButton);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine($"Error [buidChatForAgent][Prueba] {ex.Message}");
            //}
            #endregion
            try 
            {
             await restHelper.newEmptyChat("","","","","");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[buildChatForAgent] {ex.Message}");
            }
        }
    }
}
