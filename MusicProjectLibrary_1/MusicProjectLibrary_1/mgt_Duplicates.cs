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
    class mgt_Duplicates
    {
        
        public static void checkTrackDuplicates(List<string> uniqueArtists, List<string> uniquePartializedTitles, List<DuplicatesPaths> LDP)//, ListBox.ObjectCollection BoxListConsole)
        {
            int ByteTolerance = 1000;
            int totalDuplicates = 0;
            foreach (string artist in uniqueArtists)
            {
                //find all tracks 

                List<SQLTrackTable> queryGetAllTracksByArtist = new List<SQLTrackTable>();
                List<TrackDuplicates> ListTrackDuplicates = new List<TrackDuplicates>();

                mgt_SQLDatabase db = new mgt_SQLDatabase();
                queryGetAllTracksByArtist = db.GetTrackByArtist(artist);


                foreach (SQLTrackTable itemTrack in queryGetAllTracksByArtist)
                {
                    int count = 0;
                    char specialSign = ' ';
                    int specialAppear = 0;
                    string extractedString;
                    string toExtract = itemTrack.TrackName;

                    foreach (char special in toExtract)
                        if (special == specialSign) count++;
                    for (int i = 0; i <= count; i++)
                    {
                        int firstSign = toExtract.IndexOf(specialSign, specialAppear);
                        if (firstSign == -1)
                            extractedString = toExtract;
                        else
                        {
                            extractedString = toExtract.Substring(specialAppear, firstSign);
                        }
                        toExtract = toExtract.Substring(firstSign + 1, toExtract.Length - firstSign - 1);

                        if (extractedString.Length > 3)
                        {
                            if (uniquePartializedTitles.Any(uPartTrack => uPartTrack == extractedString)) ; // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie                                
                            else
                            {
                                TrackDuplicates TD = new TrackDuplicates();
                                TD.partTrackName = extractedString;
                                TD.fullTrackName = itemTrack.TrackName;
                                TD.TrackPath = itemTrack.TrackDirectory;
                                if (File.Exists(itemTrack.TrackDirectory))
                                {
                                    MusicFileDetails MFD = new MusicFileDetails();
                                    mgt_HddAnalyzer.QuickRead(itemTrack.TrackDirectory, MFD);
                                    TD.MFD = MFD;
                                }

                                ListTrackDuplicates.Add(TD);

                                uniquePartializedTitles.Add(extractedString);
                            }
                        }
                    }
                }
                foreach (TrackDuplicates itemTD in ListTrackDuplicates)
                {
                    int matchesPartName = 0;
                    int matchesBytes = 0;
                    bool firstTrigg = false;
                    bool secondTrigg = false;
                    foreach (SQLTrackTable itemTrack in queryGetAllTracksByArtist)
                    {
                        if (itemTrack.TrackName.Contains(itemTD.partTrackName))
                        {
                            matchesPartName += 1;
                            if (matchesPartName > 1)
                            {
                                //
                                //BoxListConsole.Add(itemTD.TrackPath);
                                firstTrigg = true;
                            }
                            //check bytes
                            if (File.Exists(itemTrack.TrackDirectory))
                            {
                                MusicFileDetails MFD = new MusicFileDetails();
                                mgt_HddAnalyzer.QuickRead(itemTrack.TrackDirectory, MFD);
                                try
                                {
                                    if (MFD.pickedAFile.NumberOfMusicBytes <= itemTD.MFD.pickedAFile.NumberOfMusicBytes + ByteTolerance & MFD.pickedAFile.NumberOfMusicBytes >= itemTD.MFD.pickedAFile.NumberOfMusicBytes - ByteTolerance)
                                    {
                                        matchesBytes += 1;
                                        if (matchesPartName > 1)
                                            secondTrigg = true;
                                    }
                                }
                                catch (Exception e)
                                {

                                }

                            }

                        }
                        //check lenght in seconds [todo in future]
                        if (firstTrigg & secondTrigg)
                        {
                            firstTrigg = false;
                            secondTrigg = false;
                            //BoxListConsole.Add($"duplicate: {itemTD.TrackPath} artist: {itemTrack.TrackDirectory} word: itemTD.partTrackName");
                            totalDuplicates += 1;
                            DuplicatesPaths DP = new DuplicatesPaths();
                            DP.firstPath = itemTD.TrackPath;
                            DP.secondPath = itemTrack.TrackDirectory;

                            int x1 = 0;
                            if (Int32.TryParse(itemTD.MFD.trackRating, out x1))
                            {
                                DP.ratingStarsFirstFile = x1;
                            }
                            else
                                DP.ratingStarsFirstFile = 0;

                            int x2 = 0;
                            if (Int32.TryParse(itemTrack.TrackRating.ToString(), out x2))
                            {
                                DP.ratingStarsSecondFile = x2;
                            }
                            else
                                DP.ratingStarsSecondFile = 0;
                            int x3 = 0;
                            if (Int32.TryParse(itemTD.MFD.trackIdAlbumIndex, out x3))
                            {
                                DP.firstIDAlbum = x3;
                            }
                            else
                                DP.firstIDAlbum = 0;
                            int x4 = 0;
                            if (Int32.TryParse(itemTD.MFD.trackIndex, out x4))
                            {
                                DP.firstIDTrack = x4;
                            }
                            else
                                DP.firstIDTrack = 0;

                            DP.secondIDAlbum = itemTrack.IdAlbumIndex;
                            DP.secondIDTrack = itemTrack.IndexLib;

                            LDP.Add(DP);
                        }

                    }

                }

            }
            MessageBox.Show($"total duplicates: {totalDuplicates}");
        }
        public static void OpenPairOfFolders(DataGridView DGV, int AlbumRowIndex)
        {
            dataGridColumnsDuplicates DGCD = new dataGridColumnsDuplicates();

            DGV.MultiSelect = false;
            string GridValuePath1 = DGV.Rows[AlbumRowIndex].Cells[DGCD.colFirstPath].Value.ToString();
            string GridValuePath2 = DGV.Rows[AlbumRowIndex].Cells[DGCD.colSecondPath].Value.ToString();
            Process.Start(GridValuePath1);
            Process.Start(GridValuePath2);
        }
        public static void DeleteTracksFromDuplicates(DataGridView DGV, ListBox.ObjectCollection LBOX, int TrackID, int AlbumID, string purgatoryTrackPath)
        {
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            if (AlbumID != 0)
            {
                List<SQLTrackTable> LTT = new List<SQLTrackTable>();
                LTT = db.GetTrackByAlbumId(AlbumID);
                if (LTT.Count == 0)
                {
                    DialogResult res1 = MessageBox.Show($"There are no related tracks in Album ID: {AlbumID} Delete album from DB?", "Delete Album", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (res1 == DialogResult.OK)
                    {
                        mgt_SQLDatabase.DeleteAlbumByAlbumID(AlbumID);
                    }
                }
            }
            if (TrackID != 0)
            {
                DialogResult res2 = MessageBox.Show($"Delete Track from SQL DB? \n Selected Track ID: {TrackID}", "Delete Tracks", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res2 == DialogResult.OK)
                {
                    mgt_SQLDatabase.DeleteTracksByAlbumID(TrackID);
                    File.Delete(purgatoryTrackPath);
                    LBOX.Add($"DELETED from DB TRACK ID: {purgatoryTrackPath}");
                    LBOX.Add($"DELETED FILE: {purgatoryTrackPath}");
                }
            }

        }
    }
}
