using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1.AppForms
{
    public partial class PickAlbumGeneralGenre : Form
    {
        public bool selectedDiscogs;
        public PickAlbumGeneralGenre()
        {
            InitializeComponent();
        }

        private void PickAlbumGeneralGenre_Load(object sender, EventArgs e)
        {
            selectedDiscogs = false;
            tbxSelectedAlbum.Text = GlobalVariables.SelectedAlbum;
            btnDiscogs.Focus();
        }

        private void btnDiscogs_Click(object sender, EventArgs e)
        {
            selectedDiscogs = true;
            this.Close();

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

        private void btnVariousGenre_Click(object sender, EventArgs e)
        {
            UpdateGeneralGenre(GlobalVariables.globalSelectedGridAlbumID, "Various Genres");
        }
        private void UpdateGeneralGenre(int AlbumID, string updateGenre)
        {
            List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();

            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(AlbumID);

            MusicFileDetails MFD = new MusicFileDetails();
            foreach (SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
            {
                mgt_HddAnalyzer.QuickRead(itemTrack.TrackDirectory, MFD);
                
                    
                    MFD.pickedAFile.GENRE = updateGenre;
                    MFD.pickedAFile.Save(true);

                    db.UpdateAlbumGenreByAlbumID(AlbumID, updateGenre);
                    db.UpdateTrackGenreByAlbumID(AlbumID, updateGenre);            
                
            }
            this.Close();           
        }

        private void btnWrittenGenre_Click(object sender, EventArgs e)
        {
            UpdateGeneralGenre(GlobalVariables.globalSelectedGridAlbumID, tbxWriteGenre.Text);
            
        }
    }
}
