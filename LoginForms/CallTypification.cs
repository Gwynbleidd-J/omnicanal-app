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
    public partial class CallTypification : Form
    {
        RestHelper rh = new RestHelper();
        Json jsonNetwork;
        string valor;
        //string userid;
        public CallTypification()//string userId
        {
            //userid = userId;
            InitializeComponent();
            ComboBoxGetNetwork();
        }
        
        private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            CallItems classItems = (CallItems)cmbNetwork.SelectedItem;
            valor = classItems.Id;
        }
        private async void ComboBoxGetNetwork()
        {
            try
            {
                string networkCategories = await rh.getNetworkCategories();
                Json jsonNetworkCategories = jsonNetwork = JsonConvert.DeserializeObject<Json>(networkCategories);
                for (int i = 0; i < jsonNetworkCategories.data.networks.Count; i++)
                {
                    cmbNetwork.Items.Add(new CallItems(jsonNetwork.data.networks[i].description, jsonNetwork.data.networks[i].typification, jsonNetwork.data.networks[i].id));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[getNetworkCategories] {ex}");
            }
        }

        private void txtScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }



        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbNetwork.Text) || !string.IsNullOrEmpty(txtScore.Text) || !string.IsNullOrEmpty(txtComments.Text))
                {
                    await rh.UpdateNetworkCategoryCalls(valor);
                    //MessageBox.Show("Datos guardados correctamente", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Llena todos los campos", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error[btnAccept] {ex.Message}");
            }
        }


    }

    class CallItems
    {
        public string Name;
        public string Value;
        public string Id;

        public CallItems(string name, string value, string id)
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
