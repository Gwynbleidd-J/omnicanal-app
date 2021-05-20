using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;
using LoginForms.Shared;
using LoginForms.Models;


namespace LoginForms
{
    public partial class WhatsApp : Form
    {
        private int usedHeight = 0;
        private int contAlternador = 1;
        private string lastMessage = string.Empty;
        private string lastLabel = string.Empty;
        private int posYFinal = 0;

        RestHelper rh = new RestHelper();

        public WhatsApp()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            //Task task = new Task(client.Connect);
            //task.Start();

        }

        private void WhatsApp_Load(object sender, EventArgs e)
        {
            //var json = await rh.GetAll("172", "35"); a
            ////string json = rh.BeautifyJson(json);
            //MessageBox.Show(json);
            //rtxtResponseMessage.Text = json;

         //   string response = await rh.GetAllMessage("174", "");

        }

        //public void ActualizarTxtBox(string message)
        //{
        //    try
        //    {
        //        rtxtResponseMessage.Text = message;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("Error[ActualizarTxtBox]: " + ex.Message);
        //    }
        //}

        //Evento:   btnSend_Click
        //Modificò: J. Carlos Lara
        //Fecha:    23/Abril/2021
        //Motivo:   Implementar el envìo de mensajes desde el 

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //AsynchronousClient client = new AsynchronousClient(this.rtxtResponseMessage, this.labelChatId, this.lblClient, this.lblPlatformIdentifier);
                //Enviar el mensaje al servidor:
                Console.WriteLine("Enviando al servidor: " + rtxtSendMessage.Text.ToString());
                //client.Send(GlobalSocket.GlobalVarible, rtxtSendMessage.Text.ToString());
                await rh.SendMessage(rtxtSendMessage.Text, labelChatId.Text, lblClient.Text, lblPlatformIdentifier.Text);//aqui es donde el chat id no debe de ser estatico
                rtxtSendMessage.Text += "";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[btnSend_Click]: " + ex.Message);
            }

        }
        private async void rtxtSendMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                try
                {
                    //AsynchronousClient client = new AsynchronousClient(this.rtxtResponseMessage, this.labelChatId, this.lblClient, this.lblPlatformIdentifier);
                    //Enviar el mensaje al servidor:
                    Console.WriteLine("Enviando al servidor: " + rtxtSendMessage.Text.ToString());
                    //client.Send(GlobalSocket.GlobalVarible, rtxtSendMessage.Text.ToString());
                    await rh.SendMessage(rtxtSendMessage.Text, labelChatId.Text, lblClient.Text, lblPlatformIdentifier.Text);
                    rtxtSendMessage.Text += "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error[btnSend_Click]: " + ex.Message);
                }
            }
        }

        private void Notificacion()
        {

        }
    }
}

