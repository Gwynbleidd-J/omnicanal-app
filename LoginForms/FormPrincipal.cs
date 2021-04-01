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
    public partial class FormPrincipal : Form
    {
        //RestHelper rh = new RestHelper();
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnWhatsApp_Click(object sender, EventArgs e)
        {
            WhatsApp whatsApp = new WhatsApp();
            whatsApp.Show();
        }

        private void btnTelegram_Click(object sender, EventArgs e)
        {
            Telegram telegram = new Telegram();
            telegram.Show();
        }

        private void btnLlamadas_Click(object sender, EventArgs e)
        {
            CallsView callsView = new CallsView();
            callsView.Show();
        }

        private void btnWhatsApp_Click_1(object sender, EventArgs e)
        {
            WhatsApp whatsApp = new WhatsApp();
            whatsApp.Show();
        }
    }
}
