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
            panelTop = new Panel();
            cbFavoriteTeam = new ComboBox();
            lblLoading = new Label();
            btnSettings = new Button();
            tabMain = new TabControl();
            tabPlayers = new TabPage();
            tablePlayers = new TableLayoutPanel();
            lblFavoritePlayersHeader = new Label();
            lblOtherPlayersHeader = new Label();
            flowFavoritePlayers = new FlowLayoutPanel();
            flowOtherPlayers = new FlowLayoutPanel();
            tabRankings = new TabPage();
            tableRankings = new TableLayoutPanel();
            lblPlayerRankings = new Label();
            lblMatchRankings = new Label();
            dgvPlayerRankings = new DataGridView();
            dgvMatchRankings = new DataGridView();
            contextMenuPlayers.SuspendLayout();
            panelTop.SuspendLayout();
            tabMain.SuspendLayout();
            tabPlayers.SuspendLayout();
            tablePlayers.SuspendLayout();
            tabRankings.SuspendLayout();
            tableRankings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlayerRankings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMatchRankings).BeginInit();
            SuspendLayout();
            // 
            // contextMenuPlayers
            // 
            contextMenuPlayers.ImageScalingSize = new Size(20, 20);
            contextMenuPlayers.Items.AddRange(new ToolStripItem[] { menuItemAddToFavorites, menuItemRemoveFromFavorites, menuItemSetPicture });
            contextMenuPlayers.Name = "contextMenuPlayers";
            contextMenuPlayers.Size = new Size(230, 76);
            contextMenuPlayers.Opening += contextMenuPlayers_Opening;
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
            // panelTop
            // 
            panelTop.Controls.Add(cbFavoriteTeam);
            panelTop.Controls.Add(lblLoading);
            panelTop.Controls.Add(btnSettings);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12, 12, 12, 8);
            panelTop.Size = new Size(1175, 60);
            panelTop.TabIndex = 0;
            // 
            // cbFavoriteTeam
            // 
            cbFavoriteTeam.FormattingEnabled = true;
            cbFavoriteTeam.Location = new Point(15, 15);
            cbFavoriteTeam.Name = "cbFavoriteTeam";
            cbFavoriteTeam.Size = new Size(220, 28);
            cbFavoriteTeam.TabIndex = 0;
            cbFavoriteTeam.SelectedIndexChanged += CbFavoriteTeam_SelectedIndexChanged;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Location = new Point(248, 18);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(72, 20);
            lblLoading.TabIndex = 1;
            lblLoading.Text = "Loading...";
            lblLoading.Visible = false;
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSettings.BackColor = Color.White;
            btnSettings.Location = new Point(1069, 13);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(94, 29);
            btnSettings.TabIndex = 2;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += BtnSettings_Click;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabPlayers);
            tabMain.Controls.Add(tabRankings);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 60);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1175, 675);
            tabMain.TabIndex = 1;
            // 
            // tabPlayers
            // 
            tabPlayers.Controls.Add(tablePlayers);
            tabPlayers.Location = new Point(4, 29);
            tabPlayers.Name = "tabPlayers";
            tabPlayers.Padding = new Padding(3);
            tabPlayers.Size = new Size(1167, 642);
            tabPlayers.TabIndex = 0;
            tabPlayers.Text = "Players";
            tabPlayers.UseVisualStyleBackColor = true;
            // 
            // tablePlayers
            // 
            tablePlayers.ColumnCount = 2;
            tablePlayers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tablePlayers.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tablePlayers.Controls.Add(lblFavoritePlayersHeader, 0, 0);
            tablePlayers.Controls.Add(lblOtherPlayersHeader, 1, 0);
            tablePlayers.Controls.Add(flowFavoritePlayers, 0, 1);
            tablePlayers.Controls.Add(flowOtherPlayers, 1, 1);
            tablePlayers.Dock = DockStyle.Fill;
            tablePlayers.Location = new Point(3, 3);
            tablePlayers.Name = "tablePlayers";
            tablePlayers.RowCount = 2;
            tablePlayers.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablePlayers.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablePlayers.Size = new Size(1161, 636);
            tablePlayers.TabIndex = 0;
            // 
            // lblFavoritePlayersHeader
            // 
            lblFavoritePlayersHeader.AutoSize = true;
            lblFavoritePlayersHeader.Dock = DockStyle.Fill;
            lblFavoritePlayersHeader.Location = new Point(3, 0);
            lblFavoritePlayersHeader.Margin = new Padding(3, 0, 3, 4);
            lblFavoritePlayersHeader.Name = "lblFavoritePlayersHeader";
            lblFavoritePlayersHeader.Size = new Size(574, 24);
            lblFavoritePlayersHeader.TabIndex = 0;
            lblFavoritePlayersHeader.Text = "Favorite players";
            // 
            // lblOtherPlayersHeader
            // 
            lblOtherPlayersHeader.AutoSize = true;
            lblOtherPlayersHeader.Dock = DockStyle.Fill;
            lblOtherPlayersHeader.Location = new Point(583, 0);
            lblOtherPlayersHeader.Margin = new Padding(3, 0, 3, 4);
            lblOtherPlayersHeader.Name = "lblOtherPlayersHeader";
            lblOtherPlayersHeader.Size = new Size(575, 24);
            lblOtherPlayersHeader.TabIndex = 1;
            lblOtherPlayersHeader.Text = "Other players";
            // 
            // flowFavoritePlayers
            // 
            flowFavoritePlayers.AllowDrop = true;
            flowFavoritePlayers.AutoScroll = true;
            flowFavoritePlayers.BorderStyle = BorderStyle.FixedSingle;
            flowFavoritePlayers.ContextMenuStrip = contextMenuPlayers;
            flowFavoritePlayers.Dock = DockStyle.Fill;
            flowFavoritePlayers.FlowDirection = FlowDirection.TopDown;
            flowFavoritePlayers.Location = new Point(3, 28);
            flowFavoritePlayers.Margin = new Padding(3, 0, 3, 3);
            flowFavoritePlayers.Name = "flowFavoritePlayers";
            flowFavoritePlayers.Size = new Size(574, 605);
            flowFavoritePlayers.TabIndex = 2;
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
            flowOtherPlayers.Dock = DockStyle.Fill;
            flowOtherPlayers.FlowDirection = FlowDirection.TopDown;
            flowOtherPlayers.Location = new Point(583, 28);
            flowOtherPlayers.Margin = new Padding(3, 0, 3, 3);
            flowOtherPlayers.Name = "flowOtherPlayers";
            flowOtherPlayers.Size = new Size(575, 605);
            flowOtherPlayers.TabIndex = 3;
            flowOtherPlayers.WrapContents = false;
            flowOtherPlayers.DragDrop += PlayerPanel_DragDrop;
            flowOtherPlayers.DragEnter += PlayerPanel_DragEnter;
            // 
            // tabRankings
            // 
            tabRankings.Controls.Add(tableRankings);
            tabRankings.Location = new Point(4, 29);
            tabRankings.Name = "tabRankings";
            tabRankings.Padding = new Padding(3);
            tabRankings.Size = new Size(1167, 642);
            tabRankings.TabIndex = 1;
            tabRankings.Text = "Rankings";
            tabRankings.UseVisualStyleBackColor = true;
            // 
            // tableRankings
            // 
            tableRankings.ColumnCount = 2;
            tableRankings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableRankings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableRankings.Controls.Add(lblPlayerRankings, 0, 0);
            tableRankings.Controls.Add(lblMatchRankings, 1, 0);
            tableRankings.Controls.Add(dgvPlayerRankings, 0, 1);
            tableRankings.Controls.Add(dgvMatchRankings, 1, 1);
            tableRankings.Dock = DockStyle.Fill;
            tableRankings.Location = new Point(3, 3);
            tableRankings.Name = "tableRankings";
            tableRankings.RowCount = 2;
            tableRankings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableRankings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableRankings.Size = new Size(1161, 636);
            tableRankings.TabIndex = 0;
            // 
            // lblPlayerRankings
            // 
            lblPlayerRankings.AutoSize = true;
            lblPlayerRankings.Dock = DockStyle.Fill;
            lblPlayerRankings.Location = new Point(3, 0);
            lblPlayerRankings.Margin = new Padding(3, 0, 3, 4);
            lblPlayerRankings.Name = "lblPlayerRankings";
            lblPlayerRankings.Size = new Size(690, 24);
            lblPlayerRankings.TabIndex = 0;
            lblPlayerRankings.Text = "Player rankings";
            // 
            // lblMatchRankings
            // 
            lblMatchRankings.AutoSize = true;
            lblMatchRankings.Dock = DockStyle.Fill;
            lblMatchRankings.Location = new Point(699, 0);
            lblMatchRankings.Margin = new Padding(3, 0, 3, 4);
            lblMatchRankings.Name = "lblMatchRankings";
            lblMatchRankings.Size = new Size(459, 24);
            lblMatchRankings.TabIndex = 1;
            lblMatchRankings.Text = "Match rankings";
            // 
            // dgvPlayerRankings
            // 
            dgvPlayerRankings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlayerRankings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPlayerRankings.Dock = DockStyle.Fill;
            dgvPlayerRankings.Location = new Point(3, 28);
            dgvPlayerRankings.Margin = new Padding(3, 0, 3, 3);
            dgvPlayerRankings.Name = "dgvPlayerRankings";
            dgvPlayerRankings.ReadOnly = true;
            dgvPlayerRankings.RowHeadersWidth = 51;
            dgvPlayerRankings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlayerRankings.Size = new Size(690, 605);
            dgvPlayerRankings.TabIndex = 2;
            // 
            // dgvMatchRankings
            // 
            dgvMatchRankings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMatchRankings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMatchRankings.Dock = DockStyle.Fill;
            dgvMatchRankings.Location = new Point(699, 28);
            dgvMatchRankings.Margin = new Padding(3, 0, 3, 3);
            dgvMatchRankings.Name = "dgvMatchRankings";
            dgvMatchRankings.ReadOnly = true;
            dgvMatchRankings.RowHeadersWidth = 51;
            dgvMatchRankings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMatchRankings.Size = new Size(459, 605);
            dgvMatchRankings.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1175, 735);
            Controls.Add(tabMain);
            Controls.Add(panelTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            contextMenuPlayers.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            tabMain.ResumeLayout(false);
            tabPlayers.ResumeLayout(false);
            tablePlayers.ResumeLayout(false);
            tablePlayers.PerformLayout();
            tabRankings.ResumeLayout(false);
            tableRankings.ResumeLayout(false);
            tableRankings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlayerRankings).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMatchRankings).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private ComboBox cbFavoriteTeam;
        private Label lblLoading;
        private Button btnSettings;
        private TabControl tabMain;
        private TabPage tabPlayers;
        private TabPage tabRankings;
        private TableLayoutPanel tablePlayers;
        private Label lblFavoritePlayersHeader;
        private Label lblOtherPlayersHeader;
        private FlowLayoutPanel flowFavoritePlayers;
        private FlowLayoutPanel flowOtherPlayers;
        private TableLayoutPanel tableRankings;
        private Label lblPlayerRankings;
        private Label lblMatchRankings;
        private DataGridView dgvPlayerRankings;
        private DataGridView dgvMatchRankings;
        private ContextMenuStrip contextMenuPlayers;
        private ToolStripMenuItem menuItemAddToFavorites;
        private ToolStripMenuItem menuItemRemoveFromFavorites;
        private ToolStripMenuItem menuItemSetPicture;
    }
}
