using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Models;
using System.Reflection;

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

                tbPageChat.addEmptyControls();
                //tbPageChat.threadBuildTabPage();
                tbPageChat.threadBuildTabPage();
                tbPageChatTemporal = tbPageChat;
                //tbControlChats.Controls.Add(tbPageChatTemporal);
                //tbControlChats.TabPages.Add(tbPageChatTemporal.tbPage);
                tbControlChats.Controls.Add(tbPageChatTemporal.tbPage);
                //Intentar que el tab creado sea el que se seleccione en ese momento
                tbControlChats.SelectedTab = tbPageChatTemporal.tbPage;

                //arrTabPage[nextTabPagePosition] = tbPageChatTemporal;
                arrTabPageChat.Add(tbPageChatTemporal);
                //nextTabPagePosition++;
                //tbControlChats.Controls.Add(arrTabPage[nextTabPagePosition]);

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

                        if (tbControlChats.SelectedTab != arrTabPageChat[position].tbPage)
                        {
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
