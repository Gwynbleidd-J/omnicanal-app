using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class Login : Form
    {
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
            string spassword = Encrypt.GetSHA256(txtPassword.Text.Trim());
            
            using (Models.loginEntities db = new Models.loginEntities())
            {
                var lst = from d in db.Usuario
                          where d.Nombre == txtNombre.Text
                          && d.Password == spassword
                          select d;

                if (lst.Count() >0)
                {
                    this.Hide();
                    MessageBox.Show($"Bienvenido {txtNombre.Text}");
                    Main main = new Main();
                    main.FormClosed += (s, args) => this.Close();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas");
                }

            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
