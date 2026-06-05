namespace WorldCupStats.WinForms
{
    partial class PlayerUserControl
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblName = new Label();
            lblShirtNumber = new Label();
            lblPosition = new Label();
            lblCaptain = new Label();
            lblFavoriteStar = new Label();
            pbPlayerImage = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbPlayerImage).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblName.Location = new Point(8, 8);
            lblName.Name = "lblName";
            lblName.Size = new Size(51, 20);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            // 
            // lblShirtNumber
            // 
            lblShirtNumber.AutoSize = true;
            lblShirtNumber.Location = new Point(8, 36);
            lblShirtNumber.Name = "lblShirtNumber";
            lblShirtNumber.Size = new Size(26, 20);
            lblShirtNumber.TabIndex = 1;
            lblShirtNumber.Text = "#0";
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(8, 60);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(61, 20);
            lblPosition.TabIndex = 2;
            lblPosition.Text = "Position";
            // 
            // lblCaptain
            // 
            lblCaptain.AutoSize = true;
            lblCaptain.Location = new Point(8, 84);
            lblCaptain.Name = "lblCaptain";
            lblCaptain.Size = new Size(0, 20);
            lblCaptain.TabIndex = 3;
            // 
            // lblFavoriteStar
            // 
            lblFavoriteStar.AutoSize = true;
            lblFavoriteStar.Font = new Font("Segoe UI", 12F);
            lblFavoriteStar.Location = new Point(180, 8);
            lblFavoriteStar.Name = "lblFavoriteStar";
            lblFavoriteStar.Size = new Size(0, 28);
            lblFavoriteStar.TabIndex = 4;
            // 
            // pbPlayerImage
            // 
            pbPlayerImage.BorderStyle = BorderStyle.FixedSingle;
            pbPlayerImage.Location = new Point(248, 3);
            pbPlayerImage.Name = "pbPlayerImage";
            pbPlayerImage.Size = new Size(121, 118);
            pbPlayerImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbPlayerImage.TabIndex = 5;
            pbPlayerImage.TabStop = false;
            // 
            // PlayerUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pbPlayerImage);
            Controls.Add(lblFavoriteStar);
            Controls.Add(lblCaptain);
            Controls.Add(lblPosition);
            Controls.Add(lblShirtNumber);
            Controls.Add(lblName);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(220, 120);
            Name = "PlayerUserControl";
            Size = new Size(372, 125);
            ((System.ComponentModel.ISupportInitialize)pbPlayerImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        private Label lblShirtNumber;
        private Label lblPosition;
        private Label lblCaptain;
        private Label lblFavoriteStar;
        private PictureBox pbPlayerImage;
    }
}
