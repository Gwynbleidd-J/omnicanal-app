
namespace LoginForms
{
    partial class MisAgentes
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flpAgentInfo = new System.Windows.Forms.FlowLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.NombreAgente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChatsActivos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chatsCerrados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chatsMaximos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llamadaActiva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llamadasTerminadas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRecargar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpAgentInfo
            // 
            this.flpAgentInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flpAgentInfo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpAgentInfo.Location = new System.Drawing.Point(31, 28);
            this.flpAgentInfo.Margin = new System.Windows.Forms.Padding(4);
            this.flpAgentInfo.Name = "flpAgentInfo";
            this.flpAgentInfo.Size = new System.Drawing.Size(930, 514);
            this.flpAgentInfo.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NombreAgente,
            this.ChatsActivos,
            this.chatsCerrados,
            this.chatsMaximos,
            this.llamadaActiva,
            this.llamadasTerminadas,
            this.Email});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(987, 458);
            this.dataGridView1.TabIndex = 1;
            // 
            // NombreAgente
            // 
            this.NombreAgente.HeaderText = "Nombre del Agente";
            this.NombreAgente.MinimumWidth = 6;
            this.NombreAgente.Name = "NombreAgente";
            this.NombreAgente.ReadOnly = true;
            this.NombreAgente.Width = 115;
            // 
            // ChatsActivos
            // 
            this.ChatsActivos.HeaderText = "Chats Activos";
            this.ChatsActivos.MinimumWidth = 6;
            this.ChatsActivos.Name = "ChatsActivos";
            this.ChatsActivos.ReadOnly = true;
            this.ChatsActivos.Width = 121;
            // 
            // chatsCerrados
            // 
            this.chatsCerrados.HeaderText = "Chats Cerrados";
            this.chatsCerrados.MinimumWidth = 6;
            this.chatsCerrados.Name = "chatsCerrados";
            this.chatsCerrados.ReadOnly = true;
            this.chatsCerrados.Width = 132;
            // 
            // chatsMaximos
            // 
            this.chatsMaximos.HeaderText = "Chats Maximos";
            this.chatsMaximos.MinimumWidth = 6;
            this.chatsMaximos.Name = "chatsMaximos";
            this.chatsMaximos.ReadOnly = true;
            this.chatsMaximos.Width = 134;
            // 
            // llamadaActiva
            // 
            this.llamadaActiva.HeaderText = "Llamada Activa";
            this.llamadaActiva.MinimumWidth = 6;
            this.llamadaActiva.Name = "llamadaActiva";
            this.llamadaActiva.ReadOnly = true;
            this.llamadaActiva.Width = 131;
            // 
            // llamadasTerminadas
            // 
            this.llamadasTerminadas.HeaderText = "Llamadas terminadas";
            this.llamadasTerminadas.MinimumWidth = 6;
            this.llamadasTerminadas.Name = "llamadasTerminadas";
            this.llamadasTerminadas.ReadOnly = true;
            this.llamadasTerminadas.Width = 171;
            // 
            // Email
            // 
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 6;
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            this.Email.Width = 77;
            // 
            // btnRecargar
            // 
            this.btnRecargar.Location = new System.Drawing.Point(3, 528);
            this.btnRecargar.Name = "btnRecargar";
            this.btnRecargar.Size = new System.Drawing.Size(146, 37);
            this.btnRecargar.TabIndex = 2;
            this.btnRecargar.Text = "Recargar";
            this.btnRecargar.UseVisualStyleBackColor = true;
            this.btnRecargar.Click += new System.EventHandler(this.btnRecargar_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnRecargar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(993, 657);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // MisAgentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 681);
            this.Controls.Add(this.flpAgentInfo);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MisAgentes";
            this.Text = "MisAgentes";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpAgentInfo;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreAgente;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChatsActivos;
        private System.Windows.Forms.DataGridViewTextBoxColumn chatsCerrados;
        private System.Windows.Forms.DataGridViewTextBoxColumn chatsMaximos;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadaActiva;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadasTerminadas;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.Button btnRecargar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}