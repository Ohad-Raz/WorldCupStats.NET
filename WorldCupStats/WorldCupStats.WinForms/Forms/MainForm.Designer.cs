namespace WorldCupStats.WinForms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
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
            components = new System.ComponentModel.Container();
            contextMenuPlayers = new ContextMenuStrip(components);
            menuItemAddToFavorites = new ToolStripMenuItem();
            menuItemRemoveFromFavorites = new ToolStripMenuItem();
            menuItemSetPicture = new ToolStripMenuItem();
            lblFavoritePlayersHeader = new Label();
            lblOtherPlayersHeader = new Label();
            flowFavoritePlayers = new FlowLayoutPanel();
            flowOtherPlayers = new FlowLayoutPanel();
            cbFavoriteTeam = new ComboBox();
            btnSettings = new Button();
            lblLoading = new Label();
            contextMenuPlayers.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuPlayers
            // 
            contextMenuPlayers.ImageScalingSize = new Size(20, 20);
            contextMenuPlayers.Items.AddRange(new ToolStripItem[] { menuItemAddToFavorites, menuItemRemoveFromFavorites, menuItemSetPicture });
            contextMenuPlayers.Name = "contextMenuPlayers";
            contextMenuPlayers.Size = new Size(230, 76);
            // 
            // menuItemAddToFavorites
            // 
            menuItemAddToFavorites.Name = "menuItemAddToFavorites";
            menuItemAddToFavorites.Size = new Size(229, 24);
            menuItemAddToFavorites.Text = "Add to favorites";
            menuItemAddToFavorites.Click += MenuItemAddToFavorites_Click;
            // 
            // menuItemRemoveFromFavorites
            // 
            menuItemRemoveFromFavorites.Name = "menuItemRemoveFromFavorites";
            menuItemRemoveFromFavorites.Size = new Size(229, 24);
            menuItemRemoveFromFavorites.Text = "Remove from favorites";
            menuItemRemoveFromFavorites.Click += MenuItemRemoveFromFavorites_Click;
            // 
            // menuItemSetPicture
            // 
            menuItemSetPicture.Name = "menuItemSetPicture";
            menuItemSetPicture.Size = new Size(229, 24);
            menuItemSetPicture.Text = "Set picture";
            menuItemSetPicture.Click += menuItemSetPicture_Click;
            // 
            // lblFavoritePlayersHeader
            // 
            lblFavoritePlayersHeader.AutoSize = true;
            lblFavoritePlayersHeader.Location = new Point(20, 100);
            lblFavoritePlayersHeader.Name = "lblFavoritePlayersHeader";
            lblFavoritePlayersHeader.Size = new Size(112, 20);
            lblFavoritePlayersHeader.TabIndex = 1;
            lblFavoritePlayersHeader.Text = "Favorite players";
            // 
            // lblOtherPlayersHeader
            // 
            lblOtherPlayersHeader.AutoSize = true;
            lblOtherPlayersHeader.Location = new Point(440, 100);
            lblOtherPlayersHeader.Name = "lblOtherPlayersHeader";
            lblOtherPlayersHeader.Size = new Size(97, 20);
            lblOtherPlayersHeader.TabIndex = 2;
            lblOtherPlayersHeader.Text = "Other players";
            // 
            // flowFavoritePlayers
            // 
            flowFavoritePlayers.AllowDrop = true;
            flowFavoritePlayers.AutoScroll = true;
            flowFavoritePlayers.BorderStyle = BorderStyle.FixedSingle;
            flowFavoritePlayers.ContextMenuStrip = contextMenuPlayers;
            flowFavoritePlayers.FlowDirection = FlowDirection.TopDown;
            flowFavoritePlayers.Location = new Point(20, 128);
            flowFavoritePlayers.Margin = new Padding(3, 4, 3, 4);
            flowFavoritePlayers.Name = "flowFavoritePlayers";
            flowFavoritePlayers.Size = new Size(400, 360);
            flowFavoritePlayers.TabIndex = 3;
            flowFavoritePlayers.WrapContents = false;
            flowFavoritePlayers.DragDrop += PlayerPanel_DragDrop;
            flowFavoritePlayers.DragEnter += PlayerPanel_DragEnter;
            // 
            // flowOtherPlayers
            // 
            flowOtherPlayers.AllowDrop = true;
            flowOtherPlayers.AutoScroll = true;
            flowOtherPlayers.BorderStyle = BorderStyle.FixedSingle;
            flowOtherPlayers.ContextMenuStrip = contextMenuPlayers;
            flowOtherPlayers.FlowDirection = FlowDirection.TopDown;
            flowOtherPlayers.Location = new Point(440, 128);
            flowOtherPlayers.Margin = new Padding(3, 4, 3, 4);
            flowOtherPlayers.Name = "flowOtherPlayers";
            flowOtherPlayers.Size = new Size(400, 360);
            flowOtherPlayers.TabIndex = 4;
            flowOtherPlayers.WrapContents = false;
            flowOtherPlayers.DragDrop += PlayerPanel_DragDrop;
            flowOtherPlayers.DragEnter += PlayerPanel_DragEnter;
            // 
            // cbFavoriteTeam
            // 
            cbFavoriteTeam.FormattingEnabled = true;
            cbFavoriteTeam.Location = new Point(20, 24);
            cbFavoriteTeam.Margin = new Padding(3, 4, 3, 4);
            cbFavoriteTeam.Name = "cbFavoriteTeam";
            cbFavoriteTeam.Size = new Size(200, 28);
            cbFavoriteTeam.TabIndex = 0;
            cbFavoriteTeam.Text = "Favorite Team";
            cbFavoriteTeam.SelectedIndexChanged += CbFavoriteTeam_SelectedIndexChanged;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.White;
            btnSettings.Location = new Point(737, 23);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(94, 29);
            btnSettings.TabIndex = 5;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += BtnSettings_Click;
            // 
            // lblLoading
            // 
            lblLoading.Location = new Point(20, 23);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(200, 29);
            lblLoading.TabIndex = 6;
            lblLoading.Text = "Loading...";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 520);
            Controls.Add(lblFavoritePlayersHeader);
            Controls.Add(btnSettings);
            Controls.Add(flowOtherPlayers);
            Controls.Add(flowFavoritePlayers);
            Controls.Add(lblOtherPlayersHeader);
            Controls.Add(cbFavoriteTeam);
            Controls.Add(lblLoading);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            contextMenuPlayers.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbFavoriteTeam;
        private Label lblFavoritePlayersHeader;
        private Label lblOtherPlayersHeader;
        private FlowLayoutPanel flowFavoritePlayers;
        private FlowLayoutPanel flowOtherPlayers;
        private ContextMenuStrip contextMenuPlayers;
        private ToolStripMenuItem menuItemAddToFavorites;
        private ToolStripMenuItem menuItemRemoveFromFavorites;
        private Button btnSettings;
        private Label lblLoading;
        private ToolStripMenuItem menuItemSetPicture;
    }
}
