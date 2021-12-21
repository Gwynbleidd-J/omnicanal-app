using System;
using System.Configuration;
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
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private async void userLogin()
        {
            string ipAddress = rh.GetLocalIpAddress();

            try
            {
                FormPrincipal formPrincipal = new FormPrincipal();
                var jsonLogin = await rh.Login(txtUserName.Text, txtPassword.Text, ipAddress);
                //Se deshabilita la actualizacion de ip aqui pues se utilizara el puerto que mande la api como ip
                //var jsonUpdateAgentActiveIp = await rh.updateAgentActiveIp(txtUserName.Text, ipAddress);
                Json jsonUser = JsonConvert.DeserializeObject<Json>(jsonLogin);
                User user = rh.GetUser(jsonLogin);
                //string agentStatus = GlobalSocket.currentUser.status.id;
                GlobalSocket.currentUser = jsonUser.data.user;
                GlobalSocket.currentUser.activeIp = ipAddress;
                GlobalSocket.currentUser.token = user.token;
                //sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, port, proxy);

                this.Hide();
                formPrincipal.FormClosed += (s, args) => this.Close();
                formPrincipal.Show();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[Login]: {ex}");
                MessageBox.Show($"Error[Login]: {ex.Message}", $"Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
