namespace LoginForms
{
    partial class ExcelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelForm));
            this.mcGenerarExcel = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAgentes = new System.Windows.Forms.ComboBox();
            this.btnGenerarExcelUsuario = new System.Windows.Forms.Button();
            this.btnGenerarExcelFecha = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mcGenerarExcel
            // 
            this.mcGenerarExcel.Location = new System.Drawing.Point(18, 68);
            this.mcGenerarExcel.MaxSelectionCount = 90;
            this.mcGenerarExcel.Name = "mcGenerarExcel";
            this.mcGenerarExcel.TabIndex = 2;
            this.mcGenerarExcel.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Selecciona Un Agente";
            // 
            // cmbAgentes
            // 
            this.cmbAgentes.FormattingEnabled = true;
            this.cmbAgentes.Location = new System.Drawing.Point(18, 35);
            this.cmbAgentes.Name = "cmbAgentes";
            this.cmbAgentes.Size = new System.Drawing.Size(155, 21);
            this.cmbAgentes.Sorted = true;
            this.cmbAgentes.TabIndex = 5;
            this.cmbAgentes.SelectedIndexChanged += new System.EventHandler(this.cmbAgentes_SelectedIndexChanged);
            // 
            // btnGenerarExcelUsuario
            // 
            this.btnGenerarExcelUsuario.Location = new System.Drawing.Point(18, 246);
            this.btnGenerarExcelUsuario.Name = "btnGenerarExcelUsuario";
            this.btnGenerarExcelUsuario.Size = new System.Drawing.Size(111, 40);
            this.btnGenerarExcelUsuario.TabIndex = 6;
            this.btnGenerarExcelUsuario.Text = "Generar Excel\r\nPor Agente\r\n";
            this.btnGenerarExcelUsuario.UseVisualStyleBackColor = true;
            this.btnGenerarExcelUsuario.Click += new System.EventHandler(this.btnGenerarExcelUsuario_Click);
            // 
            // btnGenerarExcelFecha
            // 
            this.btnGenerarExcelFecha.Location = new System.Drawing.Point(168, 246);
            this.btnGenerarExcelFecha.Name = "btnGenerarExcelFecha";
            this.btnGenerarExcelFecha.Size = new System.Drawing.Size(98, 40);
            this.btnGenerarExcelFecha.TabIndex = 7;
            this.btnGenerarExcelFecha.Text = "Generar Excel\r\nPor Fecha";
            this.btnGenerarExcelFecha.UseVisualStyleBackColor = true;
            this.btnGenerarExcelFecha.Click += new System.EventHandler(this.btnGenerarExcelFecha_Click);
            // 
            // ExcelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 324);
            this.Controls.Add(this.btnGenerarExcelFecha);
            this.Controls.Add(this.btnGenerarExcelUsuario);
            this.Controls.Add(this.cmbAgentes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mcGenerarExcel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExcelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generar Excel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MonthCalendar mcGenerarExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAgentes;
        private System.Windows.Forms.Button btnGenerarExcelUsuario;
        private System.Windows.Forms.Button btnGenerarExcelFecha;
    }
}