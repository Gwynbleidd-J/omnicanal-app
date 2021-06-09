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
                var json = await rh.Login(txtUserName.Text, txtPassword.Text, ipAddress);
                Json userNuevo = JsonConvert.DeserializeObject<Json>(json);
                //string message = rh.ResponseMessage(json);
                //string message = rh.DeserializarJson(json);
                User user = rh.GetUser(json);
                GlobalSocket.currentUser = userNuevo.data.user;
                this.Hide();
                //MessageBox.Show($"Bienvenido {txtUserName.Text}");
                FormPrincipal formPrincipal = new FormPrincipal();
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
    }
}
