using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;
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
    public partial class AgentInformation : Form
    {
        RestHelper rh = new RestHelper();
        string idAgent;
        public AgentInformation(string individualId)
        {
            idAgent = individualId;
            InitializeComponent();
        }

        private void AgentInformation_Load(object sender, EventArgs e)
        {
            agentDetails(idAgent);
        }

        private async void agentDetails(string idAgent)
        {
            try
            {
                string agentDetails = await rh.getAgentsDetails(idAgent);
                Json jsonAgentDetails = JsonConvert.DeserializeObject<Json>(agentDetails);

                /*
                MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(agentDetails);
                var data = resp.data;
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string json1 = rh.BeautifyJson(data.ToString());
                UserDetails userDet = JsonConvert.DeserializeObject<UserDetails>(json1);
                 */
                FlowLayoutPanel panelAgentDetails = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(145, 153, 179),
                    BorderStyle = BorderStyle.FixedSingle,
                    FlowDirection = FlowDirection.TopDown,
                    Size = new Size(390, 140)
                };

                flpAgentInformation.Controls.Add(panelAgentDetails);

                Label labelNombreAgente = new Label
                {
                    Text = $"Nombre Agente:{jsonAgentDetails.data.details[0].name} {jsonAgentDetails.data.details[0].paternalSurname} {jsonAgentDetails.data.details[0].maternalSurname}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelEmail = new Label
                {
                    Text = $"Email: {jsonAgentDetails.data.details[0].email}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelActiveChats = new Label
                {
                    Text = $"Active Chats: {jsonAgentDetails.data.details[0].activeChats}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelRol = new Label
                {
                    Text = $"Permisos: {jsonAgentDetails.data.details[0].rol.name}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelStatus = new Label
                {
                    Text = $"Estado Agente: {jsonAgentDetails.data.details[0].status.status}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                //Label labelSolvedChats = new Label
                //{
                //    Text = $"Solved Chats: {jsonAgentDetails.data.details.solvedChats}",
                //    ForeColor = Color.FromArgb(19, 34, 38),
                //    Font = new Font("Microsoft Sans Serif", 11),
                //    AutoSize = true
                //};

                //Label labelStatus = new Label
                //{
                //    Text = $"Estatus: {jsonAgentDetails.data.details.status}",
                //    ForeColor = Color.FromArgb(19, 34, 38),
                //    Font = new Font("Microsoft Sans Serif", 11),
                //    AutoSize = true
                //};

                //panelAgentDetails.Controls.AddRange(new Control[] { labelNombreAgente, labelActiveCalls, labelActiveChats, labelScore, labelSolvedCalls, labelActiveChats, labelStatus });
                panelAgentDetails.Controls.AddRange(new Control[] { labelNombreAgente, labelEmail, labelActiveChats, labelRol, labelStatus });


            }

            catch (Exception ex)
            {
                //Console.WriteLine($"Error[agentsDetails]: {ex}");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
