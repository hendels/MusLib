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
    public class dataGridColumnsDuplicates
    {

        public int colFirstPath = 0;
        public int colFirstIdAlbum = 1;
        public int colFirstIdTrack = 2;
        public int colFirstRating = 3;

        public int colSecondPath = 4;
        public int colSecondIdAlbum = 5;
        public int colSecondIdTrack = 6;
        public int colSecondRating = 7;

    }
    public partial class duplicatesForm : Form
    {
        public int currentAlbumID;
        public int currentTrackID;
        public string TrackPurgatoryPath;
        public duplicatesForm()
        {
            InitializeComponent();
        }
        class MyStruct
        {
            public string MusicLibraryPath { get; set; }            
            public int firstIdAlbum { get; set; }
            public int firstIDTrack { get; set; }
            public int RatedStarsFirstPath { get; set; }

            public string PurgatoryPath { get; set; }   
            public int secondIdAlbum { get; set; }
            public int secondIDTrack { get; set; }
            public int RatedStarsSecondPath { get; set; }

            public MyStruct(string p1, int IDA1, int IDT1, int rating1, string p2, int IDA2, int IDT2, int rating2)
            {
                MusicLibraryPath = p1;
                RatedStarsFirstPath = rating1;
                firstIdAlbum = IDA1;
                firstIDTrack = IDT1;
                PurgatoryPath = p2;
                RatedStarsSecondPath = rating2;
                secondIdAlbum = IDA2;
                secondIDTrack = IDT2;

            }
        }
        private void duplicates_Load(object sender, EventArgs e)
        {
            
        }

        private void dgvPaths_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            returnTagsFromFiles();
        }

        private void dgvPaths_DoubleClick(object sender, EventArgs e)
        {
            DirectoryManagement.OpenPairOfFolders(dgvPaths, dgvPaths.SelectedCells[0].RowIndex);
        }

        private void btnCheckDuplicates_Click(object sender, EventArgs e)
        {
            dataGridColumnsDuplicates DGCD = new dataGridColumnsDuplicates();
            List<string> uniqueArtists = new List<string>();
            List<string> uniquePartializedTitles = new List<string>();
            List<DuplicatesPaths> LDP = new List<DuplicatesPaths>();

            Functions.createArtistsList(uniqueArtists);
            Functions.checkTrackDuplicates(uniqueArtists, uniquePartializedTitles, LDP);

            var source = new BindingSource();
            List<MyStruct> list = new List<MyStruct>();
            foreach (DuplicatesPaths DP in LDP)
            {
                list.Add(new MyStruct(DP.firstPath, DP.firstIDAlbum, DP.firstIDTrack, DP.ratingStarsFirstFile, DP.secondPath,DP.secondIDAlbum, DP.secondIDTrack, DP.ratingStarsSecondFile));
            }

            source.DataSource = list;
            dgvPaths.DataSource = source;
            dgvPaths.Columns[DGCD.colFirstPath].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvPaths.Columns[DGCD.colSecondPath].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            for (int rows = 0; rows < dgvPaths.Rows.Count; rows++)
            {
                int intVal1 = Convert.ToInt32(dgvPaths.Rows[rows].Cells[DGCD.colFirstRating].Value);
                int intVal2 = Convert.ToInt32(dgvPaths.Rows[rows].Cells[DGCD.colSecondRating].Value);
                if (intVal1 != intVal2 & intVal2 != 0)
                {
                    dgvPaths.Rows[rows].DefaultCellStyle.BackColor = Color.Beige;
                }
                
            }
            

        }

        private void dgvPaths_SelectionChanged(object sender, EventArgs e)
        {
            returnTagsFromFiles();
        }
        private void returnTagsFromFiles()
        {
            try
            {
                dataGridColumnsDuplicates DGCD = new dataGridColumnsDuplicates();
                dgvPaths.Rows[dgvPaths.SelectedCells[0].RowIndex].Selected = true;
                TrackPurgatoryPath = dgvPaths.Rows[dgvPaths.SelectedCells[0].RowIndex].Cells[DGCD.colSecondPath].Value.ToString();
                MusicFileDetails MFD = new MusicFileDetails();
                MusicFileMgt.QuickRead(TrackPurgatoryPath, MFD);
                tbxSelectedAlbumID.Text = MFD.trackIdAlbumIndex;
                tbxSelectedTrackID.Text = MFD.trackIndex;
                int x1 = 0;
                int x2 = 0;
                if (Int32.TryParse(MFD.trackIdAlbumIndex, out x1))
                {
                    currentAlbumID = x1;
                }
                else
                    currentAlbumID = 0;

                if (Int32.TryParse(MFD.trackIndex, out x2))
                {
                    currentTrackID = x2;
                }
                else
                    currentTrackID = 0;



            }
            catch (Exception ex)
            { }
            
        }

        private void btnDeleteTrackDBSQL_Click(object sender, EventArgs e)
        {
            dataGridColumnsDuplicates DGCD = new dataGridColumnsDuplicates();
            foreach(var row in dgvPaths.SelectedRows)
            {
                DirectoryManagement.DeleteTracksFromDuplicates(dgvPaths, lbxConsole.Items, currentTrackID, currentAlbumID, TrackPurgatoryPath);
                dgvPaths.Rows.RemoveAt(dgvPaths.SelectedCells[0].RowIndex);

            }
            
        }
        
    }
}
