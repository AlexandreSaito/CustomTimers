﻿
namespace T
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddAudio = new System.Windows.Forms.Button();
            this.gvTimers = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ddlAudio = new System.Windows.Forms.ComboBox();
            this.ddlOption = new System.Windows.Forms.ComboBox();
            this.btnAddTimer = new System.Windows.Forms.Button();
            this.txtCustomName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtOldName = new System.Windows.Forms.TextBox();
            this.txtOptionTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOptionName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveOption = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.gvOpcoes = new System.Windows.Forms.DataGridView();
            this.OptionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptionCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountTimer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptionFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvTimers)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvOpcoes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddAudio
            // 
            this.btnAddAudio.Location = new System.Drawing.Point(474, 250);
            this.btnAddAudio.Name = "btnAddAudio";
            this.btnAddAudio.Size = new System.Drawing.Size(90, 23);
            this.btnAddAudio.TabIndex = 0;
            this.btnAddAudio.Text = "Adicionar Audio";
            this.btnAddAudio.UseVisualStyleBackColor = true;
            this.btnAddAudio.Click += new System.EventHandler(this.btnAddAudio_Click);
            // 
            // gvTimers
            // 
            this.gvTimers.AllowUserToAddRows = false;
            this.gvTimers.AllowUserToDeleteRows = false;
            this.gvTimers.AllowUserToOrderColumns = true;
            this.gvTimers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTimers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Name,
            this.Time,
            this.CurrentTime});
            this.gvTimers.Location = new System.Drawing.Point(12, 38);
            this.gvTimers.MultiSelect = false;
            this.gvTimers.Name = "gvTimers";
            this.gvTimers.Size = new System.Drawing.Size(445, 399);
            this.gvTimers.TabIndex = 1;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 50;
            // 
            // Name
            // 
            this.Name.HeaderText = "Nome";
            this.Name.Name = "Name";
            this.Name.Width = 150;
            // 
            // Time
            // 
            this.Time.HeaderText = "Tempo";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 70;
            // 
            // CurrentTime
            // 
            this.CurrentTime.HeaderText = "Tempo Decorrido";
            this.CurrentTime.Name = "CurrentTime";
            this.CurrentTime.Width = 70;
            // 
            // ddlAudio
            // 
            this.ddlAudio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAudio.FormattingEnabled = true;
            this.ddlAudio.Location = new System.Drawing.Point(66, 78);
            this.ddlAudio.Name = "ddlAudio";
            this.ddlAudio.Size = new System.Drawing.Size(242, 21);
            this.ddlAudio.TabIndex = 2;
            // 
            // ddlOption
            // 
            this.ddlOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlOption.FormattingEnabled = true;
            this.ddlOption.Location = new System.Drawing.Point(12, 12);
            this.ddlOption.Name = "ddlOption";
            this.ddlOption.Size = new System.Drawing.Size(328, 21);
            this.ddlOption.TabIndex = 3;
            this.ddlOption.SelectedIndexChanged += new System.EventHandler(this.ddlOption_SelectedIndexChanged);
            // 
            // btnAddTimer
            // 
            this.btnAddTimer.Location = new System.Drawing.Point(677, 10);
            this.btnAddTimer.Name = "btnAddTimer";
            this.btnAddTimer.Size = new System.Drawing.Size(111, 23);
            this.btnAddTimer.TabIndex = 4;
            this.btnAddTimer.Text = "Adicionar Timer";
            this.btnAddTimer.UseVisualStyleBackColor = true;
            this.btnAddTimer.Click += new System.EventHandler(this.btnAddTimer_Click);
            // 
            // txtCustomName
            // 
            this.txtCustomName.Location = new System.Drawing.Point(464, 12);
            this.txtCustomName.Name = "txtCustomName";
            this.txtCustomName.Size = new System.Drawing.Size(207, 20);
            this.txtCustomName.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancelar);
            this.groupBox1.Controls.Add(this.txtOldName);
            this.groupBox1.Controls.Add(this.txtOptionTime);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtOptionName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSaveOption);
            this.groupBox1.Controls.Add(this.ddlAudio);
            this.groupBox1.Location = new System.Drawing.Point(474, 279);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 159);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(91, 130);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtOldName
            // 
            this.txtOldName.Location = new System.Drawing.Point(207, 132);
            this.txtOldName.Name = "txtOldName";
            this.txtOldName.Size = new System.Drawing.Size(100, 20);
            this.txtOldName.TabIndex = 9;
            this.txtOldName.TabStop = false;
            this.txtOldName.Visible = false;
            // 
            // txtOptionTime
            // 
            this.txtOptionTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtOptionTime.Location = new System.Drawing.Point(66, 52);
            this.txtOptionTime.Name = "txtOptionTime";
            this.txtOptionTime.Size = new System.Drawing.Size(242, 20);
            this.txtOptionTime.TabIndex = 8;
            this.txtOptionTime.Value = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Audio";
            // 
            // txtOptionName
            // 
            this.txtOptionName.Location = new System.Drawing.Point(66, 26);
            this.txtOptionName.Name = "txtOptionName";
            this.txtOptionName.Size = new System.Drawing.Size(242, 20);
            this.txtOptionName.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tempo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nome";
            // 
            // btnSaveOption
            // 
            this.btnSaveOption.Location = new System.Drawing.Point(9, 130);
            this.btnSaveOption.Name = "btnSaveOption";
            this.btnSaveOption.Size = new System.Drawing.Size(75, 23);
            this.btnSaveOption.TabIndex = 3;
            this.btnSaveOption.Text = "Salvar";
            this.btnSaveOption.UseVisualStyleBackColor = true;
            this.btnSaveOption.Click += new System.EventHandler(this.btnSaveOption_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(359, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Nome Customizado";
            // 
            // gvOpcoes
            // 
            this.gvOpcoes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvOpcoes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OptionName,
            this.OptionCount,
            this.CountTimer,
            this.OptionFile});
            this.gvOpcoes.Location = new System.Drawing.Point(464, 39);
            this.gvOpcoes.Name = "gvOpcoes";
            this.gvOpcoes.Size = new System.Drawing.Size(324, 150);
            this.gvOpcoes.TabIndex = 8;
            // 
            // OptionName
            // 
            this.OptionName.HeaderText = "Name";
            this.OptionName.Name = "OptionName";
            this.OptionName.ReadOnly = true;
            // 
            // OptionCount
            // 
            this.OptionCount.HeaderText = "Contador";
            this.OptionCount.Name = "OptionCount";
            this.OptionCount.ReadOnly = true;
            this.OptionCount.Width = 60;
            // 
            // CountTimer
            // 
            this.CountTimer.HeaderText = "Tempo";
            this.CountTimer.Name = "CountTimer";
            this.CountTimer.ReadOnly = true;
            this.CountTimer.Width = 60;
            // 
            // OptionFile
            // 
            this.OptionFile.HeaderText = "Arquivo";
            this.OptionFile.Name = "OptionFile";
            this.OptionFile.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gvOpcoes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtCustomName);
            this.Controls.Add(this.btnAddTimer);
            this.Controls.Add(this.ddlOption);
            this.Controls.Add(this.gvTimers);
            this.Controls.Add(this.btnAddAudio);
            //this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvTimers)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvOpcoes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddAudio;
        private System.Windows.Forms.DataGridView gvTimers;
        private System.Windows.Forms.ComboBox ddlAudio;
        private System.Windows.Forms.ComboBox ddlOption;
        private System.Windows.Forms.Button btnAddTimer;
        private System.Windows.Forms.TextBox txtCustomName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSaveOption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOptionName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker txtOptionTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentTime;
        private System.Windows.Forms.DataGridView gvOpcoes;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptionCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn OptionFile;
        private System.Windows.Forms.TextBox txtOldName;
        private System.Windows.Forms.Button btnCancelar;
    }
}
