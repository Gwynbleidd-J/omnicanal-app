using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using LoginForms.Shared;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;

namespace LoginForms
{
    public partial class screenMonitor : Form
    {
        AsynchronousClient client = new AsynchronousClient();
        RestHelper rh = new RestHelper();
        bool Iniciado = false;
        bool Monitoreando = false;
        int idAgent = 0;
        string Agent = "";
        int idSupervisor = int.Parse(GlobalSocket.currentUser.ID);
        public static bool Reconectado = false;
        
        static StringBuilder builder = new StringBuilder();


        public screenMonitor()
        {
            InitializeComponent();
            GetAllAgentsAsync();          

            pictureBox1.MinimumSize = new Size(400, 400);
            panel1.Visible = false;

            NetworkChange.NetworkAvailabilityChanged += new
             NetworkAvailabilityChangedEventHandler(AddressChangedCallback);

            //Console.WriteLine("Esperando cambios en direccion de red, presione cualquier tecla para salir.");
            //Console.ReadLine();

        }

        public void AddressChangedCallback(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable == true)
            {
                builder.Length = 0;
                builder.Append("Se ha recuperado la conexion a internet");
                builder.AppendLine();
                builder.Append("Reconectando con el servidor...");
                textBox1.Text = builder.ToString();

                client.Connect();
            }
            else {
                builder.Length = 0;
                builder.Append("Se ha perdido la conexion");
                textBox1.Text = builder.ToString();
                //AsynchronousClient.conexionPerdidaMonitoreo = true;

                //AsynchronousClient.Monitoreando = false;
                SocketIOClient.Monitoreando = true;
                button1.Text = "Comenzar monitoreo";
                comboBox1.Enabled = true;
                Monitoreando = false;
                pictureBox1.Image = null;
            }

            //NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            //foreach (NetworkInterface n in adapters)
            //{
            //    Console.WriteLine("\n   {0} is {1}", n.Name, n.OperationalStatus);
            //}
        }

        public void ReconexionServidor() {
            if (Reconectado == true)
            {
                Reconectado = false;
                //AsynchronousClient.conexionPerdidaMonitoreo = false;

                builder.Length = 0;
                builder.Append("Se ha recuperado la conexion a internet");
                builder.AppendLine();
                textBox1.Text = builder.ToString();
            }
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

        private async void button1_Click(object sender, EventArgs e)
        {

            if (Monitoreando)
            {
                //AsynchronousClient.Monitoreando = false;
                SocketIOClient.Monitoreando = false;
                button1.Text = "Comenzar monitoreo";
                comboBox1.Enabled = true;
                panel1.Visible = false;
                textBox1.Text = "";
                Monitoreando = false;
                pictureBox1.Image = null;

            }
            else
            {
                button1.Text = "Detener monitoreo";
                comboBox1.Enabled = false;
                panel1.Visible = true;
                builder.Length = 0;

                builder.Append("Monitoreando a: ");
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine(Agent);
                textBox1.Text = builder.ToString();

                Monitoreando = true;

                //AsynchronousClient.Monitoreando = true;
                SocketIOClient.Monitoreando = true;
                var response =  await rh.startMonitoring(idAgent, idSupervisor);
                if (response == "InternalServerError")
                {
                    builder.Length = 0;
                    builder.Append("El agente seleccionado no se encuentra conectado");
                    textBox1.Text = builder.ToString();
                }

                Console.WriteLine("La respuesta a la peticion es:" +response);

            }
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
