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

namespace LoginForms.Utils
{
    public class PanelControl
    {
        #region Atributos
        //variable string que almacena el chatId para construir los controles
        public String chatId { get; set; }
        //Definición de cada control que integra el Panel
        public Panel pnlControls { get; set; }
        public TextBox txtSendMessage { get; set; }
        public TextBox txtChatHistoric { get; set; }
        public Button btnSendMessage { get; set; }
        RestHelper restHelper = new RestHelper();
        #endregion

        #region Métodos
        public PanelControl()
        {
            try
            {

                pnlControls = new Panel();
                txtSendMessage = new TextBox();
                txtChatHistoric = new TextBox();
                btnSendMessage = new Button();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[constructor PanelControl]: " + ex.Message);
            }
        }

        public void addEmptyControls()
        {
            try
            {
                pnlControls = new Panel();
                txtSendMessage = new TextBox();
                txtChatHistoric = new TextBox();
                btnSendMessage = new Button();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[addEmptyControls]: " + ex.Message);
            }
        }
        public PanelControl buildPanel()
        {
            try
            {   //Agregar el panel de los controles generales del chat, para envío de mensajes  
                pnlControls.Name = $"panelControls_{chatId}";
                pnlControls.Tag = $"panelControls_{chatId}";
                pnlControls.Size = new Size(640, 81);
                pnlControls.Location = new Point(18, 375);

                //Agregar al textbox para el envío de mensajes 
                txtSendMessage.Name = $"txtSendMessage_{chatId}";
                txtSendMessage.Tag = $"txtSendMessage_{chatId}";
                txtSendMessage.Size = new Size(523, 20);
                txtSendMessage.Location = new Point(3, 55);
                txtSendMessage.Text = "Buen día, soy su agente a cargo, en qué le puedo ayudar?";
                //txtSendMessage.KeyPress += async(s, e) => {
                //    try
                //    {
                //        if ((int)e.KeyChar == (int)Keys.Enter) 
                //            sendMessageFromPanelControl(); 
                //    }
                //    catch(Exception ex)
                //    {
                //        Console.WriteLine("Error[txtSendMessage.KeyPress]: " + ex.Message);
                //    }

                //};
                pnlControls.Controls.Add(txtSendMessage);

                //Agregar al textbox para la recepción de mensajes del chat 
                txtChatHistoric.Name = $"txtChatHistoric_{chatId}";
                txtChatHistoric.Tag = $"txtChatHistoric_{chatId}";
                txtChatHistoric.Size = new Size(197, 20);
                txtChatHistoric.Location = new Point(238, 13);
                pnlControls.Controls.Add(txtChatHistoric);

                ////Agregar una etiqueta en la que se estará guardando el registro del último id de mensage cargado en el panel 
                ////(Este dato ayuda a mandarlo en las siguientes peticiones del chat y solo traer los msg que falten de cargar) 
                //lblLastMessageId.Name = $"lblLastMessageId_{chatId}";
                //panelControls.Controls.Add(lblLastMessageId);

                //Agregar al boton para el envío de mensajes 
                btnSendMessage.Name = $"btnSendMessage_{chatId}";
                btnSendMessage.Tag  = $"btnSendMessage_{chatId}";
                btnSendMessage.Text = "Enviar";
                btnSendMessage.Size = new Size(75, 23);
                btnSendMessage.Location = new Point(549, 55);
                 
                btnSendMessage.Click += async (s, e) => {
                    try
                    { 
                        if (!string.IsNullOrEmpty(txtSendMessage.Text.ToString()))
                            if (await sendMessageFromPanelControl())
                            {
                                string resulAskForNewMessages = await askForNewMessages();
                                MessageBox.Show("Nuevos mensajes recibidos");
                            }
                    }
                    catch(Exception ex)         
                    {       
                        Console.WriteLine("Error[btnSendMessage.Click]: " + ex.Message);     
                    }       
                };

                //btnSendMessage.Click += new pr;
                pnlControls.Controls.Add(btnSendMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[buildPanel]: " + ex.Message);
                pnlControls = null;
            }
            return this;
        }

        public async Task<bool> sendMessageFromPanelControl()
        {
            bool resultSendMessageFromPanelControl = false;
            try
            { 
                string statusCodeSendMessage = await restHelper.SendMessage(txtSendMessage.Text.ToString(), chatId, "whatsapp:+5214621257826", "w");
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

        public async Task<string> askForNewMessages()
        {
            string resultNewMessages = string.Empty;
            try
            {
                resultNewMessages = await restHelper.GetAllMessage(chatId, "1");
                if (!string.IsNullOrEmpty(resultNewMessages) && resultNewMessages == "OK")
                {
                    resultNewMessages = "OK"; 
                }
                return resultNewMessages;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[sendMessageFromPanelControl]: " + ex.Message); 
            }
            return resultNewMessages;
        }
        #endregion

    }
}
