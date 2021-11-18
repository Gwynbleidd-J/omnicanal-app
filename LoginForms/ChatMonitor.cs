using LoginForms.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class ChatMonitor : Form
    {

        RestHelper rh = new RestHelper();
        int currentAgentId = 0;
        int transferAgentId = 0;
        string transferAgentName = "";
        bool Iniciado = false;

        public ChatMonitor()
        {
            InitializeComponent();
            getActiveChats();
            getAllAgentsAsync();
        }

        public async void getActiveChats()
        {
            string agentId = GlobalSocket.currentUser.ID.ToString();
            var data = await rh.RecoverAllActiveChats(agentId);
            var cleanData = (JObject)JsonConvert.DeserializeObject(data);
            var algo = cleanData["data"].Children();

            DataTable table = new DataTable();
            table.Columns.Add("ID Chat", typeof(int));
            table.Columns.Add("ID Agente", typeof(int));
            table.Columns.Add("Agente asignado", typeof(string));
            table.Columns.Add("Hora de inicio", typeof(string));
            table.Columns.Add("Plataforma", typeof(string));
            //table.Columns.Add("Chats del agente", typeof(string));
            //table.Columns.Add("Chats Max. del agente", typeof(string));

            foreach (var item in algo)
            {
                var chatId = item["chatId"].Value<string>();
                var userId = item["userId"].Value<string>();
                var agent = item["asignedAgent"].Value<string>();
                var hora = item["startTime"].Value<string>();
                var platform = item["platformIdentifier"].Value<string>();
                var activeChats = item["activeChats"].Value<string>();
                var maxActiveChats = item["maxActiveChats"].Value<string>();

                table.Rows.Add(chatId, userId, agent, hora, platform);
            }

            dataGridView1.DataSource = table;

            DataGridViewButtonColumn detailsButtonColumn = new DataGridViewButtonColumn();
            detailsButtonColumn.Name = "Transferir";
            detailsButtonColumn.Text = "Transferir";
            int columnIndex = 5;
            detailsButtonColumn.UseColumnTextForButtonValue = true;
            if (dataGridView1.Columns["Transferir"] == null)
            {
                dataGridView1.Columns.Insert(columnIndex, detailsButtonColumn);
            }

        }

        private async void getAllAgentsAsync()
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

                    lista.Add(new KeyValuePair<string, string>(agentID,agentName));
                }

                comboBox1.DataSource = lista;
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";

                comboBox1.SelectedIndex = 0;

                transferAgentId = int.Parse(comboBox1.SelectedValue.ToString());
                transferAgentName = comboBox1.GetItemText(comboBox1.SelectedItem);

                Console.WriteLine("\nEl agente seleccionado por ahora es:" +transferAgentName +" \nCon el valor:"+ transferAgentId);
                Console.WriteLine("\nY los datos del combo son:" +comboBox1.GetItemText(comboBox1.SelectedItem));

            }
            catch (Exception _e)
            {

                throw _e;
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Transferir"].Index)
            {
                var row = dataGridView1.CurrentRow;
                groupBox1.Visible = true;

                currentAgentId = int.Parse(textBox1.Text = row.Cells[2].Value.ToString());
                textBox1.Text = row.Cells[3].Value.ToString();
                textBox1.Enabled = false;

                
                Iniciado = true;


                //MessageBox.Show("Detalles del chat: \nAsignado al agente:"+row.Cells[3].Value);
                //MessageBox.Show("Esta seguro(a) que desea transferir este chat?");
            }
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            if (currentAgentId == transferAgentId)
            {
                MessageBox.Show("No puede transferir el chat al mismo agente");
            }
            else
            {
                var estado = await rh.validateTransferAgent(transferAgentId.ToString());
                Console.WriteLine("\nEstado del agente a transferir:" + estado.ToString());
                MessageBox.Show("Esta seguro de que desea transferir el chat a \n"+ transferAgentName +"?");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Iniciado)
            {
                transferAgentId = int.Parse(comboBox1.SelectedValue.ToString());
                transferAgentName = comboBox1.Text;
                MessageBox.Show("Agente Seleccionado:" + comboBox1.Text + "\nIdentificador:" + comboBox1.SelectedValue);
            }
        }
    }
}
