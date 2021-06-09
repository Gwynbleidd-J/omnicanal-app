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
                Console.WriteLine(jsonAgentDetails.ToString());
                UserDetails userDetails = JsonConvert.DeserializeObject<UserDetails>(jsonAgentDetails.data.ToString());

                //MsjApi resp = JsonConvert.DeserializeObject<MsjApi>(agentDetails);
                //var data = resp.data;
                //string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                //string json = rh.BeautifyJson(data.ToString());
                //UserDetails userDet = JsonConvert.DeserializeObject<UserDetails>(json);

                //Comentario se comento el for para probar otra solucion
                //for (int i = 0; i < jsonAgentDetails.data.details.Count; i++)
                //{
                //string details = await rh.getAgentsDetails(jsonAgentDetails.data.details.ToString());
                //UserDetails detail = new UserDetails();
                //Json jsonDetails = JsonConvert.DeserializeObject<Json>(details);
                //detail = jsonDetails.data.details;

                FlowLayoutPanel panelAgentDetails = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(145, 153, 179),
                    BorderStyle = BorderStyle.FixedSingle,
                    FlowDirection = FlowDirection.TopDown,
                    Size = new Size(320, 125)
                };

                flpAgentInformation.Controls.Add(panelAgentDetails);

                //    Label labelNombreAgente = new Label
                //    {
                //        Text = $"Nombre Agente: {jsonAgentDetails.data.details.name}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //    Label labelActiveCalls = new Label
                //    {
                //        Text = $"Chats Activos: {jsonAgentDetails.data.details.activeCalls}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //    Label labelActiveChats = new Label
                //    {
                //        Text = $"Active Chats: {jsonAgentDetails.data.details.activeChats}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //    Label labelScore = new Label
                //    {
                //        Text = $"Score: {jsonAgentDetails.data.details.score}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //    Label labelSolvedCalls = new Label
                //    {
                //        Text = $"Solved Calls: {jsonAgentDetails.data.details.solvedCalls}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //    Label labelSolvedChats = new Label
                //    {
                //        Text = $"Solved Chats: {jsonAgentDetails.data.details.solvedChats}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //    Label labelStatus = new Label
                //    {
                //        Text = $"Estatus: {jsonAgentDetails.data.details.status}",
                //        ForeColor = Color.FromArgb(19, 34, 38),
                //        Font = new Font("Microsoft Sans Serif", 11),
                //        AutoSize = true
                //    };

                //panelAgentDetails.Controls.AddRange(new Control[] { labelNombreAgente, labelActiveCalls, labelActiveChats, labelScore, labelSolvedCalls, labelActiveChats, labelStatus});


                //}
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error[agentsDetails]: {ex}");
            }
        }
    }
}
