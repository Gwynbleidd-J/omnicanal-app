using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Models;
using System.Reflection;
using System.Drawing;
using LoginForms.Properties;

namespace LoginForms.Utils
{
    public class ChatWindow
    {
        #region atributos
        public string chatId { get; set; }
        public Models.Message chatGenerals { get; set; }
        public int chatsForAgent;
        public int nextTabPagePosition = 0;
        public TabControl tbControlChats { get; set; } 
        public List<TabPageChat> arrTabPageChat{ get; set; } 
        public List<TabPage> arrTabPage { get; set; } 
        public TabPageChat tbPageChat { get; set; }
        public bool MinThread = false;
        public bool firstRecoveredChatsLoading = false;
        public static int contadorActiveChats = 0;


        #endregion
        #region Métodos
        public ChatWindow(TabControl tabControlFromForm)
        {
            try
            { 
                chatsForAgent = 5;
                //Control tbControl = frmPrueba.Controls["tabControlChats"];
                if (tabControlFromForm != null)
                    tbControlChats = tabControlFromForm;

                //Codigo para eliminar la primera pestaña
                tbControlChats.TabPages.Remove(tbControlChats.TabPages[0]);

                arrTabPageChat = new List<TabPageChat>();
                arrTabPage = new List<TabPage>();
                tbPageChat = new TabPageChat();

                //Console.WriteLine("\n\nEsto es lo que trae el constructor ChatWindow:" +tabControlFromForm+"\n\n Y esto trae tbControlChats:"+tbControlChats);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error[construct ChatWindow]: " + ex.Message);
            }
        }
        public void addTabPage(Object temp)
        {
            try
            {
                Thread threadCreateNewTabChat = new Thread((invokeAddTabPage));    
                threadCreateNewTabChat.Start(temp);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error[addTabPage]: " + ex.Message);
            }
        }
          
        public void invokeAddTabPage(Object temp)
        {
            try
            {
                if (tbControlChats.InvokeRequired)
                {
                    var chat = (Models.Message)temp;
                    if (!MinThread)
                    {
                        MinThread = true;
                        Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    }
                    tbControlChats.Invoke(new Action(() => threadAddTabPage(chat)));
                }
                else
                {
                    var chat = (Models.Message)temp;
                    threadAddTabPage(chat);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[invokeAddTabPage]" + ex.Message);
            }
        }

        public void threadAddTabPage(Models.Message temp)
        {
            try
            { 
                tbPageChat = new TabPageChat();
                //tbPageChat = new TabPageChat();
                TabPageChat tbPageChatTemporal = null;
                tbPageChatTemporal = new TabPageChat();
                //tbPageChatTemporal.Name = "tbTemporal_" + chatId;
                //tbPageChatTemporal.Text = chatId;
                
                tbPageChat.chatId = temp.chatId;
                tbPageChat.platformIdentifier = temp.platformIdentifier;
                tbPageChat.clientPlatformIdentifier = temp.clientPlatformIdentifier;
                //tbControlChats.Controls.Add(tbPageChat.tbPage);

                Console.WriteLine("\nSe ha creado el siguiente objeto tbPageChat:" +  tbPageChat.ToString());

                tbPageChat.addEmptyControls();
                //tbPageChat.threadBuildTabPage();
                tbPageChat.threadBuildTabPage();
                tbPageChatTemporal = tbPageChat;

                Console.WriteLine("Antes de agregar el tab, la cuenta de controles era:" +tbControlChats.Controls.Count +"\n y el arreglo:" +arrTabPage.Count);

                //tbControlChats.Controls.Add(tbPageChatTemporal);
                //tbControlChats.TabPages.Add(tbPageChatTemporal.tbPage);
                tbControlChats.Controls.Add(tbPageChatTemporal.tbPage);
                //Intentar que el tab creado sea el que se seleccione en ese momento
                tbControlChats.SelectedTab = tbPageChatTemporal.tbPage;

                //arrTabPage[nextTabPagePosition] = tbPageChatTemporal;
                arrTabPageChat.Add(tbPageChatTemporal);

                Console.WriteLine("Despues de agregar el tab, la cuenta de controles es:" + tbControlChats.Controls.Count + "\n y el arreglo:" + arrTabPageChat.Count);

                addImageTab(tbControlChats, tbPageChatTemporal);
                //nextTabPagePosition++;
                //tbControlChats.Controls.Add(arrTabPage[nextTabPagePosition]);

                FormPrincipal frmP = (FormPrincipal)Application.OpenForms["FormPrincipal"];
                contadorActiveChats += 1;
                frmP.lblChatsActual.Text = "Chats Activos:" +contadorActiveChats;
                frmP.lblChatsActual.ForeColor = Color.Red;


                //TabPage n = new TabPage();
                //n.Text = "tryChat: " + chatId;
                //n.BackColor = System.Drawing.Color.LightGray;
                //tbControlChats.Controls.Add(n); 

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[threadAddTabPage]: " + ex.Message);    
            }
            //return tbPage;
        }

        public void addImageTab(TabControl tabControl, TabPageChat tabPageChat) {
            ImageList iconList = new ImageList();
            iconList.TransparentColor = Color.White;
            iconList.ColorDepth = ColorDepth.Depth32Bit;
            iconList.ImageSize = new Size(40,40);

            var telegram = Resources.telegram;
            var whatsApp = Resources.whatsapp;

            iconList.Images.Add(telegram);
            iconList.Images.Add(whatsApp);

            //iconList.Images.Add(Image.FromFile("C:/Users/KODE/Downloads/telegram.png"));
            //iconList.Images.Add(Image.FromFile("C:/Users/KODE/Downloads/whatsapp.png"));
            tabControl.ImageList = iconList;

            if (tabPageChat.platformIdentifier == "t")
            {
                tabPageChat.tbPage.ImageIndex = 0;
            }
            else if (tabPageChat.platformIdentifier == "w")
            {
                tabPageChat.tbPage.ImageIndex = 1;
            }
        
        }

        public void threadAddNewMessages(object temp)
        {
            try
            {
                    Thread threadAddNewMessages = new Thread((invokeAddNewMessages));
                    threadAddNewMessages.Start(temp);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error[ChatWindow ThreadAddNewMessages]: " + ex.Message); 
            } 
        } 
        public void invokeAddNewMessages(object temp)
        {
            try
            {
                if (tbControlChats.InvokeRequired)
                {
                    var obj = (Models.Message)temp;
                    tbControlChats.Invoke(new Action(() => AddNewMessages(obj)));
                }
                else
                {
                    var obj = (Models.Message)temp;
                    AddNewMessages(obj);
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("[invokeAddTabPage]" + ex.Message);
            } 
        }

        public void NewMessageNotificaction(object tbPageObjectIn)
        {
            TabPage tbPageIn = (TabPage)tbPageObjectIn;
            string OriginalText = tbPageIn.Text;
            string Notification = "NUEVO(S) MENSAJE(S)";
            while (tbControlChats.SelectedTab != tbPageIn)
            {
                Thread.Sleep(1000);
                tbPageIn.Text = Notification;
                Thread.Sleep(1000);
                tbPageIn.Text = OriginalText;
            }
            tbPageIn.Text = OriginalText;
        }

        public void AddNewMessages(Models.Message temp)
        {
            try
            {
                for (int position = 0; position < arrTabPageChat.Count; position++)
                {
                    if (arrTabPageChat[position].tbPage != null && arrTabPageChat[position].tbPage.Name == "tabPageChat_" + temp.chatId) {
                        arrTabPageChat[position].askForNewMessages();

                        if (tbControlChats.SelectedTab != arrTabPageChat[position].tbPage && firstRecoveredChatsLoading)
                        {
                            Console.WriteLine("****************** \n Llego mensaje nuevo a pestaña oculta");
                            Thread NewMessageThread = new Thread(new ParameterizedThreadStart(NewMessageNotificaction));
                            NewMessageThread.Start(arrTabPageChat[position].tbPage);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[AddNewMessages]: " + ex.Message);
            }
            //return tbPage;
        }
        #endregion
    }
}
