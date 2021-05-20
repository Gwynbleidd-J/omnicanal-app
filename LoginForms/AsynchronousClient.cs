﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using LoginForms.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Web.Script.Serialization;
using LoginForms.Models;

using LoginForms.Utils;
using System.Drawing;

namespace LoginForms
{
    class AsynchronousClient
    {
        private RichTextBox container;
        private Form whatsapp;
        private Prueba prueba;
        private Form fPrincipal;
        //private TabControl tbControlContainer;
        //private ChatWindow chatWindow;

        public AsynchronousClient(RichTextBox container, Form whatsapp, Prueba prueba, Form fPrincipal)
        {
            try
            {
                this.container = container;
                this.whatsapp = whatsapp;
                this.prueba = prueba;
                this.fPrincipal = fPrincipal;
                //this.tbControlContainer = tabControlFromForm;
                //inicializarChatWindow();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error[construct AsynchronousClient]: " + ex.ToString());
            }
        }

        public RestHelper rh = new RestHelper();
        //Puerto en server de jCarlos       8000
        //Puerto en server de Localhost     8124
        private const int port = 8000;


        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static string response = string.Empty;

        public void Connect()
        {
            try
            {
                RestHelper rh = new RestHelper();

                //IP de Server jCarlos      192.168.1.102    
                //IP de Server Localhost    127.0.0.1
                IPAddress ipAddress = IPAddress.Parse("192.168.1.102");
                //IPAddress ipAddress = IPAddress.Parse("192.168.1.158");
                //IPAddress ipAddress = IPAddress.Parse(rh.GetLocalIpAddress());
                //IPAddress ipAddress = IPAddress.Parse("192.168.100.13");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                //Socket client = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
                GlobalSocket.GlobalVarible = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                //client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                //connectDone.WaitOne();

                GlobalSocket.GlobalVarible.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), GlobalSocket.GlobalVarible);
                connectDone.WaitOne();

                //// Receive the response from the remote device.
                Receive();
                receiveDone.WaitOne();
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

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //public void Send(Socket client, String data)
        //{
        //    // Convert the string data to byte data using ASCII encoding.  
        //    byte[] byteData = Encoding.ASCII.GetBytes(data);

        //    // Begin sending the data to the remote device.  
        //    client.BeginSend(byteData, 0, byteData.Length, 0,
        //        new AsyncCallback(SendCallback), client);
        //}

        //private static void SendCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the socket from the state object.  
        //        Socket client = (Socket)ar.AsyncState;

        //        // Complete sending the data to the remote device.  
        //        int bytesSent = client.EndSend(ar);
        //        Console.WriteLine("Sent {0} bytes to server.", bytesSent);

        //        // Signal that all bytes have been sent.  
        //        sendDone.Set();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        public void Receive()
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = GlobalSocket.GlobalVarible;

                // Begin receiving the data from the remote device.  
                GlobalSocket.GlobalVarible.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);


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
                    Console.WriteLine("Cuando es menor que 0");
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

        public void treatNotification(string socketNotification)
        {
            try
            {
                Chat notification = JsonConvert.DeserializeObject<Chat>(socketNotification);
                
                prueba.treatNotification(notification);
                
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
    }
}
