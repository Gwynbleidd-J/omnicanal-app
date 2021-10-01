using LoginForms.Models;
using LoginForms.Shared;
using LoginForms.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class WebChat : Form
    {

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

        public WebChat()
        {
            InitializeComponent();
            chatWindowLocal = new ChatWindow(this.tabControlWebChats);
            tbPage = new TabPage();
        }


        private void WebChat_Load(object sender, EventArgs e)
        {
            try
            {
                string agentId = GlobalSocket.currentUser.ID;
                //recoverActiveChats(agentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[Prueba_Load]: " + ex.ToString());
            }
        }

        //private static readonly string baseUrl = ConfigurationManager.AppSettings["IpServidor"];


        #region Principal

        public void treatNotification(Models.Message newNotification)
        {
            try
            {
                if (!tabChatExits(newNotification.chatId))
                {
                    buildNewTabChat(newNotification);
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

        public void buildNewTabChat(Models.Message chatGenerals)
        {
            try
            {
                //chatWindowLocal.chatGenerals = chatGenerals;
                Thread tBuildNewTabChat = new Thread(chatWindowLocal.addTabPage);
                tBuildNewTabChat.Start(chatGenerals);

                Console.WriteLine("Empezando a construir el tab chat desde web chat");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[buildNewTabChat]: " + ex.ToString());
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

        public void buildNewMessagesLabels(Models.Message chatGenerals)
        {
            try
            {

                //chatWindowLocal.chatGenerals = chatGenerals;
                Thread tBuildNewMessagesLabels = new Thread(chatWindowLocal.threadAddNewMessages);
                tBuildNewMessagesLabels.Start(chatGenerals);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[buildNewMessagesLabels]: " + ex.ToString());
            }
        }

        //public async void recoverActiveChats(string agentId)
        //{
        //    //preguntarle a Juan Carlos que pasa con el chatId
        //    //y tambien como pensaba implementar este metodo
        //    //el problema está en como mandar llamar el chatId en esta clase
        //    //Ver si está implementacion del chatId funciona
        //    //sino pensar como cambiarla
        //    try
        //    {
        //        //agentId = GlobalSocket.currentUser.activeIp;
        //        //string recoveredChatsFromAPI = await restHelper.RecoverActiveChats(chat.chatId, agentId);
        //        string recoveredChatsFromAPI = await restHelper.RecoverActiveChats(agentId);

        //        jsonRecoveredChats = JsonConvert.DeserializeObject<Models.Json>(recoveredChatsFromAPI);
        //        //Models.Message fakeNotification = new Models.Message();                
        //        //MessageBox.Show("Mensajes activos recuperados: " + recoveredChats);

        //        foreach (LoginForms.Models.Chat chatGenerals in jsonRecoveredChats.data.chats)
        //        {
        //            Models.Message fakeNotification = new Models.Message();
        //            fakeNotification.chatId = chatGenerals.id;
        //            fakeNotification.platformIdentifier = chatGenerals.platformIdentifier;
        //            fakeNotification.clientPlatformIdentifier = chatGenerals.clientPlatformIdentifier;
        //            treatNotification(fakeNotification);

        //            //Thread.Sleep(9000);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error[recoverActiveChats]: " + ex.ToString());
        //    }
        //}

        #endregion



    }
}
