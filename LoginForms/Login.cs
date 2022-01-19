﻿using System;
using System.Configuration;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;
using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;


namespace LoginForms
{
    public partial class Login : Form
    {
        readonly RestHelper rh = new RestHelper();
        //readonly AsynchronousClient asynchronousClient = new AsynchronousClient();

        public Login()
        {
            //BackColor = Color.FromArgb(226, 224, 224);
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            #region Llamada al metodo para cerrar el socket desde la aplicacion
            //try
            //{
            //    if (GlobalSocket.GlobalVarible.Connected)
            //    {
            //        asynchronousClient.CloseSocketConnection();
            //        Application.Exit();
            //    }
            //    else
            //    {
            //        Console.WriteLine($"No se pudo cerrar sesión, problemas con el socket");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error [BtnCerrar_Click][Login] {ex.Message}");
            //}
            #endregion

        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Campos Vacios", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                userLogin();
            }

        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar == (int)Keys.Enter)
            {
                if (txtUserName.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Campos Vacios", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else 
                {
                    userLogin();
                }
            }
        }

        private async void userLogin(IProgress<int> progress = null)
        {
            string ipAddress = rh.GetPublicIpAddress();

            try
            {
                FormPrincipal formPrincipal = new FormPrincipal();
                var jsonLogin = await rh.Login(txtUserName.Text, txtPassword.Text, ipAddress);
                if(jsonLogin == "Unauthorized")
                {
                    MessageBox.Show($"Correo o contraseña Incorrecta, revisa tus credenciales", $"SIDI Omnichannel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Text = "";
                }
                else
                {
                    //Se deshabilita la actualizacion de ip aqui pues se utilizara el puerto que mande la api como ip
                    //Aquí cuando la contraseña o el correo son incorrectos manda un error 401, Unauthorized, 201 OK
                    //var jsonUpdateAgentActiveIp = await rh.updateAgentActiveIp(txtUserName.Text, ipAddress);
                    Json jsonUser = JsonConvert.DeserializeObject<Json>(jsonLogin);
                    User user = rh.GetUser(jsonLogin);
                    //string agentStatus = GlobalSocket.currentUser.status.id;
                    GlobalSocket.currentUser = jsonUser.data.user;
                    //GlobalSocket.currentUser.activeIp = ipAddress;
                    GlobalSocket.currentUser.activeIp = "0";
                    GlobalSocket.currentUser.token = user.token;
                    //sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, port, proxy);
                    this.Hide();
                    formPrincipal.FormClosed += (s, args) => this.Close();
                    formPrincipal.Show();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[Login]: {ex}");
                MessageBox.Show($"Error[Login]: {ex.Message}", $"Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEntrar_MouseEnter(object sender, EventArgs e)
        {
            btnEntrar.BackgroundImage = Properties.Resources.boton_largo_presionado;
        }

        private void btnEntrar_MouseUp(object sender, MouseEventArgs e)
        {
            btnEntrar.BackgroundImage = Properties.Resources.boton_chico;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
