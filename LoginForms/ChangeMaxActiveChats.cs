using LoginForms.Shared;
using System;
using System.Windows.Forms;
using LoginForms.Utils;

namespace LoginForms
{
    public partial class ChangeMaxActiveChats : Form
    {
        RestHelper rh = new RestHelper();
        string AgentID;
        string valor;
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ApplicationLogs\";

        public ChangeMaxActiveChats(string agentId)
        {
            InitializeComponent();
            AgentID = agentId;
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            try
            {
                int intTemp;
                string temp = textBox1.Text;
                if (temp != null && int.TryParse(temp, out intTemp))
                {
                    if (intTemp > 0)
                    {
                        valor = temp;
                        await rh.updateAgentMaxActiveChats(AgentID, valor);
                      //  log.Add($"[ChangeMaxActiveChats][btnAccept_Click]: cambiando los chats maximos al agente:{AgentID} chats maximos{valor}");
                        MessageBox.Show("Chats simultaneos actualizados correctamente");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Por favor ingrese un dato positivo");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor ingrese un dato valido");
                }
            }
            catch (Exception _e)
            {
               // log.Add($"[ChangeMaxActiveChats][btnAccept_Click]:{_e.Message}");
                throw _e;
            }

        }
    }
}
