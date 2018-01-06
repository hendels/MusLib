using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class DirectoryManagement
    {
        public static bool DoubleClickOnGridCallBack(DataGridView DGV, PickGenre pickGenreForm, ListBox boxListConsole, int AlbumRowIndex)
        {
            DGV.MultiSelect = false;
            SQLDataValidate.dataGridColumns DGC = new SQLDataValidate.dataGridColumns();
     
            int AlbumColIndex = DGV.CurrentCell.ColumnIndex; // [przemy knowledge - select column in data grid view]
            string GridValueString = "";

                
            if (AlbumColIndex == DGC.colAlbumDirectory)
            {
                GridValueString = DGV.Rows[AlbumRowIndex].Cells[AlbumColIndex].Value.ToString();
                try
                {
                    Process.Start(GridValueString);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Can't reach this location!");
                }
                return false;
            }
            else if (AlbumColIndex == DGC.colDirectoryGenre)
            {
                pickGenreForm.ShowDialog();
                int countRecord = DBFunctions.AutoSearchDatabaseAlbums("", DGV);                
                boxListConsole.Items.Add("Album table updated: " + countRecord.ToString());
                boxListConsole.SelectedIndex = boxListConsole.Items.Count - 1;

                //DGV.ClearSelection();                               //[przemy knowledge - zaznaczanie data grid view]
                //DGV.CurrentCell = DGV.Rows[AlbumRowIndex].Cells[0]; //[przemy knowledge - zaznaczanie data grid view]
                //DGV.Rows[AlbumRowIndex].Selected = true;            //[przemy knowledge - zaznaczanie data grid view]
                return true;
            }   
            else if (AlbumColIndex == DGC.colAlbumGeneralGenre)
            {
                DialogResult res = MessageBox.Show("Download Genres from Discogs?", "Discogs API", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    string ArtistGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colArtistName].Value.ToString();
                    string ReleaseGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colAlbumName].Value.ToString();
                    int AlbumID = Convert.ToInt32(DGV.Rows[AlbumRowIndex].Cells[DGC.colIndexAlbum].Value);

                    DiscogsManagement.startDiscogs(boxListConsole.Items, ArtistGrid, ReleaseGrid, AlbumID);
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            return false;
        }
        public static void OpenPairOfFolders(DataGridView DGV, int AlbumRowIndex)
        {
            DGV.MultiSelect = false;
            string GridValuePath1 = DGV.Rows[AlbumRowIndex].Cells[0].Value.ToString();
            string GridValuePath2 = DGV.Rows[AlbumRowIndex].Cells[1].Value.ToString();
            Process.Start(GridValuePath1);
            Process.Start(GridValuePath2);
        }
        public static void CreateDirectoryForAlbum(string path, string artistName, string albumName, ListBox.ObjectCollection boxListConsole, TextBox tbxPurgPath, TextBox tbxGeneralPath,int AlbumId, string AlbumDirectory, int AlbumRowIndex)
        {
            //ARTIST_ALBUMNAME
            artistName = Functions.findProhibitedSigns(artistName);
            albumName = Functions.findProhibitedSigns(albumName);

            string buildPath = path + @"\" + artistName + "_" + albumName;
            
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
                boxListConsole.Add($"...creating new Album folder in: {buildPath}");
                //move
                MoveFiles(boxListConsole, tbxPurgPath, tbxGeneralPath, AlbumId, AlbumDirectory, buildPath, AlbumRowIndex);

            }
            else
            {
                boxListConsole.Add($"...Path: {buildPath} - already exist.");
                //try move
                MoveFiles(boxListConsole, tbxPurgPath, tbxGeneralPath, AlbumId, AlbumDirectory, buildPath, AlbumRowIndex);
            }
                
        }
        private static void MoveFiles(ListBox.ObjectCollection boxListConsole, TextBox tbxPurgPath, TextBox tbxGeneralPath, int AlbumId, string AlbumDirectory, string buildPath, int AlbumRowIndex)
        {
                      
            // if purgatory path exist in AlbumDirectory
            if (AlbumDirectory.Contains(tbxPurgPath.Text))
            {
                
                List<SQLTrackTable> LSQTT = new List<SQLTrackTable>();
                DBFunctions db = new DBFunctions();
                LSQTT = db.GetTrackByAlbumId(AlbumId);
                int AlbumTransaction = 0;

                DateTime dateNow = DateTime.Now;
                string dateString = String.Format("{0:MM/dd/yyyy}", dateNow);
                string trackDirectory = "";
                string movedAlbumDirectory = "";
                foreach (SQLTrackTable item in LSQTT)
                {
                    string prepareMoveString = item.TrackDirectory; // with file name
                    prepareMoveString = Functions.findProhibitedSigns(prepareMoveString);
                    trackDirectory = Path.GetDirectoryName(item.TrackDirectory);
                    int IndexLibTrack = Convert.ToInt32(item.IndexLib);

                    if (Directory.Exists(trackDirectory))
                    {
                        if (item.FileExtension != "" & item.TrackName != "" & IndexLibTrack != 0)
                        {
                            string newFileName = item.TrackName + "." + item.FileExtension;
                            try
                            {
                                string musicFileFullPath = buildPath + @"\" + newFileName;
                                if (!File.Exists(musicFileFullPath))
                                {
                                    if (item.TrackRating != 1)
                                    {
                                        File.Move(prepareMoveString, musicFileFullPath);
                                        movedAlbumDirectory = Path.GetDirectoryName(musicFileFullPath);
                                        boxListConsole.Add($"...[File moved: {musicFileFullPath}]!");
                                    }
                                    else
                                    {
                                        //
                                        movedAlbumDirectory = Path.GetDirectoryName(musicFileFullPath);
                                        MusicFileDetails MFD = new MusicFileDetails(); // deklaruj klase
                                        MusicFileMgt.QuickRead(prepareMoveString, MFD);
                                        try
                                        {
                                            int TrackIndex = Convert.ToInt32(MFD.trackIndex);
                                            if (TrackIndex > 1)
                                            {
                                                File.Delete(prepareMoveString); // delete all one star files   
                                                
                                                db.UpdateTrackFileStatusByIndexLib(TrackIndex, "DELETED");
                                                
                                                db.UpdateTrackFileDateProceed(TrackIndex, dateString);
                                                boxListConsole.Add($"...[File deleted (one star reason): {prepareMoveString}]!");
                                            }
                                            else
                                                boxListConsole.Add($"...[No index file - error while deleting.): {prepareMoveString}]!");
                                        }
                                        catch (Exception e)
                                        {
                                            boxListConsole.Add($"...[No index file - error while deleting.): {prepareMoveString}]!");
                                        }


                                        
                                    }                                        
                                    //update album - one time

                                    AlbumTransaction += 1;
                                    if (AlbumTransaction == 1)
                                    {
                                        db.UpdateDirectoryPathByAlbumID(AlbumId, buildPath);
                                        db.UpdateAlbumProceedDate(AlbumId, dateString);
                                    }
                                        
                                    //update track                            
                                    db.UpdateTrackDirectoryPathByIndexLib(IndexLibTrack, musicFileFullPath);                                    
                                    db.UpdateTrackFileDateProceed(IndexLibTrack, dateString);
                                }

                                else
                                {
                                    //override file
                                    boxListConsole.Add($"...{musicFileFullPath} already exist!");

                                    var confirmResult = MessageBox.Show($"Are you sure to override this item: {musicFileFullPath}?",
                                     "Confirm Override.",
                                     MessageBoxButtons.YesNo);
                                    if (confirmResult == DialogResult.Yes)
                                    {
                                        //show file compared view - tag in new form [todo]
                                        File.Delete(musicFileFullPath);
                                        File.Move(prepareMoveString, musicFileFullPath);
                                        //update album - one time
                                        AlbumTransaction += 1;
                                        if (AlbumTransaction == 1)
                                        {
                                            db.UpdateDirectoryPathByAlbumID(AlbumId, buildPath);
                                            db.UpdateAlbumProceedDate(AlbumId, dateString);
                                        }
                                           
                                        //update track                            
                                        db.UpdateTrackDirectoryPathByIndexLib(IndexLibTrack, musicFileFullPath);
                                        db.UpdateTrackFileDateProceed(IndexLibTrack, dateString);
                                    }                                    
                                }     
                            }
                            catch (FileNotFoundException e)
                            {
                                boxListConsole.Add($"...[no File in: {prepareMoveString}]!");
                            }               
                        }
                        else
                            boxListConsole.Add($"...[track validation failed - track name or file extension or IndexLib = empty]!: {AlbumId}");
                    }
                    else
                        boxListConsole.Add($"...desired move path expired!: {AlbumId}");
                }
                boxListConsole.Add($"...album successfuly moved!: {AlbumId}");
                //check if directory is filled with additional files
                try
                {
                    Directory.Delete(trackDirectory);
                }
                catch (Exception e)
                {
                    if (trackDirectory != "")
                    {
                        GlobalVariables.IgnoreCurrentFolder = 0;
                        Functions.TreeDirectorySearch(trackDirectory, movedAlbumDirectory, boxListConsole);
                    }
                    else
                        boxListConsole.Add($"...track path is empty!: {AlbumId}");
                }


            }
            else
                boxListConsole.Add($"...purgatory path error while moving from: {AlbumDirectory}");
            
        }
        public static void DeleteAlbum(DataGridView DGV, int AlbumRowIndex)
        {
            SQLDataValidate.dataGridColumns DGC = new SQLDataValidate.dataGridColumns();
            int AlbumId = Convert.ToInt32(DGV.Rows[AlbumRowIndex].Cells[DGC.colIndexAlbum].Value);
            string AlbumName = DGV.Rows[AlbumRowIndex].Cells[DGC.colAlbumName].Value.ToString();
            string AlbumArtist = DGV.Rows[AlbumRowIndex].Cells[DGC.colArtistName].Value.ToString();

            DBFunctions db = new DBFunctions();
            List<SQLTrackTable> LTT = new List<SQLTrackTable>();
            LTT = db.GetTrackByAlbumId(AlbumId);

            DialogResult res = MessageBox.Show($"Delete Album from SQL DB? \n Selected Album ID: {AlbumId} \n Album Name: {AlbumName} \n Artist: {AlbumArtist} \n Track Count to delete also: {LTT.Count}", "Delete Album & Tracks", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                DBFunctions.DeleteAlbumByAlbumID(AlbumId);
                DBFunctions.DeleteTracksByAlbumID(AlbumId);
            }
        }
    }
}
