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
        string currentAgentName = "";

        bool Iniciado = false;
        int chatId = 0;

        int indexSearch = 0;
        string stringSearch = "";
        bool fechaInicioSearch = false;

        DateTime TiempoI = new DateTime();
        DateTime TiempoF = new DateTime();

        public ChatMonitor()
        {
            InitializeComponent();
            GetActiveChats();
            GetAllAgentsAsync();
            FillSearchCombo();
            setTimeFilters();
        }

        public void FillSearchCombo() {

            List<KeyValuePair<string, int>> listaSearch = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if ( (dataGridView1.Columns[i]) is DataGridViewTextBoxColumn)
                {
                    listaSearch.Add(new KeyValuePair<string, int>(dataGridView1.Columns[i].HeaderText, i));
                }
            }

            comboBox2.DataSource = listaSearch;
            comboBox2.DisplayMember = "Key";
            comboBox2.ValueMember = "Value";

            comboBox2.SelectedIndex = 0;
        }

        public void setTimeFilters() {

            string todayDate = DateTime.Now.ToShortDateString();

            TiempoI = DateTime.Parse(todayDate + " 00:00:00");
            TiempoF = DateTime.Parse(todayDate + " 23:59:59");

            dateTimePicker1.Value = TiempoI;
            dateTimePicker2.Value = TiempoF;
        }


        public void FilteredResults() {
            // Prevent exception when hiding rows out of view
            //CurrencyManager currencyManager = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            //currencyManager.SuspendBinding();

            // Show all lines
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                dataGridView1.Rows[u].Visible = true;
            }

            string valor = "";

            // Hide the ones that you want with the filter you want.
            for (int u = 0; u < dataGridView1.RowCount; u++)
            {
                valor = dataGridView1.Rows[u].Cells[indexSearch].Value.ToString();
                var fechaInicioFila = dataGridView1.Rows[u].Cells[HoraInicio.Index].Value;
                if (DateTime.TryParse(fechaInicioFila.ToString(), out _))
                {

                }

                if ((valor == stringSearch || stringSearch == ""))
                {
                    dataGridView1.Rows[u].Visible = true;
                }
                else
                {
                    dataGridView1.Rows[u].Visible = false;
                }
            }

            // Resume data grid view binding
            //currencyManager.ResumeBinding();
        }

        public async void GetActiveChats()
        {
            string agentId = GlobalSocket.currentUser.ID.ToString();
            var data = await rh.RecoverAllActiveChats(agentId);
            var cleanData = (JObject)JsonConvert.DeserializeObject(data);
            var algo = cleanData["data"].Children();

            //DataTable table = new DataTable();

            //table.Columns.Add("ID Chat", typeof(int));
            //table.Columns.Add("ID Agente", typeof(int));
            //table.Columns.Add("Agente asignado", typeof(string));
            //table.Columns.Add("Hora de inicio", typeof(string));
            //table.Columns.Add("Plataforma", typeof(string));
            //table.Columns.Add("Chats del agente", typeof(string));
            //table.Columns.Add("Chats Max. del agente", typeof(string));

            foreach (var item in algo)
            {
                var chatId = item["chatId"].Value<string>();
                var userId = item["userId"].Value<string>();
                var agent = item["asignedAgent"].Value<string>();
                var hora = item["startTime"].Value<string>();
                var platform = item["platformIdentifier"].Value<string>();
                //var activeChats = item["activeChats"].Value<string>();
                //var maxActiveChats = item["maxActiveChats"].Value<string>();

                dataGridView1.Rows.Add(chatId, userId, agent, hora, platform);
                //table.Rows.Add(chatId, userId, agent, hora, platform);
            }

            //dataGridView1.DataSource = table;

            //DataGridViewButtonColumn detailsButtonColumn = new DataGridViewButtonColumn
            //{
            //    Name = "Transferir",
            //    Text = "Transferir"
            //};
            //int columnIndex = 5;
            //detailsButtonColumn.UseColumnTextForButtonValue = true;
            //if (dataGridView1.Columns["Transferir"] == null)
            //{
            //    dataGridView1.Columns.Insert(columnIndex, detailsButtonColumn);
            //}

        }

        public async void RefreshDataGridView() {
            try
            {
                //Este metodo no muestra resultado despues
                dataGridView1.Rows.Clear();

                string agentId = GlobalSocket.currentUser.ID.ToString();
                var data = await rh.RecoverAllActiveChats(agentId);
                var cleanData = (JObject)JsonConvert.DeserializeObject(data);
                var algo = cleanData["data"].Children();

                foreach (var item in algo)
                {
                    var chatId = item["chatId"].Value<string>();
                    var userId = item["userId"].Value<string>();
                    var agent = item["asignedAgent"].Value<string>();
                    var hora = item["startTime"].Value<string>();
                    var platform = item["platformIdentifier"].Value<string>();
                    var activeChats = item["activeChats"].Value<string>();
                    var maxActiveChats = item["maxActiveChats"].Value<string>();

                    dataGridView1.Rows.Add(chatId, userId, agent, hora, platform);
                }

                dataGridView1.Refresh();

            }
            catch (Exception _e)
            {
                throw _e;
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

                currentAgentId = int.Parse(row.Cells[1].Value.ToString());
                currentAgentName = row.Cells[2].Value.ToString();
                textBox1.Text = row.Cells[2].Value.ToString();
                textBox1.Enabled = false;
                chatId = int.Parse(row.Cells[0].Value.ToString());
                
                Iniciado = true;
                

            }
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                if (currentAgentId == transferAgentId)
                {
                    MessageBox.Show("No puede transferir el chat al mismo agente");
                }
                else
                {
                    var estado = await rh.validateTransferAgent(transferAgentId.ToString());
                    var cleanData = (JObject)JsonConvert.DeserializeObject(estado);
                    var algo = cleanData["data"];

                    var capacidad = algo["capacidad"].Value<bool>();
                    var disponibilidad = algo["disponibilidad"].Value<bool>();

                    Console.WriteLine("\nEstado del agente a transferir:" + estado.ToString());

                    if (capacidad == true && disponibilidad == true)
                    {
                        DialogResult result = MessageBox.Show("Esta seguro de que desea transferir el chat a \n" + transferAgentName + " ? ", "Advertencia", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            var respuestaTransferncia = await rh.transferChat(currentAgentId.ToString(), transferAgentId.ToString(), chatId.ToString());
                            if (respuestaTransferncia == "OK")
                            {
                                MessageBox.Show("Se ha transferido el chat correctamente");

                                currentAgentName = transferAgentName;
                                currentAgentId = transferAgentId;
                                textBox1.Text = currentAgentName;
                                RefreshDataGridView();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Se ha cancelado la transferencia del chat");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El agente seleccionado no puede atender el chat en este momento");
                    }
                }
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
                transferAgentId = int.Parse(comboBox1.SelectedValue.ToString());
                transferAgentName = comboBox1.Text;
                //MessageBox.Show("Agente Seleccionado:" + comboBox1.Text + "\nIdentificador:" + comboBox1.SelectedValue);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
                indexSearch = comboBox2.SelectedIndex;
                Console.WriteLine("\nIndice seleccionado:" + indexSearch + "\nColumna:" + comboBox1.GetItemText(comboBox2.SelectedItem));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var tiempoInicial = dateTimePicker1.Value;
            var tiempoFinal = dateTimePicker2.Value;

            TiempoI = DateTime.Parse(tiempoInicial.ToString());
            TiempoF = DateTime.Parse(tiempoFinal.ToString());

            Console.WriteLine("\nHora del primer timer:" +tiempoInicial +"\nHora del segudo timer:"+tiempoFinal );

            stringSearch = textBox2.Text.Trim();
            FilteredResults();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
    }
}
