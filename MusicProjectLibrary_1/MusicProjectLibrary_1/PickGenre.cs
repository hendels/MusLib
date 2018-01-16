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
        public string ListPickedGenres = "";
        public PickGenre()
        {
            InitializeComponent();
        }

        private void PickGenre_Load(object sender, EventArgs e)
        {
            PickGenreMgt.createGenreList(chlstGenres);
            PickGenreMgt.createSecondaryGenreList(GlobalVariables.SelectedArtist , chlstSuggestedGenres);
        }

        

        private void chlstGenres_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            PickGenreMgt.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_SelectedValueChanged(object sender, EventArgs e)
        {
            //PickGenreMgt.pickedGenre(tbxSelectedGenre, chlstGenres);
            PickGenreMgt.findRelatedGenres(chlstGenres.SelectedItem.ToString(), chlstRelatedGenresOnHDD);
        }

        private void chlstGenres_Validated(object sender, EventArgs e)
        {
            PickGenreMgt.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_MouseLeave(object sender, EventArgs e)
        {
            PickGenreMgt.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void chlstGenres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                PickGenreMgt.pickedGenre(tbxSelectedGenre, chlstGenres);
        }

        private void btnDeclareGenre_Click(object sender, EventArgs e)
        {
            
            if (!TextChanged)            
                ListPickedGenres = PickGenreMgt.pickedGenre(tbxSelectedGenre, chlstGenres);            
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

        private void chlstRelatedGenresOnHDD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chlstSuggestedGenres_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }

        private void chlstSuggestedGenres_KeyPress(object sender, KeyPressEventArgs e)
        {
            
                
        }

        private void chlstRelatedGenresOnHDD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                tbxSelectedGenre.Text = chlstRelatedGenresOnHDD.SelectedItem.ToString();
                ListPickedGenres = chlstRelatedGenresOnHDD.SelectedItem.ToString();
            }
        }

        private void chlstRelatedGenresOnHDD_SelectedValueChanged(object sender, EventArgs e)
        {
            //tbxSelectedGenre.Text = chlstRelatedGenresOnHDD.SelectedItem.ToString();
            //ListPickedGenres = chlstRelatedGenresOnHDD.SelectedItem.ToString();
        }

        private void chlstRelatedGenresOnHDD_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            tbxSelectedGenre.Text = chlstRelatedGenresOnHDD.SelectedItem.ToString();
            ListPickedGenres = chlstRelatedGenresOnHDD.SelectedItem.ToString();
        }
    }
}
