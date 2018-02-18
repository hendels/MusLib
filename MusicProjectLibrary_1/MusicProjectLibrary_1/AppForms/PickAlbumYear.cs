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
    public partial class PickAlbumYear : Form
    {
        public bool selectedDiscogs;
        public bool selectedYear;

        public PickAlbumYear()
        {
            InitializeComponent();
        }

        private void cmbSelectYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PickAlbumYear_Load(object sender, EventArgs e)
        {
            selectedDiscogs = false;
            selectedYear = false;
            tbxSelectedAlbum.Text = GlobalVariables.SelectedAlbumFullString;
            cmbSelectYear.Items.Add(1992);
            cmbSelectYear.Items.Add(1993);
            cmbSelectYear.Items.Add(1994);
            cmbSelectYear.Items.Add(1995);
            cmbSelectYear.Items.Add(1996);
            cmbSelectYear.Items.Add(1997);
            cmbSelectYear.Items.Add(1998);
            cmbSelectYear.Items.Add(1999);
            cmbSelectYear.Items.Add(2000);
            cmbSelectYear.Items.Add(2001);
            cmbSelectYear.Items.Add(2002);
            cmbSelectYear.Items.Add(2003);
            cmbSelectYear.Items.Add(2004);
            cmbSelectYear.Items.Add(2005);
            cmbSelectYear.Items.Add(2006);
            cmbSelectYear.Items.Add(2007);
            cmbSelectYear.Items.Add(2008);
            cmbSelectYear.Items.Add(2009);
            cmbSelectYear.Items.Add(2010);
            cmbSelectYear.Items.Add(2011);
            cmbSelectYear.Items.Add(2012);
            cmbSelectYear.Items.Add(2013);
        }

        private void btnDiscogs_Click(object sender, EventArgs e)
        {
            selectedDiscogs = true;
            this.Close();
        }

        private void btnAddYear_Click(object sender, EventArgs e)
        {
            selectedYear = true;
            List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();

            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(GlobalVariables.globalSelectedGridAlbumID);
            MusicFileDetails MFD = new MusicFileDetails();
            foreach (SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
            {
                mgt_HddAnalyzer.QuickRead(itemTrack.TrackDirectory, MFD);

                MFD.pickedAFile.DATE = cmbSelectYear.Text;
                MFD.pickedAFile.Save(true);
                db.UpdateAlbumReleaseYearByAlbumId(GlobalVariables.globalSelectedGridAlbumID, Convert.ToInt32(cmbSelectYear.Text));
                //listBoxConsole.Add($"...year on album {GlobalVariables.globalSelectedGridAlbumID} has been updated by value {cmbSelectYear.Text}.");

            }
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
    }
}
