using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Models;

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
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error[construct ChatWindow]: " + ex.Message);
            }
        }
        public void addTabPage()
        {
            try
            { 
                Thread threadCreateNewTabChat = new Thread((invokeAddTabPage)); 
                threadCreateNewTabChat.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error[addTabPage]: " + ex.Message);
            }
        }
          
        public void invokeAddTabPage()
        {
            try
            {
                if (tbControlChats.InvokeRequired)      
                    tbControlChats.Invoke(new Action(() => threadAddTabPage()));    
            }
            catch (Exception ex)
            {
                Console.WriteLine("[invokeAddTabPage]" + ex.Message);
            }
        }

        public void threadAddTabPage()
        {
            try
            { 
                tbPageChat = new TabPageChat();
                //tbPageChat = new TabPageChat();
                TabPageChat tbPageChatTemporal = null;
                tbPageChatTemporal = new TabPageChat();
                //tbPageChatTemporal.Name = "tbTemporal_" + chatId;
                //tbPageChatTemporal.Text = chatId;
                
                tbPageChat.chatId = chatGenerals.chatId;
                tbPageChat.platformIdentifier = chatGenerals.platformIdentifier;
                tbPageChat.clientPlatformIdentifier = chatGenerals.clientPlatformIdentifier;
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

        public void threadAddNewMessages()
        {
            try
            {
                Thread threadAddNewMessages = new Thread((invokeAddNewMessages)); 
                threadAddNewMessages.Start(); 
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error[ChatWindow ThreadAddNewMessages]: " + ex.Message); 
            } 
        } 
        public void invokeAddNewMessages()
        {
            try
            {
                if (tbControlChats.InvokeRequired)
                    tbControlChats.Invoke(new Action(() => AddNewMessages()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[invokeAddTabPage]" + ex.Message);
            } 
        }

        public void AddNewMessages()
        {
            try
            {
                for (int position = 0; position < arrTabPageChat.Count; position++)
                    if (arrTabPageChat[position].tbPage != null && arrTabPageChat[position].tbPage.Name == "tabPageChat_" + chatGenerals.chatId)
                        arrTabPageChat[position].askForNewMessages();
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
