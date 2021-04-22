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
    public partial class WhatsApp : Form
    {
        AsynchronousClient client = new AsynchronousClient();
        public WhatsApp()
        {
            InitializeComponent();
        }

        private void WhatsApp_Load(object sender, EventArgs e)
        {
            Task task = new Task(client.Connect);
            task.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }
    }
}
