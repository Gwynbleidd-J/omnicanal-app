using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;

namespace LoginForms
{
    public partial class ChangeAgentStatus : Form
    {
        RestHelper rh = new RestHelper();
        Json jsonStatus;
        string valor;
        string idAgent;
        public ChangeAgentStatus(string individualId)
        {
            idAgent = individualId;
            InitializeComponent();
            ComboBoxGetAgentStatus();
        }

        private async void ComboBoxGetAgentStatus()
        {
            try
            {
                string jsonUserStatus = await rh.getUserStatus();
                jsonStatus = JsonConvert.DeserializeObject<Json>(jsonUserStatus);

                for (int i = 0; i < jsonStatus.data.status.Count; i++)
                {
                    cmbAgentStatus.Items.Add(new ListItem(jsonStatus.data.status[i].status, jsonStatus.data.status[i].id));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[GetAgentStatus][ChangeAgentStatus]: {ex}");
                MessageBox.Show($"{ex}");
            }
        }

        private async void cmbAgentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                valor = "";
                for (int i = 0; i < jsonStatus.data.status.Count; i++)
                {
                    if (jsonStatus.data.status[i].status == cmbAgentStatus.SelectedItem.ToString())
                    {
                        valor = jsonStatus.data.status[i].id.ToString();
                    }
                }
                await rh.updateUserStatus(valor, idAgent);
                MessageBox.Show("Estatus Agente Cambiado", "Estatus Agente", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[cmbUserStatus_SelectedIndexChanged] {ex.Message}");
            }
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    await rh.updateUserStatus(valor, idAgent);
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
                Console.WriteLine($"Error[btnAccept] {ex.Message}");
            }
        }
    }
}
