using MusicProjectLibrary_1.AppForms;
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
    class mgt_Directory
    {
        
        
        
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
                mgt_SQLDatabase db = new mgt_SQLDatabase();
                LSQTT = db.GetTrackByAlbumId(AlbumId);
                int AlbumTransaction = 0;

                DateTime dateNow = DateTime.Now;
                string dateString = String.Format("{0:MM/dd/yyyy}", dateNow);
                string trackDirectory = "";
                string movedAlbumDirectory = "";
                foreach (SQLTrackTable item in LSQTT)
                {
                    string primaryPath = item.TrackDirectory; // with file name
                    primaryPath = Functions.findProhibitedSigns(primaryPath);
                    trackDirectory = Path.GetDirectoryName(item.TrackDirectory);
                    int IndexLibTrack = Convert.ToInt32(item.IndexLib);

                    if (Directory.Exists(trackDirectory))
                    {
                        if (item.FileExtension != "" & item.TrackName != "" & IndexLibTrack != 0)
                        {
                            string ItemName = Functions.findProhibitedSigns(item.TrackName);
                            string newFileName = ItemName + "." + item.FileExtension;
                            try
                            {
                                string musicFileFullPath = buildPath + @"\" + newFileName;
                                if (!File.Exists(musicFileFullPath))
                                {
                                    if (item.TrackRating != 1)
                                    {
                                        File.Move(primaryPath, musicFileFullPath);
                                        movedAlbumDirectory = Path.GetDirectoryName(musicFileFullPath);
                                        boxListConsole.Add($"...[File moved: {musicFileFullPath}]!");
                                    }
                                    else
                                    {
                                        //
                                        mgt_Tracks.deleteOneStars(primaryPath, dateString, boxListConsole);
                                        /*
                                        //movedAlbumDirectory = Path.GetDirectoryName(musicFileFullPath);
                                        MusicFileDetails MFD = new MusicFileDetails(); // deklaruj klase
                                        mgt_HddAnalyzer.QuickRead(primaryPath, MFD);
                                        try
                                        {
                                            int TrackIndex = Convert.ToInt32(MFD.trackIndex);
                                            if (TrackIndex > 1)
                                            {
                                                File.Delete(primaryPath); // delete all one star files   
                                                
                                                db.UpdateTrackFileStatusByIndexLib(TrackIndex, "DELETED");                                                
                                                db.UpdateTrackFileDateProceed(TrackIndex, dateString);
                                                boxListConsole.Add($"...[File deleted (one star reason): {primaryPath}]!");
                                            }
                                            else
                                                boxListConsole.Add($"...[No index file - error while deleting.): {primaryPath}]!");
                                        }
                                        catch (Exception e)
                                        {
                                            boxListConsole.Add($"...[No index file - error while deleting.): {primaryPath}]!");
                                        }
                                        */



                                    }                                        
                                    //update album - one time

                                    AlbumTransaction += 1;
                                    if (AlbumTransaction == 1)
                                    {
                                        db.UpdateAlbumDirectoryPathByAlbumID(AlbumId, buildPath);
                                        db.UpdateAlbumProceedDate(AlbumId, dateString, true);
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
                                        File.Move(primaryPath, musicFileFullPath);
                                        //update album - one time
                                        AlbumTransaction += 1;
                                        if (AlbumTransaction == 1)
                                        {
                                            db.UpdateAlbumDirectoryPathByAlbumID(AlbumId, buildPath);
                                            db.UpdateAlbumProceedDate(AlbumId, dateString, true);
                                        }
                                           
                                        //update track                            
                                        db.UpdateTrackDirectoryPathByIndexLib(IndexLibTrack, musicFileFullPath);
                                        db.UpdateTrackFileDateProceed(IndexLibTrack, dateString);
                                    }                                    
                                }     
                            }
                            catch (FileNotFoundException e)
                            {
                                boxListConsole.Add($"...[no File in: {primaryPath}]!");
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
                        Functions.baseDirectory = trackDirectory;
                        Functions.FindFilesInTreeDirectory(false, trackDirectory, movedAlbumDirectory, boxListConsole);
                    }
                    else
                        boxListConsole.Add($"...track path is empty!: {AlbumId}");
                }


            }
            else
                boxListConsole.Add($"...purgatory path error while moving from: {AlbumDirectory}");
            
        }

        public static void DeleteAlbumFromAlbumDGV(DataGridView DGV, int AlbumRowIndex)
        {
            mgt_SQLValidation.dataGridColumns DGC = new mgt_SQLValidation.dataGridColumns();
            int AlbumId = Convert.ToInt32(DGV.Rows[AlbumRowIndex].Cells[DGC.colIndexAlbum].Value);
            string AlbumName = DGV.Rows[AlbumRowIndex].Cells[DGC.colAlbumName].Value.ToString();
            string AlbumArtist = DGV.Rows[AlbumRowIndex].Cells[DGC.colArtistName].Value.ToString();

            mgt_SQLDatabase db = new mgt_SQLDatabase();
            List<SQLTrackTable> LTT = new List<SQLTrackTable>();
            LTT = db.GetTrackByAlbumId(AlbumId);

            DialogResult res = MessageBox.Show($"Delete Album from SQL DB? \n Selected Album ID: {AlbumId} \n Album Name: {AlbumName} \n Artist: {AlbumArtist} \n Track Count to delete also: {LTT.Count}", "Delete Album & Tracks", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                mgt_SQLDatabase.DeleteTracksByAlbumID(AlbumId);
                mgt_SQLDatabase.DeleteAlbumByAlbumID(AlbumId);                
            }
        }
        public static void DeleteAlbumFromDuplicates(int AlbumID)
        {            
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            List<SQLTrackTable> LTT = new List<SQLTrackTable>();
            LTT = db.GetTrackByAlbumId(AlbumID);
            
            DialogResult res = MessageBox.Show($"Delete Album from SQL DB + related files? \n Selected Album ID: {AlbumID} \n Track Count to delete also: {LTT.Count}", "Delete Album & Tracks", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                //
                //delete SQL INFO
                //
                mgt_SQLDatabase.DeleteAlbumByAlbumID(AlbumID);
                mgt_SQLDatabase.DeleteTracksByAlbumID(AlbumID);
                //
                //delete PURG physical files
                //


            }
        }
        
    }
}
