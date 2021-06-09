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
using LoginForms.Models;
using LoginForms.Shared;
using LoginForms.Utils;
using Newtonsoft.Json;
using Label = System.Windows.Forms.Label;

namespace LoginForms
{
    public partial class FormPrincipal : Form
    {
        //Json json = JsonConvert.DeserializeObject<Json>();
        WhatsApp whatsApp;
        Prueba prueba;
        //RestHelper rh = new RestHelper();
        AsynchronousClient client;
        RestHelper rh = new RestHelper();
        public string rolId;
        Json jsonStatus;

        public FormPrincipal()
        {
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
                //string permissions = await rh.getPermissions(rolId);
                //Label labelAccount = new Label();
                //labelAccount.Name = "userInfo";
                //labelAccount.Text = "Administrador";
                //labelAccount.Font = new Font("Arial", 11);
                //Controls.Add(labelAccount);
                //flpDynamicButtons.Controls.Add(labelAccount);


                for (int i = 0; i < GlobalSocket.currentUser.rol.permission.Count; i++)
                {
                    System.Windows.Forms.Button dynamicButton = new System.Windows.Forms.Button();
                    Console.WriteLine(dynamicButton.Name);
                    dynamicButton.Height = 40;
                    dynamicButton.Width = 95;
                    dynamicButton.Text = $"{GlobalSocket.currentUser.rol.permission[i].menu.description}";

                    //Controls.Add(dynamicButton);  ////se comenta para definir bien su utilidad 

                    Assembly asm = Assembly.GetEntryAssembly();
                    Type formtype = asm.GetType(string.Format("{0}.{1}", "LoginForms", GlobalSocket.currentUser.rol.permission[i].menu.name));

                    Form f = (Form)Activator.CreateInstance(formtype);
                    //f.Show();
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
                        client.prueba = f as Prueba;
                    };
                    flpDynamicButtons.Controls.Add(dynamicButton);
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
                ForeColor = Color.FromArgb(19, 34, 38),
                Font = new Font("Microsoft Sans Serif", 10),
                AutoSize = true
            };

            Label labelAgentRol = new Label
            {
                Text = $"{GlobalSocket.currentUser.rol.name}",
                ForeColor = Color.FromArgb(19, 34, 38),
                Font = new Font("Microsoft Sans Serif", 10),
                AutoSize = true
            };

            flpAgentStatus.Controls.AddRange(new Control[] { labelAgentName, labelAgentRol});
        }


        private async void comboBoxGetUserStatus()
        {
            try
            {
                string jsonUserStatus = await rh.getUserStatus();
                jsonStatus = JsonConvert.DeserializeObject<Json>(jsonUserStatus);

                for (int i = 0; i < jsonStatus.data.status.Count;  i++)
                {
                    cmbUserStatus.Items.Add(new ListItem(jsonStatus.data.status[i].status, jsonStatus.data.status[i].id));
                }
            }
            catch(Exception ex)
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
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[cmbUserStatus_SelectedIndexChanged] {ex.Message}");
            }
        }
    }
}
