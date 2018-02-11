namespace MusicProjectLibrary_1.AppForms
{
    partial class PickArtist
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
            this.dgvArtists = new System.Windows.Forms.DataGridView();
            this.btnVariousArtists = new System.Windows.Forms.Button();
            this.tbxSelectedAlbum = new System.Windows.Forms.TextBox();
            this.btnTextbox = new System.Windows.Forms.Button();
            this.tbxWrittenArtist = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArtists)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvArtists
            // 
            this.dgvArtists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvArtists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArtists.Location = new System.Drawing.Point(2, 97);
            this.dgvArtists.Name = "dgvArtists";
            this.dgvArtists.Size = new System.Drawing.Size(304, 445);
            this.dgvArtists.TabIndex = 44;
            this.dgvArtists.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArtists_CellDoubleClick);
            // 
            // btnVariousArtists
            // 
            this.btnVariousArtists.Location = new System.Drawing.Point(214, 66);
            this.btnVariousArtists.Name = "btnVariousArtists";
            this.btnVariousArtists.Size = new System.Drawing.Size(92, 23);
            this.btnVariousArtists.TabIndex = 46;
            this.btnVariousArtists.Text = "Various Artists";
            this.btnVariousArtists.UseVisualStyleBackColor = true;
            this.btnVariousArtists.Click += new System.EventHandler(this.btnVariousArtists_Click);
            // 
            // tbxSelectedAlbum
            // 
            this.tbxSelectedAlbum.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbxSelectedAlbum.Location = new System.Drawing.Point(2, 7);
            this.tbxSelectedAlbum.Multiline = true;
            this.tbxSelectedAlbum.Name = "tbxSelectedAlbum";
            this.tbxSelectedAlbum.ReadOnly = true;
            this.tbxSelectedAlbum.Size = new System.Drawing.Size(304, 40);
            this.tbxSelectedAlbum.TabIndex = 47;
            this.tbxSelectedAlbum.Text = "<no selected album>";
            // 
            // btnTextbox
            // 
            this.btnTextbox.Location = new System.Drawing.Point(119, 66);
            this.btnTextbox.Name = "btnTextbox";
            this.btnTextbox.Size = new System.Drawing.Size(89, 23);
            this.btnTextbox.TabIndex = 48;
            this.btnTextbox.Text = "Tbx Artist";
            this.btnTextbox.UseVisualStyleBackColor = true;
            this.btnTextbox.Click += new System.EventHandler(this.btnTextbox_Click);
            // 
            // tbxWrittenArtist
            // 
            this.tbxWrittenArtist.Location = new System.Drawing.Point(2, 66);
            this.tbxWrittenArtist.Name = "tbxWrittenArtist";
            this.tbxWrittenArtist.Size = new System.Drawing.Size(111, 20);
            this.tbxWrittenArtist.TabIndex = 49;
            // 
            // PickArtist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 543);
            this.Controls.Add(this.tbxWrittenArtist);
            this.Controls.Add(this.btnTextbox);
            this.Controls.Add(this.tbxSelectedAlbum);
            this.Controls.Add(this.btnVariousArtists);
            this.Controls.Add(this.dgvArtists);
            this.Name = "PickArtist";
            this.Text = "Select Artist";
            this.Load += new System.EventHandler(this.PickArtist_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArtists)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvArtists;
        private System.Windows.Forms.Button btnVariousArtists;
        private System.Windows.Forms.TextBox tbxSelectedAlbum;
        private System.Windows.Forms.Button btnTextbox;
        private System.Windows.Forms.TextBox tbxWrittenArtist;
    }
}