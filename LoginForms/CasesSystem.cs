using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class CasesSystem : Form
    {
        public CasesSystem()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(txtAnswer.Text == "")
            {
                MessageBox.Show("No se recibio un mensaje, por favor escriba uno.","mensaje",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string texto = txtAnswer.Text.Trim(new char[] { '¡', '!', ',', '¿', '?', '.' }).ToLower();
                lblResponse.Text = texto;
                MessageBox.Show(texto);
                switch (texto)
                {
                    case "buenos dias pueden ayudarme":
                        lblResponse.Text = "buen día, en un momento lo atendemos";
                        break;
                    case "buenos dias necesito soporte tecnico":
                        lblResponse.Text = "buen día, en un momento lo atendemos";
                        break;

                    case "buen dia necesito ayuda":
                        lblResponse.Text = "buen día, en un momento lo atendemos";
                        break;

                    case "buen dia necesito soporte tecnico":
                        lblResponse.Text = "buen día, en un momento lo atendemos";
                        break;

                    //Preguntarle a Diego sobre las ideas que tengo para implementar los casos
                    //lo de cadena asincrona
                    //quitar los caracteres y dejar la cadena un poco estandar

                    default:
                        lblResponse.Text = "No se recibió una palabra valida";
                        break;
                }

            }
        }

        private async void txtAnswer_TextChanged(object sender, EventArgs e)
        {
        //    char[] charsRead = new char[txtAnswer.Text.Length];
            //using (StringReader reader = new StringReader(txtAnswer.Text))
            //{
            //    await reader.ReadAsync(charsRead, 0, txtAnswer.Text.Length);
            //}

            //StringBuilder reformattedText = new StringBuilder();
            //using (StringWriter writer = new StringWriter(reformattedText))
            //{
            //    foreach (char c in charsRead)
            //    {
            //        if (char.IsLetter(c) || char.IsWhiteSpace(c))
            //        {
            //            await writer.WriteAsync(char.ToLower(c));
            //        }
            //    }
            //}
            //lblResponse.Text = reformattedText.ToString();
        }
    }
}

