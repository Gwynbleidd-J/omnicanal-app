using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using LoginForms.Shared;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace LoginForms
{
    public partial class screenMonitor : Form
    {
        RestHelper rh = new RestHelper();
        bool Iniciado = false;
        bool Monitoreando = false;
        int idAgent = 0;
        string Agent = "";
        int idSupervisor = 0;
        string tiempoMonitoreo = "1";
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan timeSpanTemp = new TimeSpan();

        StringBuilder builder = new StringBuilder();

        public screenMonitor()
        {
            InitializeComponent();
            GetAllAgentsAsync();          

            pictureBox1.MinimumSize = new Size(400, 400);
            timer1.Enabled = false;
            timer1.Interval = 1000;
            panel1.Visible = false;
            
        }

        private async void GetAllAgentsAsync()
        {
            try
            {
                var agents = await rh.getAllAgents();
                var cleanData = (JObject)JsonConvert.DeserializeObject(agents);
                var algo = cleanData["data"].Children();

                List<KeyValuePair<string, string>> lista = new List<KeyValuePair<string, string>>();

                foreach (var item in algo)
                {
                    var agentID = item["id"].Value<string>();
                    var agentName = item["agente"].Value<string>();

                    lista.Add(new KeyValuePair<string, string>(agentID, agentName));
                }

                comboBox1.DataSource = lista;
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
                comboBox1.SelectedIndex = 0;

                idAgent = int.Parse(comboBox1.SelectedValue.ToString());
                Agent = comboBox1.GetItemText(comboBox1.SelectedItem);

                Console.WriteLine("\nLos datos del combo son:" + comboBox1.GetItemText(comboBox1.SelectedItem));

            }
            catch (Exception _e)
            {
                throw _e;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Iniciado)
            {
                idAgent = int.Parse(comboBox1.SelectedValue.ToString());
                Agent = comboBox1.Text;
                Console.WriteLine("El agente a monitorear es:" +idAgent +comboBox1.Text);
            }
        }

        public void setImage( Bitmap imagen)
        {
            pictureBox1.Image = imagen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Monitoreando)
            {
                button1.Text = "Comenzar monitoreo";
                timer1.Enabled = false;
                comboBox1.Enabled = true;
                panel1.Visible = false;
                stopWatch.Reset();
            }
            else {
                button1.Text = "Detener monitoreo";
                timer1.Enabled = true;
                comboBox1.Enabled = false;
                stopWatch.Restart();

                panel1.Visible = true;
                builder.Length = 0;
                builder.Append("Monitoreando a: ");
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine(Agent);

                builder.AppendLine();
                builder.AppendLine();
                builder.Append("Tiempo monitoreando:" +tiempoMonitoreo);
                textBox1.Text = builder.ToString();

            }
            Monitoreando = !Monitoreando;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            idSupervisor = int.Parse(GlobalSocket.currentUser.ID);
            await rh.startMonitoring(idAgent, idSupervisor);
            timeSpanTemp = stopWatch.Elapsed;
            if (timeSpanTemp.Seconds > 0)
            {
                string tempTiempoMonitoreo = tiempoMonitoreo;
                tiempoMonitoreo = timeSpanTemp.Seconds.ToString();
                builder.Replace(tempTiempoMonitoreo,tiempoMonitoreo);
                textBox1.Text = builder.ToString();
            }
            Console.WriteLine("Tick ejecutado por el timer");
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            Iniciado = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            const int padding = 3;
            // get number of lines (first line is 0, so add 1)
            int numLines = this.textBox1.GetLineFromCharIndex(this.textBox1.TextLength) + 1;
            // get border thickness
            int border = this.textBox1.Height - this.textBox1.ClientSize.Height;
            // set height (height of one line * number of lines + spacing)
            this.textBox1.Height = this.textBox1.Font.Height * numLines + padding + border;
        }
    }
}
