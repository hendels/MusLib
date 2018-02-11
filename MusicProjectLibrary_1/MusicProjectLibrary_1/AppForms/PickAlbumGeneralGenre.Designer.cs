namespace MusicProjectLibrary_1.AppForms
{
    partial class PickAlbumGeneralGenre
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
            this.btnVariousGenre = new System.Windows.Forms.Button();
            this.btnDiscogs = new System.Windows.Forms.Button();
            this.btnWrittenGenre = new System.Windows.Forms.Button();
            this.tbxWriteGenre = new System.Windows.Forms.TextBox();
            this.tbxSelectedAlbum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnVariousGenre
            // 
            this.btnVariousGenre.Location = new System.Drawing.Point(13, 79);
            this.btnVariousGenre.Name = "btnVariousGenre";
            this.btnVariousGenre.Size = new System.Drawing.Size(75, 23);
            this.btnVariousGenre.TabIndex = 0;
            this.btnVariousGenre.Text = "Various";
            this.btnVariousGenre.UseVisualStyleBackColor = true;
            this.btnVariousGenre.Click += new System.EventHandler(this.btnVariousGenre_Click);
            // 
            // btnDiscogs
            // 
            this.btnDiscogs.Location = new System.Drawing.Point(107, 79);
            this.btnDiscogs.Name = "btnDiscogs";
            this.btnDiscogs.Size = new System.Drawing.Size(75, 23);
            this.btnDiscogs.TabIndex = 1;
            this.btnDiscogs.Text = "Discogs";
            this.btnDiscogs.UseVisualStyleBackColor = true;
            this.btnDiscogs.Click += new System.EventHandler(this.btnDiscogs_Click);
            // 
            // btnWrittenGenre
            // 
            this.btnWrittenGenre.Location = new System.Drawing.Point(202, 79);
            this.btnWrittenGenre.Name = "btnWrittenGenre";
            this.btnWrittenGenre.Size = new System.Drawing.Size(75, 23);
            this.btnWrittenGenre.TabIndex = 2;
            this.btnWrittenGenre.Text = "Tbx Genre";
            this.btnWrittenGenre.UseVisualStyleBackColor = true;
            this.btnWrittenGenre.Click += new System.EventHandler(this.btnWrittenGenre_Click);
            // 
            // tbxWriteGenre
            // 
            this.tbxWriteGenre.Location = new System.Drawing.Point(13, 52);
            this.tbxWriteGenre.Name = "tbxWriteGenre";
            this.tbxWriteGenre.Size = new System.Drawing.Size(169, 20);
            this.tbxWriteGenre.TabIndex = 3;
            this.tbxWriteGenre.Text = "<writeGenre>";
            // 
            // tbxSelectedAlbum
            // 
            this.tbxSelectedAlbum.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbxSelectedAlbum.Location = new System.Drawing.Point(14, 8);
            this.tbxSelectedAlbum.Multiline = true;
            this.tbxSelectedAlbum.Name = "tbxSelectedAlbum";
            this.tbxSelectedAlbum.ReadOnly = true;
            this.tbxSelectedAlbum.Size = new System.Drawing.Size(269, 20);
            this.tbxSelectedAlbum.TabIndex = 8;
            this.tbxSelectedAlbum.Text = "<no selected album>";
            // 
            // PickAlbumGeneralGenre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 113);
            this.Controls.Add(this.tbxSelectedAlbum);
            this.Controls.Add(this.tbxWriteGenre);
            this.Controls.Add(this.btnWrittenGenre);
            this.Controls.Add(this.btnDiscogs);
            this.Controls.Add(this.btnVariousGenre);
            this.Name = "PickAlbumGeneralGenre";
            this.Text = "General Genre";
            this.Load += new System.EventHandler(this.PickAlbumGeneralGenre_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVariousGenre;
        private System.Windows.Forms.Button btnDiscogs;
        private System.Windows.Forms.Button btnWrittenGenre;
        private System.Windows.Forms.TextBox tbxWriteGenre;
        private System.Windows.Forms.TextBox tbxSelectedAlbum;
    }
}