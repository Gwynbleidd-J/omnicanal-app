
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flpAgentInfo = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvAgentesActivos = new System.Windows.Forms.DataGridView();
            this.NombreAgente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstatusActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChatsActivos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chatsCerrados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chatsMaximos = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.llamadasSalida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llamadasEntrada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llamadaActiva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Disponible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoDisponible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Capacitacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Calidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sanitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Break = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRecargar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentesActivos)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpAgentInfo
            // 
            this.flpAgentInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flpAgentInfo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpAgentInfo.Location = new System.Drawing.Point(31, 28);
            this.flpAgentInfo.Margin = new System.Windows.Forms.Padding(4);
            this.flpAgentInfo.Name = "flpAgentInfo";
            this.flpAgentInfo.Size = new System.Drawing.Size(931, 514);
            this.flpAgentInfo.TabIndex = 0;
            // 
            // dgvAgentesActivos
            // 
            this.dgvAgentesActivos.AllowUserToAddRows = false;
            this.dgvAgentesActivos.AllowUserToDeleteRows = false;
            this.dgvAgentesActivos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAgentesActivos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvAgentesActivos.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvAgentesActivos.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAgentesActivos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAgentesActivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgentesActivos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NombreAgente,
            this.EstatusActual,
            this.ChatsActivos,
            this.chatsCerrados,
            this.chatsMaximos,
            this.llamadasSalida,
            this.llamadasEntrada,
            this.llamadaActiva,
            this.Email,
            this.Disponible,
            this.NoDisponible,
            this.ACW,
            this.Capacitacion,
            this.Calidad,
            this.Sanitario,
            this.Comida,
            this.Break,
            this.ID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAgentesActivos.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAgentesActivos.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvAgentesActivos.Location = new System.Drawing.Point(3, 2);
            this.dgvAgentesActivos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvAgentesActivos.Name = "dgvAgentesActivos";
            this.dgvAgentesActivos.RowHeadersWidth = 51;
            this.dgvAgentesActivos.RowTemplate.Height = 24;
            this.dgvAgentesActivos.Size = new System.Drawing.Size(987, 458);
            this.dgvAgentesActivos.TabIndex = 1;
            this.dgvAgentesActivos.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAgentesActivos_CurrentCellDirtyStateChanged);
            // 
            // NombreAgente
            // 
            this.NombreAgente.HeaderText = "Nombre del Agente";
            this.NombreAgente.MinimumWidth = 6;
            this.NombreAgente.Name = "NombreAgente";
            this.NombreAgente.Width = 115;
            // 
            // EstatusActual
            // 
            this.EstatusActual.HeaderText = "Estatus Actual";
            this.EstatusActual.MinimumWidth = 6;
            this.EstatusActual.Name = "EstatusActual";
            this.EstatusActual.Width = 127;
            // 
            // ChatsActivos
            // 
            this.ChatsActivos.HeaderText = "Chats Activos";
            this.ChatsActivos.MinimumWidth = 6;
            this.ChatsActivos.Name = "ChatsActivos";
            this.ChatsActivos.Width = 121;
            // 
            // chatsCerrados
            // 
            this.chatsCerrados.HeaderText = "Chats Cerrados";
            this.chatsCerrados.MinimumWidth = 6;
            this.chatsCerrados.Name = "chatsCerrados";
            this.chatsCerrados.Width = 132;
            // 
            // chatsMaximos
            // 
            this.chatsMaximos.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.chatsMaximos.DisplayStyleForCurrentCellOnly = true;
            this.chatsMaximos.HeaderText = "Chats Maximos";
            this.chatsMaximos.MinimumWidth = 6;
            this.chatsMaximos.Name = "chatsMaximos";
            this.chatsMaximos.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chatsMaximos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chatsMaximos.Width = 134;
            // 
            // llamadasSalida
            // 
            this.llamadasSalida.HeaderText = "Llamadas Salida";
            this.llamadasSalida.MinimumWidth = 6;
            this.llamadasSalida.Name = "llamadasSalida";
            this.llamadasSalida.Width = 136;
            // 
            // llamadasEntrada
            // 
            this.llamadasEntrada.HeaderText = "Llamadas Entrada";
            this.llamadasEntrada.MinimumWidth = 6;
            this.llamadasEntrada.Name = "llamadasEntrada";
            this.llamadasEntrada.Width = 148;
            // 
            // llamadaActiva
            // 
            this.llamadaActiva.HeaderText = "Llamada Activa";
            this.llamadaActiva.MinimumWidth = 6;
            this.llamadaActiva.Name = "llamadaActiva";
            this.llamadaActiva.Width = 131;
            // 
            // Email
            // 
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 6;
            this.Email.Name = "Email";
            this.Email.Width = 77;
            // 
            // Disponible
            // 
            this.Disponible.HeaderText = "Disponible";
            this.Disponible.MinimumWidth = 6;
            this.Disponible.Name = "Disponible";
            this.Disponible.Width = 112;
            // 
            // NoDisponible
            // 
            this.NoDisponible.HeaderText = "No Disponible";
            this.NoDisponible.MinimumWidth = 6;
            this.NoDisponible.Name = "NoDisponible";
            this.NoDisponible.Width = 125;
            // 
            // ACW
            // 
            this.ACW.HeaderText = "ACW";
            this.ACW.MinimumWidth = 6;
            this.ACW.Name = "ACW";
            this.ACW.Width = 73;
            // 
            // Capacitacion
            // 
            this.Capacitacion.HeaderText = "Capacitacion";
            this.Capacitacion.MinimumWidth = 6;
            this.Capacitacion.Name = "Capacitacion";
            this.Capacitacion.Width = 127;
            // 
            // Calidad
            // 
            this.Calidad.HeaderText = "Calidad";
            this.Calidad.MinimumWidth = 6;
            this.Calidad.Name = "Calidad";
            this.Calidad.Width = 90;
            // 
            // Sanitario
            // 
            this.Sanitario.HeaderText = "Sanitario";
            this.Sanitario.MinimumWidth = 6;
            this.Sanitario.Name = "Sanitario";
            this.Sanitario.Width = 101;
            // 
            // Comida
            // 
            this.Comida.HeaderText = "Comida";
            this.Comida.MinimumWidth = 6;
            this.Comida.Name = "Comida";
            this.Comida.Width = 92;
            // 
            // Break
            // 
            this.Break.HeaderText = "Break";
            this.Break.MinimumWidth = 6;
            this.Break.Name = "Break";
            this.Break.Width = 78;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.Visible = false;
            this.ID.Width = 53;
            // 
            // btnRecargar
            // 
            this.btnRecargar.Location = new System.Drawing.Point(3, 2);
            this.btnRecargar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRecargar.Name = "btnRecargar";
            this.btnRecargar.Size = new System.Drawing.Size(147, 37);
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
            this.tableLayoutPanel1.Controls.Add(this.dgvAgentesActivos, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(993, 657);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnRecargar);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 508);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(474, 104);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = " Recargar Estados";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MisAgentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 681);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flpAgentInfo);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MisAgentes";
            this.Text = "SIDI Omnichannel";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentesActivos)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpAgentInfo;
        private System.Windows.Forms.DataGridView dgvAgentesActivos;
        private System.Windows.Forms.Button btnRecargar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreAgente;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstatusActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChatsActivos;
        private System.Windows.Forms.DataGridViewTextBoxColumn chatsCerrados;
        private System.Windows.Forms.DataGridViewComboBoxColumn chatsMaximos;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadasSalida;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadasEntrada;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadaActiva;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Disponible;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoDisponible;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACW;
        private System.Windows.Forms.DataGridViewTextBoxColumn Capacitacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Calidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sanitario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comida;
        private System.Windows.Forms.DataGridViewTextBoxColumn Break;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
    }
}