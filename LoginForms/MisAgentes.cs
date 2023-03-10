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
        string idAgent;
        string rolId = "1";
        
        public MisAgentes()
        {
            InitializeComponent();
            ShowAgents();
            //agentsInformation(GlobalSocket.currentUser.ID);
            //SupervisorAgents(rolId);
        }

        public MisAgentes(string individualId)
        {
            idAgent = individualId;
        }

        private void ShowAgents()
        {
            if(GlobalSocket.currentUser.rol.Id.ToString() == "3")
            {
                agentsInformation(GlobalSocket.currentUser.ID);
            }
            else
            {
                SupervisorAgents(rolId);
            }
        }

        private async void agentsInformation(string leaderId)
        {
            try
            {
                string myAgentesInformation = await rh.getMyAgents(leaderId);
                Json jsonMyAgentsInformation = JsonConvert.DeserializeObject<Json>(myAgentesInformation);
                for (int i = 0; i < jsonMyAgentsInformation.data.users.Count; i++)
                {
                    string individualId = jsonMyAgentsInformation.data.users[i].ID;

                    FlowLayoutPanel panelAgentInformation = new FlowLayoutPanel
                    {
                        BackColor = Color.FromArgb(145, 153, 179),
                        BorderStyle = BorderStyle.FixedSingle,
                        FlowDirection = FlowDirection.TopDown,
                        Size = new Size(320, 200)
                    };
                    flpAgentInfo.Controls.Add(panelAgentInformation);

                    LinkLabel labelAgentName = new LinkLabel
                    {
                        Text = $"Nombre Agente: {jsonMyAgentsInformation.data.users[i].name} {jsonMyAgentsInformation.data.users[i].paternalSurname} {jsonMyAgentsInformation.data.users[i].maternalSurname}",
                        LinkColor = Color.FromArgb(19, 34, 38),
                        VisitedLinkColor = Color.FromArgb(19, 34, 38),
                        ActiveLinkColor = Color.FromArgb(255, 255, 255),
                        Font = new Font("Microsoft Sans Serif", 11),
                        AutoSize = true,
                        LinkBehavior = LinkBehavior.NeverUnderline
                    };

                    Label labelActiveChats = new Label
                    {
                        Text = $"Chats Activos: {jsonMyAgentsInformation.data.users[i].activeChats}",
                        ForeColor = Color.FromArgb(19, 34, 38),
                        Font = new Font("Microsoft Sans Serif", 11),
                        AutoSize = true
                    };

                    Label labelEmail = new Label
                    {
                        Text = $"Email: {jsonMyAgentsInformation.data.users[i].email}",
                        ForeColor = Color.FromArgb(19, 34, 38),
                        Font = new Font("Microsoft Sans Serif", 11),
                        AutoSize = true
                    };

                    Button buttonChangeAgentStatus = new Button
                    {
                        Name = $"btnChangeAgentStatus",
                        Text = $"Cambiar estatus agente",
                        Font = new Font("Microsoft Sans Serif", 10),
                        Size = new Size(210, 35),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Black
                    };

                    Button buttonCheckAgents = new Button
                    {
                        Name = $"buttonCheckAgents",
                        Text = $"Consultar Agentes Asignados",
                        Font = new Font("Microsoft Sans Serif", 10),
                        Size = new Size(210, 35),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Black
                    };

                    Button buttonChangeMaxActiveChats = new Button
                    {
                        Name = $"btnChangeMaxActiveChats",
                        Text = $"Cambiar chats simultaneos",
                        Font = new Font("Microsoft Sans Serif", 10),
                        Size = new Size(210, 35),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Black
                    };
                    
                    if (jsonMyAgentsInformation.data.users[i].rolID == "2" || jsonMyAgentsInformation.data.users[i].rolID =="3")
                    {
                        panelAgentInformation.Controls.AddRange(new Control[] { labelAgentName, labelEmail, buttonChangeAgentStatus, buttonCheckAgents, buttonChangeMaxActiveChats });

                        labelAgentName.Click += (s, e) =>
                        {
                            AgentInformation agentInformation = new AgentInformation(individualId);
                            agentInformation.ShowDialog();
                        };

                        buttonChangeAgentStatus.Click += (s, e) =>
                        {
                            ChangeAgentStatus agentStatus = new ChangeAgentStatus(individualId);
                            agentStatus.ShowDialog();
                        };

                        buttonCheckAgents.Click += (s, e) =>
                        {
                            CheckAgents checkAgents = new CheckAgents(individualId);
                            checkAgents.ShowDialog();
                        };

                        buttonChangeMaxActiveChats.Click += (s, e) =>
                        {
                            ChangeMaxActiveChats agentMaxChats = new ChangeMaxActiveChats(individualId);
                            agentMaxChats.ShowDialog();
                        };
                    }
                    else
                    {
                        panelAgentInformation.Controls.AddRange(new Control[] { labelAgentName, labelEmail });
                    }
                }
            }
            
            catch(Exception ex)
            {
                Console.WriteLine($"Error[agentInfo]: {ex.Message}");
            }

        }

        private async void SupervisorAgents(string id)
        {
            string supervisorAgents = await rh.getSupervisorAgents(id);
            Json jsonSupervisorAgents = JsonConvert.DeserializeObject<Json>(supervisorAgents);
            for (int i = 0; i< jsonSupervisorAgents.data.users.Count; i ++)
            {
                FlowLayoutPanel panelAgentInformation = new FlowLayoutPanel
                {
                    BackColor = Color.FromArgb(145, 153, 179),
                    BorderStyle = BorderStyle.FixedSingle,
                    FlowDirection = FlowDirection.TopDown,
                    Size = new Size(320, 200)
                };
                flpAgentInfo.Controls.Add(panelAgentInformation);

                LinkLabel labelAgentName = new LinkLabel
                {
                    Text = $"Nombre Agente: {jsonSupervisorAgents.data.users[i].name} {jsonSupervisorAgents.data.users[i].paternalSurname} {jsonSupervisorAgents.data.users[i].maternalSurname}",
                    LinkColor = Color.FromArgb(19, 34, 38),
                    VisitedLinkColor = Color.FromArgb(19, 34, 38),
                    ActiveLinkColor = Color.FromArgb(255, 255, 255),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true,
                    LinkBehavior = LinkBehavior.NeverUnderline
                };

                Label labelActiveChats = new Label
                {
                    Text = $"Chats Activos: {jsonSupervisorAgents.data.users[i].activeChats}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                Label labelEmail = new Label
                {
                    Text = $"Email: {jsonSupervisorAgents.data.users[i].email}",
                    ForeColor = Color.FromArgb(19, 34, 38),
                    Font = new Font("Microsoft Sans Serif", 11),
                    AutoSize = true
                };

                panelAgentInformation.Controls.AddRange(new Control[] { labelAgentName,labelActiveChats,labelEmail });
            }
        }

    }
}

