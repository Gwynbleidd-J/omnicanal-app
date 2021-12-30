using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Media;
using LoginForms.Models;
using LoginForms.Shared;
using LoginForms.Utils;
using Newtonsoft.Json;
using Label = System.Windows.Forms.Label;

namespace LoginForms
{
    public partial class FormPrincipal : Form
    {
        WhatsApp whatsApp;
        Prueba prueba;
        AsynchronousClient client;
        RestHelper rh = new RestHelper();
        Json jsonStatus;
        Login login;
        AsynchronousClient asynchronousClient = new AsynchronousClient();
        public string rolId;

        double dummy = 50;
        bool pestañaChatActiva = false;
        bool pestañaSoftphoneActiva = false;

        public FormPrincipal()//string agent
        {
            //agentStatus = agent
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            whatsApp = new WhatsApp();
            prueba = new Prueba();
            //this.IsMdiContainer = true;
            //whatsApp.MdiParent = this;
            //whatsApp.Show();
            //prueba.MdiParent = this;
            //prueba.Show();
            prueba.TopLevel = false;
            prueba.Parent = pnlChatMessages;
            prueba.ControlBox = false;

            //webchat.TopLevel = false;
            //webchat.Parent = pnlChatMessages;
            //webchat.ControlBox = false;
            //prueba.Show();
            client = new AsynchronousClient(whatsApp.rtxtResponseMessage, this, prueba, this);
            //client.inicializarChatWindow();
        }


        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Task task = new Task(client.Connect);
            task.Start();
            dynamicUserButtons();
            labelAgentStatus();
            comboBoxGetUserStatus();
            setStatusAgent();
            createAgentInformationForm();

            //this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            whatsApp.Show();
            //whatsApp.MdiParent = this;
            //abrirForm(new WhatsApp());
        }

        private void btnTelegram_Click(object sender, EventArgs e)
        {
            //Telegram telegram = new Telegram();
            //telegram.Show();
            //telegram.MdiParent = this;
            //Prueba prueba = new Prueba();
            prueba.Show();
            //prueba.MdiParent = this;
            //abrirForm(new Telegram());
        }

        private void btnLlamadas_Click(object sender, EventArgs e)
        {
            CallsView callsView = new CallsView();
            callsView.TopLevel = false;
            pnlChatMessages.Controls.Add(callsView);
            callsView.Show();
            //callsView.MdiParent = this;
            //abrirForm(new CallsView());
        }

        private void dynamicUserButtons()
        {
            try
            {
                tableLayoutPanel4.ColumnCount = GlobalSocket.currentUser.rol.permission.Count + 1;
                System.Windows.Forms.PictureBox homeImage = new PictureBox();
                homeImage.Height = 80;
                homeImage.Width = 135;
                homeImage.SizeMode = PictureBoxSizeMode.StretchImage;
                homeImage.Image = Properties.Resources.home;
                homeImage.Name = "Home";
                homeImage.Click += (e, s) =>
                {
                    var temp1 = (PictureBox)tableLayoutPanel4.Controls["Chats"];
                    var temp2 = (PictureBox)tableLayoutPanel4.Controls["Softphone"];
                    temp1.Image = Properties.Resources.chat;
                    temp2.Image = Properties.Resources.llamadas;
                    homeImage.Image = Properties.Resources.home_presionado;
                };

                tableLayoutPanel4.Controls.Add(homeImage,0,0);

                for (int i = 0; i < GlobalSocket.currentUser.rol.permission.Count; i++)
                {
                    System.Windows.Forms.Button dynamicButton = new System.Windows.Forms.Button();
                    Console.WriteLine(dynamicButton.Name);
                    dynamicButton.Height = 40;
                    dynamicButton.Width = 95;
                    dynamicButton.Text = $"{GlobalSocket.currentUser.rol.permission[i].menu.description}";
                    var menu = $"{GlobalSocket.currentUser.rol.permission[i].menu.description}";

                    System.Windows.Forms.PictureBox dynamicImage = new PictureBox();
                    dynamicImage.Height = 80;
                    dynamicImage.Width = 135;
                    dynamicImage.SizeMode = PictureBoxSizeMode.StretchImage;

                    Assembly asm = Assembly.GetEntryAssembly();
                    Type formtype = asm.GetType(string.Format("{0}.{1}", "LoginForms", GlobalSocket.currentUser.rol.permission[i].menu.name));

                    Form f = (Form)Activator.CreateInstance(formtype);
                    //f.Show();


                    if (menu == "Chats")
                    {
                        dynamicImage.Image = Properties.Resources.chat;
                        dynamicImage.Name = "Chats";
                        dynamicImage.Click += (s, e) =>
                        {
                            f.TopLevel = false;
                            f.Parent = pnlChatMessages;
                            f.ControlBox = false;
                            f.BringToFront();
                            f.Location = new Point(0, 0);
                            f.Dock = DockStyle.Fill;
                            f.Focus();
                            f.Show();
                            f.FormBorderStyle = FormBorderStyle.None;
                            f.BackColor = ColorTranslator.FromHtml("#e2e0e1");
                            client.prueba = f as Prueba;

                            var temp1 = (PictureBox)tableLayoutPanel4.Controls["Home"];
                            var temp2 = (PictureBox)tableLayoutPanel4.Controls["Softphone"];
                            temp1.Image = Properties.Resources.home;
                            temp2.Image = Properties.Resources.llamadas;
                            dynamicImage.Image = Properties.Resources.chat_presionado;

                        };

                    }
                    else if (menu == "Softphone")
                    {
                        dynamicImage.Image = Properties.Resources.llamadas;
                        dynamicImage.Name = "Softphone";
                        dynamicImage.Click += (s, e) =>
                        {
                            f.TopLevel = false;
                            f.Parent = pnlChatMessages;
                            f.ControlBox = false;
                            f.BringToFront();
                            f.Location = new Point(0, 0);
                            f.Dock = DockStyle.Fill;
                            f.Focus();
                            f.Show();
                            f.FormBorderStyle = FormBorderStyle.None;
                            f.BackColor = ColorTranslator.FromHtml("#e2e0e1");
                            client.prueba = f as Prueba;

                            var temp1 = (PictureBox)tableLayoutPanel4.Controls["Home"];
                            var temp2 = (PictureBox)tableLayoutPanel4.Controls["Chats"];
                            temp1.Image = Properties.Resources.home;
                            temp2.Image = Properties.Resources.chat;
                            dynamicImage.Image = Properties.Resources.llamadas_presionado;

                        };

                    }

                    dynamicButton.Click += (s, e) =>
                    {
                        f.TopLevel = false;
                        f.Parent = pnlChatMessages;
                        f.ControlBox = false;
                        f.BringToFront();
                        f.Location = new Point(0, 0);
                        f.Dock = DockStyle.Fill;
                        f.Focus();
                        f.Show();
                        f.FormBorderStyle = FormBorderStyle.None;
                        f.BackColor= SystemColors.ButtonHighlight;
                        client.prueba = f as Prueba;
                    };
                    flpDynamicButtons.Controls.Add(dynamicButton);
                    tableLayoutPanel4.Controls.Add(dynamicImage, i+1,0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DynamicButtons]Error:  {ex.Message}");
            }
        }

        private void labelAgentStatus()
        {
            Label labelAgentName = new Label
            {
                Text = $"{GlobalSocket.currentUser.name} {GlobalSocket.currentUser.paternalSurname} {GlobalSocket.currentUser.maternalSurname}",
                ForeColor = System.Drawing.Color.FromArgb(19, 34, 38),
                Font = new Font("Microsoft Sans Serif", 10),
                AutoSize = true
            };

            Label labelAgentRol = new Label
            {
                Text = $"{GlobalSocket.currentUser.rol.name}",
                ForeColor = System.Drawing.Color.FromArgb(19, 34, 38),
                Font = new Font("Microsoft Sans Serif", 10),
                AutoSize = true
            };

            flpAgentStatus.Controls.AddRange(new Control[] { labelAgentName, labelAgentRol});
        }

        private void setStatusAgent()
        {
            cmbUserStatus.Text = GlobalSocket.currentUser.status.status;
        }

        private async void comboBoxGetUserStatus()
        {
            try
            {
                string jsonUserStatus = await rh.getUserStatus();
                jsonStatus = JsonConvert.DeserializeObject<Json>(jsonUserStatus);

                for (int i = 0; i < jsonStatus.data.status.Count; i++)
                {
                    cmbUserStatus.Items.Add(new ListItem(jsonStatus.data.status[i].status, jsonStatus.data.status[i].id));
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[comboBoxGetUserStatus] {ex.Message}");
            }
        }

        private async void cmbUserStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string userId = GlobalSocket.currentUser.ID;
                string valor = "";
                for (int i = 0; i < jsonStatus.data.status.Count; i++)
                {
                    if (jsonStatus.data.status[i].status == cmbUserStatus.SelectedItem.ToString())
                    {
                        valor = jsonStatus.data.status[i].id.ToString();
                    }
                }
                await rh.updateUserStatus(valor, userId);
                MessageBox.Show("Estatus Agente Cambiado", "Estatus Agente", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[cmbUserStatus_SelectedIndexChanged] {ex.Message}");
            }
        }

        private void btnCloseSesion_Click(object sender, EventArgs e)
        {
            login = new Login();
            //asynchronousClient = new AsynchronousClient();
            string userToken = GlobalSocket.currentUser.token;
            userToken = "";

            if (string.IsNullOrEmpty(userToken))
            {
                asynchronousClient.CloseSocketConnection();
                MessageBox.Show("usuario cerró sesión", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
                login.Show();
            }

        }

        public string createAgentInformationForm()
        {
            string indidualId = GlobalSocket.currentUser.ID;
            return indidualId;
        }

        //Metodos que no recuerdo para que se utilizan, pero deben de tener una utilidad
        //no los borro ya que despues voy a evaluar si dejarlos o borrarlos.s

        //private void AddFormInPanel(Form form)
        //{
        //    if (this.pnlChatMessages.Controls.Count > 0)
        //        this.pnlChatMessages.Controls.RemoveAt(0);
        //    form.TopLevel = false;
        //    form.FormBorderStyle = FormBorderStyle.None;
        //    form.Dock = DockStyle.Fill;
        //    this.pnlChatMessages.Controls.Add(form);
        //    this.pnlChatMessages.Tag = form;
        //    form.Show();
        //}

        //private void btnAgentDetails_Click(object sender, EventArgs e)
        //{
        //    string userId = GlobalSocket.currentUser.ID;
        //    var form = Application.OpenForms.OfType<AgentMainPage>().FirstOrDefault();
        //    AgentMainPage agentMainPage = form ?? new AgentMainPage();
        //    AddFormInPanel(agentMainPage);
        //}
    }
}
