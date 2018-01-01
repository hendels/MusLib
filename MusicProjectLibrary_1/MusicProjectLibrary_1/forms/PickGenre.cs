using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    public partial class PickGenre : Form
    {
        public bool TextChanged;
        public PickGenre()
        {
            InitializeComponent();
        }

        private void PickGenre_Load(object sender, EventArgs e)
        {
            Functions.createGenreList(chlstGenres);            
        }

        

        private void chlstGenres_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Functions.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_SelectedValueChanged(object sender, EventArgs e)
        {
            Functions.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_Validated(object sender, EventArgs e)
        {
            Functions.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_MouseLeave(object sender, EventArgs e)
        {
            Functions.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                Functions.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void btnDeclareGenre_Click(object sender, EventArgs e)
        {
            string ListPickedGenres = "";
            if (!TextChanged)            
                ListPickedGenres = Functions.pickedGenre(tbxSelectedGenre, chlstGenres);            
            else            
                ListPickedGenres = tbxWriteGenre.Text;            
                
            DBFunctions db = new DBFunctions();
            db.UpdateDirectoryGenreByAlbumID(GlobalVariables.globalSelectedGridAlbumID, ListPickedGenres);
            this.Close();
            
        }

        private void tbxWriteGenre_TextChanged(object sender, EventArgs e)
        {
            TextChanged = true;
        }
    }
}
