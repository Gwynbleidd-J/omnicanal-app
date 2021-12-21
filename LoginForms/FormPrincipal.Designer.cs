
namespace LoginForms
{
    partial class FormPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.btnTelegram = new System.Windows.Forms.Button();
            this.btnLlamadas = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.flpDynamicButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlChatMessages = new System.Windows.Forms.Panel();
            this.flpAgentStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUserStatus = new System.Windows.Forms.ComboBox();
            this.btnCloseSesion = new System.Windows.Forms.Button();
            this.flpDynamicButtons.SuspendLayout();
            this.flpAgentStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTelegram
            // 
            this.btnTelegram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTelegram.Image = ((System.Drawing.Image)(resources.GetObject("btnTelegram.Image")));
            this.btnTelegram.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTelegram.Location = new System.Drawing.Point(3, 47);
            this.btnTelegram.Name = "btnTelegram";
            this.btnTelegram.Size = new System.Drawing.Size(95, 38);
            this.btnTelegram.TabIndex = 1;
            this.btnTelegram.Text = "Telegram";
            this.btnTelegram.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTelegram.UseVisualStyleBackColor = true;
            this.btnTelegram.Visible = false;
            this.btnTelegram.Click += new System.EventHandler(this.btnTelegram_Click);
            // 
            // btnLlamadas
            // 
            this.btnLlamadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLlamadas.Image = ((System.Drawing.Image)(resources.GetObject("btnLlamadas.Image")));
            this.btnLlamadas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLlamadas.Location = new System.Drawing.Point(3, 91);
            this.btnLlamadas.Name = "btnLlamadas";
            this.btnLlamadas.Size = new System.Drawing.Size(95, 40);
            this.btnLlamadas.TabIndex = 2;
            this.btnLlamadas.Text = "Llamadas";
            this.btnLlamadas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLlamadas.UseVisualStyleBackColor = true;
            this.btnLlamadas.Visible = false;
            this.btnLlamadas.Click += new System.EventHandler(this.btnLlamadas_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 38);
            this.button1.TabIndex = 5;
            this.button1.Text = "Telegram";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // flpDynamicButtons
            // 
            this.flpDynamicButtons.BackColor = System.Drawing.Color.Transparent;
            this.flpDynamicButtons.Controls.Add(this.button1);
            this.flpDynamicButtons.Controls.Add(this.btnTelegram);
            this.flpDynamicButtons.Controls.Add(this.btnLlamadas);
            this.flpDynamicButtons.Location = new System.Drawing.Point(2, 110);
            this.flpDynamicButtons.Name = "flpDynamicButtons";
            this.flpDynamicButtons.Size = new System.Drawing.Size(175, 466);
            this.flpDynamicButtons.TabIndex = 8;
            // 
            // pnlChatMessages
            // 
            this.pnlChatMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChatMessages.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnlChatMessages.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlChatMessages.Location = new System.Drawing.Point(183, 4);
            this.pnlChatMessages.Name = "pnlChatMessages";
            this.pnlChatMessages.Size = new System.Drawing.Size(792, 639);
            this.pnlChatMessages.TabIndex = 9;
            // 
            // flpAgentStatus
            // 
            this.flpAgentStatus.Controls.Add(this.label1);
            this.flpAgentStatus.Controls.Add(this.cmbUserStatus);
            this.flpAgentStatus.Location = new System.Drawing.Point(6, 4);
            this.flpAgentStatus.Name = "flpAgentStatus";
            this.flpAgentStatus.Size = new System.Drawing.Size(171, 100);
            this.flpAgentStatus.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cambia tu estado.";
            // 
            // cmbUserStatus
            // 
            this.cmbUserStatus.FormattingEnabled = true;
            this.cmbUserStatus.Location = new System.Drawing.Point(3, 16);
            this.cmbUserStatus.Name = "cmbUserStatus";
            this.cmbUserStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbUserStatus.TabIndex = 0;
            this.cmbUserStatus.SelectedIndexChanged += new System.EventHandler(this.cmbUserStatus_SelectedIndexChanged);
            // 
            // btnCloseSesion
            // 
            this.btnCloseSesion.Location = new System.Drawing.Point(5, 583);
            this.btnCloseSesion.Name = "btnCloseSesion";
            this.btnCloseSesion.Size = new System.Drawing.Size(95, 32);
            this.btnCloseSesion.TabIndex = 11;
            this.btnCloseSesion.Text = "Cerrar Sesión";
            this.btnCloseSesion.UseVisualStyleBackColor = true;
            this.btnCloseSesion.Click += new System.EventHandler(this.btnCloseSesion_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 653);
            this.Controls.Add(this.btnCloseSesion);
            this.Controls.Add(this.flpAgentStatus);
            this.Controls.Add(this.pnlChatMessages);
            this.Controls.Add(this.flpDynamicButtons);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plataforma Omnicanal";
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.flpDynamicButtons.ResumeLayout(false);
            this.flpAgentStatus.ResumeLayout(false);
            this.flpAgentStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnTelegram;
        private System.Windows.Forms.Button btnLlamadas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.FlowLayoutPanel flpDynamicButtons;
        private System.Windows.Forms.Panel pnlChatMessages;
        private System.Windows.Forms.FlowLayoutPanel flpAgentStatus;
        private System.Windows.Forms.ComboBox cmbUserStatus;
        private System.Windows.Forms.Button btnCloseSesion;
        private System.Windows.Forms.Label label1;
    }
}