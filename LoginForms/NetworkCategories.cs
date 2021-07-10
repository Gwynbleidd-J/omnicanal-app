using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Shared;
using LoginForms.Models;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using LoginForms.Utils;

namespace LoginForms
{
    public partial class NetworkCategories : Form
    {

        //Formulario Hijo
        string chatid;
        Json jsonNetwork;
        RestHelper rh = new RestHelper();
        string valor;
        public NetworkCategories(string chatId)
        {
            InitializeComponent();
            chatid = chatId;
            //updateNetworkCategory();
        }

        private async void NetworkCategories_Load(object sender, EventArgs e)
        {
            await rh.getNetworkCategories();
            comboBoxGetNetworkCategory();
        }

        private async void comboBoxGetNetworkCategory()
        {
            try { 
                string jsonNetworkCategories = await rh.getNetworkCategories();
                jsonNetwork = JsonConvert.DeserializeObject<Json>(jsonNetworkCategories);

                for (int i = 0; i < jsonNetwork.data.networks.Count; i++)
                {
                    cmbNetwork.Items.Add(new ListItem(jsonNetwork.data.networks[i].name, jsonNetwork.data.networks[i].id));
                    //cmbNetwork.Items.AddRange(new string[] { jsonNetwork.data.networks[i].id});
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[getNetworkCategories] {ex}");
            }
        }

        //private async void updateNetworkCategory()
        //{
        //    try 
        //    {
        //        string jsonNetworkCategories = await rh.getNetworkCategories();
        //        jsonNetwork = JsonConvert.DeserializeObject<Json>(jsonNetworkCategories);

        //        string cmbItemId;

        //        for (int i = 0; i < jsonNetwork.data.networks.Count; i++)
        //        {
        //            cmbNetwork.Items.Add(new ListItem(jsonNetwork.data.networks[i].name, jsonNetwork.data.networks[i].id));
        //        }

        //        cmbItemId = cmbNetwork.SelectedIndex.ToString();

        //        string networkCategory = await rh.updateNetworkCategories(chatid, cmbItemId);
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine($"Error[updateNetworkCategory] {ex.Message}");
        //    }

        //}

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    await rh.updateNetworkCategories(chatid, valor);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No has seleccionado una sucursal", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[btnAccept] {ex.Message}");
            }

        }

        private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show($"Nombre: {cmbNetwork.SelectedItem}, Index: {cmbNetwork.SelectedValue}");
                valor = "";
                for (int i = 0; i < jsonNetwork.data.networks.Count; i++)
                {
                    if (jsonNetwork.data.networks[i].name == cmbNetwork.SelectedItem.ToString())
                    {
                        valor = jsonNetwork.data.networks[i].id.ToString();
                    }
                }
               // MessageBox.Show($"Nombre: {cmbNetwork.SelectedItem}, Index: {valor}");
                // cmbNetwork.SelectedValue.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error [cmbNetwork_SelectedIndexChanged] {ex.Message}");
            }
        }
    }
}
