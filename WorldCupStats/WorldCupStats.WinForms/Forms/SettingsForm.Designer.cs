namespace WorldCupStats.WinForms.Forms
{
    partial class SettingsForm
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
            CbChampionship = new ComboBox();
            CbDataSource = new ComboBox();
            BtnConfirm = new Button();
            BtnCancel = new Button();
            SuspendLayout();
            // 
            // CbChampionship
            // 
            CbChampionship.FormattingEnabled = true;
            CbChampionship.Location = new Point(199, 211);
            CbChampionship.Name = "CbChampionship";
            CbChampionship.Size = new Size(151, 28);
            CbChampionship.TabIndex = 0;
            CbChampionship.SelectedIndexChanged += CbChampionship_SelectedIndexChanged;
            // 
            // CbDataSource
            // 
            CbDataSource.DropDownStyle = ComboBoxStyle.DropDownList;
            CbDataSource.FormattingEnabled = true;
            CbDataSource.Location = new Point(199, 260);
            CbDataSource.Name = "CbDataSource";
            CbDataSource.Size = new Size(151, 28);
            CbDataSource.TabIndex = 1;
            // 
            // BtnConfirm
            // 
            BtnConfirm.BackColor = Color.MediumAquamarine;
            BtnConfirm.Location = new Point(418, 380);
            BtnConfirm.Name = "BtnConfirm";
            BtnConfirm.Size = new Size(142, 58);
            BtnConfirm.TabIndex = 2;
            BtnConfirm.Text = "Confirm";
            BtnConfirm.UseVisualStyleBackColor = false;
            BtnConfirm.Click += BtnConfirm_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.BackColor = Color.DarkRed;
            BtnCancel.ForeColor = Color.White;
            BtnCancel.Location = new Point(251, 380);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(142, 58);
            BtnCancel.TabIndex = 3;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseCompatibleTextRendering = true;
            BtnCancel.UseVisualStyleBackColor = false;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnCancel);
            Controls.Add(BtnConfirm);
            Controls.Add(CbDataSource);
            Controls.Add(CbChampionship);
            Name = "SettingsForm";
            Text = "SettingsForm";
            Load += SettingsForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox CbChampionship;
        private ComboBox CbDataSource;
        private Button BtnConfirm;
        private Button BtnCancel;
    }
}
