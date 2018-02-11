namespace MusicProjectLibrary_1.AppForms
{
    partial class PickAlbumName
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
            this.tbxWriteName = new System.Windows.Forms.TextBox();
            this.btnAddName = new System.Windows.Forms.Button();
            this.tbxSelectedAlbum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbxWriteName
            // 
            this.tbxWriteName.Location = new System.Drawing.Point(6, 56);
            this.tbxWriteName.Name = "tbxWriteName";
            this.tbxWriteName.Size = new System.Drawing.Size(260, 20);
            this.tbxWriteName.TabIndex = 0;
            // 
            // btnAddName
            // 
            this.btnAddName.Location = new System.Drawing.Point(191, 82);
            this.btnAddName.Name = "btnAddName";
            this.btnAddName.Size = new System.Drawing.Size(75, 23);
            this.btnAddName.TabIndex = 1;
            this.btnAddName.Text = "Add Name";
            this.btnAddName.UseVisualStyleBackColor = true;
            this.btnAddName.Click += new System.EventHandler(this.btnAddName_Click);
            // 
            // tbxSelectedAlbum
            // 
            this.tbxSelectedAlbum.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbxSelectedAlbum.Location = new System.Drawing.Point(6, 6);
            this.tbxSelectedAlbum.Multiline = true;
            this.tbxSelectedAlbum.Name = "tbxSelectedAlbum";
            this.tbxSelectedAlbum.ReadOnly = true;
            this.tbxSelectedAlbum.Size = new System.Drawing.Size(260, 44);
            this.tbxSelectedAlbum.TabIndex = 48;
            this.tbxSelectedAlbum.Text = "<no selected album>";
            // 
            // PickAlbumName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 111);
            this.Controls.Add(this.tbxSelectedAlbum);
            this.Controls.Add(this.btnAddName);
            this.Controls.Add(this.tbxWriteName);
            this.Name = "PickAlbumName";
            this.Text = "Select Album Name";
            this.Load += new System.EventHandler(this.PickAlbumName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxWriteName;
        private System.Windows.Forms.Button btnAddName;
        private System.Windows.Forms.TextBox tbxSelectedAlbum;
    }
}