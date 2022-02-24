
namespace LoginForms
{
    partial class DashboardAgente
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pieLlamadas = new LiveCharts.WinForms.PieChart();
            this.pieChats = new LiveCharts.WinForms.PieChart();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLlamadasTotales = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblChatsTotales = new System.Windows.Forms.Label();
            this.btnRecargar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.pieLlamadas, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.pieChats, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRecargar, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1079, 675);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pieLlamadas
            // 
            this.pieLlamadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieLlamadas.Location = new System.Drawing.Point(539, 137);
            this.pieLlamadas.Margin = new System.Windows.Forms.Padding(2);
            this.pieLlamadas.Name = "pieLlamadas";
            this.pieLlamadas.Size = new System.Drawing.Size(211, 333);
            this.pieLlamadas.TabIndex = 19;
            this.pieLlamadas.Text = "pieChart1";
            // 
            // pieChats
            // 
            this.pieChats.BackColorTransparent = true;
            this.pieChats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChats.Location = new System.Drawing.Point(324, 137);
            this.pieChats.Margin = new System.Windows.Forms.Padding(2);
            this.pieChats.Name = "pieChats";
            this.pieChats.Size = new System.Drawing.Size(211, 333);
            this.pieChats.TabIndex = 16;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblLlamadasTotales, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(793, 237);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(132, 132);
            this.tableLayoutPanel3.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 66);
            this.label3.TabIndex = 13;
            this.label3.Text = "LLamadas";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLlamadasTotales
            // 
            this.lblLlamadasTotales.AutoSize = true;
            this.lblLlamadasTotales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLlamadasTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLlamadasTotales.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLlamadasTotales.Location = new System.Drawing.Point(2, 66);
            this.lblLlamadasTotales.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLlamadasTotales.Name = "lblLlamadasTotales";
            this.lblLlamadasTotales.Size = new System.Drawing.Size(128, 66);
            this.lblLlamadasTotales.TabIndex = 14;
            this.lblLlamadasTotales.Text = "label2";
            this.lblLlamadasTotales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblChatsTotales, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(148, 246);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(132, 114);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 57);
            this.label1.TabIndex = 11;
            this.label1.Text = " Chats";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblChatsTotales
            // 
            this.lblChatsTotales.AutoSize = true;
            this.lblChatsTotales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblChatsTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChatsTotales.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChatsTotales.Location = new System.Drawing.Point(2, 57);
            this.lblChatsTotales.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblChatsTotales.Name = "lblChatsTotales";
            this.lblChatsTotales.Size = new System.Drawing.Size(128, 57);
            this.lblChatsTotales.TabIndex = 12;
            this.lblChatsTotales.Text = "label5";
            this.lblChatsTotales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRecargar
            // 
            this.btnRecargar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRecargar.BackColor = System.Drawing.Color.Transparent;
            this.btnRecargar.BackgroundImage = global::LoginForms.Properties.Resources.recargar;
            this.btnRecargar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRecargar.FlatAppearance.BorderSize = 0;
            this.btnRecargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecargar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRecargar.Location = new System.Drawing.Point(997, 45);
            this.btnRecargar.Margin = new System.Windows.Forms.Padding(2);
            this.btnRecargar.Name = "btnRecargar";
            this.btnRecargar.Size = new System.Drawing.Size(51, 45);
            this.btnRecargar.TabIndex = 17;
            this.btnRecargar.UseVisualStyleBackColor = false;
            this.btnRecargar.Click += new System.EventHandler(this.btnRecargar_Click);
            // 
            // DashboardAgente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 675);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DashboardAgente";
            this.Text = "SIDI Omnichannel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblChatsTotales;
        private LiveCharts.WinForms.PieChart pieChats;
        private System.Windows.Forms.Button btnRecargar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblLlamadasTotales;
        private LiveCharts.WinForms.PieChart pieLlamadas;
    }
}