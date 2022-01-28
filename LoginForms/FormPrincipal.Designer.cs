
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
            this.btnCloseSesion = new System.Windows.Forms.Button();
            this.flpAgentStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUserStatus = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableDebug = new System.Windows.Forms.TableLayoutPanel();
            this.lblSocket = new System.Windows.Forms.Label();
            this.btnReconexion = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.flpDynamicButtons.SuspendLayout();
            this.pnlChatMessages.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableDebug.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTelegram
            // 
            this.btnTelegram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTelegram.Image = ((System.Drawing.Image)(resources.GetObject("btnTelegram.Image")));
            this.btnTelegram.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTelegram.Location = new System.Drawing.Point(4, 59);
            this.btnTelegram.Margin = new System.Windows.Forms.Padding(4);
            this.btnTelegram.Name = "btnTelegram";
            this.btnTelegram.Size = new System.Drawing.Size(127, 47);
            this.btnTelegram.TabIndex = 1;
            this.btnTelegram.Text = "Telegram";
            this.btnTelegram.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTelegram.UseVisualStyleBackColor = true;
            this.btnTelegram.Visible = false;
            this.btnTelegram.Click += new System.EventHandler(this.btnTelegram_Click);
            // 
            // btnLlamadas
            // 
            this.btnLlamadas.BackColor = System.Drawing.Color.DarkGray;
            this.btnLlamadas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLlamadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLlamadas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnLlamadas.Image = ((System.Drawing.Image)(resources.GetObject("btnLlamadas.Image")));
            this.btnLlamadas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLlamadas.Location = new System.Drawing.Point(4, 114);
            this.btnLlamadas.Margin = new System.Windows.Forms.Padding(4);
            this.btnLlamadas.Name = "btnLlamadas";
            this.btnLlamadas.Size = new System.Drawing.Size(127, 49);
            this.btnLlamadas.TabIndex = 2;
            this.btnLlamadas.Text = "Llamadas";
            this.btnLlamadas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLlamadas.UseVisualStyleBackColor = false;
            this.btnLlamadas.Visible = false;
            this.btnLlamadas.Click += new System.EventHandler(this.btnLlamadas_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(4, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 47);
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
            this.flpDynamicButtons.Location = new System.Drawing.Point(1237, 0);
            this.flpDynamicButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpDynamicButtons.Name = "flpDynamicButtons";
            this.flpDynamicButtons.Size = new System.Drawing.Size(139, 458);
            this.flpDynamicButtons.TabIndex = 8;
            this.flpDynamicButtons.Visible = false;
            // 
            // pnlChatMessages
            // 
            this.pnlChatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChatMessages.BackColor = System.Drawing.Color.Transparent;
            this.pnlChatMessages.Controls.Add(this.btnCloseSesion);
            this.pnlChatMessages.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnlChatMessages.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlChatMessages.Location = new System.Drawing.Point(140, 2);
            this.pnlChatMessages.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlChatMessages.Name = "pnlChatMessages";
            this.pnlChatMessages.Size = new System.Drawing.Size(1094, 571);
            this.pnlChatMessages.TabIndex = 9;
            // 
            // btnCloseSesion
            // 
            this.btnCloseSesion.BackgroundImage = global::LoginForms.Properties.Resources.cerrar_sesion_1;
            this.btnCloseSesion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseSesion.FlatAppearance.BorderSize = 0;
            this.btnCloseSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSesion.Location = new System.Drawing.Point(1057, 19);
            this.btnCloseSesion.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseSesion.Name = "btnCloseSesion";
            this.btnCloseSesion.Size = new System.Drawing.Size(256, 59);
            this.btnCloseSesion.TabIndex = 11;
            this.btnCloseSesion.UseVisualStyleBackColor = true;
            this.btnCloseSesion.Visible = false;
            this.btnCloseSesion.Click += new System.EventHandler(this.btnCloseSesion_Click);
            // 
            // flpAgentStatus
            // 
            this.flpAgentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flpAgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.flpAgentStatus.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.flpAgentStatus.Location = new System.Drawing.Point(478, 62);
            this.flpAgentStatus.Margin = new System.Windows.Forms.Padding(4);
            this.flpAgentStatus.Name = "flpAgentStatus";
            this.flpAgentStatus.Size = new System.Drawing.Size(209, 89);
            this.flpAgentStatus.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Snow;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cambia tu estado";
            // 
            // cmbUserStatus
            // 
            this.cmbUserStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbUserStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbUserStatus.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserStatus.FormattingEnabled = true;
            this.cmbUserStatus.Location = new System.Drawing.Point(4, 58);
            this.cmbUserStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cmbUserStatus.Name = "cmbUserStatus";
            this.cmbUserStatus.Size = new System.Drawing.Size(191, 30);
            this.cmbUserStatus.TabIndex = 12;
            this.cmbUserStatus.SelectedIndexChanged += new System.EventHandler(this.cmbUserStatus_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.BackgroundImage = global::LoginForms.Properties.Resources.header2;
            this.tableLayoutPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.flpAgentStatus, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1382, 155);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(694, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(685, 151);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::LoginForms.Properties.Resources.cerrar_sesion_1;
            this.pictureBox1.Location = new System.Drawing.Point(378, 49);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(233, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.cmbUserStatus, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(71, 32);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(200, 98);
            this.tableLayoutPanel6.TabIndex = 13;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableDebug, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 581);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1376, 99);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(209, 2);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(957, 95);
            this.tableLayoutPanel8.TabIndex = 13;
            // 
            // tableDebug
            // 
            this.tableDebug.ColumnCount = 1;
            this.tableDebug.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDebug.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableDebug.Controls.Add(this.lblSocket, 0, 0);
            this.tableDebug.Controls.Add(this.btnReconexion, 0, 1);
            this.tableDebug.Location = new System.Drawing.Point(1172, 2);
            this.tableDebug.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableDebug.Name = "tableDebug";
            this.tableDebug.RowCount = 2;
            this.tableDebug.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableDebug.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableDebug.Size = new System.Drawing.Size(178, 76);
            this.tableDebug.TabIndex = 12;
            // 
            // lblSocket
            // 
            this.lblSocket.AutoSize = true;
            this.lblSocket.Font = new System.Drawing.Font("Calibri Light", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSocket.Location = new System.Drawing.Point(3, 0);
            this.lblSocket.Name = "lblSocket";
            this.lblSocket.Size = new System.Drawing.Size(14, 21);
            this.lblSocket.TabIndex = 14;
            this.lblSocket.Text = " ";
            // 
            // btnReconexion
            // 
            this.btnReconexion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReconexion.Location = new System.Drawing.Point(3, 40);
            this.btnReconexion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReconexion.Name = "btnReconexion";
            this.btnReconexion.Size = new System.Drawing.Size(172, 34);
            this.btnReconexion.TabIndex = 15;
            this.btnReconexion.Text = "Reconectar";
            this.btnReconexion.UseVisualStyleBackColor = true;
            this.btnReconexion.Click += new System.EventHandler(this.btnReconexion_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.Controls.Add(this.flpDynamicButtons, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.pnlChatMessages, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1376, 575);
            this.tableLayoutPanel5.TabIndex = 14;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 161);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1382, 682);
            this.tableLayoutPanel7.TabIndex = 15;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1382, 853);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel7);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(1135, 809);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plataforma Omnicanal";
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.flpDynamicButtons.ResumeLayout(false);
            this.pnlChatMessages.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableDebug.ResumeLayout(false);
            this.tableDebug.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableDebug;
        private System.Windows.Forms.Button btnReconexion;
        public System.Windows.Forms.Label lblSocket;
    }
}