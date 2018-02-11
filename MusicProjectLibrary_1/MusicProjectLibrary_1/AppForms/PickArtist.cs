using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MusicProjectLibrary_1.mgt_SQLValidation;

namespace MusicProjectLibrary_1.AppForms
{
    
    public partial class PickArtist : Form
    {
        public bool ArtistSelected = false;
        public PickArtist()
        {
            InitializeComponent();
        }

        private void PickArtist_Load(object sender, EventArgs e)
        {
            tbxSelectedAlbum.Text = GlobalVariables.SelectedAlbum;
            mgt_SQLDatabase.AutoSearchDatabaseArtists("", dgvArtists);
        }
        private void AddArtistToFile(int ArtistId, string ArtistName)
        {
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            db.UpdateAlbumArtistID(GlobalVariables.globalSelectedGridAlbumID, ArtistId);
            db.UpdateAlbumArtistNameByAlbumId(GlobalVariables.globalSelectedGridAlbumID, ArtistName);
            db.UpdateTrackArtistNameByAlbumID(GlobalVariables.globalSelectedGridAlbumID, ArtistName);

            List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();

            queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(GlobalVariables.globalSelectedGridAlbumID);

            MusicFileDetails MFD = new MusicFileDetails();
            foreach (SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
            {
                mgt_HddAnalyzer.QuickRead(itemTrack.TrackDirectory, MFD);

                MFD.pickedAFile.ARTIST = ArtistName;
                MFD.pickedAFile.Save(true);
            }

            ArtistSelected = true;
            this.Close();
        }
        private void dgvArtists_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ArtistDataGridColumns ArtistDGC = new ArtistDataGridColumns();
            int AlbumRowIndex = dgvArtists.SelectedCells[0].RowIndex;
            int ArtistId = Convert.ToInt32(dgvArtists.Rows[AlbumRowIndex].Cells[ArtistDGC.colIdArtist].Value);
            string ArtistName = dgvArtists.Rows[AlbumRowIndex].Cells[ArtistDGC.colArtistName].Value.ToString();

            AddArtistToFile(ArtistId, ArtistName);

        }
        

        private void btnVariousArtists_Click(object sender, EventArgs e)
        {
            AddArtistToFile(69, "Various Artists");
        }
        

        private void btnTextbox_Click(object sender, EventArgs e)
        {
            if (tbxWrittenArtist.Text != "")
            {
                mgt_SQLDatabase db = new mgt_SQLDatabase();

                db.InsertArtist(tbxWrittenArtist.Text, "0/0", "0/0", 0, 0, 0, 0);
                AddArtistToFile(mgt_Artists.getArtistIdByName(tbxWrittenArtist.Text), tbxWrittenArtist.Text);                
            }            
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
