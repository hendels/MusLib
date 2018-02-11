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
    public partial class PickAlbumName : Form
    {
        public bool ArtistNameFilled = false;
        public PickAlbumName()
        {
            InitializeComponent();
        }

        private void PickAlbumName_Load(object sender, EventArgs e)
        {
            tbxSelectedAlbum.Text = GlobalVariables.SelectedAlbum;
        }

        private void btnAddName_Click(object sender, EventArgs e)
        {
            if (tbxWriteName.Text != "")
            {
                mgt_SQLDatabase db = new mgt_SQLDatabase();
                db.UpdateAlbumNameByAlbumId(GlobalVariables.globalSelectedGridAlbumID, tbxWriteName.Text);

                List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();

                queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(GlobalVariables.globalSelectedGridAlbumID);

                MusicFileDetails MFD = new MusicFileDetails();
                foreach (SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
                {
                    mgt_HddAnalyzer.QuickRead(itemTrack.TrackDirectory, MFD);

                    MFD.pickedAFile.ALBUM = tbxWriteName.Text;
                    MFD.pickedAFile.Save(true);
                    ArtistNameFilled = true;
                    this.Close();
                }
            }
            else
                MessageBox.Show("album name is empty");
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
