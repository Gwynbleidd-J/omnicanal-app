using System;
using System.Drawing;
using System.Windows.Forms;
using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;

namespace LoginForms
{
    public partial class AgentMainPage : Form
    {
        RestHelper rh = new RestHelper();
        FormPrincipal formPrincipal = new FormPrincipal();

        public AgentMainPage()
        {
            InitializeComponent();
        }

        private void AgentMainPage_Load(object sender, EventArgs e)
        {
            AgentDetail();
        }

        private async void AgentDetail()
        {
            try
            {
                string individualId = formPrincipal.createAgentInformationForm();
                string agentDetail = await rh.getAgentsDetails(individualId);
                Json jsonAgentDetails = JsonConvert.DeserializeObject<Json>(agentDetail);
                FlowLayoutPanel panelAgentDetails = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(145, 153, 179),
                    BorderStyle = BorderStyle.FixedSingle,
                    FlowDirection = FlowDirection.TopDown,
                    Size = new Size(430, 220)
                };

                flpAgentInformation.Controls.Add(panelAgentDetails);

                Label labelNombreAgente = new Label
                {
                    Text = $"Nombre Agente:{jsonAgentDetails.data.details.name} {jsonAgentDetails.data.details.paternalSurname} {jsonAgentDetails.data.details.maternalSurname}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelEmail = new Label
                {
                    Text = $"Email: {jsonAgentDetails.data.details.email}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelRol = new Label
                {
                    Text = $"Permisos: {jsonAgentDetails.data.details.rol.name}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelStatus = new Label
                {
                    Text = $"Estado Agente: {jsonAgentDetails.data.details.status.status}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelSolvedChats = new Label
                {
                    Text = $"Chats Terminados: {jsonAgentDetails.data.solvedChats.solvedchats}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelActiveChats = new Label
                {
                    Text = $"Chats Activos: {jsonAgentDetails.data.activeChats.activechats}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelSolvedCalls = new Label
                {
                    Text = $"Llamadas Terminadas: {jsonAgentDetails.data.SolvedCalls.solvedcalls}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelActiveCalls = new Label
                {
                    Text = $"Llamadas Activas: {jsonAgentDetails.data.ActiveCalls.activecalls}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                panelAgentDetails.Controls.AddRange(new Control[] { labelNombreAgente, labelEmail, labelRol, labelStatus, labelSolvedChats,
                labelActiveChats,labelSolvedCalls,labelActiveCalls});
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[AgentDetail][AgentMainPage] {ex.Message}");
            }

        }
    }
}
