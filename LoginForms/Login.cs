using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;


namespace LoginForms
{
    public partial class Login : Form
    {
        RestHelper rh = new RestHelper();
        public Login()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
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
                var jsonUpdateAgentActiveIp = await rh.updateAgentActiveIp(txtUserName.Text, ipAddress);
                Json userNuevo = JsonConvert.DeserializeObject<Json>(jsonLogin);
                //string message = rh.ResponseMessage(json);
                //string message = rh.DeserializarJson(json);
                User user = rh.GetUser(jsonLogin);
                //string agentStatus = GlobalSocket.currentUser.status.id;

                var token = user.token;
                GlobalSocket.currentUser = userNuevo.data.user;
                GlobalSocket.currentUser.activeIp = ipAddress;

                this.Hide();
                //MessageBox.Show($"Bienvenido {txtUserName.Text}");
                //formPrincipal.rolId = user.rolID;
                //formPrincipal.lblToken.Text = user.token;
                formPrincipal.FormClosed += (s, args) => this.Close();
                formPrincipal.Show();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[Login]: {ex}");
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("¿Deseas salir de la aplicación?", "Omnicanal",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
                Application.Exit();
            }
        }
    }
}
