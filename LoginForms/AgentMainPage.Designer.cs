
namespace LoginForms
{
    partial class AgentMainPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flpAgentInformation = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpAgentInformation
            // 
            this.flpAgentInformation.Location = new System.Drawing.Point(13, 13);
            this.flpAgentInformation.Name = "flpAgentInformation";
            this.flpAgentInformation.Size = new System.Drawing.Size(637, 449);
            this.flpAgentInformation.TabIndex = 0;
            // 
            // AgentMainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 474);
            this.Controls.Add(this.flpAgentInformation);
            this.Name = "AgentMainPage";
            this.Text = "Información Agente";
            this.Load += new System.EventHandler(this.AgentMainPage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpAgentInformation;
    }
}