using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

///
using System.IO;
using MusicProjectLibrary_1;
using MaterialSkin;
using Dapper;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static MusicProjectLibrary_1.mgt_SQLValidation;

namespace MusicProjectLibrary_1
{
    
    public partial class MusicLibraryWindow : Form //MaterialSkin.Controls.MaterialForm
    {
        public class AppSearchDefinitions
        {
            public SearchAlbumParameters SAP;

            public AppSearchDefinitions()
            {
                SAP = defineSearchAlbumsParameters();
            }
        }
        List<SQLAlbumTable> AlbumList = new List<SQLAlbumTable>(); //sqlprzemy - table : Albums
        List<SQLTrackTable> TrackList = new List<SQLTrackTable>(); //sqlprzemy - table : Tracks
        mgt_XML info = new mgt_XML();
        dataGridColumns publicDGC = new dataGridColumns();

        public int AlbumRowIndex = 0;
        public MusicLibraryWindow()
        {

            InitializeComponent();     
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //
            //load config file
            //
            if (File.Exists("config.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(mgt_XML));
                using (FileStream read = new FileStream("config.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        info = (mgt_XML)xs.Deserialize(read);
                        tbxPointsMin.Text = info.PointsMin;
                        tbxPointsMax.Text = info.PointsMax;
                        tbxTrackRatingMin.Text = info.RateMin;
                        tbxTrackRatingMin.Text = info.RateMax;
                        chbProceed.Checked = info.ShowProceed;
                        tbxAlbumCount.Text = info.AlbumCount.ToString();
                        tbxTrackCount.Text = info.TrackCount.ToString();
                        //checkboxes
                        GlobalVariables.FullyRated = false;
                        GlobalVariables.ShowProcessed = info.ShowProceed;
                        GlobalVariables.showAll = false;
                        //
                        //textboxes
                        GlobalVariables.SearchAlbumString = "";
                        GlobalVariables.showAlbumLimiter = info.AlbumCount;
                        //
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error while loading XML");
                    }
                }

            }
            Functions.getPurgatoryPath(tbxPickedPath,true);
            Functions.getMainDirectoryPath(tbxMusicPath, true);
            Functions.getDriveGeneralPath(tbxDriveMainPath, true);

            int countRecordTracks = mgt_SearchAlbums.RefreshSpecificTable(2, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            BoxListConsole.Items.Add("Tracks count: " + countRecordTracks.ToString());
            mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);

            int countRecordAlbums = mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            BoxListConsole.Items.Add("Album count: " + countRecordAlbums.ToString());

            int countRecordArtist = mgt_SearchAlbums.RefreshSpecificTable(3, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            BoxListConsole.Items.Add("Artist table updated: " + countRecordArtist.ToString());

            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
            GlobalVariables globalProcCatalog = new GlobalVariables();
            
            //
            //refresh
            //
            //RefreshSpecificTable(1);
            mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
        }

        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            try
            {
                mgt_XML SaveInfo = new mgt_XML();
                SaveInfo.PointsMin = tbxPointsMin.Text;
                SaveInfo.PointsMax = tbxPointsMax.Text;
                SaveInfo.RateMin = tbxTrackRatingMin.Text;
                SaveInfo.RateMax = tbxTrackRatingMin.Text;
                SaveInfo.ShowProceed = chbProceed.Checked;
                SaveInfo.AlbumCount = Convert.ToInt32(tbxAlbumCount.Text);
                SaveInfo.TrackCount = Convert.ToInt32(tbxTrackCount.Text);

                Functions.xmlSave(SaveInfo, "config.xml");
                MessageBox.Show("saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void checkFilesInDirectory_Click(object sender, EventArgs e)
        {
            Functions.pickPath(1, tbxPickedPath);
            //Functions.showFolderBrowserDialog(ListBoxItems.Items);
        }
        private void btnChangeGeneralCatalogPath_Click(object sender, EventArgs e)
        {
            Functions.pickPath(2, tbxMusicPath);
        }
        private void btnReadSelected_Click(object sender, EventArgs e)
        {
            string pickedPath = dgvAlbums.Rows[AlbumRowIndex].Cells[publicDGC.colAlbumDirectory].Value.ToString();
            var confirmResult = MessageBox.Show($"Read tags for \n {pickedPath} ?",
                    "Music Library", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                mgt_HddAnalyzer.readFiles(BoxListConsole.Items, progBar, pickedPath, "blah", lblProgress);
                int countRecordAlbums = mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());

                BoxListConsole.Items.Add("..........processing path done.");
                BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
                mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
            }
            

        }
        private void btnReadTag_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalChecker.TestSqlAlbumIdQuery = 0;
            mgt_HddAnalyzer.readFiles(BoxListConsole.Items, progBar, tbxPickedPath.Text, tbxDriveMainPath.Text, lblProgress);

            int countRecordAlbums = mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            int countRecordArtist = mgt_SearchAlbums.RefreshSpecificTable(3, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());

            BoxListConsole.Items.Add("Album table updated: " + countRecordAlbums.ToString());
            BoxListConsole.Items.Add("Artist table updated: " + countRecordArtist.ToString());
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;

            BoxListConsole.Items.Add("Total SQL query [album id] = " + GlobalChecker.TestSqlAlbumIdQuery.ToString());
            watch.Stop();
            double elapsedMs = watch.ElapsedMilliseconds;
            BoxListConsole.Items.Add("..........processing done in time: " + Math.Round(elapsedMs / 1000, 2) + "s.");
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
            mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
        }        

        private void CheckBoxProcessCatalogs_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxProcessCatalogs.Checked)
                GlobalVariables.globalProcessCatalog = true;            
            else            
                GlobalVariables.globalProcessCatalog = false;
            
        }
        private void CheckBoxModifyFIles_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxProcessCatalogs.Checked == true)
                GlobalVariables.globalModifyFIles = true;
            else
                GlobalVariables.globalModifyFIles = false;
        }
                 
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

            }
        }
        private void btnFindAlbums_Click(object sender, EventArgs e)
        {
            mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
        }         
    
        private void btnFindTracks_Click(object sender, EventArgs e)
        {
            int countRecord = mgt_SearchAlbums.RefreshSpecificTable(2, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            BoxListConsole.Items.Add("Tracks count: " + countRecord.ToString());
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;            
        }
        ////////////////////////////////////////////////////////[SETUP]////////////////////////////////////////
        private void CheckBoxWriteIndexes_CheckedChanged(object sender, EventArgs e)
        {
            if (chbWriteTrackIndex.Checked == true)
                GlobalVariables.globalwriteIndexes = true;
            else
                GlobalVariables.globalwriteIndexes = false;
        }
        private void chbWriteAlbumIndex_CheckedChanged(object sender, EventArgs e)
        {
            if (chbWriteValidationPoints.Checked == true)
                GlobalVariables.writeValidationPoints = true;
            else
                GlobalVariables.writeValidationPoints = false;
        }
        ////////////////////////////////////////////////////////DGV[1]////////////////////////////////////////
        private void AlbumsDataGridView_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //AlbumsDataGridView.Sort(this.AlbumsDataGridView.Columns["AlbumDirectory"], ListSortDirection.Ascending); //[przemy knowledge - sorting datagridview]
            foreach (DataGridViewRow r in dgvAlbums.Rows)
            {
                r.Cells["AlbumDirectory"] = new DataGridViewLinkCell();
            }
            foreach (DataGridViewColumn c in dgvAlbums.Columns)
            {
                mgt_SQLValidation.dataGridColumns DGC = new mgt_SQLValidation.dataGridColumns();
                if (c.Index == DGC.colAlbumDirectory || c.Index == DGC.colDirectoryGenre || c.Index == DGC.colAlbumGeneralGenre
                    || c.Index == DGC.colArtistCheck || c.Index == DGC.colAlbumCheck || c.Index == DGC.colGenreCheck || c.Index == DGC.colRatingCheck || c.Index == DGC.colIndexCheck)
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            }
        }        
        private void AlbumsDataGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                dgvAlbums.Rows[dgvAlbums.SelectedCells[0].RowIndex].Selected = true;
                AlbumRowIndex = dgvAlbums.SelectedCells[0].RowIndex;
            }
            catch (Exception ex)
            {

            }
        }
        private void AlbumsDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            mgt_DGV_Albums.DoubleClickOnGridCallBack(dgvAlbums, BoxListConsole, AlbumRowIndex, dgvAlbums.CurrentCell.ColumnIndex, new AppSearchDefinitions());
            mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
        }
        private void AlbumsDataGridView_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dgvAlbums.SelectedCells.Count > 0)
            {
                AlbumRowIndex = dgvAlbums.SelectedCells[0].RowIndex; //[knowledge get row index from data grid view]
                GlobalVariables.globalSelectedGridAlbumID = (int)dgvAlbums[0, AlbumRowIndex].Value; //[knowledge get value from specific column in datagrid view]  
                GlobalVariables.SelectedAlbum = "[" + GlobalVariables.globalSelectedGridAlbumID + "] - " + dgvAlbums.Rows[AlbumRowIndex].Cells[publicDGC.colAlbumName].Value
                    + " | " + dgvAlbums.Rows[AlbumRowIndex].Cells[publicDGC.colAlbumDirectory].Value;
            }
        }
        private void DgvAlbumsCellDoubleClick(object sender, int columnIndex)
        {
            //PickGenre pickGenreForm = new PickGenre();
            int masterRow = AlbumRowIndex;
            if (mgt_DGV_Albums.DoubleClickOnGridCallBack(dgvAlbums, BoxListConsole, AlbumRowIndex, columnIndex, new AppSearchDefinitions()))
            {
                mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
                mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
                try
                {
                    dgvAlbums.ClearSelection();                               //[przemy knowledge - zaznaczanie data grid view]
                    dgvAlbums.CurrentCell = dgvAlbums.Rows[masterRow].Cells[0]; //[przemy knowledge - zaznaczanie data grid view]
                    dgvAlbums.Rows[masterRow].Selected = true;            //[przemy knowledge - zaznaczanie data grid view]
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void AlbumsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DgvAlbumsCellDoubleClick(sender, dgvAlbums.CurrentCell.ColumnIndex);
        }

        private void AlbumsDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (mgt_DGV_Albums.SingleClickOnGridCallBack(dgvAlbums, AlbumRowIndex))
            {

            }
        }
        ////////////////////////////////////////////////////////DGV[1][AboveButtons]////////////////////////////////////////
        private void btnCheckTrackDuplicates_Click(object sender, EventArgs e)
            {

                duplicatesForm duplicatesForm = new duplicatesForm();
                PassConsole lstbxConsole = new PassConsole();
                lstbxConsole.listboxConsole = BoxListConsole;
                duplicatesForm.ShowDialog();

            }
            private void btnDeleteAlbum_Click(object sender, EventArgs e)
            {
                mgt_Directory.DeleteAlbumFromAlbumDGV(dgvAlbums, AlbumRowIndex);
                mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
                mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
                MessageBox.Show("Album & connected Tracks deleted");
            }
            private void btnDeclareGenre_Click_1(object sender, EventArgs e)
            {
                PickGenre pickGenreForm = new PickGenre();
                pickGenreForm.ShowDialog();

                int countRecord = mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            dgvAlbums.Rows[AlbumRowIndex].Selected = true;
                //
            }
        private void btnProcessSelected_Click(object sender, EventArgs e)
        {

            //validuj SQL'a
            AlbumRowIndex = dgvAlbums.SelectedCells[0].RowIndex;
            int hardIndex = AlbumRowIndex;
            //if (hardIndex != 0)
            //{
                mgt_SQLValidation.ReadDataGrid(dgvAlbums, BoxListConsole.Items, tbxPickedPath, tbxMusicPath, AlbumRowIndex);

            int countRecordAlbums = mgt_SearchAlbums.RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, defineSearchAlbumsParameters(), defineSearchTracksParameters(), defineSearchArtistsParameters());
            BoxListConsole.Items.Add("Album table updated: " + countRecordAlbums.ToString());
                BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;

                dgvAlbums.ClearSelection();                                                //[przemy knowledge - zaznaczanie data grid view]
                try
                {
                    dgvAlbums.CurrentCell = dgvAlbums.Rows[hardIndex].Cells[0]; //[przemy knowledge - zaznaczanie data grid view]
                    dgvAlbums.Rows[hardIndex].Selected = true;                         //[przemy knowledge - zaznaczanie data grid view]
                }
                catch (Exception ex)
                { }

                mgt_SQLValidation.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
                //rzuć info ile wyjebał MB z dysku
            //}
            //else
            //    BoxListConsole.Items.Add("Cancelled. Select album index!");
        }
        ////////////////////////////////////////////////////////DGV[2]////////////////////////////////////////
        private void TracksDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        ////////////////////////////////////////////////////////DGV[3]////////////////////////////////////////

        
        
        private void checkBoxCreateBackUp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCreateBackUp.Checked == true)
                GlobalVariables.globalCreateBackup = true;
            else
                GlobalVariables.globalCreateBackup = false;

        }
        private void btnDiscogs_Click(object sender, EventArgs e)
        {

        }       
        private void btnSelectHealthy_Click(object sender, EventArgs e)
        {
            BoxListConsole.Items.Add("Start checking...");
            mgt_SQLValidation.ReadDataGridForAll(dgvAlbums, BoxListConsole.Items);
        }
        ////////////////////////////////////////////////////////<<INTERNAL FUNCTIONS////////////////////////////////////////
        private static SearchAlbumParameters defineSearchAlbumsParameters()
        {            
            SearchAlbumParameters SAP = new SearchAlbumParameters();
            SAP.fullRated = GlobalVariables.FullyRated;
            SAP.processedAlbums = GlobalVariables.ShowProcessed;
            SAP.searchAlbumsString = GlobalVariables.SearchAlbumString;
            SAP.showAlbumLimiter = GlobalVariables.showAlbumLimiter;
            SAP.showAll = GlobalVariables.showAll;

            return SAP;
        }
        private SearchTrackParameters defineSearchTracksParameters()
        {
            SearchTrackParameters STP = new SearchTrackParameters();
            STP.maxTrackRating = Convert.ToInt32(tbxTrackRatingMax.Text);
            STP.minTrackRating = Convert.ToInt32(tbxTrackRatingMin.Text);
            STP.searchTracksString = tbxSearchTracks.Text;
            STP.showTracksLimiter = Convert.ToInt32(tbxTrackCount.Text);
            
            return STP;
        }
        private SearchArtistParameters defineSearchArtistsParameters()
        {
            SearchArtistParameters SARP = new SearchArtistParameters();
            SARP.searchArtistsString = tbxSearchArtist.Text;
            return SARP;
        }
        ////////////////////////////////////////////////////////INTERNAL FUNCTIONS>>////////////////////////////////////////
        private void btnWriteIndexAlbum_Click(object sender, EventArgs e)
        {
            mgt_HddAnalyzer.writeAlbumIndexToFile(dgvAlbums, progBar);
        }

        private void chbCheckGeneralPath_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCheckGeneralPath.Checked == true)
                GlobalVariables.checkGeneralPath = true;
            else
                GlobalVariables.checkGeneralPath = false;
        }

        private void dgvAlbums_KeyDown(object sender, KeyEventArgs e)
        {
            dataGridColumns DGC = new dataGridColumns();
            if (e.KeyData == (Keys.Control | Keys.D5))
            {                
                DgvAlbumsCellDoubleClick(sender, DGC.colDirectoryGenre);
            }
            if (e.KeyData == (Keys.Control | Keys.D2))
            {
                DgvAlbumsCellDoubleClick(sender, DGC.colArtistName);
            }
            if (e.KeyData == (Keys.Control | Keys.D3))
            {
                DgvAlbumsCellDoubleClick(sender, DGC.colAlbumName);
            }
            if (e.KeyData == (Keys.Control | Keys.D1))
            {
                DgvAlbumsCellDoubleClick(sender, DGC.colAbumReleaseYear);
            }
            if (e.KeyData == (Keys.Control | Keys.D4))
            {
                DgvAlbumsCellDoubleClick(sender, DGC.colAlbumGeneralGenre);
            }
            if (e.KeyData == (Keys.Control | Keys.Q))
            {
                btnProcessSelected_Click(sender, e);
            }
            
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                var confirmResult = MessageBox.Show($"Exit app ?",
                    "Music Library", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    this.Close();
                    return true;
                }
                    
            }
            return base.ProcessDialogKey(keyData);
        }

        private void tbxSearchAlbums_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //enter key is down
            }
        }

        private void chb_ShowExceptRating_CheckedChanged(object sender, EventArgs e)
        {
            
            if (chbShowExceptRating.Checked == true)
                GlobalVariables.FullyRated = true;
            else
                GlobalVariables.FullyRated = false;
        }

        private void chbProceed_CheckedChanged(object sender, EventArgs e)
        {
            if (chbProceed.Checked == true)
                GlobalVariables.ShowProcessed = true;
            else
                GlobalVariables.ShowProcessed = false;
        }

        private void tbxSearchAlbums_Validated(object sender, EventArgs e)
        {
            GlobalVariables.SearchAlbumString = tbxSearchAlbums.Text;
        }

        private void tbxAlbumCount_Validated(object sender, EventArgs e)
        {
            GlobalVariables.showAlbumLimiter = Convert.ToInt32(tbxAlbumCount.Text);
        }

        private void chbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chbShowAll.Checked == true)
                GlobalVariables.showAll = true;
            else
                GlobalVariables.showAll = false;
        }
    }
}
