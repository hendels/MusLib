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

namespace MusicProjectLibrary_1
{
    public partial class MusicLibraryWindow : Form //MaterialSkin.Controls.MaterialForm
    {
        List<SQLAlbumTable> AlbumList = new List<SQLAlbumTable>(); //sqlprzemy - table : Albums
        List<SQLTrackTable> TrackList = new List<SQLTrackTable>(); //sqlprzemy - table : Tracks
        public int AlbumRowIndex = 0;
        public MusicLibraryWindow()
        {

            InitializeComponent();     
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Functions.getPurgatoryPath(tbxPickedPath,true);
            Functions.getMainDirectoryPath(tbxMusicPath, true);
            Functions.getDriveGeneralPath(tbxDriveMainPath, true);

            int countRecordTracks = RefreshSpecificTable(1);
            BoxListConsole.Items.Add("Tracks count: " + countRecordTracks.ToString());
            SQLDataValidate.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);

            int countRecordAlbums = RefreshSpecificTable(2);
            BoxListConsole.Items.Add("Album count: " + countRecordAlbums.ToString());

            int countRecordArtist = RefreshSpecificTable(3);
            BoxListConsole.Items.Add("Artist table updated: " + countRecordArtist.ToString());

            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
            GlobalVariables globalProcCatalog = new GlobalVariables();
            //
            //load config file
            //
            if (File.Exists("config.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(xmlSaveInformation));
                using (FileStream read = new FileStream("config.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    xmlSaveInformation info = (xmlSaveInformation)xs.Deserialize(read);
                    tbxPointsMin.Text = info.PointsMin;
                    tbxPointsMax.Text = info.PointsMax;
                    tbxTrackRatingMin.Text = info.RateMin;
                    tbxTrackRatingMin.Text = info.RateMax;
                    chbProceed.Checked = info.ShowProceed; 
                    tbxAlbumCount.Text = info.AlbumCount.ToString();
                    tbxTrackCount.Text = info.TrackCount.ToString();
                }
                
            }
        }
        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            try
            {
                xmlSaveInformation SaveInfo = new xmlSaveInformation();
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

        private void ButtonReadTag_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalChecker.TestSqlAlbumIdQuery = 0;
            MusicFileMgt.readFiles(BoxListConsole.Items, progBar, tbxPickedPath.Text, tbxDriveMainPath.Text, lblProgress);

            int countRecordAlbum = DBFunctions.AutoSearchDatabaseAlbums(0, dgvAlbums, 1, 0, 0, 0, chbProceed.Checked);
            int countRecordArtist = DBFunctions.AutoSearchDatabaseArtists("", dgvArtists);
            BoxListConsole.Items.Add("Album table updated: " + countRecordAlbum.ToString());
            BoxListConsole.Items.Add("Artist table updated: " + countRecordArtist.ToString());
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;

            BoxListConsole.Items.Add("Total SQL query [album id] = " + GlobalChecker.TestSqlAlbumIdQuery.ToString());
            watch.Stop();
            double elapsedMs = watch.ElapsedMilliseconds;
            BoxListConsole.Items.Add("..........processing done in time: " + Math.Round(elapsedMs / 1000, 2) + "s.");
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
        }        

        private void CheckBoxProcessCatalogs_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxProcessCatalogs.Checked == true)
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
        private void button1_Click(object sender, EventArgs e)
        {          
        }        

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                //musicLibraryDataSet.Track.AddTrackRow(musicLibraryDataSet.Track.NewTrackRow());
                musicLibraryDataSetBindingSource.MoveLast();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //musicLibraryDataSet.RejectChanges();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                Validate();
                musicLibraryDataSetBindingSource.EndEdit();

                MessageBox.Show("updated");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            int countRecord = RefreshSpecificTable(1);
            BoxListConsole.Items.Add("Album count: " + countRecord.ToString());
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;
            SQLDataValidate.ReadDataGridForAll(dgvAlbums, BoxListConsole.Items);
        }         
    
        private void btnFindTracks_Click(object sender, EventArgs e)
        {
            int countRecord = RefreshSpecificTable(2);
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
                SQLDataValidate.dataGridColumns DGC = new SQLDataValidate.dataGridColumns();
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
            PickGenre pickGenreForm = new PickGenre();
            DirectoryManagement.DoubleClickOnGridCallBack(dgvAlbums, pickGenreForm, BoxListConsole, AlbumRowIndex);
            SQLDataValidate.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
        }
        private void AlbumsDataGridView_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dgvAlbums.SelectedCells.Count > 0)
            {
                AlbumRowIndex = dgvAlbums.SelectedCells[0].RowIndex; //[knowledge get row index from data grid view]
                GlobalVariables.globalSelectedGridAlbumID = (int)dgvAlbums[0, AlbumRowIndex].Value; //[knowledge get value from specific column in datagrid view]                
            }
        }
        private void AlbumsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PickGenre pickGenreForm = new PickGenre();
            int masterRow = AlbumRowIndex;
            if (DirectoryManagement.DoubleClickOnGridCallBack(dgvAlbums, pickGenreForm, BoxListConsole, AlbumRowIndex))
            {
                RefreshSpecificTable(1);
                SQLDataValidate.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
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
        private void AlbumsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void AlbumsDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (DirectoryManagement.SingleClickOnGridCallBack(dgvAlbums, AlbumRowIndex))
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
                DirectoryManagement.DeleteAlbumFromAlbumDGV(dgvAlbums, AlbumRowIndex);
                MessageBox.Show("Album & connected Tracks deleted");
            }
            private void btnDeclareGenre_Click_1(object sender, EventArgs e)
            {
                PickGenre pickGenreForm = new PickGenre();
                pickGenreForm.ShowDialog();

                int countRecord = RefreshSpecificTable(1);
                dgvAlbums.Rows[AlbumRowIndex].Selected = true;
                //
            }
        private void btnProcessSelected_Click(object sender, EventArgs e)
        {

            //validuj SQL'a
            AlbumRowIndex = dgvAlbums.SelectedCells[0].RowIndex;
            int hardIndex = AlbumRowIndex;
            SQLDataValidate.ReadDataGrid(dgvAlbums, BoxListConsole.Items, tbxPickedPath, tbxMusicPath, AlbumRowIndex);

            int countRecord = DBFunctions.AutoSearchDatabaseAlbums(0, dgvAlbums, 0, 0, 0, 0, chbProceed.Checked);
            BoxListConsole.Items.Add("Album table updated: " + countRecord.ToString());
            BoxListConsole.SelectedIndex = BoxListConsole.Items.Count - 1;

            dgvAlbums.ClearSelection();                                                //[przemy knowledge - zaznaczanie data grid view]
            try
            {
                dgvAlbums.CurrentCell = dgvAlbums.Rows[hardIndex].Cells[0]; //[przemy knowledge - zaznaczanie data grid view]
                dgvAlbums.Rows[hardIndex].Selected = true;                         //[przemy knowledge - zaznaczanie data grid view]
            }
            catch (Exception ex)
            { }
            

            SQLDataValidate.bw_ReadDataGridForAll(sender, dgvAlbums, BoxListConsole.Items);
            // aktualizuj tabele tracks - ze usunieto plik / aktualizuj, również że istnieje plik
            //rzuć info ile wyjebał MB z dysku

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

            //string SearchArtist = "";
            //string SearchRelease = "";
            //DiscogsManagement.startDiscogs(BoxListConsole.Items, SearchArtist, SearchRelease, 0);
            //this.Show();
        }       
        private void btnSelectHealthy_Click(object sender, EventArgs e)
        {
            BoxListConsole.Items.Add("Start checking...");
            SQLDataValidate.ReadDataGridForAll(dgvAlbums, BoxListConsole.Items);
        }
        ////////////////////////////////////////////////////////INTERNAL FUNCTIONS////////////////////////////////////////
        private int RefreshSpecificTable(int RefreshTableNo)
        {
            int counter = 0;
            int subcounter = 0;
            int subcounter2 = 0;
            int x = 0;
            switch (RefreshTableNo)
            {                
                case 1:                    
                    if (Int32.TryParse(tbxSearchAlbums.Text, out x))
                    {

                        subcounter = DBFunctions.AutoSearchDatabaseAlbums(x, dgvAlbums, 1, 0, 0, 0, chbProceed.Checked);
                        subcounter2 = DBFunctions.AutoSearchDatabaseTracks(x, dgvTracks, 0, 0 ,0);
                        counter = subcounter + subcounter;
                    }                        
                    else
                    {
                        int AlbumCount = 0;
                        if (tbxAlbumCount.Text != "")
                            AlbumCount = Convert.ToInt32(tbxAlbumCount.Text);

                         counter = DBFunctions.AutoSearchDatabaseAlbums(0, dgvAlbums, 0, AlbumCount, Convert.ToInt32(tbxPointsMin.Text), Convert.ToInt32(tbxPointsMax.Text), chbProceed.Checked);
                    }
                        
                    return counter;
                case 2:                    
                    if (Int32.TryParse(tbxSearchTracks.Text, out x))
                        counter = DBFunctions.AutoSearchDatabaseTracks(x, dgvTracks, 0, 0 , 0);
                    else
                        counter = DBFunctions.AutoSearchDatabaseTracks(0, dgvTracks, Convert.ToInt32(tbxTrackCount.Text), Convert.ToInt32(tbxTrackRatingMin.Text), Convert.ToInt32(tbxTrackRatingMax.Text));
                    return counter;
                case 3:                    
                        counter = DBFunctions.AutoSearchDatabaseArtists(tbxSearchArtist.Text, dgvArtists);
                    return counter;
            }
            return counter;
        }

        private void btnWriteIndexAlbum_Click(object sender, EventArgs e)
        {
            MusicFileMgt.writeAlbumIndexToFile(dgvAlbums, progBar);
        }

        private void chbCheckGeneralPath_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCheckGeneralPath.Checked == true)
                GlobalVariables.checkGeneralPath = true;
            else
                GlobalVariables.checkGeneralPath = false;
        }

        
    }
}
