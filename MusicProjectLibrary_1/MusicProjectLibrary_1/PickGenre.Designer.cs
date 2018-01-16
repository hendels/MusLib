﻿namespace MusicProjectLibrary_1
{
    partial class PickGenre
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
            this.chlstGenres = new System.Windows.Forms.CheckedListBox();
            this.btnDeclareGenre = new System.Windows.Forms.Button();
            this.chlstSuggestedGenres = new System.Windows.Forms.CheckedListBox();
            this.tbxSelectedGenre = new System.Windows.Forms.TextBox();
            this.tbxWriteGenre = new System.Windows.Forms.TextBox();
            this.chlstRelatedGenresOnHDD = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // chlstGenres
            // 
            this.chlstGenres.FormattingEnabled = true;
            this.chlstGenres.Location = new System.Drawing.Point(12, 12);
            this.chlstGenres.Name = "chlstGenres";
            this.chlstGenres.Size = new System.Drawing.Size(260, 319);
            this.chlstGenres.TabIndex = 0;
            this.chlstGenres.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlstGenres_ItemCheck);
            this.chlstGenres.SelectedValueChanged += new System.EventHandler(this.chlstGenres_SelectedValueChanged);
            this.chlstGenres.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chlstGenres_KeyPress);
            this.chlstGenres.MouseLeave += new System.EventHandler(this.chlstGenres_MouseLeave);
            this.chlstGenres.Validated += new System.EventHandler(this.chlstGenres_Validated);
            // 
            // btnDeclareGenre
            // 
            this.btnDeclareGenre.Location = new System.Drawing.Point(12, 404);
            this.btnDeclareGenre.Name = "btnDeclareGenre";
            this.btnDeclareGenre.Size = new System.Drawing.Size(88, 23);
            this.btnDeclareGenre.TabIndex = 36;
            this.btnDeclareGenre.Text = "Declare Genre";
            this.btnDeclareGenre.UseVisualStyleBackColor = true;
            this.btnDeclareGenre.Click += new System.EventHandler(this.btnDeclareGenre_Click);
            // 
            // chlstSuggestedGenres
            // 
            this.chlstSuggestedGenres.FormattingEnabled = true;
            this.chlstSuggestedGenres.Location = new System.Drawing.Point(279, 12);
            this.chlstSuggestedGenres.Name = "chlstSuggestedGenres";
            this.chlstSuggestedGenres.Size = new System.Drawing.Size(260, 109);
            this.chlstSuggestedGenres.TabIndex = 37;
            this.chlstSuggestedGenres.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlstSuggestedGenres_ItemCheck);
            this.chlstSuggestedGenres.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chlstSuggestedGenres_KeyPress);
            // 
            // tbxSelectedGenre
            // 
            this.tbxSelectedGenre.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSelectedGenre.Location = new System.Drawing.Point(12, 363);
            this.tbxSelectedGenre.Multiline = true;
            this.tbxSelectedGenre.Name = "tbxSelectedGenre";
            this.tbxSelectedGenre.ReadOnly = true;
            this.tbxSelectedGenre.Size = new System.Drawing.Size(260, 35);
            this.tbxSelectedGenre.TabIndex = 38;
            this.tbxSelectedGenre.Text = "<select genre>";
            // 
            // tbxWriteGenre
            // 
            this.tbxWriteGenre.Location = new System.Drawing.Point(13, 337);
            this.tbxWriteGenre.Name = "tbxWriteGenre";
            this.tbxWriteGenre.Size = new System.Drawing.Size(259, 20);
            this.tbxWriteGenre.TabIndex = 39;
            this.tbxWriteGenre.Text = "<LastUsedGenre>";
            this.tbxWriteGenre.TextChanged += new System.EventHandler(this.tbxWriteGenre_TextChanged);
            // 
            // chlstRelatedGenresOnHDD
            // 
            this.chlstRelatedGenresOnHDD.FormattingEnabled = true;
            this.chlstRelatedGenresOnHDD.Location = new System.Drawing.Point(279, 132);
            this.chlstRelatedGenresOnHDD.Name = "chlstRelatedGenresOnHDD";
            this.chlstRelatedGenresOnHDD.Size = new System.Drawing.Size(260, 199);
            this.chlstRelatedGenresOnHDD.TabIndex = 40;
            this.chlstRelatedGenresOnHDD.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlstRelatedGenresOnHDD_ItemCheck);
            this.chlstRelatedGenresOnHDD.SelectedIndexChanged += new System.EventHandler(this.chlstRelatedGenresOnHDD_SelectedIndexChanged);
            this.chlstRelatedGenresOnHDD.SelectedValueChanged += new System.EventHandler(this.chlstRelatedGenresOnHDD_SelectedValueChanged);
            this.chlstRelatedGenresOnHDD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chlstRelatedGenresOnHDD_KeyPress);
            // 
            // PickGenre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 549);
            this.Controls.Add(this.chlstRelatedGenresOnHDD);
            this.Controls.Add(this.tbxWriteGenre);
            this.Controls.Add(this.tbxSelectedGenre);
            this.Controls.Add(this.chlstSuggestedGenres);
            this.Controls.Add(this.btnDeclareGenre);
            this.Controls.Add(this.chlstGenres);
            this.Name = "PickGenre";
            this.Text = "Pick Genre";
            this.Load += new System.EventHandler(this.PickGenre_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chlstGenres;
        private System.Windows.Forms.Button btnDeclareGenre;
        private System.Windows.Forms.CheckedListBox chlstSuggestedGenres;
        private System.Windows.Forms.TextBox tbxSelectedGenre;
        private System.Windows.Forms.TextBox tbxWriteGenre;
        private System.Windows.Forms.CheckedListBox chlstRelatedGenresOnHDD;
    }
}