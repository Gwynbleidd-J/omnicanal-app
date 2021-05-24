using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Shared;

namespace LoginForms
{
    public partial class FormPrincipal : Form
    {
        WhatsApp whatsApp;
        Prueba prueba;
        //RestHelper rh = new RestHelper();
        AsynchronousClient client;

        public FormPrincipal()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;            
            whatsApp = new WhatsApp();
            prueba = new Prueba();
            

            this.IsMdiContainer = true;
            //whatsApp.MdiParent = this;
            //whatsApp.Show();
            prueba.MdiParent = this;
            prueba.Show();
            client = new AsynchronousClient(whatsApp.rtxtResponseMessage, this, prueba, this);
            //client.inicializarChatWindow();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Task task = new Task(client.Connect);
            task.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            whatsApp.Show();
            whatsApp.MdiParent = this;
            //abrirForm(new WhatsApp());
        }

        private void btnTelegram_Click(object sender, EventArgs e)
        {
            //Telegram telegram = new Telegram();
            //telegram.Show();
            //telegram.MdiParent = this;
            //Prueba prueba = new Prueba();
            prueba.Show();
            prueba.MdiParent = this;
            //abrirForm(new Telegram());
        }

        private void btnLlamadas_Click(object sender, EventArgs e)
        {
            CallsView callsView = new CallsView();
            callsView.Show();
            callsView.MdiParent = this;
            //abrirForm(new CallsView());
        }


        //public void abrirForm(object formSecundario)
        //{
        //    if(this.panelContenedor.Controls.Count > 0)
        //        this.panelContenedor.Controls.RemoveAt(0);
        //    Form form = formSecundario as Form;
        //    form.TopLevel = false;
        //    form.Dock = DockStyle.Fill;
        //    this.panelContenedor.Controls.Add(form);
        //    this.panelContenedor.Tag = form;
        //    form.Show();
        //}



    }
}
