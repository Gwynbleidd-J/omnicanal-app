using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Shared;
using LoginForms.Models;
using Newtonsoft.Json;

namespace LoginForms.Utils
{
    public class TabPageChat
    {
        //FORMULARIO PADRE
        #region Atributos
        public string chatId { get; set; } 
        public string platformIdentifier { get; set; }
        public string clientPlatformIdentifier { get; set; }
        public string lastMessageId { get; set; }
        public int lastUsedHeigth { get; set; }
        public string chatMessagestHistoric { get; set; }

        public TabPage tbPage { get; set; } 
        public Label lblLastMessageId { get; set; }
        public Label lblChatId { get; set; }
        public Label lblClientPlatformIdentifier { get; set; } 
        public Label lblLastHeighUsed { get; set; } 
        public Label lblPlatformIdentifier { get; set; } 
        public Panel pnlMessages { get; set; }
        public Label lastLabel { get; set; }

        public Button btnCloseButton { get; set; }

        //public PanelControl panelControl { get; set; }

        /// Prueba para tener el botón y textBox de envío de mensajes al mismo nivel del tabPage
        public TextBox txtSendMessage { get; set; } 
        public Button btnSendMessage { get; set; }
        RestHelper restHelper = new RestHelper();

        #endregion

        #region Métodos
        public TabPageChat()
        {
            try
            {
                tbPage = new TabPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[constructor TabPageChat]: " + ex.Message);
            }
        }

          
        public void buildTabPage()
        {
            try
            {
                //chatId = "10";
                Thread threadCreateNewTabPage = new Thread((addEmptyControls));
                threadCreateNewTabPage.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[hilo]: " + ex.Message);
            }
        }
        public void addEmptyControls()
        {
            try
            {
                lastMessageId = "1";

                tbPage = new TabPage();
                lblLastMessageId = new Label();
                lblClientPlatformIdentifier = new Label();
                lblLastHeighUsed = new Label();
                lblPlatformIdentifier = new Label();
                lblChatId = new Label();
                pnlMessages = new Panel();
                //panelControl = new PanelControl(); 
                //Prueba de controles a nivel del tabPage    
                txtSendMessage = new TextBox(); 
                btnSendMessage = new Button();
                lastLabel = new Label();
                btnCloseButton = new Button();

                tbPage.Controls.Add(lblLastHeighUsed);
                tbPage.Controls.Add(lblLastMessageId);
                tbPage.Controls.Add(lblClientPlatformIdentifier);
                tbPage.Controls.Add(lblPlatformIdentifier);
                tbPage.Controls.Add(lblChatId);
                tbPage.Controls.Add(pnlMessages);
                //tbPage.Controls.Add(panelControl.pnlControls);
                tbPage.Controls.Add(txtSendMessage);
                tbPage.Controls.Add(btnSendMessage);
                tbPage.Controls.Add(btnCloseButton);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[addEmptyControls]: " + ex.Message); 
            }

        }
        public void threadBuildTabPage()
        {
            try
            {
                //tbPage = new TabPage();
                //if (platformIdentifier == "w")
                //    tbPage.Text = $"Mensaje desde Whatsapp: {chatId}";
                //else if (platformIdentifier == "t")
                //    tbPage.Text = $"Mensaje desde Telegram: {chatId}";
                //else if (platformIdentifier == "c")
                //    tbPage.Text = $"Mensaje desde Web: {chatId}";

                tbPage.Name = $"tabPageChat_{chatId}"; 
                tbPage.Tag = $"tabPageChat_{chatId}";
                tbPage.Size = new Size(675, 462);                 
                
                //Agregar al textbox para el envío de mensajes
                lblLastHeighUsed.Name = $"lblLastHeighUsed_{chatId}";
                lblLastHeighUsed.Tag = $"lblLastHeighUsed_{chatId}";
                lblLastHeighUsed.Text = "0";
                lblLastHeighUsed.Visible = false;
                //tbPage.Controls.Add(lblLastHeighUsed); 

                //Agregar las etiquetas que nos ayudarán a almacenar información gral del chat para la recepción de mensajes del chat 
                lblLastMessageId.Name = $"lblLastMessageId_{chatId}";
                lblLastMessageId.Tag = $"lblLastMessageId_{chatId}";
                lblLastMessageId.Text = "1";
                lblLastMessageId.Visible = false;
                //tbPage.Controls.Add(lblLastMessageId); 

                lblChatId.Name = $"lblChatId_{chatId}";
                lblChatId.Tag = $"lblChatId_{chatId}";
                lblChatId.Text = chatId;
                lblChatId.Visible = false;

                lblClientPlatformIdentifier.Name = $"lblClientPlatformIdentifier_{chatId}";
                lblClientPlatformIdentifier.Tag = $"lblClientPlatformIdentifier_{chatId}";
                lblClientPlatformIdentifier.Text = clientPlatformIdentifier;
                lblClientPlatformIdentifier.Visible = false;
                //lblNombreCliente.Text = "etiqueta de cliente";
                //tbPage.Controls.Add(lblNombreCliente); 

                lblPlatformIdentifier.Name = $"lblPlatformIdentifier_{chatId}";
                lblPlatformIdentifier.Tag = $"lblPlatformIdentifier_{chatId}";
                lblPlatformIdentifier.Text = platformIdentifier;
                lblPlatformIdentifier.Visible = false;
                //tbPage.Controls.Add(lblPlatformIdentifier); 

                pnlMessages.Name = $"lblPlatformIdentifier_{chatId}";
                pnlMessages.Tag = $"lblPlatformIdentifier_{chatId}";
                pnlMessages.Size = new Size(640, 335); 
                pnlMessages.Location = new Point(18,8);
                pnlMessages.BackColor = Color.FromArgb(236, 229, 221);
                pnlMessages.AutoScroll = true;
                //tbPage.Controls.Add(pnlMessages);

                //panelControl.chatId = chatId;
                //panelControl.buildPanel(); 


                //prueba agregando los controles a nivel TabPage
                //Agregar al textbox para el envío de mensajes 
                txtSendMessage.Name = $"txtSendMessage_{chatId}";
                txtSendMessage.Tag = $"txtSendMessage_{chatId}";
                txtSendMessage.Size = new Size(480, 20);
                txtSendMessage.Location = new Point(10, 432);
                txtSendMessage.Text = "Buen día, soy su agente a cargo, ¿En qué le puedo ayudar?";
                txtSendMessage.KeyPress += async (s, e) => {
                    try
                    {
                        if ((int)e.KeyChar == (int)Keys.Enter)
                            await sendMessageFromPanelControl();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error[txtSendMessage.KeyPress]: " + ex.Message);
                    }

                };

                //Agregar al boton para el envío de mensajes 
                btnSendMessage.Name = $"btnSendMessage_{chatId}";
                btnSendMessage.Tag = $"btnSendMessage_{chatId}";
                btnSendMessage.Text = "Enviar";
                btnSendMessage.Size = new Size(82, 23);
                btnSendMessage.Location = new Point(498, 429);
                btnSendMessage.Click += async (s, e) => {
                    try
                    {
                        if (!string.IsNullOrEmpty(txtSendMessage.Text.ToString()))
                            await sendMessageFromPanelControl();
                            //if (await sendMessageFromPanelControl())
                            //    askForNewMessages();
                                    
                                    //MessageBox.Show("Mensages nuevos cargados correctamente!");
                                    //addLabelMessages();
                                //if(!string.IsNullOrEmpty(resulAskForNewMessages))
                            
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error[btnSendMessage.Click]: " + ex.Message);
                    }
                };


                btnCloseButton.Name = $"btnCloseButton_{chatId}";
                btnCloseButton.Tag = $"btnCloseButton_{chatId}";
                btnCloseButton.Text = $"Cerrar Chat";
                btnCloseButton.Size = new Size(86,23);
                btnCloseButton.Location = new Point(586, 429);

                btnCloseButton.Click += async (s, e) =>
                {
                    if (await closeChat())
                    {
                        NetworkCategories networkCategories = new NetworkCategories(chatId);
                        networkCategories.ShowDialog();
                        removeTabChat();

                    }    
                };
            }
            catch (Exception ex) 
            { 
                Console.WriteLine("Error[threadBuildTabPage]: " + ex.Message); 
                //tbPage = null;
            }
            //return tbPage;
        }

        public async void removeTabChat()
        {
            var userId = GlobalSocket.currentUser.ID;
            Control parentTabControlChat = tbPage.Parent;
            parentTabControlChat.Controls.Remove(tbPage);
            await restHelper.getSubstactActiveChat(userId);
            MessageBox.Show("Chat Cerrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public async Task<bool> sendMessageFromPanelControl()
        {
            bool resultSendMessageFromPanelControl = false;
            try
            {
                var agentId = GlobalSocket.currentUser.activeIp;
                var numberToSend = GlobalSocket.numberToSend;
                //string statusCodeSendMessage = await restHelper.SendMessage(txtSendMessage.Text.ToString(), chatId, "whatsapp:+5214621257826", "w");
                string statusCodeSendMessage = await restHelper.SendMessage(txtSendMessage.Text.ToString(), lblChatId.Text.ToString(), lblClientPlatformIdentifier.Text.ToString(), lblPlatformIdentifier.Text.ToString(), agentId);
                if (!string.IsNullOrEmpty(statusCodeSendMessage) && statusCodeSendMessage == "OK")
                {
                    resultSendMessageFromPanelControl = true;
                    txtSendMessage.Text = "";
                }
                return resultSendMessageFromPanelControl;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[sendMessageFromPanelControl]: " + ex.Message);
                resultSendMessageFromPanelControl = false;
            }
            return resultSendMessageFromPanelControl;
        }

        //public async Task<bool> askForNewMessages()
        public async void askForNewMessages()
        {
            string resultNewMessages = string.Empty;
            bool gotNewMessages = false;
            try
            {
                resultNewMessages = await restHelper.GetAllMessage(chatId, lastMessageId);
                if (!string.IsNullOrEmpty(resultNewMessages))
                {
                    chatMessagestHistoric = resultNewMessages;
                    addLabelMessages();
                    gotNewMessages = true;
                }
                //return gotNewMessages;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[askForNewMessages]: " + ex.Message); 
            }
            //return gotNewMessages;
        } 

        public void addLabelMessages()
        {
            try
            {
                //Implementar el uso de páneles independientes para resolver el espaciado creciente en el panel de mensajes
                //Label lastLabel = new Label();
                Json jsonChatMessagestHistoric = JsonConvert.DeserializeObject<Json>(chatMessagestHistoric);

                for (int i = jsonChatMessagestHistoric.data.messages.Count - 1; i >= 0; i--)
                {
                    //Iniciio de construcción de la etiqueta dinámica
                    Label newLabelMessage = new Label();
                    newLabelMessage.Width = (pnlMessages.Width/2) + 15;
                    newLabelMessage.Height = 30; //20                     
                    newLabelMessage.AutoSize = false;
                    newLabelMessage.Name = jsonChatMessagestHistoric.data.messages[i].id;
                    //newLabelMessage.BackColor = Color.LightGray;
                    newLabelMessage.Text = jsonChatMessagestHistoric.data.messages[i].text;

                    if(lastLabel == null)
                        newLabelMessage.Location = new Point(0, 10);//10
                    else
                        newLabelMessage.Location = new Point(0, lastLabel.Location.Y + 40); //30

                    lastLabel = newLabelMessage;
                    lastMessageId = newLabelMessage.Name;
                    lastUsedHeigth += newLabelMessage.Height; 

                    //Validación para la alineación de la etiqueta según el Transmitter  <-- cliente / agente -->
                    if (jsonChatMessagestHistoric.data.messages[i].transmitter == "c")
                    { 
                        newLabelMessage.Left = 10;
                        newLabelMessage.TextAlign = ContentAlignment.MiddleLeft;
                        newLabelMessage.BackColor = Color.FromArgb(66,66,74);
                        newLabelMessage.ForeColor = Color.FromArgb(255, 255, 255);
                    }
                    else
                    { 
                        newLabelMessage.Left = newLabelMessage.Width -60;
                        newLabelMessage.TextAlign = ContentAlignment.MiddleRight;
                        newLabelMessage.BackColor = Color.FromArgb(13, 75, 70);
                        newLabelMessage.ForeColor = Color.FromArgb(255, 255, 255);
                    }

                    //lastUsedHeigth = lastUsedHeigth + 20;
                    ////Proceso original: Buscaba desde el Form en controls hasta llegar al panel de mensajes para irle agregando las etiquetas
                    //Control ctnPanelMessages = tabControlChats.Controls["tabPageChat_" + chatId].Controls["panelMessages_" + chatId];
                    //ctnPanelMessages.Controls.Add(newLabelMessage);
                    //ctnPanelMessages.ScrollControlIntoView(newLabelMessage);
                    
                    ////Nuevo proceso: Estando al mismo nivel de controles en el tabPage, intentar llegar al panel de mensajes solamente con el nombre del objeto
                    pnlMessages.Controls.Add(newLabelMessage);
                    pnlMessages.ScrollControlIntoView(newLabelMessage);

                }

                chatMessagestHistoric = "";
                //tabPageChat_1.Text = "Chat: " + jsonChatMessagestHistoric.data.chat[0].chatId;
                //Control ctn = panelMessages.Controls[lastLabel];

                //MessageBox.Show("Último msg recibido en: " + ctn.Name + ", decía: " + ctn.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[addLabelMessages]: " + ex.Message);
            }
        }

        public async Task<bool> closeChat()
        {
            bool resultCloseChat = false;
            try
            {
                //closeChatId = chatId;
                string statusCodeMessage = await restHelper.getCloseChat(chatId);
                if (!string.IsNullOrEmpty(statusCodeMessage) && statusCodeMessage == "OK")
                {
                    resultCloseChat = true;
                }
                return resultCloseChat;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[CloseChat] {ex.Message}");
                resultCloseChat = false;
            }

            return resultCloseChat;
        }
        #endregion
    }
}
