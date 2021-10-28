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
    public partial class ChangeParameters : Form
    {
        RestHelper rh = new RestHelper();
        string credentialsId;
        public ChangeParameters()
        {
            InitializeComponent();
            GetParameters();
            ComboBoxGetUsers();
        }


        private async void GetParameters()
        {
            try
            {
                string appParameters = await rh.getAppParameters();
                Json jsonAppParameters = JsonConvert.DeserializeObject<Json>(appParameters);
                for (int i = 0; i < jsonAppParameters.data.botParameters.Count; i++)
                {
                    lblSIDTwilioAccount.Text = jsonAppParameters.data.botParameters[i].twilioAccountSID;
                    lblTokenTwilioAccount.Text = jsonAppParameters.data.botParameters[i].twilioAuthToken;
                    lblWhatsappAccount.Text = jsonAppParameters.data.botParameters[i].whatsappAccount;
                    lblBotTokenTelegram.Text = jsonAppParameters.data.botParameters[i].botTokenTelegram;

                    txtSIDTwilioAccount.Text = jsonAppParameters.data.botParameters[i].twilioAccountSID;
                    txtTokenTwilioAccount.Text = jsonAppParameters.data.botParameters[i].twilioAuthToken;
                    txtWhatsappAccount.Text = jsonAppParameters.data.botParameters[i].whatsappAccount;
                    txtBotTokenTelegram.Text = jsonAppParameters.data.botParameters[i].botTokenTelegram;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ChangeParameters: {ex.Message}");
            }
        }

        private async void SetBotParameters()
        {
            await rh.AppParameters(txtSIDTwilioAccount.Text, txtTokenTwilioAccount.Text, txtWhatsappAccount.Text, txtBotTokenTelegram.Text);
        }

        private void SetValuesOnLabels()
        {
            GetParameters();
        }

        private void ClearTextbox()
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtDomain.Clear();
            txtDisplayName.Clear();
            txtAuthName.Clear();
            txtServer.Clear();
            txtPort.Clear();
            txtSIDTwilioAccount.Clear();
            txtTokenTwilioAccount.Clear();
            txtWhatsappAccount.Clear();
            txtBotTokenTelegram.Clear();
        }

        private async void ComboBoxGetUsers()
        {
            string users = await rh.getUsers();
            Json jsonUsers = JsonConvert.DeserializeObject<Json>(users);
            for (int i = 0; i < jsonUsers.data.users.Count; i++)
            {
                cmbAgents.Items.Add(new ParametersItems(jsonUsers.data.users[i].name, jsonUsers.data.users[i].ID));
            }
        }


        private async void comboBoxGetSOftphoneCredentials()
        {
            ParametersItems classItems = (ParametersItems)cmbAgents.SelectedItem;
            string softphoneCredentials = await rh.SoftphoneParameters(classItems.Value);
            Json jsonSoftphoneCredentials = JsonConvert.DeserializeObject<Json>(softphoneCredentials);
            credentialsId = jsonSoftphoneCredentials.data.user.credentials.id;
            lblUserName.Text = jsonSoftphoneCredentials.data.user.credentials.userName;
            lblDisplayName.Text = jsonSoftphoneCredentials.data.user.credentials.displayName;
            lblDomain.Text = jsonSoftphoneCredentials.data.user.credentials.domain;
            lblServer.Text = jsonSoftphoneCredentials.data.user.credentials.server;
            lblPassword.Text = jsonSoftphoneCredentials.data.user.credentials.password;
            lblAuthName.Text = jsonSoftphoneCredentials.data.user.credentials.authName;
            lblPort.Text = jsonSoftphoneCredentials.data.user.credentials.port;

            txtUserName.Text = jsonSoftphoneCredentials.data.user.credentials.userName;
            txtDisplayName.Text = jsonSoftphoneCredentials.data.user.credentials.displayName;
            txtDomain.Text = jsonSoftphoneCredentials.data.user.credentials.domain;
            txtServer.Text = jsonSoftphoneCredentials.data.user.credentials.server;
            txtPassword.Text = jsonSoftphoneCredentials.data.user.credentials.password;
            txtAuthName.Text = jsonSoftphoneCredentials.data.user.credentials.authName;
            txtPort.Text = jsonSoftphoneCredentials.data.user.credentials.port;

        }


        private async Task<string> SetSoftphoneParameters()
        {
            string data = await rh.updateSoftphoneParameters(credentialsId, txtUserName.Text, txtDisplayName.Text, txtDomain.Text, txtServer.Text, txtPassword.Text, txtAuthName.Text, txtPort.Text);
            Console.WriteLine(data);
            return data;
        }

        private async void btnChangeSoftphoneParameters_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtDomain.Text) && !string.IsNullOrEmpty(txtDisplayName.Text) && !string.IsNullOrEmpty(txtAuthName.Text) && !string.IsNullOrEmpty(txtServer.Text) && !string.IsNullOrEmpty(txtPort.Text))
            {

                if (await SetSoftphoneParameters() == "OK")
                {
                    cmbAgents_SelectedIndexChanged(sender, e);
                    MessageBox.Show("Parametros del Softphone Actualizados Correctamente", "Omnicanal");
                }
                else
                {
                    MessageBox.Show("Hubo un error al actualizar la información de Softphone");
                }


                //ClearTextbox();
            }
            else
            {
                MessageBox.Show("Completa todos los campos", "Omnicanal");
            }
        }

        private void btnChangeBotsParameters_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSIDTwilioAccount.Text) && !string.IsNullOrEmpty(txtTokenTwilioAccount.Text) && !string.IsNullOrEmpty(txtWhatsappAccount.Text) && !string.IsNullOrEmpty(txtBotTokenTelegram.Text))
            {
                SetBotParameters();
                MessageBox.Show("Parametros de los bots de WhatsApp y Telegram Actualizados Correctamente", "Omnicanal");
                SetValuesOnLabels();
                //ClearTextbox();
            }
            else
            {
                MessageBox.Show("Completa todos los campos", "Omnicanal");
            }
        }

        private void cmbAgents_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxGetSOftphoneCredentials();
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }

    class ParametersItems
    {
        public string Name;
        public string Value;

        public ParametersItems(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
