using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Shared;
namespace LoginForms
{
    public partial class UserSignUp : Form
    {
        RestHelper rh = new RestHelper();
        public UserSignUp()
        {
            InitializeComponent();
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == "" || txtUserName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Campos Vacios", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var response = await rh.RegistrerUser(txtUserName.Text, txtEmail.Text, txtPassword.Text);
                string message = rh.DeserializarJson(response);
                //string message = rh.ResponseMessage(response);
                if (message == "Register succesfull")
                {
                    MessageBox.Show("Usuario Registrado con Éxito", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmail.Text = "";
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                }
                else
                {
                    MessageBox.Show(message + " ", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
