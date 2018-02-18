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
        public string GeneratedGenreString { get; set; }

        public PickGenre()
        {
            InitializeComponent();
        }

        private void PickGenre_Load(object sender, EventArgs e)
        {
            mgt_PickGenre.createGenreList(chlstGenres);
            mgt_PickGenre.createSecondaryGenreList(GlobalVariables.SelectedArtist , chlstSuggestedGenres);
            if (GlobalVariables.LastUsedGenre != "")
                tbxWriteGenre.Text = GlobalVariables.LastUsedGenre;

            tbxSelectedAlbum.Text = GlobalVariables.SelectedAlbumFullString;
        }
        
        private  void callPickedGenre(CheckedListBox chlstBox)
        {
            mgt_PickGenre.pickedGenre(tbxSelectedGenre, chlstBox);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////[<<all genres check list box]////////////////////////////////////////////
        private void chlstGenres_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            callPickedGenre(chlstGenres);
        }

        private void chlstGenres_SelectedValueChanged(object sender, EventArgs e)
        {
            mgt_PickGenre.findRelatedGenres(chlstGenres.SelectedItem.ToString(), chlstRelatedGenresOnHDD);
        }

        private void chlstGenres_Validated(object sender, EventArgs e)
        {
            callPickedGenre(chlstGenres);
        }

        private void chlstGenres_MouseLeave(object sender, EventArgs e)
        {

        }

        private void chlstGenres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                callPickedGenre(chlstGenres);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////[all genres check list box>>]////////////////////////////////////////////
                
        //////////////////////////////////////////////////////////////////////////////////////////////[<<suggested check list box]////////////////////////////////////////////
        private void chlstSuggestedGenres_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }
        private void chlstSuggestedGenres_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                mgt_PickGenre.findRelatedGenres(chlstSuggestedGenres.SelectedItem.ToString(), chlstRelatedGenresOnHDD);
            }
            catch (Exception ex)
            {

            }
        }
        private void chlstSuggestedGenres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                callPickedGenre(chlstSuggestedGenres);
                //tbxSelectedGenre.Text = chlstSuggestedGenres.SelectedItem.ToString();
                //ListPickedGenres = chlstSuggestedGenres.SelectedItem.ToString();
            }

        }
        //////////////////////////////////////////////////////////////////////////////////////////////[suggested check list box>>]////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////[<<related genres on HDD check list box]////////////////////////////////////////////
        private void chlstRelatedGenresOnHDD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                tbxSelectedGenre.Text = chlstRelatedGenresOnHDD.SelectedItem.ToString();
                ListPickedGenres = chlstRelatedGenresOnHDD.SelectedItem.ToString();
            }
        }

        private void chlstRelatedGenresOnHDD_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int currentIndex = chlstRelatedGenresOnHDD.SelectedIndex;
            for (int i = 0; i < chlstRelatedGenresOnHDD.Items.Count; i++)
            {
                bool state = false;
                //if (chlstRelatedGenresOnHDD.GetItemChecked(i) )
                //{
                    //chlstRelatedGenresOnHDD.SetItemCheckState
                    //chlstRelatedGenresOnHDD.SetItemChecked(i, (state ? CheckState.Checked : CheckState.Unchecked));
                //}
            }


            tbxSelectedGenre.Text = chlstRelatedGenresOnHDD.SelectedItem.ToString();
            ListPickedGenres = chlstRelatedGenresOnHDD.SelectedItem.ToString();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////[related genres on HDD check list box>>]////////////////////////////////////////////
        private void tbxWriteGenre_TextChanged(object sender, EventArgs e)
        {
            tbxSelectedGenre.Text = tbxWriteGenre.Text;
        }
        private void btnDeclareGenre_Click(object sender, EventArgs e)
        {

            //if (!TextChanged)
                ListPickedGenres = tbxSelectedGenre.Text;
            //else
                //ListPickedGenres = tbxWriteGenre.Text;

            GeneratedGenreString = ListPickedGenres;

            mgt_SQLDatabase db = new mgt_SQLDatabase();
            db.UpdateDirectoryGenreByAlbumID(GlobalVariables.globalSelectedGridAlbumID, ListPickedGenres);
            GlobalVariables.LastUsedGenre = ListPickedGenres;
            this.Close();

        }

        private void tbxWriteGenre_Click(object sender, EventArgs e)
        {
            tbxSelectedGenre.Text = tbxWriteGenre.Text;
            TextChanged = true;
        }

        private void PickGenre_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("ss");
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
