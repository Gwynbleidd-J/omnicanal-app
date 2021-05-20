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
         
        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            string ipAddress = rh.GetLocalIpAddress();
            if (txtUserName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Campos Vacios", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    var json = await rh.Login(txtUserName.Text, txtPassword.Text, ipAddress);
                    string mensage = rh.ResponseMessage(json);
                    //MessageBox.Show(json, Text);
                    User user = rh.GetUser(json);
                    this.Hide();
                    //MessageBox.Show($"Bienvenido {txtUserName.Text}");
                    FormPrincipal formPrincipal = new FormPrincipal();
                    //formPrincipal.lblToken.Text = user.token;
                    formPrincipal.FormClosed += (s, args) => this.Close();
                    formPrincipal.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK);
                    // throw ex;
                }

            }

        }

        private async void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar == (int)Keys.Enter)
            {
                            string ipAddress = rh.GetLocalIpAddress();
            if (txtUserName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Campos Vacios", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                try 
                {
                    var json = await rh.Login(txtUserName.Text, txtPassword.Text, ipAddress);
                    string mensage = rh.ResponseMessage(json);
                    //MessageBox.Show(json, Text);
                    User user = rh.GetUser(json);
                    this.Hide();
                    MessageBox.Show($"Bienvenido {txtUserName.Text}");
                    FormPrincipal formPrincipal = new FormPrincipal();
                    //formPrincipal.lblToken.Text = user.token;
                    formPrincipal.FormClosed += (s, args) => this.Close();
                    formPrincipal.Show();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK);
                   // throw ex;
                }

            }
            }
        }
    }
}
