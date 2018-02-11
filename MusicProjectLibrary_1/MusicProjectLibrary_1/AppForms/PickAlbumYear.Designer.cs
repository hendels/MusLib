namespace MusicProjectLibrary_1.AppForms
{
    partial class PickAlbumYear
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
            this.btnAddYear = new System.Windows.Forms.Button();
            this.btnDiscogs = new System.Windows.Forms.Button();
            this.cmbSelectYear = new System.Windows.Forms.ComboBox();
            this.tbxSelectedAlbum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAddYear
            // 
            this.btnAddYear.Location = new System.Drawing.Point(136, 80);
            this.btnAddYear.Name = "btnAddYear";
            this.btnAddYear.Size = new System.Drawing.Size(81, 23);
            this.btnAddYear.TabIndex = 3;
            this.btnAddYear.Text = "Add Year";
            this.btnAddYear.UseVisualStyleBackColor = true;
            this.btnAddYear.Click += new System.EventHandler(this.btnAddYear_Click);
            // 
            // btnDiscogs
            // 
            this.btnDiscogs.Location = new System.Drawing.Point(49, 80);
            this.btnDiscogs.Name = "btnDiscogs";
            this.btnDiscogs.Size = new System.Drawing.Size(81, 23);
            this.btnDiscogs.TabIndex = 4;
            this.btnDiscogs.Text = "Discogs";
            this.btnDiscogs.UseVisualStyleBackColor = true;
            this.btnDiscogs.Click += new System.EventHandler(this.btnDiscogs_Click);
            // 
            // cmbSelectYear
            // 
            this.cmbSelectYear.FormattingEnabled = true;
            this.cmbSelectYear.Location = new System.Drawing.Point(12, 53);
            this.cmbSelectYear.Name = "cmbSelectYear";
            this.cmbSelectYear.Size = new System.Drawing.Size(205, 21);
            this.cmbSelectYear.TabIndex = 5;
            this.cmbSelectYear.SelectedIndexChanged += new System.EventHandler(this.cmbSelectYear_SelectedIndexChanged);
            // 
            // tbxSelectedAlbum
            // 
            this.tbxSelectedAlbum.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbxSelectedAlbum.Location = new System.Drawing.Point(12, 12);
            this.tbxSelectedAlbum.Multiline = true;
            this.tbxSelectedAlbum.Name = "tbxSelectedAlbum";
            this.tbxSelectedAlbum.ReadOnly = true;
            this.tbxSelectedAlbum.Size = new System.Drawing.Size(205, 20);
            this.tbxSelectedAlbum.TabIndex = 7;
            this.tbxSelectedAlbum.Text = "<no selected album>";
            // 
            // PickAlbumYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 109);
            this.Controls.Add(this.tbxSelectedAlbum);
            this.Controls.Add(this.cmbSelectYear);
            this.Controls.Add(this.btnDiscogs);
            this.Controls.Add(this.btnAddYear);
            this.Name = "PickAlbumYear";
            this.Text = "PickAlbumYear";
            this.Load += new System.EventHandler(this.PickAlbumYear_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddYear;
        private System.Windows.Forms.Button btnDiscogs;
        private System.Windows.Forms.ComboBox cmbSelectYear;
        private System.Windows.Forms.TextBox tbxSelectedAlbum;
    }
}