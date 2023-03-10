
namespace LoginForms
{
    partial class WhatsApp
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
            this.cmbConnector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAgregarLabels = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.rtxtResponseMessage = new System.Windows.Forms.RichTextBox();
            this.rtxtSendMessage = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelChatId = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.lblPlatformIdentifier = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "WhatsApp connector";
            // 
            // cmbConnector
            // 
            this.cmbConnector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbConnector.FormattingEnabled = true;
            this.cmbConnector.Location = new System.Drawing.Point(12, 30);
            this.cmbConnector.Name = "cmbConnector";
            this.cmbConnector.Size = new System.Drawing.Size(645, 24);
            this.cmbConnector.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(319, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Telefono cliente formato: país + área + telefono";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(13, 104);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(644, 22);
            this.textBox1.TabIndex = 8;
            // 
            // btnAgregarLabels
            // 
            this.btnAgregarLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarLabels.Location = new System.Drawing.Point(122, 563);
            this.btnAgregarLabels.Name = "btnAgregarLabels";
            this.btnAgregarLabels.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarLabels.TabIndex = 11;
            this.btnAgregarLabels.Text = "Agregar";
            this.btnAgregarLabels.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(403, 563);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "Enviar";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // rtxtResponseMessage
            // 
            this.rtxtResponseMessage.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtResponseMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtResponseMessage.Location = new System.Drawing.Point(12, 180);
            this.rtxtResponseMessage.Name = "rtxtResponseMessage";
            this.rtxtResponseMessage.ReadOnly = true;
            this.rtxtResponseMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtxtResponseMessage.Size = new System.Drawing.Size(616, 329);
            this.rtxtResponseMessage.TabIndex = 15;
            this.rtxtResponseMessage.Text = "";
            // 
            // rtxtSendMessage
            // 
            this.rtxtSendMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtSendMessage.Location = new System.Drawing.Point(12, 532);
            this.rtxtSendMessage.Multiline = false;
            this.rtxtSendMessage.Name = "rtxtSendMessage";
            this.rtxtSendMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxtSendMessage.Size = new System.Drawing.Size(616, 25);
            this.rtxtSendMessage.TabIndex = 14;
            this.rtxtSendMessage.Text = "";
            this.rtxtSendMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtxtSendMessage_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Mensaje - emoticones: win + .";
            // 
            // labelChatId
            // 
            this.labelChatId.AutoSize = true;
            this.labelChatId.Location = new System.Drawing.Point(516, 74);
            this.labelChatId.Name = "labelChatId";
            this.labelChatId.Size = new System.Drawing.Size(0, 13);
            this.labelChatId.TabIndex = 17;
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(424, 73);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(0, 13);
            this.lblClient.TabIndex = 18;
            // 
            // lblPlatformIdentifier
            // 
            this.lblPlatformIdentifier.AutoSize = true;
            this.lblPlatformIdentifier.Location = new System.Drawing.Point(382, 73);
            this.lblPlatformIdentifier.Name = "lblPlatformIdentifier";
            this.lblPlatformIdentifier.Size = new System.Drawing.Size(0, 13);
            this.lblPlatformIdentifier.TabIndex = 19;
            // 
            // WhatsApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(670, 604);
            this.Controls.Add(this.lblPlatformIdentifier);
            this.Controls.Add(this.lblClient);
            this.Controls.Add(this.labelChatId);
            this.Controls.Add(this.rtxtResponseMessage);
            this.Controls.Add(this.rtxtSendMessage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnAgregarLabels);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbConnector);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WhatsApp";
            this.Text = "WhatsApp";
            this.Load += new System.EventHandler(this.WhatsApp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbConnector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAgregarLabels;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.RichTextBox rtxtResponseMessage;
        private System.Windows.Forms.RichTextBox rtxtSendMessage;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label labelChatId;
        public System.Windows.Forms.Label lblClient;
        public System.Windows.Forms.Label lblPlatformIdentifier;
    }
}