using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class MisAgentes : Form
    {
        RestHelper rh = new RestHelper();
        public MisAgentes()
        {
            InitializeComponent();
        }

        private void MisAgentes_Load(object sender, EventArgs e)
        {
            agentsInformation(GlobalSocket.currentUser.ID);
        }

        private async void agentsInformation(string leaderId)
        {
            try
            {
                string myAgentesInformation = await rh.getMyAgents(leaderId);
                Json jsonMyAgentsInformation = JsonConvert.DeserializeObject<Json>(myAgentesInformation);
                for (int i = 0; i < jsonMyAgentsInformation.data.users.Count; i++)
                {
                    string users = await rh.getMyAgents(jsonMyAgentsInformation.data.users[i].leaderId);
                    User user = new User();
                    Json jsonUsers = JsonConvert.DeserializeObject<Json>(users);
                    user = jsonUsers.data.users[i];
                    string individualId = user.ID;

                    FlowLayoutPanel panelAgentInformation = new FlowLayoutPanel
                    {
                        BackColor = Color.FromArgb(145, 153, 179),
                        BorderStyle = BorderStyle.FixedSingle,
                        FlowDirection = FlowDirection.TopDown,
                        Size = new Size(320, 100)
                    };
                    flpAgentInfo.Controls.Add(panelAgentInformation);

                    LinkLabel labelAgentName = new LinkLabel
                    {
                        Text = $"Nombre Agente: {user.name} {user.paternalSurname} {user.maternalSurname}",
                        LinkColor = Color.FromArgb(19, 34, 38),
                        VisitedLinkColor = Color.FromArgb(19, 34, 38),
                        ActiveLinkColor = Color.FromArgb(255, 255, 255),
                        Font = new Font("Microsoft Sans Serif", 11),
                        AutoSize = true,
                        LinkBehavior = LinkBehavior.NeverUnderline
                    };

                    Label labelActiveChats = new Label
                    {
                        Text = $"Chats Activos: {user.activeChats}",
                        ForeColor = Color.FromArgb(19, 34, 38),
                        Font = new Font("Microsoft Sans Serif", 11),
                        AutoSize = true
                    };
                    
                    
                    Label labelEmail = new Label 
                    { 
                        Text = $"Email: {user.email}",
                        ForeColor = Color.FromArgb(19, 34, 38),
                        Font = new Font("Microsoft Sans Serif", 11),
                        AutoSize = true
                    };
                    panelAgentInformation.Controls.AddRange(new Control[] { labelAgentName, labelEmail });
                    //panelAgentInformation.Controls.AddRange(new Control[] { labelAgentName, labelActiveChats, labelEmail });
                    
                    labelAgentName.Click += (s, e) =>
                    {
                        AgentInformation agentInformation = new AgentInformation(individualId);
                        agentInformation.ShowDialog();
                    };

                }
            }

            catch(Exception ex)
            {
                Console.WriteLine($"Error[agentInfo]: {ex.Message}");
            }

        }

    }
}

