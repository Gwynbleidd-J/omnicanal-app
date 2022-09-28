
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
            this.dgvAgentesActivos = new System.Windows.Forms.DataGridView();
            this.btnRecargar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFiltroAgente = new System.Windows.Forms.Button();
            this.cChUserStatus = new LiveCharts.WinForms.CartesianChart();
            this.NombreAgente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstatusActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChatsActivos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chatsCerrados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chatsMaximos = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.llamadasSalida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llamadasEntrada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llamadaActiva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Disponible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoDisponible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Capacitacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Calidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sanitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Break = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Monitoreo = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentesActivos)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpAgentInfo
            // 
            this.flpAgentInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flpAgentInfo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpAgentInfo.Location = new System.Drawing.Point(17, -13);
            this.flpAgentInfo.Name = "flpAgentInfo";
            this.flpAgentInfo.Size = new System.Drawing.Size(698, 418);
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAgentesActivos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
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
            this.Disponible,
            this.NoDisponible,
            this.ACW,
            this.Capacitacion,
            this.Calidad,
            this.Sanitario,
            this.Comida,
            this.Break,
            this.ID,
            this.Monitoreo});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAgentesActivos.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAgentesActivos.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvAgentesActivos.Location = new System.Drawing.Point(2, 2);
            this.dgvAgentesActivos.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAgentesActivos.Name = "dgvAgentesActivos";
            this.dgvAgentesActivos.RowHeadersWidth = 51;
            this.dgvAgentesActivos.RowTemplate.Height = 24;
            this.dgvAgentesActivos.Size = new System.Drawing.Size(734, 259);
            this.dgvAgentesActivos.TabIndex = 1;
            this.dgvAgentesActivos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAgentesActivos_CellClick);
            this.dgvAgentesActivos.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAgentesActivos_CurrentCellDirtyStateChanged);
            // 
            // btnRecargar
            // 
            this.btnRecargar.Location = new System.Drawing.Point(2, 2);
            this.btnRecargar.Margin = new System.Windows.Forms.Padding(2);
            this.btnRecargar.Name = "btnRecargar";
            this.btnRecargar.Size = new System.Drawing.Size(116, 30);
            this.btnRecargar.TabIndex = 2;
            this.btnRecargar.Text = "Recargar";
            this.btnRecargar.UseVisualStyleBackColor = true;
            this.btnRecargar.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvAgentesActivos, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 10);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.35931F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.64069F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(738, 462);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cChUserStatus, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 268);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(732, 191);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnRecargar);
            this.flowLayoutPanel1.Controls.Add(this.btnFiltroAgente);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(142, 186);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnFiltroAgente
            // 
            this.btnFiltroAgente.BackgroundImage = global::LoginForms.Properties.Resources.Excel_Logo_1_;
            this.btnFiltroAgente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFiltroAgente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltroAgente.ForeColor = System.Drawing.Color.Transparent;
            this.btnFiltroAgente.Location = new System.Drawing.Point(2, 36);
            this.btnFiltroAgente.Margin = new System.Windows.Forms.Padding(2);
            this.btnFiltroAgente.Name = "btnFiltroAgente";
            this.btnFiltroAgente.Size = new System.Drawing.Size(72, 68);
            this.btnFiltroAgente.TabIndex = 3;
            this.btnFiltroAgente.UseVisualStyleBackColor = true;
            this.btnFiltroAgente.Click += new System.EventHandler(this.btnFiltroAgente_Click);
            // 
            // cChUserStatus
            // 
            this.cChUserStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cChUserStatus.Location = new System.Drawing.Point(149, 3);
            this.cChUserStatus.Name = "cChUserStatus";
            this.cChUserStatus.Size = new System.Drawing.Size(580, 185);
            this.cChUserStatus.TabIndex = 3;
            this.cChUserStatus.Text = "cartesianChart1";
            // 
            // NombreAgente
            // 
            this.NombreAgente.HeaderText = "Nombre del Agente";
            this.NombreAgente.MinimumWidth = 6;
            this.NombreAgente.Name = "NombreAgente";
            this.NombreAgente.Width = 94;
            // 
            // EstatusActual
            // 
            this.EstatusActual.HeaderText = "Estatus Actual";
            this.EstatusActual.MinimumWidth = 6;
            this.EstatusActual.Name = "EstatusActual";
            this.EstatusActual.Width = 104;
            // 
            // ChatsActivos
            // 
            this.ChatsActivos.HeaderText = "Chats Activos";
            this.ChatsActivos.MinimumWidth = 6;
            this.ChatsActivos.Name = "ChatsActivos";
            this.ChatsActivos.Width = 98;
            // 
            // chatsCerrados
            // 
            this.chatsCerrados.HeaderText = "Chats Cerrados";
            this.chatsCerrados.MinimumWidth = 6;
            this.chatsCerrados.Name = "chatsCerrados";
            this.chatsCerrados.Width = 107;
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
            this.chatsMaximos.Width = 108;
            // 
            // llamadasSalida
            // 
            this.llamadasSalida.HeaderText = "Llamadas Salida";
            this.llamadasSalida.MinimumWidth = 6;
            this.llamadasSalida.Name = "llamadasSalida";
            this.llamadasSalida.Width = 112;
            // 
            // llamadasEntrada
            // 
            this.llamadasEntrada.HeaderText = "Llamadas Entrada";
            this.llamadasEntrada.MinimumWidth = 6;
            this.llamadasEntrada.Name = "llamadasEntrada";
            this.llamadasEntrada.Width = 123;
            // 
            // llamadaActiva
            // 
            this.llamadaActiva.HeaderText = "Llamada Activa";
            this.llamadaActiva.MinimumWidth = 6;
            this.llamadaActiva.Name = "llamadaActiva";
            this.llamadaActiva.Width = 109;
            // 
            // Disponible
            // 
            this.Disponible.HeaderText = "Disponible";
            this.Disponible.MinimumWidth = 6;
            this.Disponible.Name = "Disponible";
            this.Disponible.Width = 91;
            // 
            // NoDisponible
            // 
            this.NoDisponible.HeaderText = "No Disponible";
            this.NoDisponible.MinimumWidth = 6;
            this.NoDisponible.Name = "NoDisponible";
            this.NoDisponible.Width = 101;
            // 
            // ACW
            // 
            this.ACW.HeaderText = "ACW";
            this.ACW.MinimumWidth = 6;
            this.ACW.Name = "ACW";
            this.ACW.Width = 60;
            // 
            // Capacitacion
            // 
            this.Capacitacion.HeaderText = "Capacitacion";
            this.Capacitacion.MinimumWidth = 6;
            this.Capacitacion.Name = "Capacitacion";
            this.Capacitacion.Width = 105;
            // 
            // Calidad
            // 
            this.Calidad.HeaderText = "Calidad";
            this.Calidad.MinimumWidth = 6;
            this.Calidad.Name = "Calidad";
            this.Calidad.Width = 74;
            // 
            // Sanitario
            // 
            this.Sanitario.HeaderText = "Sanitario";
            this.Sanitario.MinimumWidth = 6;
            this.Sanitario.Name = "Sanitario";
            this.Sanitario.Width = 83;
            // 
            // Comida
            // 
            this.Comida.HeaderText = "Comida";
            this.Comida.MinimumWidth = 6;
            this.Comida.Name = "Comida";
            this.Comida.Width = 75;
            // 
            // Break
            // 
            this.Break.HeaderText = "Break";
            this.Break.MinimumWidth = 6;
            this.Break.Name = "Break";
            this.Break.Width = 66;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.Visible = false;
            this.ID.Width = 46;
            // 
            // Monitoreo
            // 
            this.Monitoreo.HeaderText = "Pantalla Remota";
            this.Monitoreo.Name = "Monitoreo";
            this.Monitoreo.Text = "Monitorear";
            this.Monitoreo.Width = 97;
            // 
            // MisAgentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 481);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flpAgentInfo);
            this.Name = "MisAgentes";
            this.Text = "SIDI Omnichannel";
            this.Load += new System.EventHandler(this.MisAgentes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgentesActivos)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpAgentInfo;
        private System.Windows.Forms.DataGridView dgvAgentesActivos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnFiltroAgente;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private LiveCharts.WinForms.CartesianChart cChUserStatus;
        public System.Windows.Forms.Button btnRecargar;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreAgente;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstatusActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChatsActivos;
        private System.Windows.Forms.DataGridViewTextBoxColumn chatsCerrados;
        private System.Windows.Forms.DataGridViewComboBoxColumn chatsMaximos;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadasSalida;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadasEntrada;
        private System.Windows.Forms.DataGridViewTextBoxColumn llamadaActiva;
        private System.Windows.Forms.DataGridViewTextBoxColumn Disponible;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoDisponible;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACW;
        private System.Windows.Forms.DataGridViewTextBoxColumn Capacitacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Calidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sanitario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comida;
        private System.Windows.Forms.DataGridViewTextBoxColumn Break;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewButtonColumn Monitoreo;
    }
}