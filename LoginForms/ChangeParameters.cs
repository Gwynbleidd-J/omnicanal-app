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
using LoginForms.Utils;

namespace LoginForms
{
    public partial class ChangeParameters : Form
    {
        RestHelper rh = new RestHelper();
        string credentialsId;
        string userId;
        string tipoUsuario;
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ApplicationLogs\";
        public ChangeParameters()
        {
            InitializeComponent();
            //GetParameters();
            ComboBoxGetUsers();
            ComboBoxUserType();
        }

        //METODO QUE LLENA LOS COMBO BOX
        private async void ComboBoxGetUsers()
        {
            Log log = new Log(appPath);
            string users = await rh.getUsers();
            Json jsonUsers = JsonConvert.DeserializeObject<Json>(users);
           // log.Add($"[ChangeParameters][ComboBoxGetUsers]: Usuarios encontrados:{jsonUsers.data.users.Count}");
            for (int i = 0; i < jsonUsers.data.users.Count; i++)
            {
                if(jsonUsers.data.users[i].existe == 1)
                {
                    if (jsonUsers.data.users[i].rolID == "1" || jsonUsers.data.users[i].rolID == "2")
                    {
                        //cmbAgents.Items.Add(new ParametersItems(jsonUsers.data.users[i].name, jsonUsers.data.users[i].ID));
                        cmbUsers.Items.Add(new ParametersItems(jsonUsers.data.users[i].name, jsonUsers.data.users[i].ID));
                    }
                }
            }
        }

        private void ComboBoxUserType()
        {
            cmbTipoUsuario.Text = "Tipo Usuario";
            cmbTipoUsuario.Enabled = false;
            cmbTipoUsuario.Items.Add(new ParametersItems("Analista", "1"));
            cmbTipoUsuario.Items.Add(new ParametersItems("Supervisor", "2"));

        }
        
        private async void comboBoxGetSoftphoneCredentials()
        {
            Log log = new Log(appPath);
            ParametersItems classItems = (ParametersItems)cmbAgents.SelectedItem;
            string softphoneCredentials = await rh.SoftphoneParameters(classItems.Value);
            Json jsonSoftphoneCredentials = JsonConvert.DeserializeObject<Json>(softphoneCredentials);
            if (jsonSoftphoneCredentials.data.user.credentials == null)
            {
                credentialsId = "NA";
                lblUserName.Text = "NA";
                lblDisplayName.Text = "NA";
                lblDomain.Text = "NA";
                lblServer.Text = "NA";
                lblPassword.Text = "NA";
                lblAuthName.Text = "NA";
                lblPort.Text = "NA";

                txtUserName.Text = "NA";
                txtDisplayName.Text = "NA";
                txtDomain.Text = "NA";
                txtServer.Text = "NA";
                txtPassword.Text = "NA";
                txtAuthName.Text = "NA";
                txtPort.Text = "NA";
            }
            else
            {
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
               // log.Add($"[ChangeParameters][comboBoxGetSOftphoneCredentials]:Usuario Seleccionado:{jsonSoftphoneCredentials.data.user.credentials.userName}");
            }
        }

        private void cmbTipoUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersItems classItems = (ParametersItems)cmbTipoUsuario.SelectedItem;
            tipoUsuario = classItems.Value;
            Console.WriteLine($"Tipo Usuario: {tipoUsuario}");
        }

        private async void comboBoxGetUserData()
        {
            ParametersItems classItems = (ParametersItems)cmbUsers.SelectedItem;
            string usersData = await rh.GetUserData(classItems.Value);
            Json jsonUserData = JsonConvert.DeserializeObject<Json>(usersData);
            
            userId = jsonUserData.data.user.ID;
            lblNombre.Text = jsonUserData.data.user.name;
            lblApePaterno.Text = jsonUserData.data.user.paternalSurname;
            lblApeMaterno.Text = jsonUserData.data.user.maternalSurname;
            lblEmail.Text = jsonUserData.data.user.email;
            lblSiglasUser.Text = jsonUserData.data.user.siglasUser;

            txtNombre.Text = jsonUserData.data.user.name;
            txtApePaterno.Text = jsonUserData.data.user.paternalSurname;
            txtApeMaterno.Text = jsonUserData.data.user.maternalSurname;
            txtEmail.Text = jsonUserData.data.user.email;
            txtSiglasUser.Text = jsonUserData.data.user.siglasUser;
        }

        private async Task<string> SetSoftphoneParameters()
        {
            Log log = new Log(appPath);
            string data = await rh.updateSoftphoneParameters(credentialsId, txtUserName.Text, txtDisplayName.Text, txtDomain.Text, txtServer.Text, txtPassword.Text, txtAuthName.Text, txtPort.Text);
            Console.WriteLine(data);
          //  log.Add($"[ChangeParameters][SetSoftphoneParameters]:Respuesta desde servidor:{data}");
            return data;
        }

        private async Task<string> CreateUser()
        {
            Log log = new Log(appPath);
            string nombre = txtNombre.Text, apPaterno = txtApePaterno.Text, apMaterno = txtApeMaterno.Text, email = txtEmail.Text, contrasena = txtContrasena.Text, siglas = txtSiglasUser.Text;
            string data = await rh.SaveUser(nombre, apPaterno, apMaterno, email, contrasena, siglas, tipoUsuario);
            Console.WriteLine(data);
            //log.Add($"[ChangeParameters][CreateUser]:Respuesta desde servidor:{data}");
            return data;
        }

        private async Task<string> UpdateUser()
        {
            Log log = new Log(appPath);
            string nombre = txtNombre.Text, apPaterno = txtApePaterno.Text, apMaterno = txtApeMaterno.Text, email = txtEmail.Text, contrasena = txtContrasena.Text, siglas = txtSiglasUser.Text;
            string data = await rh.UpdateUser(userId, nombre, apPaterno, apMaterno, email, contrasena, siglas, tipoUsuario);
            Console.WriteLine(data);
          //  log.Add($"[ChangeParameters][UpdateUser]:Respuesta desde servidor:{data}");
            return data;
        }

        private async Task<string> DeleteUser()
        {
            Log log = new Log(appPath);
            string data = await rh.DeleteUser(userId);
            Console.WriteLine(data);
          //  log.Add($"[ChangeParameters][Deleteuser]:Respuesta desde servidor:{data}");
            return data;
        }

        private void cmbAgents_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxGetSoftphoneCredentials();
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxGetUserData();
        }

        private async void btnChangeSoftphoneParameters_Click(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtDomain.Text) && !string.IsNullOrEmpty(txtDisplayName.Text) && !string.IsNullOrEmpty(txtAuthName.Text) && !string.IsNullOrEmpty(txtServer.Text) && !string.IsNullOrEmpty(txtPort.Text))
            {

                if (await SetSoftphoneParameters() == "OK")
                {
                    cmbAgents_SelectedIndexChanged(sender, e);
                  //  log.Add($"[ChangeParameters][btnChangeSoftphoneParameters_Click]: respuesta desde el servidor:{SetSoftphoneParameters()}");
                    MessageBox.Show("Parametros del Softphone Actualizados Correctamente", "Omnicanal");
                }
                else
                {
                    MessageBox.Show("Hubo un error al actualizar la información de Softphone", "Omnichannel", MessageBoxButtons.OK);
                }


                //ClearTextbox();
            }
            else
            {
                MessageBox.Show("Completa todos los campos", "Omnichannel", MessageBoxButtons.OK);
            }
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

        private void rbNewUser_CheckedChanged(object sender, EventArgs e)
        {
            cmbUsers.Enabled = false;
            cmbTipoUsuario.Enabled = true;
            btnDeleteUser.Enabled = false;
            btnSaveUser.Enabled = true;
            btnModifyUser.Enabled = false;

            txtNombre.Enabled = true;
            txtApePaterno.Enabled = true;
            txtApeMaterno.Enabled = true;
            txtContrasena.Enabled = true;
            txtEmail.Enabled = true;
            txtSiglasUser.Enabled = true;

            cmbUsers.Text = "Selecciona un agente";
            txtNombre.Text = "";
            txtApePaterno.Text = "";
            txtApeMaterno.Text = "";
            txtContrasena.Text = "";
            txtEmail.Text = "";
            txtSiglasUser.Text = "";
            

            lblNombre.Text = "";
            lblApePaterno.Text = "";
            lblApeMaterno.Text = "";
            lblEmail.Text = "";
            lblSiglasUser.Text = "";
        }

        private void rbModifyUser_CheckedChanged(object sender, EventArgs e)
        {
            cmbUsers.Enabled = true;
            cmbUsers.Items.Clear();
            ComboBoxGetUsers();
            cmbTipoUsuario.Enabled = true;
            btnSaveUser.Enabled = false;
            btnModifyUser.Enabled = true;
            btnDeleteUser.Enabled = false;
            
            txtNombre.Enabled = true;
            txtApePaterno.Enabled = true;
            txtApeMaterno.Enabled = true;
            txtContrasena.Enabled = true;
            txtEmail.Enabled = true;
            txtSiglasUser.Enabled = true;
        }

        private void rbDeleteUser_CheckedChanged(object sender, EventArgs e)
        {
            cmbUsers.Enabled = true;
            cmbTipoUsuario.Enabled = false;
            btnDeleteUser.Enabled = true;
            btnSaveUser.Enabled = false;
            btnModifyUser.Enabled = false;

            txtNombre.Enabled = false;
            txtApePaterno.Enabled = false;
            txtApeMaterno.Enabled = false;
            txtContrasena.Enabled = false;
            txtEmail.Enabled = false;
            txtSiglasUser.Enabled = false;

            cmbUsers.Text = "Selecciona un agente";
            txtNombre.Text = "";
            txtApePaterno.Text = "";
            txtApeMaterno.Text = "";
            txtContrasena.Text = "";
            txtEmail.Text = "";
            txtSiglasUser.Text = "";

            lblNombre.Text = "";
            lblApePaterno.Text = "";
            lblApeMaterno.Text = "";
            lblEmail.Text = "";
            lblSiglasUser.Text = "";
        }

        private async void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                Log log = new Log(appPath);
                if(!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtApePaterno.Text) && !string.IsNullOrEmpty(txtApeMaterno.Text) && !string.IsNullOrEmpty(txtSiglasUser.Text) && !string.IsNullOrEmpty(txtContrasena.Text) && !string.IsNullOrEmpty(txtEmail.Text))
                {
                    if(await CreateUser() == "OK")
                    {
                        //cmbUsers_SelectedIndexChanged(sender, e);
                        //log.Add($"[ChangeParameters][btnSaveUser_Click]: respuesta desde el servidor:OK");
                        MessageBox.Show("Nuevo Analista Creado Correctamente", "Omnicanal");
                        txtNombre.Text = "";
                        txtApePaterno.Text = "";
                        txtApeMaterno.Text = "";
                        txtSiglasUser.Text = "";
                        txtContrasena.Text = "";
                        txtEmail.Text = "";
                        cmbTipoUsuario.Text = "Tipo Usuario";
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error al crear el analista", "Omnichannel", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Llena todos los campos", "Omnichannel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ChangeParameters][btnSaveUser_Click]: {ex.Message}");
            }
        }

        private async void btnModifyUser_Click(object sender, EventArgs e)
        {
            try
            {
                Log log = new Log(appPath);
                if (await UpdateUser() == "OK")
                {
                   // log.Add($"[ChangeParameters][btnSaveUser_Click]: respuesta desde el servidor:OK");
                    MessageBox.Show("Se cambiaron los datos del usuario correctamente", "Omnicanal");
                    txtNombre.Text = "";
                    txtApePaterno.Text = "";
                    txtApeMaterno.Text = "";
                    txtSiglasUser.Text = "";
                    txtContrasena.Text = "";
                    txtEmail.Text = "";
                    lblNombre.Text = "";
                    lblApePaterno.Text = "";
                    lblApeMaterno.Text = "";
                    lblEmail.Text = "";
                    lblSiglasUser.Text = "";
                    cmbTipoUsuario.Text = "Tipo Usuario";
                    cmbUsers.Items.Clear();
                    ComboBoxGetUsers();
                }
                else
                {
                    MessageBox.Show("Hubo un error al modificar el analista", "Omnichannel", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChangeParameters][btnSaveUser_Click]: {ex.Message}");
            }
        }

        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                Log log = new Log(appPath);
                if (await DeleteUser() == "OK")
                {
                   // log.Add($"[ChangeParameters][btnSaveUser_Click]: respuesta desde el servidor:OK");
                    MessageBox.Show("Se borro el usuario correctamente", "Omnicanal");
                    txtNombre.Text = "";
                    txtApePaterno.Text = "";
                    txtApeMaterno.Text = "";
                    txtSiglasUser.Text = "";
                    txtContrasena.Text = "";
                    txtEmail.Text = "";
                    lblNombre.Text = "";
                    lblApePaterno.Text = "";
                    lblApeMaterno.Text = "";
                    lblEmail.Text = "";
                    lblSiglasUser.Text = "";
                    cmbUsers.Text = "Selecciona un agente";
                    cmbUsers.Items.Clear();
                    ComboBoxGetUsers();
                }
                else
                {
                    MessageBox.Show("Hubo un error borrar al analista", "Omnichannel", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ChangeParameters][btnSaveUser_Click]: {ex.Message}");
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
