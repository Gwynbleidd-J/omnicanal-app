
namespace LoginForms
{
    partial class ChangeAgentStatus
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAgentStatus = new System.Windows.Forms.ComboBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 34);
            this.label1.TabIndex = 6;
            this.label1.Text = "Selecciona un estado para cambiarlo\r\na un agente.\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbAgentStatus
            // 
            this.cmbAgentStatus.FormattingEnabled = true;
            this.cmbAgentStatus.Location = new System.Drawing.Point(130, 97);
            this.cmbAgentStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAgentStatus.Name = "cmbAgentStatus";
            this.cmbAgentStatus.Size = new System.Drawing.Size(128, 24);
            this.cmbAgentStatus.TabIndex = 5;
            this.cmbAgentStatus.Text = "Estado Agente";
            this.cmbAgentStatus.SelectedIndexChanged += new System.EventHandler(this.cmbAgentStatus_SelectedIndexChanged);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(145, 141);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(4);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(100, 28);
            this.btnAccept.TabIndex = 4;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // ChangeAgentStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 218);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAgentStatus);
            this.Controls.Add(this.btnAccept);
            this.Name = "ChangeAgentStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambiar Estatus Agente";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAgentStatus;
        private System.Windows.Forms.Button btnAccept;
    }
}