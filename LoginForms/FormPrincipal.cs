using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using LoginForms.Models;
using LoginForms.Shared;
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
        public string rolId;
        string valor = "";

        public string TextSocket
        {
            get { return lblSocket.Text; }
            set { lblSocket.Text = value; }
        }

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
            GlobalSocket.algo = cmbUserStatus;

            //webchat.TopLevel = false;
            //webchat.Parent = pnlChatMessages;
            //webchat.ControlBox = false;
            //prueba.Show();
            client = new AsynchronousClient(whatsApp.rtxtResponseMessage, this, prueba, this);
            //client.inicializarChatWindow();
            //this.BackColor = ColorTranslator.FromHtml("#e2e0e0");
            this.BackColor = ColorTranslator.FromHtml("#d6d2d2");


            tableLayoutPanel2.BackColor = ColorTranslator.FromHtml("#1a1a26");
        }


        private async void FormPrincipal_Load(object sender, EventArgs e)
        {
            //Task task = new Task(client.Connect);
            //task.Start();
            dynamicUserButtons();
            labelAgentStatus();
            comboBoxGetUserStatus();
            setStatusAgent();
            createAgentInformationForm();
            await rh.SetStatusTime(GlobalSocket.currentUser.status.id);
            await SocketIOClient.ClienteSocketIO();
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
                tableLayoutPanel8.ColumnCount = GlobalSocket.currentUser.rol.permission.Count;
                //tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

                for (int i = 0; i < GlobalSocket.currentUser.rol.permission.Count; i++)
                {
                    System.Windows.Forms.Button dynamicButton = new System.Windows.Forms.Button();
                    Console.WriteLine(dynamicButton.Name);
                    dynamicButton.Height = 40;
                    dynamicButton.Width = 95;
                    dynamicButton.Text = $"{GlobalSocket.currentUser.rol.permission[i].menu.description}";
                    var menu = $"{GlobalSocket.currentUser.rol.permission[i].menu.description}";


                    //PictureBox dynamicImage
                    System.Windows.Forms.PictureBox dynamicImage = new PictureBox();
                    dynamicImage.Size = new Size(Width, tableLayoutPanel8.Height);
                    dynamicImage.SizeMode = PictureBoxSizeMode.Zoom;

                    Assembly asm = Assembly.GetEntryAssembly();
                    Type formtype = asm.GetType(string.Format("{0}.{1}", "LoginForms", GlobalSocket.currentUser.rol.permission[i].menu.name));
                    Form f = (Form)Activator.CreateInstance(formtype);
                    //f.Show();


                    void EjecucionPictureHomeclick(Form fo)
                    {
                        try
                        {
                            fo.TopLevel = false;
                            fo.Parent = pnlChatMessages;
                            fo.ControlBox = false;
                            fo.BringToFront();
                            fo.Location = new Point(0, 0);
                            fo.Dock = DockStyle.Fill;
                            fo.Focus();
                            fo.Show();
                            fo.FormBorderStyle = FormBorderStyle.None;
                            fo.BackColor = ColorTranslator.FromHtml("#e2e0e1");
                            client.prueba = fo as Prueba;

                            var temp1 = (PictureBox)tableLayoutPanel8.Controls["Chats"];
                            var temp2 = (PictureBox)tableLayoutPanel8.Controls["Softphone"];
                            if (temp1 != null)
                            {
                                temp1.Image = Properties.Resources.chat;

                            }
                            if (temp2 != null)
                            {
                                temp2.Image = Properties.Resources.llamadas;
                            }

                            dynamicImage.Image = Properties.Resources.home_presionado;
                        }
                        catch (Exception _e)
                        {
                            throw _e;
                        }
                    }

                    void EjecucionPictureChatsclick(Form fo)
                    {
                        try
                        {
                            fo.TopLevel = false;
                            fo.Parent = pnlChatMessages;
                            fo.ControlBox = false;
                            fo.BringToFront();
                            fo.Location = new Point(0, 0);
                            fo.Dock = DockStyle.Fill;
                            fo.Focus();
                            fo.Show();
                            fo.FormBorderStyle = FormBorderStyle.None;
                            fo.BackColor = ColorTranslator.FromHtml("#e2e0e1");
                            client.prueba = fo as Prueba;

                            var temp1 = (PictureBox)tableLayoutPanel8.Controls["Home"];
                            var temp2 = (PictureBox)tableLayoutPanel8.Controls["Softphone"];
                            if (temp1 != null)
                            {
                                temp1.Image = Properties.Resources.home;

                            }
                            if (temp2 != null)
                            {
                                temp2.Image = Properties.Resources.llamadas;
                            }

                            dynamicImage.Image = Properties.Resources.chat_presionado;
                        }
                        catch (Exception _e)
                        {
                            throw _e;
                        }
                    }


                    if (menu == "Chats")
                    {
                        
                        dynamicImage.Image = Properties.Resources.chat;
                        dynamicImage.Name = "Chats";
                        dynamicImage.Click += (s, e) =>
                        {
                            EjecucionPictureChatsclick(f);

                        };

                    }
                    else if (menu == "Softphone")
                    {
                        
                        dynamicImage.Image = Properties.Resources.llamadas;
                        dynamicImage.Name = "Softphone";

                        cmbUserStatus.SelectedIndexChanged += (e, a) => 
                        {
                            if(cmbUserStatus.SelectedItem.ToString() == "Disponible" && GlobalSocket.currentUser.rol.Id == 1)
                            {
                                CreateSoftphoneForm(f);
                                MethodInfo m = f.GetType().GetMethod("Connect");
                                m.Invoke(f, null);
                            }
                            else
                            {
                                MethodInfo m = f.GetType().GetMethod("Disconnect");
                                m.Invoke(f, null);
                                //Hace falta que se haga algo ya que la variable siPInited la detecta false y tiene que ser true
                            }
                        };

                        dynamicImage.Click += (s, e) =>
                        {
                            //softphone = f;
                            //f.TopLevel = false;
                            //f.Parent = pnlChatMessages;
                            //f.ControlBox = false;
                            //f.BringToFront();
                            //f.Location = new Point(0, 0);
                            //f.Dock = DockStyle.Fill;
                            //f.Focus();
                            //f.Show()
                            CreateSoftphoneForm(f);
                            //f.FormBorderStyle = FormBorderStyle.None;
                            //f.BackColor = ColorTranslator.FromHtml("#e2e0e1");
                            //client.prueba = f as Prueba;
                            //CreateSoftphoneForm(f);

                            var temp1 = (PictureBox)tableLayoutPanel8.Controls["Home"];
                            var temp2 = (PictureBox)tableLayoutPanel8.Controls["Chats"];
                            temp1.Image = Properties.Resources.home;
                            temp2.Image = Properties.Resources.chat;
                            dynamicImage.Image = Properties.Resources.llamadas_presionado;
                            //Console.WriteLine(softphone.Text);
                            //chats.Dispose();
                            //dashboard.Dispose();
                        };

                    }
                    else if (menu == "Panalla inicial agente")
                    {
                        
                        dynamicImage.SizeMode = PictureBoxSizeMode.Zoom;
                        dynamicImage.Image = Properties.Resources.home;
                        dynamicImage.Name = "Home";


                        dynamicImage.Click += (s, e) =>
                        {
                            EjecucionPictureHomeclick(f);

                        };

                    }

                    tableLayoutPanel8.Controls.Add(dynamicImage, i, 0);
                    dynamicImage.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
                    dynamicImage.Dock = DockStyle.Fill;

                    var tempHome = (PictureBox)tableLayoutPanel8.Controls["Home"];
                    //void tempHomeOnClick(object sender, EventArgs e){ 
                    //    base.OnClick(e);
                    //}

                    

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
                        f.BackColor = SystemColors.ButtonHighlight;
                        client.prueba = f as Prueba;
                    };


                    if (dynamicButton.Text != "Softphone" && dynamicButton.Text != "Chats")
                    {
                        flpDynamicButtons.Controls.Add(dynamicButton);
                    }

                    if (GlobalSocket.currentUser.rol.Id == 1)
                    {
                        flpDynamicButtons.Visible = false;
                        tableLayoutPanel8.Visible = true;

                        EjecucionPictureChatsclick(f);
                        EjecucionPictureHomeclick(f);
                    }
                    else
                    {
                        flpDynamicButtons.Visible = true;
                        tableLayoutPanel8.Visible = false;
                    }
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
                ForeColor = System.Drawing.Color.FromArgb(255, 255, 255),
                Font = new Font("Calibri", 12),
                //AutoSize = true,
                TextAlign = ContentAlignment.TopCenter,
            };

            Label labelAgentRol = new Label
            {
                Text = $"{GlobalSocket.currentUser.rol.name}",
                ForeColor = System.Drawing.Color.FromArgb(255, 255, 255),
                Font = new Font("Calibri", 12, FontStyle.Bold),
                //AutoSize = true,
                TextAlign = ContentAlignment.TopCenter,
            };

            flpAgentStatus.Controls.AddRange(new Control[] { labelAgentName, labelAgentRol });
            labelAgentName.Width = flpAgentStatus.Width;
            labelAgentRol.Size = new Size(flpAgentStatus.Width, Height / 2);


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
                    cmbUserStatus.Items.Add(new StatusItems(jsonStatus.data.status[i].status, jsonStatus.data.status[i].description, jsonStatus.data.status[i].id));
                    if(jsonStatus.data.status[i].id == "9")
                    {
                        cmbUserStatus.Items.RemoveAt(i);
                    }
                }
                //for (int i = 0; i < jsonStatus.data.status.Count; i++)
                //{
                //    cmbUserStatus.Items.Add(new ListItem(jsonStatus.data.status[i].status, jsonStatus.data.status[i].id));

                //}

                //Applicaction.ProductVersion
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
                //for (int i = 0; i < jsonStatus.data.status.Count; i++)
                //{
                //    if (jsonStatus.data.status[i].status == cmbUserStatus.SelectedItem.ToString())
                //    {
                //        valor = jsonStatus.data.status[i].id.ToString();
                //    }
                //}
                StatusItems statusItems = (StatusItems)cmbUserStatus.SelectedItem;
                valor = statusItems.Id;
                GlobalSocket.currentUser.status.id = valor;
                await rh.updateUserStatus(valor, userId);
                await rh.ChangeStatus(userId, valor);
                //await rh.TotalTimeStatus(GlobalSocket.currentUser.ID);
                

                MessageBox.Show("Estatus Agente Cambiado", "Estatus Agente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //if(cmbUserStatus.SelectedItem.ToString() == "Disponible")
                //{
                //}
                /*
                 * posiblemente aqui se tengan que usar hilos en los cuales se ejecute primera la cracion de la form del softphone y ya 
                 * despues lo de la instancia a dicha clase es lo unico que se me ocurre.
                 */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[cmbUserStatus_SelectedIndexChanged] {ex.Message}");
            }
        }

        private void btnCloseSesion_Click(object sender, EventArgs e)
        {
            login = new Login();
            string userToken = GlobalSocket.currentUser.token;
            userToken = "";

            if (string.IsNullOrEmpty(userToken))
            {
                //client.CloseSocketConnection();
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

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await rh.UpdateOnClosing(GlobalSocket.currentUser.ID, GlobalSocket.currentUser.status.id);
            await rh.updateUserStatus("8", GlobalSocket.currentUser.ID);
            //await rh.ChangeStatus() para el tiempo en los cambios de los estados
            pictureBox1.Image = Properties.Resources.cerrar_sesion_presionado_1;

            login = new Login();
            //asynchronousClient = new AsynchronousClient();
            string userToken = GlobalSocket.currentUser.token;
            userToken = "";

            if (string.IsNullOrEmpty(userToken))
            {
                await GlobalSocket.GlobalVarible.DisconnectAsync();
                MessageBox.Show("usuario cerró sesión", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
                login.Show();
            }
        }

        //private void pictureBox1_MouseHover(object sender, EventArgs e)
        //{
        //    pictureBox1.Image = Properties.Resources.cerrar_sesion_hover;
        //}

        //private void pictureBox1_MouseLeave(object sender, EventArgs e)
        //{
        //    pictureBox1.Image = Properties.Resources.cerrar_sesion_1;
        //}

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Properties.Resources.cerrar_sesion_1;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Properties.Resources.cerrar_sesion_presionado_1;
        }

        private async void btnReconexion_Click(object sender, EventArgs e)
        {

            string userToken = GlobalSocket.currentUser.token;
            userToken = "";

            if (string.IsNullOrEmpty(userToken))
            {
                //client.CloseSocketConnection();
            }

            //AsynchronousClient.Reconexion = true;
            //Thread.CurrentThread.Join(2000);
            await SocketIOClient.ClienteSocketIO();

            var temp = GlobalSocket.currentUser.ID.ToString();
            prueba.recoverActiveChats(temp);
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

        public void CreateSoftphoneForm(Form form)
        {
            form.Show();
            if (form.IsHandleCreated)
            {
                form.TopLevel = false;
                form.Parent = pnlChatMessages;
                form.ControlBox = false;
                form.BringToFront();
                form.Location = new Point(0, 0);
                form.Dock = DockStyle.Fill;
                form.Focus();
                form.GetType().GetMethod("Connect");
                form.FormBorderStyle = FormBorderStyle.None;
                form.BackColor = ColorTranslator.FromHtml("#e2e0e1");

                var temp2 = (PictureBox)tableLayoutPanel8.Controls["Softphone"];
                temp2.Image = Properties.Resources.llamadas_presionado;

                var temp1 = (PictureBox)tableLayoutPanel8.Controls["Home"];
                temp1.Image = Properties.Resources.home;
            }
        }

        public void DeRegisterFromServer(Form form)
        {
            CallsView calls = new CallsView();
            form.Show();
            calls.deRegisterFromServer();

        }
        private void SetProjectVersion()
        {
           // lblVersión.Text = $"Version:{Application.ProductVersion}";
        }
    }
    class StatusItems
    {
        public string Name;
        public string Value;
        public string Id;

        public StatusItems(string name, string value, string id)
        {
            Name = name;
            Value = value;
            Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
