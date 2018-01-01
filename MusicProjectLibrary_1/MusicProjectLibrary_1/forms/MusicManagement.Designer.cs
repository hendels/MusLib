namespace MusicProjectLibrary_1
{
    partial class AlbumWindow
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
            this.albumListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BoxPickedPath = new System.Windows.Forms.TextBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.moveFileButt = new System.Windows.Forms.Button();
            this.checkFilesInDirectory = new System.Windows.Forms.Button();
            this.ListBoxItems = new System.Windows.Forms.ListBox();
            this.tagTest = new System.Windows.Forms.Button();
            this.BoxListConsole = new System.Windows.Forms.ListBox();
            this.ButtonReadTag = new System.Windows.Forms.Button();
            this.CheckBoxProcessCatalogs = new System.Windows.Forms.CheckBox();
            this.CheckBoxModifyFIles = new System.Windows.Forms.CheckBox();
            this.trackListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.IdTrack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.albumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ratingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mODDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.musicLibraryDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.musicLibraryDataSet = new MusicProjectLibrary_1.MusicLibraryDataSet();
            this.btnNew = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnSave = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnDelete = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnEdit = new MaterialSkin.Controls.MaterialFlatButton();
            this.trackTableAdapter = new MusicProjectLibrary_1.MusicLibraryDataSetTableAdapters.TrackTableAdapter();
            this.tabCtrTrackAlbums = new System.Windows.Forms.TabControl();
            this.tabTracks = new System.Windows.Forms.TabPage();
            this.tabAlbums = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idAlbumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.albumDirectoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.artistDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.releaseYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genreDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.artistCheckDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.albumCheckDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.genreCheckDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ratingCheckDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.albumBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.albumDataSet = new MusicProjectLibrary_1.AlbumDataSet();
            this.musicLibraryDataSetBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.albumTableAdapter = new MusicProjectLibrary_1.AlbumDataSetTableAdapters.AlbumTableAdapter();
            this.lstAlbums = new System.Windows.Forms.ListBox();
            this.tbxSearchAlbums = new System.Windows.Forms.TextBox();
            this.btnFindAlbums = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.albumListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSet)).BeginInit();
            this.tabCtrTrackAlbums.SuspendLayout();
            this.tabTracks.SuspendLayout();
            this.tabAlbums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // albumListBindingSource
            // 
            this.albumListBindingSource.DataMember = "albumList";
            // 
            // BoxPickedPath
            // 
            this.BoxPickedPath.BackColor = System.Drawing.Color.Khaki;
            this.BoxPickedPath.Location = new System.Drawing.Point(12, 77);
            this.BoxPickedPath.Name = "BoxPickedPath";
            this.BoxPickedPath.Size = new System.Drawing.Size(469, 20);
            this.BoxPickedPath.TabIndex = 10;
            this.BoxPickedPath.TextChanged += new System.EventHandler(this.siemanoBox_TextChanged);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(12, 441);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(119, 22);
            this.updateButton.TabIndex = 11;
            this.updateButton.Text = "update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // moveFileButt
            // 
            this.moveFileButt.Location = new System.Drawing.Point(137, 441);
            this.moveFileButt.Name = "moveFileButt";
            this.moveFileButt.Size = new System.Drawing.Size(119, 23);
            this.moveFileButt.TabIndex = 12;
            this.moveFileButt.Text = "move file";
            this.moveFileButt.UseVisualStyleBackColor = true;
            this.moveFileButt.Click += new System.EventHandler(this.moveFileButt_Click);
            // 
            // checkFilesInDirectory
            // 
            this.checkFilesInDirectory.Location = new System.Drawing.Point(262, 441);
            this.checkFilesInDirectory.Name = "checkFilesInDirectory";
            this.checkFilesInDirectory.Size = new System.Drawing.Size(119, 23);
            this.checkFilesInDirectory.TabIndex = 13;
            this.checkFilesInDirectory.Text = "check files in Dir";
            this.checkFilesInDirectory.UseVisualStyleBackColor = true;
            this.checkFilesInDirectory.Click += new System.EventHandler(this.checkFilesInDirectory_Click);
            // 
            // ListBoxItems
            // 
            this.ListBoxItems.FormattingEnabled = true;
            this.ListBoxItems.Location = new System.Drawing.Point(614, 75);
            this.ListBoxItems.Name = "ListBoxItems";
            this.ListBoxItems.Size = new System.Drawing.Size(142, 56);
            this.ListBoxItems.TabIndex = 14;
            // 
            // tagTest
            // 
            this.tagTest.Location = new System.Drawing.Point(387, 441);
            this.tagTest.Name = "tagTest";
            this.tagTest.Size = new System.Drawing.Size(119, 23);
            this.tagTest.TabIndex = 15;
            this.tagTest.Text = "changeTagTest";
            this.tagTest.UseVisualStyleBackColor = true;
            this.tagTest.Click += new System.EventHandler(this.tagTest_Click);
            // 
            // BoxListConsole
            // 
            this.BoxListConsole.FormattingEnabled = true;
            this.BoxListConsole.Location = new System.Drawing.Point(12, 379);
            this.BoxListConsole.Name = "BoxListConsole";
            this.BoxListConsole.Size = new System.Drawing.Size(756, 56);
            this.BoxListConsole.TabIndex = 16;
            // 
            // ButtonReadTag
            // 
            this.ButtonReadTag.Location = new System.Drawing.Point(655, 440);
            this.ButtonReadTag.Name = "ButtonReadTag";
            this.ButtonReadTag.Size = new System.Drawing.Size(119, 23);
            this.ButtonReadTag.TabIndex = 17;
            this.ButtonReadTag.Text = "readTagTest";
            this.ButtonReadTag.UseVisualStyleBackColor = true;
            this.ButtonReadTag.Click += new System.EventHandler(this.ButtonReadTag_Click);
            // 
            // CheckBoxProcessCatalogs
            // 
            this.CheckBoxProcessCatalogs.AutoSize = true;
            this.CheckBoxProcessCatalogs.Checked = true;
            this.CheckBoxProcessCatalogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxProcessCatalogs.Location = new System.Drawing.Point(486, 77);
            this.CheckBoxProcessCatalogs.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxProcessCatalogs.Name = "CheckBoxProcessCatalogs";
            this.CheckBoxProcessCatalogs.Size = new System.Drawing.Size(107, 17);
            this.CheckBoxProcessCatalogs.TabIndex = 19;
            this.CheckBoxProcessCatalogs.Text = "Process catalogs";
            this.CheckBoxProcessCatalogs.UseVisualStyleBackColor = true;
            this.CheckBoxProcessCatalogs.CheckedChanged += new System.EventHandler(this.CheckBoxProcessCatalogs_CheckedChanged);
            // 
            // CheckBoxModifyFIles
            // 
            this.CheckBoxModifyFIles.AutoSize = true;
            this.CheckBoxModifyFIles.Location = new System.Drawing.Point(486, 98);
            this.CheckBoxModifyFIles.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxModifyFIles.Name = "CheckBoxModifyFIles";
            this.CheckBoxModifyFIles.Size = new System.Drawing.Size(81, 17);
            this.CheckBoxModifyFIles.TabIndex = 20;
            this.CheckBoxModifyFIles.Text = "Modify Files";
            this.CheckBoxModifyFIles.UseVisualStyleBackColor = true;
            this.CheckBoxModifyFIles.CheckedChanged += new System.EventHandler(this.CheckBoxModifyFIles_CheckedChanged);
            // 
            // trackListBindingSource
            // 
            this.trackListBindingSource.DataMember = "trackList";
            // 
            // dataGridView
            // 
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdTrack,
            this.albumDataGridViewTextBoxColumn,
            this.directoryDataGridViewTextBoxColumn,
            this.genreDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.ratingDataGridViewTextBoxColumn,
            this.mODDateDataGridViewTextBoxColumn});
            this.dataGridView.DataSource = this.trackBindingSource;
            this.dataGridView.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView.Location = new System.Drawing.Point(6, 6);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(746, 154);
            this.dataGridView.TabIndex = 21;
            this.dataGridView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridView_KeyPress);
            // 
            // IdTrack
            // 
            this.IdTrack.DataPropertyName = "IdTrack";
            this.IdTrack.HeaderText = "IdTrack";
            this.IdTrack.Name = "IdTrack";
            this.IdTrack.ReadOnly = true;
            // 
            // albumDataGridViewTextBoxColumn
            // 
            this.albumDataGridViewTextBoxColumn.DataPropertyName = "Album";
            this.albumDataGridViewTextBoxColumn.HeaderText = "Album";
            this.albumDataGridViewTextBoxColumn.Name = "albumDataGridViewTextBoxColumn";
            // 
            // directoryDataGridViewTextBoxColumn
            // 
            this.directoryDataGridViewTextBoxColumn.DataPropertyName = "Directory";
            this.directoryDataGridViewTextBoxColumn.HeaderText = "Directory";
            this.directoryDataGridViewTextBoxColumn.Name = "directoryDataGridViewTextBoxColumn";
            // 
            // genreDataGridViewTextBoxColumn
            // 
            this.genreDataGridViewTextBoxColumn.DataPropertyName = "Genre";
            this.genreDataGridViewTextBoxColumn.HeaderText = "Genre";
            this.genreDataGridViewTextBoxColumn.Name = "genreDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // ratingDataGridViewTextBoxColumn
            // 
            this.ratingDataGridViewTextBoxColumn.DataPropertyName = "Rating";
            this.ratingDataGridViewTextBoxColumn.HeaderText = "Rating";
            this.ratingDataGridViewTextBoxColumn.Name = "ratingDataGridViewTextBoxColumn";
            // 
            // mODDateDataGridViewTextBoxColumn
            // 
            this.mODDateDataGridViewTextBoxColumn.DataPropertyName = "MODDate";
            this.mODDateDataGridViewTextBoxColumn.HeaderText = "MODDate";
            this.mODDateDataGridViewTextBoxColumn.Name = "mODDateDataGridViewTextBoxColumn";
            // 
            // trackBindingSource
            // 
            this.trackBindingSource.DataMember = "Track";
            this.trackBindingSource.DataSource = this.musicLibraryDataSetBindingSource;
            // 
            // musicLibraryDataSetBindingSource
            // 
            this.musicLibraryDataSetBindingSource.DataSource = this.musicLibraryDataSet;
            this.musicLibraryDataSetBindingSource.Position = 0;
            // 
            // musicLibraryDataSet
            // 
            this.musicLibraryDataSet.DataSetName = "MusicLibraryDataSet";
            this.musicLibraryDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnNew
            // 
            this.btnNew.AutoSize = true;
            this.btnNew.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNew.Depth = 0;
            this.btnNew.Location = new System.Drawing.Point(12, 106);
            this.btnNew.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnNew.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnNew.Name = "btnNew";
            this.btnNew.Primary = false;
            this.btnNew.Size = new System.Drawing.Size(42, 36);
            this.btnNew.TabIndex = 22;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.Location = new System.Drawing.Point(62, 106);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Primary = false;
            this.btnSave.Size = new System.Drawing.Size(46, 36);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDelete.Depth = 0;
            this.btnDelete.Location = new System.Drawing.Point(116, 106);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDelete.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Primary = false;
            this.btnDelete.Size = new System.Drawing.Size(60, 36);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = true;
            this.btnEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEdit.Depth = 0;
            this.btnEdit.Location = new System.Drawing.Point(184, 106);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Primary = false;
            this.btnEdit.Size = new System.Drawing.Size(41, 36);
            this.btnEdit.TabIndex = 25;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // trackTableAdapter
            // 
            this.trackTableAdapter.ClearBeforeFill = true;
            // 
            // tabCtrTrackAlbums
            // 
            this.tabCtrTrackAlbums.Controls.Add(this.tabTracks);
            this.tabCtrTrackAlbums.Controls.Add(this.tabAlbums);
            this.tabCtrTrackAlbums.Location = new System.Drawing.Point(12, 151);
            this.tabCtrTrackAlbums.Name = "tabCtrTrackAlbums";
            this.tabCtrTrackAlbums.SelectedIndex = 0;
            this.tabCtrTrackAlbums.Size = new System.Drawing.Size(762, 223);
            this.tabCtrTrackAlbums.TabIndex = 26;
            // 
            // tabTracks
            // 
            this.tabTracks.Controls.Add(this.dataGridView);
            this.tabTracks.Location = new System.Drawing.Point(4, 22);
            this.tabTracks.Name = "tabTracks";
            this.tabTracks.Padding = new System.Windows.Forms.Padding(3);
            this.tabTracks.Size = new System.Drawing.Size(754, 197);
            this.tabTracks.TabIndex = 0;
            this.tabTracks.Text = "Tracks";
            this.tabTracks.UseVisualStyleBackColor = true;
            // 
            // tabAlbums
            // 
            this.tabAlbums.Controls.Add(this.lstAlbums);
            this.tabAlbums.Controls.Add(this.dataGridView1);
            this.tabAlbums.Location = new System.Drawing.Point(4, 22);
            this.tabAlbums.Name = "tabAlbums";
            this.tabAlbums.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlbums.Size = new System.Drawing.Size(754, 197);
            this.tabAlbums.TabIndex = 1;
            this.tabAlbums.Text = "Albums";
            this.tabAlbums.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idAlbumDataGridViewTextBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.albumDirectoryDataGridViewTextBoxColumn,
            this.artistDataGridViewTextBoxColumn,
            this.releaseYearDataGridViewTextBoxColumn,
            this.genreDataGridViewTextBoxColumn1,
            this.artistCheckDataGridViewCheckBoxColumn,
            this.albumCheckDataGridViewCheckBoxColumn,
            this.genreCheckDataGridViewCheckBoxColumn,
            this.ratingCheckDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.albumBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(362, 188);
            this.dataGridView1.TabIndex = 0;
            // 
            // idAlbumDataGridViewTextBoxColumn
            // 
            this.idAlbumDataGridViewTextBoxColumn.DataPropertyName = "IdAlbum";
            this.idAlbumDataGridViewTextBoxColumn.HeaderText = "IdAlbum";
            this.idAlbumDataGridViewTextBoxColumn.Name = "idAlbumDataGridViewTextBoxColumn";
            this.idAlbumDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            // 
            // albumDirectoryDataGridViewTextBoxColumn
            // 
            this.albumDirectoryDataGridViewTextBoxColumn.DataPropertyName = "AlbumDirectory";
            this.albumDirectoryDataGridViewTextBoxColumn.HeaderText = "AlbumDirectory";
            this.albumDirectoryDataGridViewTextBoxColumn.Name = "albumDirectoryDataGridViewTextBoxColumn";
            // 
            // artistDataGridViewTextBoxColumn
            // 
            this.artistDataGridViewTextBoxColumn.DataPropertyName = "Artist";
            this.artistDataGridViewTextBoxColumn.HeaderText = "Artist";
            this.artistDataGridViewTextBoxColumn.Name = "artistDataGridViewTextBoxColumn";
            // 
            // releaseYearDataGridViewTextBoxColumn
            // 
            this.releaseYearDataGridViewTextBoxColumn.DataPropertyName = "Release Year";
            this.releaseYearDataGridViewTextBoxColumn.HeaderText = "Release Year";
            this.releaseYearDataGridViewTextBoxColumn.Name = "releaseYearDataGridViewTextBoxColumn";
            // 
            // genreDataGridViewTextBoxColumn1
            // 
            this.genreDataGridViewTextBoxColumn1.DataPropertyName = "Genre";
            this.genreDataGridViewTextBoxColumn1.HeaderText = "Genre";
            this.genreDataGridViewTextBoxColumn1.Name = "genreDataGridViewTextBoxColumn1";
            // 
            // artistCheckDataGridViewCheckBoxColumn
            // 
            this.artistCheckDataGridViewCheckBoxColumn.DataPropertyName = "ArtistCheck";
            this.artistCheckDataGridViewCheckBoxColumn.HeaderText = "ArtistCheck";
            this.artistCheckDataGridViewCheckBoxColumn.Name = "artistCheckDataGridViewCheckBoxColumn";
            this.artistCheckDataGridViewCheckBoxColumn.Width = 25;
            // 
            // albumCheckDataGridViewCheckBoxColumn
            // 
            this.albumCheckDataGridViewCheckBoxColumn.DataPropertyName = "AlbumCheck";
            this.albumCheckDataGridViewCheckBoxColumn.HeaderText = "AlbumCheck";
            this.albumCheckDataGridViewCheckBoxColumn.Name = "albumCheckDataGridViewCheckBoxColumn";
            this.albumCheckDataGridViewCheckBoxColumn.Width = 25;
            // 
            // genreCheckDataGridViewCheckBoxColumn
            // 
            this.genreCheckDataGridViewCheckBoxColumn.DataPropertyName = "GenreCheck";
            this.genreCheckDataGridViewCheckBoxColumn.HeaderText = "GenreCheck";
            this.genreCheckDataGridViewCheckBoxColumn.Name = "genreCheckDataGridViewCheckBoxColumn";
            this.genreCheckDataGridViewCheckBoxColumn.Width = 25;
            // 
            // ratingCheckDataGridViewCheckBoxColumn
            // 
            this.ratingCheckDataGridViewCheckBoxColumn.DataPropertyName = "RatingCheck";
            this.ratingCheckDataGridViewCheckBoxColumn.HeaderText = "RatingCheck";
            this.ratingCheckDataGridViewCheckBoxColumn.Name = "ratingCheckDataGridViewCheckBoxColumn";
            this.ratingCheckDataGridViewCheckBoxColumn.Width = 25;
            // 
            // albumBindingSource
            // 
            this.albumBindingSource.DataMember = "Album";
            this.albumBindingSource.DataSource = this.albumDataSet;
            // 
            // albumDataSet
            // 
            this.albumDataSet.DataSetName = "AlbumDataSet";
            this.albumDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // musicLibraryDataSetBindingSource1
            // 
            this.musicLibraryDataSetBindingSource1.DataSource = this.musicLibraryDataSet;
            this.musicLibraryDataSetBindingSource1.Position = 0;
            // 
            // albumTableAdapter
            // 
            this.albumTableAdapter.ClearBeforeFill = true;
            // 
            // lstAlbums
            // 
            this.lstAlbums.FormattingEnabled = true;
            this.lstAlbums.Location = new System.Drawing.Point(372, 6);
            this.lstAlbums.Name = "lstAlbums";
            this.lstAlbums.Size = new System.Drawing.Size(369, 186);
            this.lstAlbums.TabIndex = 1;
            // 
            // tbxSearchAlbums
            // 
            this.tbxSearchAlbums.Location = new System.Drawing.Point(493, 137);
            this.tbxSearchAlbums.Name = "tbxSearchAlbums";
            this.tbxSearchAlbums.Size = new System.Drawing.Size(100, 20);
            this.tbxSearchAlbums.TabIndex = 27;
            // 
            // btnFindAlbums
            // 
            this.btnFindAlbums.Location = new System.Drawing.Point(400, 137);
            this.btnFindAlbums.Name = "btnFindAlbums";
            this.btnFindAlbums.Size = new System.Drawing.Size(75, 23);
            this.btnFindAlbums.TabIndex = 28;
            this.btnFindAlbums.Text = "Find albums";
            this.btnFindAlbums.UseVisualStyleBackColor = true;
            this.btnFindAlbums.Click += new System.EventHandler(this.btnFindAlbums_Click);
            // 
            // AlbumWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 465);
            this.Controls.Add(this.btnFindAlbums);
            this.Controls.Add(this.tbxSearchAlbums);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.tabCtrTrackAlbums);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.CheckBoxModifyFIles);
            this.Controls.Add(this.CheckBoxProcessCatalogs);
            this.Controls.Add(this.ButtonReadTag);
            this.Controls.Add(this.BoxListConsole);
            this.Controls.Add(this.tagTest);
            this.Controls.Add(this.ListBoxItems);
            this.Controls.Add(this.checkFilesInDirectory);
            this.Controls.Add(this.moveFileButt);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.BoxPickedPath);
            this.Name = "AlbumWindow";
            this.Text = "Albums";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.albumListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.musicLibraryDataSet)).EndInit();
            this.tabCtrTrackAlbums.ResumeLayout(false);
            this.tabTracks.ResumeLayout(false);
            this.tabAlbums.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.albumDataSet)).EndInit();
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
        private System.Windows.Forms.TextBox BoxPickedPath;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button moveFileButt;
        private System.Windows.Forms.Button checkFilesInDirectory;
        private System.Windows.Forms.ListBox ListBoxItems;
        private System.Windows.Forms.Button tagTest;
        private System.Windows.Forms.ListBox BoxListConsole;
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
        private System.Windows.Forms.DataGridView dataGridView;
        private MaterialSkin.Controls.MaterialFlatButton btnNew;
        private MaterialSkin.Controls.MaterialFlatButton btnSave;
        private MaterialSkin.Controls.MaterialFlatButton btnDelete;
        private MaterialSkin.Controls.MaterialFlatButton btnEdit;
        private System.Windows.Forms.BindingSource musicLibraryDataSetBindingSource;
        private MusicLibraryDataSet musicLibraryDataSet;
        private System.Windows.Forms.BindingSource trackBindingSource;
        private MusicLibraryDataSetTableAdapters.TrackTableAdapter trackTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdTrack;
        private System.Windows.Forms.DataGridViewTextBoxColumn albumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn directoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ratingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mODDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabControl tabCtrTrackAlbums;
        private System.Windows.Forms.TabPage tabTracks;
        private System.Windows.Forms.TabPage tabAlbums;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource musicLibraryDataSetBindingSource1;
        private AlbumDataSet albumDataSet;
        private System.Windows.Forms.BindingSource albumBindingSource;
        private AlbumDataSetTableAdapters.AlbumTableAdapter albumTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idAlbumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn albumDirectoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn artistDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn releaseYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genreDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn artistCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn albumCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn genreCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ratingCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.ListBox lstAlbums;
        private System.Windows.Forms.TextBox tbxSearchAlbums;
        private System.Windows.Forms.Button btnFindAlbums;
    }
}

