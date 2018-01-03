namespace MusicProjectLibrary_1
{
    partial class MusicLibraryWindow
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
            this.components = new System.ComponentModel.Container();
            this.tbxPickedPath = new System.Windows.Forms.TextBox();
            this.changeMainPath = new System.Windows.Forms.Button();
            this.ButtonReadTag = new System.Windows.Forms.Button();
            this.CheckBoxProcessCatalogs = new System.Windows.Forms.CheckBox();
            this.CheckBoxModifyFIles = new System.Windows.Forms.CheckBox();
            this.tabCtrTrackAlbums = new System.Windows.Forms.TabControl();
            this.tabAlbums = new System.Windows.Forms.TabPage();
            this.btnSelectHealthy = new System.Windows.Forms.Button();
            this.btnProcessSelected = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDeclareGenre = new System.Windows.Forms.Button();
            this.AlbumsDataGridView = new System.Windows.Forms.DataGridView();
            this.tabTracks = new System.Windows.Forms.TabPage();
            this.btnCheckTrackDuplicates = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.TracksDataGridView = new System.Windows.Forms.DataGridView();
            this.tabArtist = new System.Windows.Forms.TabPage();
            this.dgvArtists = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDiscogs = new System.Windows.Forms.Button();
            this.tabSetup = new System.Windows.Forms.TabPage();
            this.checkBoxCreateBackUp = new System.Windows.Forms.CheckBox();
            this.CheckBoxWriteIndexes = new System.Windows.Forms.CheckBox();
            this.tbxSearchAlbums = new System.Windows.Forms.TextBox();
            this.btnFindAlbums = new System.Windows.Forms.Button();
            this.btnDeleteArtistFromDB = new System.Windows.Forms.Button();
            this.tbxDeleteArtistFromDB = new System.Windows.Forms.TextBox();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.btnFindTracks = new System.Windows.Forms.Button();
            this.tbxSearchTracks = new System.Windows.Forms.TextBox();
            this.BoxListConsole = new System.Windows.Forms.ListBox();
            this.albumListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.trackListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.trackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.musicLibraryDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.albumBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.albumBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.albumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.musicLibraryDataSetBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tbxMusicPath = new System.Windows.Forms.TextBox();
            this.btnChangeGeneralCatalogPath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.chbxMove = new System.Windows.Forms.CheckBox();
            this.tabCtrTrackAlbums.SuspendLayout();
            this.tabAlbums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlbumsDataGridView)).BeginInit();
            this.tabTracks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TracksDataGridView)).BeginInit();
            this.tabArtist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArtists)).BeginInit();
            this.tabSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxPickedPath
            // 
            this.tbxPickedPath.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxPickedPath.Enabled = false;
            this.tbxPickedPath.Location = new System.Drawing.Point(19, 30);
            this.tbxPickedPath.Name = "tbxPickedPath";
            this.tbxPickedPath.Size = new System.Drawing.Size(365, 20);
            this.tbxPickedPath.TabIndex = 10;
            // 
            // changeMainPath
            // 
            this.changeMainPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeMainPath.Location = new System.Drawing.Point(386, 30);
            this.changeMainPath.Name = "changeMainPath";
            this.changeMainPath.Size = new System.Drawing.Size(86, 20);
            this.changeMainPath.TabIndex = 13;
            this.changeMainPath.Text = "Purgatory Path";
            this.changeMainPath.UseVisualStyleBackColor = true;
            this.changeMainPath.Click += new System.EventHandler(this.checkFilesInDirectory_Click);
            // 
            // ButtonReadTag
            // 
            this.ButtonReadTag.BackColor = System.Drawing.Color.PaleGreen;
            this.ButtonReadTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonReadTag.Location = new System.Drawing.Point(387, 56);
            this.ButtonReadTag.Name = "ButtonReadTag";
            this.ButtonReadTag.Size = new System.Drawing.Size(86, 24);
            this.ButtonReadTag.TabIndex = 17;
            this.ButtonReadTag.Text = "Play";
            this.ButtonReadTag.UseVisualStyleBackColor = false;
            this.ButtonReadTag.Click += new System.EventHandler(this.ButtonReadTag_Click);
            // 
            // CheckBoxProcessCatalogs
            // 
            this.CheckBoxProcessCatalogs.AutoSize = true;
            this.CheckBoxProcessCatalogs.Checked = true;
            this.CheckBoxProcessCatalogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxProcessCatalogs.Location = new System.Drawing.Point(136, 4);
            this.CheckBoxProcessCatalogs.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxProcessCatalogs.Name = "CheckBoxProcessCatalogs";
            this.CheckBoxProcessCatalogs.Size = new System.Drawing.Size(137, 17);
            this.CheckBoxProcessCatalogs.TabIndex = 19;
            this.CheckBoxProcessCatalogs.Text = "Process catalogs - SQL";
            this.CheckBoxProcessCatalogs.UseVisualStyleBackColor = true;
            this.CheckBoxProcessCatalogs.CheckedChanged += new System.EventHandler(this.CheckBoxProcessCatalogs_CheckedChanged);
            // 
            // CheckBoxModifyFIles
            // 
            this.CheckBoxModifyFIles.AutoSize = true;
            this.CheckBoxModifyFIles.BackColor = System.Drawing.Color.LightCoral;
            this.CheckBoxModifyFIles.Location = new System.Drawing.Point(294, 4);
            this.CheckBoxModifyFIles.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxModifyFIles.Name = "CheckBoxModifyFIles";
            this.CheckBoxModifyFIles.Size = new System.Drawing.Size(106, 17);
            this.CheckBoxModifyFIles.TabIndex = 20;
            this.CheckBoxModifyFIles.Text = "Auto Modify Files";
            this.CheckBoxModifyFIles.UseVisualStyleBackColor = false;
            this.CheckBoxModifyFIles.CheckedChanged += new System.EventHandler(this.CheckBoxModifyFIles_CheckedChanged);
            // 
            // tabCtrTrackAlbums
            // 
            this.tabCtrTrackAlbums.Controls.Add(this.tabAlbums);
            this.tabCtrTrackAlbums.Controls.Add(this.tabTracks);
            this.tabCtrTrackAlbums.Controls.Add(this.tabArtist);
            this.tabCtrTrackAlbums.Controls.Add(this.tabSetup);
            this.tabCtrTrackAlbums.Location = new System.Drawing.Point(12, 170);
            this.tabCtrTrackAlbums.Name = "tabCtrTrackAlbums";
            this.tabCtrTrackAlbums.SelectedIndex = 0;
            this.tabCtrTrackAlbums.Size = new System.Drawing.Size(1236, 335);
            this.tabCtrTrackAlbums.TabIndex = 26;
            // 
            // tabAlbums
            // 
            this.tabAlbums.Controls.Add(this.btnSelectHealthy);
            this.tabAlbums.Controls.Add(this.btnProcessSelected);
            this.tabAlbums.Controls.Add(this.button1);
            this.tabAlbums.Controls.Add(this.btnDeclareGenre);
            this.tabAlbums.Controls.Add(this.AlbumsDataGridView);
            this.tabAlbums.Location = new System.Drawing.Point(4, 22);
            this.tabAlbums.Name = "tabAlbums";
            this.tabAlbums.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlbums.Size = new System.Drawing.Size(1228, 309);
            this.tabAlbums.TabIndex = 0;
            this.tabAlbums.Text = "Albums";
            this.tabAlbums.UseVisualStyleBackColor = true;
            // 
            // btnSelectHealthy
            // 
            this.btnSelectHealthy.Location = new System.Drawing.Point(212, 3);
            this.btnSelectHealthy.Name = "btnSelectHealthy";
            this.btnSelectHealthy.Size = new System.Drawing.Size(105, 23);
            this.btnSelectHealthy.TabIndex = 40;
            this.btnSelectHealthy.Text = "Select Healthy";
            this.btnSelectHealthy.UseVisualStyleBackColor = true;
            this.btnSelectHealthy.Click += new System.EventHandler(this.btnSelectHealthy_Click);
            // 
            // btnProcessSelected
            // 
            this.btnProcessSelected.Location = new System.Drawing.Point(101, 3);
            this.btnProcessSelected.Name = "btnProcessSelected";
            this.btnProcessSelected.Size = new System.Drawing.Size(105, 23);
            this.btnProcessSelected.TabIndex = 39;
            this.btnProcessSelected.Text = "Process Selected";
            this.btnProcessSelected.UseVisualStyleBackColor = true;
            this.btnProcessSelected.Click += new System.EventHandler(this.btnProcessSelected_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1120, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 23);
            this.button1.TabIndex = 38;
            this.button1.Text = "Find next Genre";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnDeclareGenre
            // 
            this.btnDeclareGenre.Location = new System.Drawing.Point(6, 3);
            this.btnDeclareGenre.Name = "btnDeclareGenre";
            this.btnDeclareGenre.Size = new System.Drawing.Size(88, 23);
            this.btnDeclareGenre.TabIndex = 37;
            this.btnDeclareGenre.Text = "Declare Genre";
            this.btnDeclareGenre.UseVisualStyleBackColor = true;
            this.btnDeclareGenre.Click += new System.EventHandler(this.btnDeclareGenre_Click_1);
            // 
            // AlbumsDataGridView
            // 
            this.AlbumsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AlbumsDataGridView.Location = new System.Drawing.Point(3, 32);
            this.AlbumsDataGridView.Name = "AlbumsDataGridView";
            this.AlbumsDataGridView.Size = new System.Drawing.Size(1222, 274);
            this.AlbumsDataGridView.TabIndex = 34;
            this.AlbumsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AlbumsDataGridView_CellContentClick);
            this.AlbumsDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AlbumsDataGridView_CellContentDoubleClick);
            this.AlbumsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AlbumsDataGridView_CellDoubleClick);
            this.AlbumsDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.AlbumsDataGridView_DataBindingComplete_1);
            this.AlbumsDataGridView.SelectionChanged += new System.EventHandler(this.AlbumsDataGridView_SelectionChanged_1);
            this.AlbumsDataGridView.DoubleClick += new System.EventHandler(this.AlbumsDataGridView_DoubleClick);
            // 
            // tabTracks
            // 
            this.tabTracks.Controls.Add(this.btnCheckTrackDuplicates);
            this.tabTracks.Controls.Add(this.button4);
            this.tabTracks.Controls.Add(this.button3);
            this.tabTracks.Controls.Add(this.TracksDataGridView);
            this.tabTracks.Location = new System.Drawing.Point(4, 22);
            this.tabTracks.Name = "tabTracks";
            this.tabTracks.Padding = new System.Windows.Forms.Padding(3);
            this.tabTracks.Size = new System.Drawing.Size(1228, 309);
            this.tabTracks.TabIndex = 1;
            this.tabTracks.Text = "Tracks";
            this.tabTracks.UseVisualStyleBackColor = true;
            // 
            // btnCheckTrackDuplicates
            // 
            this.btnCheckTrackDuplicates.Location = new System.Drawing.Point(228, 6);
            this.btnCheckTrackDuplicates.Name = "btnCheckTrackDuplicates";
            this.btnCheckTrackDuplicates.Size = new System.Drawing.Size(105, 23);
            this.btnCheckTrackDuplicates.TabIndex = 44;
            this.btnCheckTrackDuplicates.Text = "Check Duplicates";
            this.btnCheckTrackDuplicates.UseVisualStyleBackColor = true;
            this.btnCheckTrackDuplicates.Click += new System.EventHandler(this.btnCheckTrackDuplicates_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(117, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 43;
            this.button4.Text = "Recheck";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 40;
            this.button3.Text = "Generate playlist";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // TracksDataGridView
            // 
            this.TracksDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TracksDataGridView.Location = new System.Drawing.Point(3, 32);
            this.TracksDataGridView.Name = "TracksDataGridView";
            this.TracksDataGridView.Size = new System.Drawing.Size(1219, 274);
            this.TracksDataGridView.TabIndex = 37;
            this.TracksDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TracksDataGridView_CellContentClick);
            // 
            // tabArtist
            // 
            this.tabArtist.Controls.Add(this.dgvArtists);
            this.tabArtist.Controls.Add(this.button2);
            this.tabArtist.Controls.Add(this.btnDiscogs);
            this.tabArtist.Location = new System.Drawing.Point(4, 22);
            this.tabArtist.Margin = new System.Windows.Forms.Padding(2);
            this.tabArtist.Name = "tabArtist";
            this.tabArtist.Padding = new System.Windows.Forms.Padding(2);
            this.tabArtist.Size = new System.Drawing.Size(1228, 309);
            this.tabArtist.TabIndex = 2;
            this.tabArtist.Text = "Artists";
            this.tabArtist.UseVisualStyleBackColor = true;
            // 
            // dgvArtists
            // 
            this.dgvArtists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArtists.Location = new System.Drawing.Point(3, 30);
            this.dgvArtists.Name = "dgvArtists";
            this.dgvArtists.Size = new System.Drawing.Size(1219, 274);
            this.dgvArtists.TabIndex = 43;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(116, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "Show Albums";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnDiscogs
            // 
            this.btnDiscogs.Location = new System.Drawing.Point(5, 5);
            this.btnDiscogs.Name = "btnDiscogs";
            this.btnDiscogs.Size = new System.Drawing.Size(105, 23);
            this.btnDiscogs.TabIndex = 41;
            this.btnDiscogs.Text = "Discogs";
            this.btnDiscogs.UseVisualStyleBackColor = true;
            this.btnDiscogs.Click += new System.EventHandler(this.btnDiscogs_Click);
            // 
            // tabSetup
            // 
            this.tabSetup.Controls.Add(this.chbxMove);
            this.tabSetup.Controls.Add(this.checkBoxCreateBackUp);
            this.tabSetup.Controls.Add(this.CheckBoxWriteIndexes);
            this.tabSetup.Controls.Add(this.CheckBoxProcessCatalogs);
            this.tabSetup.Controls.Add(this.CheckBoxModifyFIles);
            this.tabSetup.Location = new System.Drawing.Point(4, 22);
            this.tabSetup.Margin = new System.Windows.Forms.Padding(2);
            this.tabSetup.Name = "tabSetup";
            this.tabSetup.Padding = new System.Windows.Forms.Padding(2);
            this.tabSetup.Size = new System.Drawing.Size(1228, 309);
            this.tabSetup.TabIndex = 3;
            this.tabSetup.Text = "Setup";
            this.tabSetup.UseVisualStyleBackColor = true;
            // 
            // checkBoxCreateBackUp
            // 
            this.checkBoxCreateBackUp.AutoSize = true;
            this.checkBoxCreateBackUp.Location = new System.Drawing.Point(4, 24);
            this.checkBoxCreateBackUp.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxCreateBackUp.Name = "checkBoxCreateBackUp";
            this.checkBoxCreateBackUp.Size = new System.Drawing.Size(117, 17);
            this.checkBoxCreateBackUp.TabIndex = 35;
            this.checkBoxCreateBackUp.Text = "Create backup files";
            this.checkBoxCreateBackUp.UseVisualStyleBackColor = true;
            this.checkBoxCreateBackUp.CheckedChanged += new System.EventHandler(this.checkBoxCreateBackUp_CheckedChanged);
            // 
            // CheckBoxWriteIndexes
            // 
            this.CheckBoxWriteIndexes.AutoSize = true;
            this.CheckBoxWriteIndexes.Location = new System.Drawing.Point(4, 4);
            this.CheckBoxWriteIndexes.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxWriteIndexes.Name = "CheckBoxWriteIndexes";
            this.CheckBoxWriteIndexes.Size = new System.Drawing.Size(91, 17);
            this.CheckBoxWriteIndexes.TabIndex = 34;
            this.CheckBoxWriteIndexes.Text = "Write Indexes";
            this.CheckBoxWriteIndexes.UseVisualStyleBackColor = true;
            this.CheckBoxWriteIndexes.CheckedChanged += new System.EventHandler(this.CheckBoxWriteIndexes_CheckedChanged);
            // 
            // tbxSearchAlbums
            // 
            this.tbxSearchAlbums.Location = new System.Drawing.Point(467, 108);
            this.tbxSearchAlbums.Name = "tbxSearchAlbums";
            this.tbxSearchAlbums.Size = new System.Drawing.Size(100, 20);
            this.tbxSearchAlbums.TabIndex = 27;
            // 
            // btnFindAlbums
            // 
            this.btnFindAlbums.Location = new System.Drawing.Point(387, 108);
            this.btnFindAlbums.Name = "btnFindAlbums";
            this.btnFindAlbums.Size = new System.Drawing.Size(75, 23);
            this.btnFindAlbums.TabIndex = 28;
            this.btnFindAlbums.Text = "Find albums";
            this.btnFindAlbums.UseVisualStyleBackColor = true;
            this.btnFindAlbums.Click += new System.EventHandler(this.btnFindAlbums_Click);
            // 
            // btnDeleteArtistFromDB
            // 
            this.btnDeleteArtistFromDB.Location = new System.Drawing.Point(386, 160);
            this.btnDeleteArtistFromDB.Name = "btnDeleteArtistFromDB";
            this.btnDeleteArtistFromDB.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteArtistFromDB.TabIndex = 30;
            this.btnDeleteArtistFromDB.Text = "Delete Artist DB";
            this.btnDeleteArtistFromDB.UseVisualStyleBackColor = true;
            this.btnDeleteArtistFromDB.Click += new System.EventHandler(this.btnDeleteArtistFromDB_Click);
            // 
            // tbxDeleteArtistFromDB
            // 
            this.tbxDeleteArtistFromDB.Location = new System.Drawing.Point(467, 160);
            this.tbxDeleteArtistFromDB.Name = "tbxDeleteArtistFromDB";
            this.tbxDeleteArtistFromDB.Size = new System.Drawing.Size(100, 20);
            this.tbxDeleteArtistFromDB.TabIndex = 29;
            // 
            // progBar
            // 
            this.progBar.Location = new System.Drawing.Point(19, 56);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(365, 23);
            this.progBar.TabIndex = 31;
            this.progBar.Click += new System.EventHandler(this.progBar_Click);
            // 
            // btnFindTracks
            // 
            this.btnFindTracks.Location = new System.Drawing.Point(387, 134);
            this.btnFindTracks.Name = "btnFindTracks";
            this.btnFindTracks.Size = new System.Drawing.Size(75, 23);
            this.btnFindTracks.TabIndex = 32;
            this.btnFindTracks.Text = "Find tracks";
            this.btnFindTracks.UseVisualStyleBackColor = true;
            this.btnFindTracks.Click += new System.EventHandler(this.btnFindTracks_Click);
            // 
            // tbxSearchTracks
            // 
            this.tbxSearchTracks.Location = new System.Drawing.Point(467, 134);
            this.tbxSearchTracks.Name = "tbxSearchTracks";
            this.tbxSearchTracks.Size = new System.Drawing.Size(100, 20);
            this.tbxSearchTracks.TabIndex = 33;
            // 
            // BoxListConsole
            // 
            this.BoxListConsole.BackColor = System.Drawing.SystemColors.Desktop;
            this.BoxListConsole.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoxListConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.BoxListConsole.FormattingEnabled = true;
            this.BoxListConsole.Location = new System.Drawing.Point(573, 12);
            this.BoxListConsole.Name = "BoxListConsole";
            this.BoxListConsole.Size = new System.Drawing.Size(671, 134);
            this.BoxListConsole.TabIndex = 16;
            // 
            // albumListBindingSource
            // 
            this.albumListBindingSource.DataMember = "albumList";
            // 
            // trackListBindingSource
            // 
            this.trackListBindingSource.DataMember = "trackList";
            // 
            // trackBindingSource
            // 
            this.trackBindingSource.DataSource = this.musicLibraryDataSetBindingSource;
            // 
            // albumBindingSource2
            // 
            this.albumBindingSource2.DataMember = "Album";
            // 
            // albumBindingSource1
            // 
            this.albumBindingSource1.DataMember = "Album";
            // 
            // albumBindingSource
            // 
            this.albumBindingSource.DataMember = "Album";
            // 
            // tbxMusicPath
            // 
            this.tbxMusicPath.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxMusicPath.Enabled = false;
            this.tbxMusicPath.Location = new System.Drawing.Point(19, 4);
            this.tbxMusicPath.Name = "tbxMusicPath";
            this.tbxMusicPath.Size = new System.Drawing.Size(365, 20);
            this.tbxMusicPath.TabIndex = 35;
            // 
            // btnChangeGeneralCatalogPath
            // 
            this.btnChangeGeneralCatalogPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeGeneralCatalogPath.Location = new System.Drawing.Point(386, 4);
            this.btnChangeGeneralCatalogPath.Name = "btnChangeGeneralCatalogPath";
            this.btnChangeGeneralCatalogPath.Size = new System.Drawing.Size(86, 20);
            this.btnChangeGeneralCatalogPath.TabIndex = 36;
            this.btnChangeGeneralCatalogPath.Text = "General Path";
            this.btnChangeGeneralCatalogPath.UseVisualStyleBackColor = true;
            this.btnChangeGeneralCatalogPath.Click += new System.EventHandler(this.btnChangeGeneralCatalogPath_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(33, 60);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(21, 15);
            this.lblProgress.TabIndex = 37;
            this.lblProgress.Text = "<>";
            // 
            // chbxMove
            // 
            this.chbxMove.AutoSize = true;
            this.chbxMove.BackColor = System.Drawing.Color.LightCoral;
            this.chbxMove.Location = new System.Drawing.Point(136, 25);
            this.chbxMove.Margin = new System.Windows.Forms.Padding(2);
            this.chbxMove.Name = "chbxMove";
            this.chbxMove.Size = new System.Drawing.Size(103, 17);
            this.chbxMove.TabIndex = 36;
            this.chbxMove.Text = "Delete one stars";
            this.chbxMove.UseVisualStyleBackColor = false;
            // 
            // MusicLibraryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 509);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnChangeGeneralCatalogPath);
            this.Controls.Add(this.tbxMusicPath);
            this.Controls.Add(this.tbxSearchTracks);
            this.Controls.Add(this.btnFindTracks);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.btnDeleteArtistFromDB);
            this.Controls.Add(this.tbxDeleteArtistFromDB);
            this.Controls.Add(this.btnFindAlbums);
            this.Controls.Add(this.tbxSearchAlbums);
            this.Controls.Add(this.tabCtrTrackAlbums);
            this.Controls.Add(this.ButtonReadTag);
            this.Controls.Add(this.BoxListConsole);
            this.Controls.Add(this.changeMainPath);
            this.Controls.Add(this.tbxPickedPath);
            this.Name = "MusicLibraryWindow";
            this.Text = "Music Library";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabCtrTrackAlbums.ResumeLayout(false);
            this.tabAlbums.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AlbumsDataGridView)).EndInit();
            this.tabTracks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TracksDataGridView)).EndInit();
            this.tabArtist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArtists)).EndInit();
            this.tabSetup.ResumeLayout(false);
            this.tabSetup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.BindingSource albumListBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TextBox tbxPickedPath;
        private System.Windows.Forms.Button changeMainPath;
        private System.Windows.Forms.Button ButtonReadTag;
        private System.Windows.Forms.CheckBox CheckBoxProcessCatalogs;
        private System.Windows.Forms.CheckBox CheckBoxModifyFIles;
        private System.Windows.Forms.BindingSource trackListBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

        private System.Windows.Forms.BindingSource musicLibraryDataSetBindingSource;

        private System.Windows.Forms.BindingSource trackBindingSource;

        private System.Windows.Forms.TabControl tabCtrTrackAlbums;
        private System.Windows.Forms.TabPage tabAlbums;
        private System.Windows.Forms.TabPage tabTracks;
        private System.Windows.Forms.BindingSource musicLibraryDataSetBindingSource1;
        //private AlbumDataSet albumDataSet;
        private System.Windows.Forms.BindingSource albumBindingSource;
        private System.Windows.Forms.TextBox tbxSearchAlbums;
        private System.Windows.Forms.Button btnFindAlbums;
        private System.Windows.Forms.BindingSource albumBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idAlbumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn albumDirectoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn artistDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn releaseYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn artistCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn albumCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn genreCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ratingCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource albumBindingSource2;
        private System.Windows.Forms.Button btnDeleteArtistFromDB;
        private System.Windows.Forms.TextBox tbxDeleteArtistFromDB;
        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.Button btnFindTracks;
        private System.Windows.Forms.TextBox tbxSearchTracks;
        private System.Windows.Forms.CheckBox CheckBoxWriteIndexes;
        private System.Windows.Forms.ListBox BoxListConsole;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDeclareGenre;
        private System.Windows.Forms.DataGridView AlbumsDataGridView;
        private System.Windows.Forms.DataGridView TracksDataGridView;
        private System.Windows.Forms.Button btnProcessSelected;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbxMusicPath;
        private System.Windows.Forms.Button btnChangeGeneralCatalogPath;
        private System.Windows.Forms.Button btnSelectHealthy;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TabPage tabArtist;
        private System.Windows.Forms.TabPage tabSetup;
        private System.Windows.Forms.CheckBox checkBoxCreateBackUp;
        private System.Windows.Forms.Button btnDiscogs;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnCheckTrackDuplicates;
        private System.Windows.Forms.DataGridView dgvArtists;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chbxMove;
    }
}

