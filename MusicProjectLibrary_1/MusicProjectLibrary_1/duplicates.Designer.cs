namespace MusicProjectLibrary_1
{
    partial class duplicatesForm
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
            this.dgvPaths = new System.Windows.Forms.DataGridView();
            this.tbxSelectedTrackID = new System.Windows.Forms.TextBox();
            this.tbxSelectedAlbumID = new System.Windows.Forms.TextBox();
            this.lblSelectedTrackID = new System.Windows.Forms.Label();
            this.lblSelectedAlbumID = new System.Windows.Forms.Label();
            this.btnSaveXML = new System.Windows.Forms.Button();
            this.btnCheckDuplicates = new System.Windows.Forms.Button();
            this.btnDeleteAlbumDBSQL = new System.Windows.Forms.Button();
            this.btnDeleteTrackDBSQL = new System.Windows.Forms.Button();
            this.lbxConsole = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaths)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPaths
            // 
            this.dgvPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPaths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPaths.Location = new System.Drawing.Point(5, 71);
            this.dgvPaths.Name = "dgvPaths";
            this.dgvPaths.Size = new System.Drawing.Size(382, 306);
            this.dgvPaths.TabIndex = 0;
            this.dgvPaths.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaths_CellContentClick);
            this.dgvPaths.SelectionChanged += new System.EventHandler(this.dgvPaths_SelectionChanged);
            this.dgvPaths.DoubleClick += new System.EventHandler(this.dgvPaths_DoubleClick);
            // 
            // tbxSelectedTrackID
            // 
            this.tbxSelectedTrackID.Location = new System.Drawing.Point(122, 45);
            this.tbxSelectedTrackID.Name = "tbxSelectedTrackID";
            this.tbxSelectedTrackID.Size = new System.Drawing.Size(100, 20);
            this.tbxSelectedTrackID.TabIndex = 1;
            // 
            // tbxSelectedAlbumID
            // 
            this.tbxSelectedAlbumID.Location = new System.Drawing.Point(122, 15);
            this.tbxSelectedAlbumID.Name = "tbxSelectedAlbumID";
            this.tbxSelectedAlbumID.Size = new System.Drawing.Size(100, 20);
            this.tbxSelectedAlbumID.TabIndex = 2;
            // 
            // lblSelectedTrackID
            // 
            this.lblSelectedTrackID.AutoSize = true;
            this.lblSelectedTrackID.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedTrackID.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.lblSelectedTrackID.Location = new System.Drawing.Point(3, 45);
            this.lblSelectedTrackID.Name = "lblSelectedTrackID";
            this.lblSelectedTrackID.Size = new System.Drawing.Size(108, 15);
            this.lblSelectedTrackID.TabIndex = 43;
            this.lblSelectedTrackID.Text = "Purgatory Track ID";
            // 
            // lblSelectedAlbumID
            // 
            this.lblSelectedAlbumID.AutoSize = true;
            this.lblSelectedAlbumID.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedAlbumID.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedAlbumID.Location = new System.Drawing.Point(2, 17);
            this.lblSelectedAlbumID.Name = "lblSelectedAlbumID";
            this.lblSelectedAlbumID.Size = new System.Drawing.Size(114, 15);
            this.lblSelectedAlbumID.TabIndex = 44;
            this.lblSelectedAlbumID.Text = "Purgatory Album ID";
            // 
            // btnSaveXML
            // 
            this.btnSaveXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveXML.Location = new System.Drawing.Point(87, 412);
            this.btnSaveXML.Name = "btnSaveXML";
            this.btnSaveXML.Size = new System.Drawing.Size(75, 23);
            this.btnSaveXML.TabIndex = 45;
            this.btnSaveXML.Text = "Save XML result";
            this.btnSaveXML.UseVisualStyleBackColor = true;
            // 
            // btnCheckDuplicates
            // 
            this.btnCheckDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckDuplicates.Location = new System.Drawing.Point(6, 412);
            this.btnCheckDuplicates.Name = "btnCheckDuplicates";
            this.btnCheckDuplicates.Size = new System.Drawing.Size(75, 23);
            this.btnCheckDuplicates.TabIndex = 46;
            this.btnCheckDuplicates.Text = "Check Duplicates";
            this.btnCheckDuplicates.UseVisualStyleBackColor = true;
            this.btnCheckDuplicates.Click += new System.EventHandler(this.btnCheckDuplicates_Click);
            // 
            // btnDeleteAlbumDBSQL
            // 
            this.btnDeleteAlbumDBSQL.Location = new System.Drawing.Point(228, 13);
            this.btnDeleteAlbumDBSQL.Name = "btnDeleteAlbumDBSQL";
            this.btnDeleteAlbumDBSQL.Size = new System.Drawing.Size(137, 23);
            this.btnDeleteAlbumDBSQL.TabIndex = 47;
            this.btnDeleteAlbumDBSQL.Text = "Delete album DB + SQL";
            this.btnDeleteAlbumDBSQL.UseVisualStyleBackColor = true;
            // 
            // btnDeleteTrackDBSQL
            // 
            this.btnDeleteTrackDBSQL.Location = new System.Drawing.Point(228, 43);
            this.btnDeleteTrackDBSQL.Name = "btnDeleteTrackDBSQL";
            this.btnDeleteTrackDBSQL.Size = new System.Drawing.Size(137, 23);
            this.btnDeleteTrackDBSQL.TabIndex = 48;
            this.btnDeleteTrackDBSQL.Text = "Delete track DB + SQL";
            this.btnDeleteTrackDBSQL.UseVisualStyleBackColor = true;
            this.btnDeleteTrackDBSQL.Click += new System.EventHandler(this.btnDeleteTrackDBSQL_Click);
            // 
            // lbxConsole
            // 
            this.lbxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxConsole.BackColor = System.Drawing.SystemColors.Desktop;
            this.lbxConsole.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.lbxConsole.FormattingEnabled = true;
            this.lbxConsole.Location = new System.Drawing.Point(165, 381);
            this.lbxConsole.Name = "lbxConsole";
            this.lbxConsole.Size = new System.Drawing.Size(219, 56);
            this.lbxConsole.TabIndex = 49;
            // 
            // duplicatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 439);
            this.Controls.Add(this.lbxConsole);
            this.Controls.Add(this.btnDeleteTrackDBSQL);
            this.Controls.Add(this.btnDeleteAlbumDBSQL);
            this.Controls.Add(this.btnCheckDuplicates);
            this.Controls.Add(this.btnSaveXML);
            this.Controls.Add(this.lblSelectedAlbumID);
            this.Controls.Add(this.lblSelectedTrackID);
            this.Controls.Add(this.tbxSelectedAlbumID);
            this.Controls.Add(this.tbxSelectedTrackID);
            this.Controls.Add(this.dgvPaths);
            this.Name = "duplicatesForm";
            this.Text = "Track Duplicates";
            this.Load += new System.EventHandler(this.duplicates_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaths)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPaths;
        private System.Windows.Forms.TextBox tbxSelectedTrackID;
        private System.Windows.Forms.TextBox tbxSelectedAlbumID;
        private System.Windows.Forms.Label lblSelectedTrackID;
        private System.Windows.Forms.Label lblSelectedAlbumID;
        private System.Windows.Forms.Button btnSaveXML;
        private System.Windows.Forms.Button btnCheckDuplicates;
        private System.Windows.Forms.Button btnDeleteAlbumDBSQL;
        private System.Windows.Forms.Button btnDeleteTrackDBSQL;
        private System.Windows.Forms.ListBox lbxConsole;
    }
}