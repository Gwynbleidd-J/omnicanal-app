using System;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;
using LoginForms.Utils;  

namespace LoginForms
{
    public partial class ChangeAgentStatus : Form
    {
        RestHelper rh = new RestHelper();
        Json jsonStatus;
        string valor;
        string idAgent;
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ApplicationLogs\";
        public ChangeAgentStatus(string individualId)
        {
            idAgent = individualId;
            InitializeComponent();
            ComboBoxGetAgentStatus();
        }

        private async void ComboBoxGetAgentStatus()
        {
            Log log = new Log(appPath);
            try
            {
                string jsonUserStatus = await rh.getUserStatus();
                jsonStatus = JsonConvert.DeserializeObject<Json>(jsonUserStatus);
                log.Add($"[ChangeAgentStatus][ComboBoxGetAgentStatus]: Status Obtenidos:{jsonStatus.data.status.Count}");
                for (int i = 0; i < jsonStatus.data.status.Count; i++)
                {
                    cmbAgentStatus.Items.Add(new ListItem(jsonStatus.data.status[i].status, jsonStatus.data.status[i].id));
                }
            }
            catch (Exception ex)
            {
                log.Add($"[ChangeAgentStatus][ComboBoxGetAgentStatus]:{ex.Message}");
                Console.WriteLine($"Error[GetAgentStatus][ChangeAgentStatus]: {ex}");
                MessageBox.Show($"{ex}");
            }
        }

        private async void cmbAgentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            try
            {
                valor = "";
                log.Add($"[ChangeAgentStatus][cmbAgentStatus_SelectedIndexChanged]:{jsonStatus.data.status.Count}");
                for (int i = 0; i < jsonStatus.data.status.Count; i++)
                {
                    if (jsonStatus.data.status[i].status == cmbAgentStatus.SelectedItem.ToString())
                    {
                        valor = jsonStatus.data.status[i].id.ToString();
                    }
                }
                await rh.updateUserStatus(valor, idAgent);
            }
            catch (Exception ex)
            {
                log.Add($"[ChangeAgentStatus][cmbAgentStatus_SelectedIndexChanged]:{ex.Message}");
                Console.WriteLine($"Error[cmbUserStatus_SelectedIndexChanged] {ex.Message}");
            }
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            try
            {

                if (!string.IsNullOrEmpty(valor))
                {
                    await rh.updateUserStatus(valor, idAgent);
                    log.Add($"[ChangeAgentStatus][btnAccept_Click]: se guardo el statusId:{valor} del agente:{idAgent}");
                    MessageBox.Show("Estatus agente guardado correctamente", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No has seleccionado un estatus para el usuario", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                log.Add($"[ChangeAgentStatus][btnAccept_Click]:{ex.Message}");
                Console.WriteLine($"Error[btnAccept] {ex.Message}");
            }
        }
    }
}
