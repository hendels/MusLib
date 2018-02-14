﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace MusicProjectLibrary_1
{
    public class Functions
    {
        public static string globalOldPath;
        public static string filenamePath;

        

        public static void xmlSave(object obj, string filename)
        {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            sr.Serialize(writer, obj);
            writer.Close();
        }

        public static void getPurgatoryPath(TextBox currentTextBox, Boolean updateTextbox)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Functions.filenamePath = startupPath + @"\purgatoryPath.txt";
            Functions.globalOldPath = MiscFunctions.ReadFile(Functions.filenamePath);


            if (updateTextbox)
            {
                currentTextBox.Text = Functions.globalOldPath;
            }
        }
        public static void getMainDirectoryPath(TextBox currentTextBox, Boolean updateTextbox)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Functions.filenamePath = startupPath + @"\mainDirectoryPath.txt";
            Functions.globalOldPath = MiscFunctions.ReadFile(Functions.filenamePath);


            if (updateTextbox)
            {
                currentTextBox.Text = Functions.globalOldPath;
            }
        }
        public static void getDriveGeneralPath(TextBox currentTextBox, Boolean updateTextbox)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Functions.filenamePath = startupPath + @"\generalDrivePath.txt";
            Functions.globalOldPath = MiscFunctions.ReadFile(Functions.filenamePath);


            if (updateTextbox)
            {
                currentTextBox.Text = Functions.globalOldPath;
            }
        }
        public static void pickPath(int pathCase, TextBox currentTextBox)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                switch (pathCase)
                {
                    case 1:
                        getPurgatoryPath(currentTextBox, false);
                        break;
                    case 2:
                        getMainDirectoryPath(currentTextBox, false);
                        break;
                    case 3:

                        break;
                }

                string newPath = FBD.SelectedPath;
                if (Functions.globalOldPath != newPath)
                {
                    string messageText = $"stara ścieżka główna to: {Functions.globalOldPath} czy na pewno chcesz zmienić na: {FBD.SelectedPath}?";

                    if (MessageBox.Show(messageText, "różne ścieżki", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        MiscFunctions.WriteFile(Functions.filenamePath, FBD.SelectedPath);
                        currentTextBox.Text = FBD.SelectedPath;
                    }
                }
            }
        }

        public static void createArtistsList(List<string> uniqueArtists)
        {
            //List<string> uniqueArtists = new List<string>();
            List<SQLAlbumTable> queryGetAllArtists = new List<SQLAlbumTable>();

            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllArtists = db.GetAlbumArtists();
            foreach(SQLAlbumTable artist in queryGetAllArtists)
            {
                if (uniqueArtists.Any(uArtist => uArtist == artist.AlbumArtist)) // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie
                {

                }
                else
                {
                    uniqueArtists.Add(artist.AlbumArtist);
                }
            }            
        }
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
                                if(File.Exists(itemTrack.TrackDirectory))
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
                foreach(TrackDuplicates itemTD in ListTrackDuplicates)
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
                            if(File.Exists(itemTrack.TrackDirectory))
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
               
        
        public static void getRelatedToArtistGenres()
        {
            List<SQLAlbumTable> queryGetAllGenres = new List<SQLAlbumTable>();
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllGenres = db.GetAlbumsGenre();
            //chlstSuggestedGenres
        }
        

        public static string findProhibitedSigns(string checkString)
        {
            List<string> signs = new List<string>();
            signs.Add("|");
            signs.Add(@"""");
            signs.Add(@"/");
            signs.Add(@"<");
            signs.Add(@">");
            //signs.Add(@".");
            string modifiedString = checkString;
            foreach (string itemList in signs)
            {
                if (checkString.Contains(itemList))
                {
                    modifiedString = modifiedString.Replace(itemList, string.Empty);
                };

            }
            return modifiedString;
        }
        public static void showFolderBrowserDialog(ListBox.ObjectCollection currentListBoxItems)
        {

            currentListBoxItems.Clear(); //to czyści listboxa

            //string[] rootFiles = Directory.GetFiles(FBD.SelectedPath); //pobiera pliki
            //string[] rootDirectories = Directory.GetDirectories(FBD.SelectedPath); //pobiera foldery ze ścieżki
            string[] drives = Directory.GetLogicalDrives();

            foreach (string dr in drives)
            {
                DriveInfo di = new DriveInfo(dr);
                string driveString = Convert.ToString(di);
                if (driveString == @"D:\")
                {
                    MessageBox.Show("yea " + driveString);
                    DirectoryInfo rootDir = di.RootDirectory;
                    

                }
            }
            /*
            foreach (string file in rootFiles)
            {
                ListBoxItems.Items.Add(Path.GetFileName(file)); // zostaw samo file zeby wyswietlic pełną ścieżkę pliku
                ListBoxItems.Items.Add(Path.GetExtension(file)); //pobiera samo rozrzeszenie ze ścieżki
            }
            foreach (string dir in rootDirectories)
            {
                ListBoxItems.Items.Add(Path.GetFileName(dir)); // zostaw samo dir zeby wyswietlic pełną ścieżkę katalogu
            }

            foreach (string dir in rootDirectories)
            {
                int directoryCount = Directory.GetDirectories(FBD.SelectedPath).Length;
                if (directoryCount > 0)
                {
                    //WalkDirectoryTree(rootDirectories);
                }

            }
            */

        } //unused
        
        public static void TreeFileSearch(string CurrentDirectory)
        {            
            foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
            {


                try
                {
                    
                }
                catch (Exception Ex)
                {
                    
                }

            }
            foreach (string ChildDirectory in Directory.GetDirectories(CurrentDirectory))
                TreeFileSearch(ChildDirectory);
        }
        public static string baseDirectory { get; set; }
        public static void FindFilesInTreeDirectory(bool dontMoveFile, string CurrentDirectory, string movedAlbumDirectory, ListBox.ObjectCollection boxListConsole) //[przemy knowledge - best tree search]
        {
            int foundIllegalFiles = 0;
            try
            {
                foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
                {
                    if (Path.GetExtension(CurrentFile) == ".flac" || Path.GetExtension(CurrentFile) == ".mp3")
                    {
                        foundIllegalFiles += 1;
                        boxListConsole.Add($"found illegal file in subfolder {CurrentFile} - manual action needed.");
                    }

                    if (!dontMoveFile)
                    {
                        if (Path.GetExtension(CurrentFile) == ".jpg" || Path.GetExtension(CurrentFile) == ".png" 
                            || Path.GetExtension(CurrentFile) == ".tif" || Path.GetExtension(CurrentFile) == ".jpeg") 
                        {
                            try
                            {
                                File.Move(CurrentFile, movedAlbumDirectory + @"\" + Path.GetFileName(CurrentFile));
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException ex1)
            {
                MessageBox.Show(ex1.ToString());
            }

            if (foundIllegalFiles == 0 & Directory.Exists(CurrentDirectory))
            {
                try
                {
                    if (!dontMoveFile)
                    {
                        foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
                        {
                            File.Delete(CurrentFile);
                            boxListConsole.Add($"deleting trash file: {CurrentFile}");
                        }
                    }   
                    else
                    {
                        Directory.Delete(CurrentDirectory, dontMoveFile);
                        try { Directory.Delete(Directory.GetParent(baseDirectory).ToString()); }
                        catch { }
                    }
                        
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            if (!dontMoveFile)
            {
                foreach (string ChildDirectory in Directory.GetDirectories(CurrentDirectory))
                {
                    GlobalVariables.IgnoreCurrentFolder += 1;
                    FindFilesInTreeDirectory(dontMoveFile, ChildDirectory, movedAlbumDirectory, boxListConsole);
                }
                //take care of primaryFolder after all moving - delete it
                Directory.Delete(baseDirectory, true);
                try { Directory.Delete(Directory.GetParent(baseDirectory).ToString()); }
                catch { }
            }     
        }
    }
}
