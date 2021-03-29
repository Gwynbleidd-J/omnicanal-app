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
            if (txtEmail.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Campos Vacios", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                var json = await rh.Login(txtEmail.Text, txtPassword.Text);
                string message = rh.ResponseMessage(json);
                if(message == "User authorized")
                {
                    Usuario user = rh.GetUser(json);
                    string spassword = Encrypt.GetSHA256(txtPassword.Text.Trim());
                    using (Models.loginEntities db = new Models.loginEntities())
                    {
                        var lst = from d in db.Usuario
                                  where d.Email == txtEmail.Text
                                  && d.Password == spassword
                                  select d;
                        if (lst.Count() > 0)
                        {
                            this.Hide();
                            MessageBox.Show($"Bienvenido {txtEmail.Text}");
                            UserSignUp signUp = new UserSignUp();
                            signUp.FormClosed += (s, args) => this.Close();
                            signUp.Show();
                        }
                        else
                        {
                            MessageBox.Show("Credenciales incorrectas");
                        }

                    }
                }


            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
