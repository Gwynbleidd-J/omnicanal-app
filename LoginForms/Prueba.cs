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
using LoginForms.Models;
using LoginForms.Utils;
using RestSharp;

namespace LoginForms
{
    public partial class Prueba : Form
    {
        public int contPfrueba = 1;
        public Json jsonRecoveredChats { get; set; }
        public TabControl tbCtrl = new TabControl();
        RestHelper restHelper = new RestHelper();
        public ChatWindow chatWindowLocal { get; set; }
        public Prueba()
        {
            InitializeComponent();
            chatWindowLocal = new ChatWindow(this.tabControlChats);
            //
        }
        private void Prueba_Load(object sender, EventArgs e)
        {
            try
            {
                //recoverActiveChats();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[Prueba_Load]: " + ex.ToString());
            }
        }
        public async void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnSendMessageTemporal = sender as Button;
                String chatId = btnSendMessageTemporal.Name.Split('_')[1];
                AsynchronousClient client = new AsynchronousClient(this.richTextBox1, this, this, this);

                var arr = this.tabControlChats.TabPages;
                var arreglo = chatWindowLocal.arrTabPageChat;
                await restHelper.SendMessage("Mensage de prueba desde el chat: " + chatId, "188", "whatsapp:+5214621257826", "w");
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
                //chatWindowLocal.chatId = chatId;
                chatWindowLocal.chatGenerals = chatGenerals;
                Thread tBuildNewTabChat = new Thread(chatWindowLocal.addTabPage);
                tBuildNewTabChat.Start();                
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
                chatWindowLocal.chatGenerals = chatGenerals;
                Thread tBuildNewMessagesLabels = new Thread(chatWindowLocal.threadAddNewMessages);
                tBuildNewMessagesLabels.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[buildNewMessagesLabels]: " + ex.ToString());
            }
        }
        
        public async void recoverActiveChats()
        {
            try
            {
                string recoveredChatsFromAPI = await restHelper.RecoverActiveChats("10");
                jsonRecoveredChats = JsonConvert.DeserializeObject<Json>(recoveredChatsFromAPI);
                //Models.Message fakeNotification = new Models.Message();                
                //MessageBox.Show("Mensajes activos recuperados: " + recoveredChats);

                foreach (Chat chatGenerals in jsonRecoveredChats.data.chats)
                { 
                    Models.Message fakeNotification = new Models.Message();
                    fakeNotification.chatId = chatGenerals.id;
                    fakeNotification.platformIdentifier = chatGenerals.platformIdentifier;
                    fakeNotification.clientPlatformIdentifier = chatGenerals.clientPlatformIdentifier;
                    treatNotification(fakeNotification);

                    Thread.Sleep(5000);
                }
                //if (jsonRecoveredChats.data.chats[0] != null)
                //{
                //    Models.Message fakeNotification1 = new Models.Message();
                //    fakeNotification1.chatId = jsonRecoveredChats.data.chats[0].id;
                //    fakeNotification1.platformIdentifier = jsonRecoveredChats.data.chats[0].platformIdentifier;
                //    fakeNotification1.clientPlatformIdentifier = jsonRecoveredChats.data.chats[0].clientPlatformIdentifier;
                //    treatNotification(fakeNotification1);
                //    Thread.Sleep(5000);
                //}
                //Thread.Sleep(5000);
                //if (jsonRecoveredChats.data.chats[1] != null)
                //{
                //    Models.Message fakeNotification2 = new Models.Message();
                //    fakeNotification2.chatId = jsonRecoveredChats.data.chats[1].id;
                //    fakeNotification2.platformIdentifier = jsonRecoveredChats.data.chats[1].platformIdentifier;
                //    fakeNotification2.clientPlatformIdentifier = jsonRecoveredChats.data.chats[1].clientPlatformIdentifier;
                //    treatNotification(fakeNotification2);
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[recoverActiveChats]: " + ex.ToString());
            }
        }
        #endregion

    }
}
