using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
using System.Windows.Forms;
using JAudioTags;
using TagLib;
using TagLib.Id3v2;
using System.IO;

//
namespace MusicProjectLibrary_1
{
    public class MusicFileMgt
    {
         
        public static ListBox.ObjectCollection globalBoxListConsole;
        public static List<MusicFileDetails> globalMusicFileDetailsList;
        public static int publicAlbumIndex;
        // This method matches the signature required by FileWalk.
        // Its name is passed to the FileWalk constructor.
        // It is called for every FLAC or MP3 file encountered
        
        public static void changeFiles(int caseChange, string Root, ListBox.ObjectCollection boxListConsole)
        {
            MusicFileMgt.globalBoxListConsole = boxListConsole;
            //Call the FileWalk method to visit all files in tree
            switch (caseChange)
            {
                case 0:
                    //AudioFile.FileWalk(Root, ChangeTags, new List<string>() { "MP3", "FLAC" },
                    //Log, Errors);
                    break;
                case 1:
                    /*
                    if (Helpers.JGetExtension(Filename) == "FLAC")
                        AFile = new FLACFile(Filename, false);
                    AFile.MODTAGDATE = dateToday.ToString("d");
                    AFile.Save(true);
                    AudioFile.FileWalk(Root, ChangeModDate, new List<string>() { "MP3", "FLAC" });
                    */
                    break;
                default:
                    break;
            }


            MessageBox.Show("done");

        }
        public static void readFiles(ListBox.ObjectCollection boxListConsole, ProgressBar progressBar, string DirectoryPick, string GeneralDirectoryPick, Label progressLabel)
        {           
            boxListConsole.Clear(); 
            MusicFileMgt.globalBoxListConsole = boxListConsole;
            //Call the FileWalk method to visit all files in tree
            List<MusicFileDetails> ListMFD = new List<MusicFileDetails>(); 
            globalMusicFileDetailsList = ListMFD; // << tu gówno, bo nie mam jak zmienić procedur w COMMON_AudioFiles, za dużo odwołań, biblioteka się wypierdala
            progressLabel.Text = "Reading files...";
            if (GlobalVariables.checkGeneralPath)            
                AudioFile.FileWalk(GeneralDirectoryPick, ReadTags, new List<string>() { "MP3", "FLAC" });            
            else
                AudioFile.FileWalk(DirectoryPick, ReadTags, new List<string>() { "MP3", "FLAC" });


            int counterList = 0;
 
            List<uniqueCatalogs> uniqueDir = new List<uniqueCatalogs>();

            foreach (MusicFileDetails item in globalMusicFileDetailsList) //przejeb sie przez liste
            {
                counterList = counterList + 1;
                //MessageBox.Show(counterList + " " + item.trackAlbum);
                if (uniqueDir.Any(uCat => uCat.uniqeDirectory == item.trackDirectory)) // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie
                {
                    //globalBoxListConsole.Add("catalog exist for: " + item.trackDirectory);
                }
                else
                {
                    uniqueCatalogs UqC = new uniqueCatalogs();
                    UqC.uniqeDirectory = item.trackDirectory;
                    uniqueDir.Add(UqC);
                }

            }
            //debug
            foreach (uniqueCatalogs item in uniqueDir)
            {
                globalBoxListConsole.Add("unique dir: " + item.uniqeDirectory);
            }
            //            
            //Write indexes
            //
            bool writeIndex = GlobalVariables.globalwriteIndexes;
            if (writeIndex)
            {
                writeTrackIndexToFile(uniqueDir, globalMusicFileDetailsList, progressBar);
            }
            //
            //Process catalogs
            //
            bool proccessCat = GlobalVariables.globalProcessCatalog;
            if (proccessCat)
            {
                processCatalogs(uniqueDir, globalMusicFileDetailsList, progressBar, progressLabel);

            }
            ///////[<progress bar]
            progressLabel.Text = "Done.";
            ///////[progress bar>]
        }
        public static async void writeAlbumIndexToFile(DataGridView DGV, ProgressBar progressBar)
        {
            //
            //update album songs
            //
            progressBar.Step = 1;

            SQLDataValidate.dataGridColumns DGC = new SQLDataValidate.dataGridColumns();
            bool GridValueBool;
            for (int rows = 0; rows < DGV.Rows.Count; rows++)
            {
                GridValueBool = Convert.ToBoolean(DGV.Rows[rows].Cells[DGC.colWriteIndex].Value);
                if (GridValueBool)
                {

                    List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();
                    DBFunctions db = new DBFunctions();
                    int AlbumID = Convert.ToInt32(DGV.Rows[rows].Cells[DGC.colIndexAlbum].Value);
                    queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(AlbumID);
                    int maxValue = queryGetAllTracksByAlbumID.Count;
                    int processed = 0;
                    MusicFileDetails MFD = new MusicFileDetails();
                    foreach (SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
                    {
                        if(System.IO.File.Exists(itemTrack.TrackDirectory))
                        {
                            MusicFileMgt.QuickRead(itemTrack.TrackDirectory, MFD);


                            MFD.pickedAFile.INDEXALBUM = AlbumID.ToString();
                            MFD.pickedAFile.Save(true);
                            processed += 1;
                        }            
                    }
                    db.UpdateTrackAlbumIndexByAlbumID(AlbumID, AlbumID);
                    db.UpdateAlbumIndexCheckByAlbumID(AlbumID, true);

                    var progress = new Progress<int>(v =>
                    {
                        // This lambda is executed in context of UI thread,
                        // so it can safely update form controls
                        try
                        {
                            progressBar.Value = v;
                        }
                        catch (Exception e)
                        {

                        }
                    });

                    // Run operation in another thread
                    await Task.Run(() => Helper.DoWork(progress, processed, 100,""));
                }
            }            


 
        }
        static async void writeTrackIndexToFile(List<uniqueCatalogs> UQC, List<MusicFileDetails> MFD, ProgressBar progressBar)
        {           
            ///////[<progress bar]
            int processed = 0;
            int maxValue = MFD.Count;
            string progressString = "writing Indexes: ";
            progressBar.Maximum = 105;
            progressBar.Step = 1;
            ///////[progress bar>]
            foreach (MusicFileDetails itemMFD in MFD)
            {
                processed += 1;
                //check if INDEX tag exist
                if ((itemMFD.trackIndex == "") | (itemMFD.trackIndex == "0"))
                {
                        
                    List<SQLSeriesNoTable> queryGetLastIndex = new List<SQLSeriesNoTable>();
                    DBFunctions db = new DBFunctions();
                    queryGetLastIndex = db.GetSeriesLastIndex(10000);
                    foreach (SQLSeriesNoTable TrackSeriesNo in queryGetLastIndex)
                    {
                        int NextIndexTrack = TrackSeriesNo.IndexNo + 1;
                        itemMFD.pickedAFile.INDEXTRACK = NextIndexTrack.ToString();
                        itemMFD.pickedAFile.Save(true);
                        //save IndexNo in SQL
                        db.UpdateIndexTrackInSeriesNo(10000, NextIndexTrack);

                        var progress = new Progress<int>(v =>
                        {
                            // This lambda is executed in context of UI thread,
                            // so it can safely update form controls
                            try
                            {
                                progressBar.Value = v;                                
                            }
                            catch (Exception e)
                            {

                            }
                        });

                        // Run operation in another thread
                        await Task.Run(() => Helper.DoWork(progress, processed, maxValue, progressString));
                    }                         
                }                    
            }
            
        }
        static async void DoProgress(ProgressBar progressBar, int processed, int maxValue, Label progressLabel, string progressString)
        {
            Label threadLabel = new Label();
            var progress = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                try
                {
                    progressBar.Value = v;
                    
                    threadLabel.Text = progressString;
                }
                catch (Exception e)
                {
                    progressBar.Value = maxValue;
                }
            });

            // Run operation in another thread
            await Task.Run(() => Helper.DoWork(progress, processed, maxValue, progressString));
        }
        static void processCatalogs(List<uniqueCatalogs> UQC, List<MusicFileDetails> MFD, ProgressBar progressBar, Label progressLabel)
        {
            ///////[<progress bar]
            int processed = 0;
            int maxValue = MFD.Count;
            progressBar.Maximum = 101;
            progressBar.Step = 1;
            string progressString = "Processing files: ";
            ///////[progress bar>]
            foreach (uniqueCatalogs itemUQC in UQC)
            {
                processed += 1;
                if (processed == 1)
                    maxValue = UQC.Count;
                DoProgress(progressBar, processed, maxValue, progressLabel, progressString);

                List<int> ListCheckTags = new List<int>();
                List<int> ListCheckRating = new List<int>();                

                List<TagInformation> ListTagInformation = new List<TagInformation>();
                List<RatingInformation> ListRatingInformation = new List<RatingInformation>();
                List<MusicFileDetails> ListMfdAlbum = new List<MusicFileDetails>();

                GlobalRating.globalhowManyRate = 0;
                GlobalRating.globalhowManyUnrate = 0;

                foreach (MusicFileDetails itemMFD in MFD)
                {
                    if (itemUQC.uniqeDirectory == itemMFD.trackDirectory)
                    {
                        ///////MessageBox.Show("2)" + itemMFD.trackDirectory + " " + itemMFD.trackName);
                        checkTags(itemMFD, ListCheckTags, ListTagInformation);                   ///sprawdz czy wszystkie tagi są wypełnione
                                                              /// sprawdz czy wszystkie utwory mają ten sam album w danym folderze 
                        //ListAlbumsPerCatalog.Add(itemMFD.trackAlbum);
                        /// sprawdz czy istnieje okładka albumu

                        checkTagRating(itemMFD, ListCheckRating, ListRatingInformation);
                        //dodaj tracka - full info
                        ListMfdAlbum.Add(itemMFD);
                    }
                }
                /*
                var allAreRatesFilled = ListCheckRating.Distinct().Count() == 1;
                if (allAreRatesFilled == true)
                    GlobalChecker.globalCheckerRating = 1;                
                else
                    GlobalChecker.globalCheckerRating = 0;
                */
                //
                //check if album index is filled in specific directory ~ if all tracks have album index > correct
                //
                int countAlbumIndex = 0;
                publicAlbumIndex = 0;
                //int albumIndex = 0;
                foreach (MusicFileDetails iMFD in ListMfdAlbum)
                {
                    if (iMFD.trackIdAlbumIndex != "" & iMFD.trackIdAlbumIndex != null)
                    {
                        publicAlbumIndex = Convert.ToInt32(iMFD.trackIdAlbumIndex);
                        countAlbumIndex += 1;
                    }
                        

                }
                if (countAlbumIndex == ListMfdAlbum.Count & publicAlbumIndex != 0)
                {
                    DBFunctions db = new DBFunctions();
                    db.UpdateAlbumDirectoryPathByAlbumID(publicAlbumIndex, itemUQC.uniqeDirectory);
                    updateLocalDB(ListTagInformation, ListRatingInformation, ListMfdAlbum, itemUQC, ListCheckTags);
                }
                else
                {
                    updateLocalDB(ListTagInformation, ListRatingInformation, ListMfdAlbum, itemUQC, ListCheckTags);
                }
                    
            }
        }
        static void updateLocalDB(List<TagInformation> LTI, List<RatingInformation> RI, List<MusicFileDetails> LMA, uniqueCatalogs itemUQC, List<int> LCT)
        {
            //////////////////////////////////////////////////////////////[<fill albums]//////////////////////////////////////////////////////////////////
            int idAlbum = 0;
            if (publicAlbumIndex > 0)
                idAlbum = publicAlbumIndex;
            for (int i = 0; i <= 4; i++) // 0 - sprawdzaj tag o typie Album; 1- sprawdzaj tag o typie Artist; 2 - sprawdzaj tag o typie Genre
            {
                List<string> ListItem = new List<string>();
                int AlbumReleaseYear = 0;
                foreach (TagInformation itemTI in LTI)
                {                        
                    switch (i)
                    {
                        case 0:
                            ListItem.Add(itemTI.trackAlbum);
                            break;
                        case 1:
                            ListItem.Add(itemTI.trackArtist);
                            break;
                        case 2:
                            ListItem.Add(itemTI.trackGenre);
                            break;
                        case 3:
                            ListItem.Add(itemTI.trackIndexLib);
                            break;
                        case 4:
                            try
                            {
                                AlbumReleaseYear = Convert.ToInt32(itemTI.pickedAFile.DATE);
                            }
                            catch (Exception)
                            {
                                AlbumReleaseYear = 0;                                
                            }                            
                            break;
                    }
                }

                //var firstElement = ListItem.First(); //[przemy knowledge] pobierz pierwszy element w liście dla : Album / Artist / Genre
                
                //
                //UPDATE OR INSERT Records to SQL - tags: AlbumName, ArtistName, AlbumGenre
                //
                switch (i)
                {
                    case 0: //                             
                        var tagAlbumFilled = ListItem.Distinct().Count() == 1;
                        string firstElementAlbum = ListItem.First();

                        if ((tagAlbumFilled == true) & (firstElementAlbum != ""))
                        {
                            if (publicAlbumIndex == 0) 
                                idAlbum = updateAlbumsTableInDB(1, itemUQC, firstElementAlbum, true, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze jest ok [sekcja album]
                            else
                                updateAlbumsTableInDB(1, itemUQC, firstElementAlbum, true, 0, 0);
                        }
                        else
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(2, itemUQC, "heterogeneous", false, 0, 0);
                            else
                                updateAlbumsTableInDB(2, itemUQC, "heterogeneous", false, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze niepoprawnie [sekcja album]
                        }                            
                        break;

                    case 1:
                        var tagArtistFilled = ListItem.Distinct().Count() == 1;
                        string firstElementArtist = ListItem.First();

                        if ((tagArtistFilled == true) & (firstElementArtist != ""))
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(3, itemUQC, firstElementArtist, true, 0, 0);
                            else
                                updateAlbumsTableInDB(3, itemUQC, firstElementArtist, true, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze jest ok [sekcja Artist]
                            //////////////////////////////////////////////////////////////[<fill Artists]//////////////////////////////////////////////////////////////////
                            updateArtistsTableInDB(firstElementArtist, idAlbum); // first commit
                            //////////////////////////////////////////////////////////////[fill Artists>]//////////////////////////////////////////////////////////////////
                        }
                        else
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(4, itemUQC, "heterogeneous", false, 0, 0);
                            else
                                updateAlbumsTableInDB(4, itemUQC, "heterogeneous", false, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze niepoprawnie [sekcja Artist]
                        }
                        
                        break;

                    case 2:
                        var tagGenreFilled = ListItem.Distinct().Count() == 1;
                        string firstElementGenre = ListItem.First();

                        if ((tagGenreFilled == true) & (firstElementGenre != ""))
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(5, itemUQC, firstElementGenre, true, 0, 0);
                            else
                                updateAlbumsTableInDB(5, itemUQC, firstElementGenre, true, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze jest ok [sekcja Genre]
                        }                            
                        else
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(6, itemUQC, "heterogeneous", false, 0, 0);
                            else
                                updateAlbumsTableInDB(6, itemUQC, "heterogeneous", false, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze niepoprawnie [sekcja Genre]
                        }                            
                        break;
                    case 3:
                        var tagIndexFilled = LCT.Distinct().Count() == 1;
                        if (tagIndexFilled)
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(9, itemUQC, "", true, 0, 0);
                            else
                                updateAlbumsTableInDB(9, itemUQC, "", true, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze jest ok [sekcja Index]
                        }
                        else
                        {
                            if (publicAlbumIndex == 0)
                                idAlbum = updateAlbumsTableInDB(10, itemUQC, "", false, 0, 0);
                            else
                                updateAlbumsTableInDB(10, itemUQC, "", false, 0, 0);//odnajdz rekord w bazie > zaktualizuj status ze jest niepoprawnie [sekcja Index]
                        }

                        break;
                    case 4:
                        if (publicAlbumIndex == 0)
                            idAlbum = updateAlbumsTableInDB(11, itemUQC, "", false, 0, AlbumReleaseYear);
                        else
                            updateAlbumsTableInDB(11, itemUQC, "", false, 0, AlbumReleaseYear);//odnajdz rekord w bazie > zaktualizuj status ze jest ok [sekcja Index]
                        break;
                }
                    
            }
            //
            //update rating section
            //
            int RatingExist = 0;
            decimal AlbumStarSum = 0;
            int AlbumStarExist = 0;

            foreach (RatingInformation itemRI in RI)
            {
                if(itemRI.trackRating > 0)
                {
                    RatingExist += 1;   
                    if (itemRI.trackRating > 1)
                    {
                        AlbumStarSum += itemRI.trackRating;
                        AlbumStarExist += 1;
                    }
                }
            }
            if (RI.Count == RatingExist)
            {
                decimal CalcAlbumRate = 0;
                try
                {
                    CalcAlbumRate = Math.Round(AlbumStarSum / AlbumStarExist,2);
                }
                catch (DivideByZeroException e)
                {

                }
                string AlbumRateCounter = RatingExist.ToString() + "/" + RI.Count.ToString();
                if (publicAlbumIndex == 0)
                    idAlbum = updateAlbumsTableInDB(7, itemUQC, AlbumRateCounter, true, CalcAlbumRate, 0);
                else
                    updateAlbumsTableInDB(7, itemUQC, AlbumRateCounter, true, CalcAlbumRate, 0);
            }                
            else
            {
                string AlbumRateCounter = RatingExist.ToString() + "/" + RI.Count.ToString();
                if (publicAlbumIndex == 0)
                    idAlbum = updateAlbumsTableInDB(8, itemUQC, AlbumRateCounter, false, 0, 0);
                else
                    updateAlbumsTableInDB(8, itemUQC, AlbumRateCounter, false, 0, 0);
            }
            //////////////////////////////////////////////////////////////[fill albums>]//////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////[<fill tracks]//////////////////////////////////////////////////////////////////
            updateTracksTableInDB(LMA, idAlbum);
            //////////////////////////////////////////////////////////////[fill tracks>]//////////////////////////////////////////////////////////////////
        }
        
        static int updateAlbumsTableInDB(int updateCase, uniqueCatalogs itemUQC, string updateStringDB, bool checkValue, decimal updateDecimalDB, int updateIntDB)
        {
            List<SQLAlbumTable> DirectoryList = new List<SQLAlbumTable>(); //sqlprzemy
            DBFunctions db = new DBFunctions();
            DirectoryList = db.GetAlbumDirectory(itemUQC.uniqeDirectory);
            int DLcount = DirectoryList.Count;
            if (DLcount == 1) //exist in sql => update [todo] sprawdzaj po INDEXALBUM jeżeli jest uzupełnione, trzeba aktualizować po id Album [problem ścieżki w pliku]
                //pobierz ID
                //pobierz 
            {
                int idAlbum = updateAlbumsSwitch(db, updateCase, itemUQC, updateStringDB, checkValue, updateDecimalDB, updateIntDB);
                return idAlbum;
            }
            else
            {
                if (DLcount == 0)
                {
                    db.InsertAlbum("<albumName>", itemUQC.uniqeDirectory, "<artistName>", 0, "<genreName>"); //string AlbumNamee, string AlbumDir, string ArtistName, int releaseYear, string AlbumGenre
                    int idAlbum = updateAlbumsSwitch(db, updateCase, itemUQC, updateStringDB, checkValue, updateDecimalDB, updateIntDB);
                    return idAlbum;
                }
                else
                    MessageBox.Show($"Directory Duplicates in database - fatal error. \n Directory: {itemUQC.uniqeDirectory}");
                    return 0;
            }           

        }
        static int updateAlbumsSwitch(DBFunctions db, int updateCase, uniqueCatalogs itemUQC, string updateStringDB, bool checkValue, decimal updateDecimalDB, int updateIntDB)
        {
            int AlbumID = 0;
            switch (updateCase)
            {
                case 1:
                case 2:
                    AlbumID = db.UpdateAlbumCheck(itemUQC.uniqeDirectory, checkValue, updateStringDB);
                    return AlbumID;
                case 3:
                case 4:
                    AlbumID = db.UpdateArtistCheck(itemUQC.uniqeDirectory, checkValue, updateStringDB);
                    return AlbumID;
                case 5:
                case 6:
                    AlbumID = db.UpdateGenreCheck(itemUQC.uniqeDirectory, checkValue, updateStringDB);
                    return AlbumID;
                case 7:
                case 8:
                    AlbumID = db.UpdateRatingCheck(itemUQC.uniqeDirectory, checkValue, updateDecimalDB, updateStringDB);
                    return AlbumID;
                case 9:
                case 10:
                    AlbumID = db.UpdateIndexCheck(itemUQC.uniqeDirectory, checkValue);
                    return AlbumID;
                case 11:
                    AlbumID = db.UpdateAlbumReleaseYear(itemUQC.uniqeDirectory, updateIntDB);
                    return AlbumID;
            }
            return 0;
        }
        static void updateTracksTableInDB(List<MusicFileDetails> LMA, int AlbumID)
        {
            foreach (MusicFileDetails itemLMA in LMA) // LMA = ListMfdAlbum
            {
                ///////////////////////////////////[tylko jeżeli jest index]/////////////////////////////
                int indexLib = 0;
                if (Int32.TryParse(itemLMA.trackIndex, out indexLib))
                    indexLib = Int32.Parse(itemLMA.trackIndex);

                int convertedTrackRating = 0;
                if (Int32.TryParse(itemLMA.trackRating, out convertedTrackRating))
                    convertedTrackRating = Int32.Parse(itemLMA.trackRating);
                if (indexLib > 0)
                {
                    //szukaj w bazie, jezeli nie ma - dodaj
                    string FileStatus = "EXIST";

                    List<SQLTrackTable> ListIndexLib = new List<SQLTrackTable>(); //sqlprzemy
                    DBFunctions db = new DBFunctions();
                    ListIndexLib = db.GetTrackByTrackIndex(indexLib);

                    if (ListIndexLib.Count == 1)
                    {
                        string trackNameWithoutExtension = "";

                        if (itemLMA.trackName == "")
                        {
                            trackNameWithoutExtension = Path.GetFileNameWithoutExtension(itemLMA.trackAudioPath);                            
                        }
                        else
                            trackNameWithoutExtension = itemLMA.trackName;

                        db.UpdateTrackByIndexLib(AlbumID, itemLMA.trackAudioPath, itemLMA.trackGenre, trackNameWithoutExtension, itemLMA.trackArtist, convertedTrackRating, itemLMA.trackFileExtension, indexLib, FileStatus);
                    }                            
                    if (ListIndexLib.Count == 0)
                        db.InsertTrack(AlbumID, itemLMA.trackAudioPath, itemLMA.trackGenre, itemLMA.trackName, itemLMA.trackArtist, convertedTrackRating, itemLMA.trackFileExtension, indexLib, FileStatus);
                    if (ListIndexLib.Count > 1)
                        MessageBox.Show("index duplicates!");
                }
                
            }
        }
        static string countRatedAlbums()
        {
            return "3/3";
        }
        static void updateArtistsTableInDB(string ArtistName, int AlbumID)
        {
            if (ArtistName != "")
            {
                List<SQLArtistTable> ListArtistLib = new List<SQLArtistTable>(); //sqlprzemy
                DBFunctions db = new DBFunctions();
                ListArtistLib = db.GetAllByArtists(ArtistName);
                if (ListArtistLib.Count == 1)
                {
                    //update artist
                    decimal VarDecimal = 0;
                    try
                    {
                        //CalcAlbumRate = Math.Round(AlbumStarSum / AlbumStarExist, 2);
                        db.UpdateRatedAlbums(ArtistName, countRatedAlbums());
                    }
                    catch (DivideByZeroException e)
                    {

                    }
                }
                else if (ListArtistLib.Count == 0)
                {
                    decimal VarDecimal = 0;
                    
                    db.InsertArtist(ArtistName, "0/0", "0/0", VarDecimal, VarDecimal, 0, 0);
                }
                else if (ListArtistLib.Count > 1)
                {
                    MessageBox.Show("artist duplicates!");
                }
            }
            
        }
        static void checkTagRating(MusicFileDetails MFD, List<int> ListCheckRating, List<RatingInformation>  ListRatingInformation)
        {
            if (MFD.trackRating == "" || MFD.trackRating == "0") 
            {
                RatingInformation RI = new RatingInformation();

                RI.trackDirectory = MFD.trackDirectory;
                GlobalRating.globalhowManyUnrate += 1;

                ListCheckRating.Add(0);
                ListRatingInformation.Add(RI);
            }
            else
            {
                RatingInformation RI = new RatingInformation();

                RI.trackDirectory = MFD.trackDirectory;
                GlobalRating.globalhowManyRate += 1;

                switch  (MFD.trackRating)
                {
                    case "1":
                        RI.trackRating = 1;
                        ListCheckRating.Add(1);
                        break;
                    case "2":
                        RI.trackRating = 2;
                        ListCheckRating.Add(1);
                        break;
                    case "3":
                        RI.trackRating = 3;
                        ListCheckRating.Add(1);
                        break;
                    case "4":
                        RI.trackRating = 4;
                        ListCheckRating.Add(1);
                        break;
                    case "5":
                        RI.trackRating = 5;                        
                        ListCheckRating.Add(1);
                        break;
                }
                ListRatingInformation.Add(RI);
            }
                
        }
        static void checkTags(MusicFileDetails MFD, List<int> ListCheckTags, List<TagInformation> ListTagInformation) 
        ///sprawdz czy wszystkie tagi są wypełnione
        {
            TagInformation TI = new TagInformation();

            if (MFD.trackAlbum == "" || MFD.trackGenre == "" || MFD.trackName == "" || MFD.trackArtist == "" || MFD.trackIndex == "")
            {
                globalBoxListConsole.Add(MFD.trackName + " error tag");
                ListCheckTags.Add(0);
            }
            else
            {
                //DateTime dateToday = DateTime.Today;
                //MFD.pickedAFile.MODTAGDATE = dateToday.ToString("d");
                
                
                ListCheckTags.Add(1);  
            }
            TI.trackAlbum = MFD.trackAlbum;
            TI.trackArtist = MFD.trackArtist;
            TI.trackGenre = MFD.trackGenre;
            TI.trackName = MFD.trackName;
            TI.trackIndexLib = MFD.trackIndex;
            TI.pickedAFile = MFD.pickedAFile;

            ListTagInformation.Add(TI);

        }
        static void checkAlbumInCatalog(MusicFileDetails MFD, uniqueCatalogs UQC)
        {
            // do wyjebania funkcja
            foreach (MusicFileDetails itemMFD in (IEnumerable<MusicFileDetails>)MFD) //[przemy knowledge  - zastosowanie enumerable]
            {                
                //[przemy todo] sprawdzaj czy pierwszy element nie = ""
                if (itemMFD.trackAlbum.Any(o => o != itemMFD.trackAlbum[0])) //[knowledge] Linqowe sprawdzanie czy elementy w liscie są takie same
                {
                    GlobalChecker checkerAlbums = new GlobalChecker();
                    GlobalChecker.globalCheckerTags = 1;
                }
                //MFD.pickedAFile.ALBUM
            }
            //if (MFD.trackAlbum )
            
        } //TO DELETE

        static int ReadTags(string Filename, LazySW SW)
        {
            AudioFile AFile = null;
            
            if (Helpers.JGetExtension(Filename) == "FLAC")
            {
                AFile = new FLACFile(Filename, false); // Read Only = true

                string onlyDir = Path.GetDirectoryName(AFile.AudioPath);
                string TrackTitle = AFile.TITLE; //track NAME
                string TrackRating = AFile.RATING;
                string TrackModTagDate = AFile.MODTAGDATE;
                string TrackArtist = AFile.ARTIST;
                string TrackAlbum = AFile.ALBUM;
                string TrackGenre = AFile.GENRE;
                string TrackIndex = AFile.INDEXTRACK;
                string TrackAudioPath = AFile.AudioPath;
                string TrackExtension = "FLAC";
                string TrackAlbumID = AFile.INDEXALBUM;
                
                //update class
                //AFile.
                MusicFileDetails MFD = new MusicFileDetails(); // deklaruj klase
                MFD.trackDirectory = onlyDir;
                MFD.trackAudioPath = TrackAudioPath;
                MFD.trackName = TrackTitle;
                MFD.trackArtist = TrackArtist;
                MFD.trackAlbum = TrackAlbum;
                MFD.trackRating = TrackRating;
                MFD.trackModDateTag = TrackModTagDate;
                MFD.trackGenre = TrackGenre;
                MFD.trackIndex = TrackIndex;
                MFD.trackFileExtension = TrackExtension;                
                MFD.trackIdAlbumIndex = TrackAlbumID;

                MFD.pickedAFile = AFile;

                globalMusicFileDetailsList.Add(MFD); 
            }
                

            return 0;
        }
        public static void QuickRead(string FilePath, MusicFileDetails MFD)
        {
            AudioFile AFile = null;

            if (Helpers.JGetExtension(FilePath) == "FLAC")
            {
                AFile = new FLACFile(FilePath, false); // Read Only = true

                string onlyDir = Path.GetDirectoryName(AFile.AudioPath);
                string TrackTitle = AFile.TITLE; //track NAME
                string TrackRating = AFile.RATING;
                string TrackModTagDate = AFile.MODTAGDATE;
                string TrackArtist = AFile.ARTIST;
                string TrackAlbum = AFile.ALBUM;
                string TrackGenre = AFile.GENRE;
                string TrackIndex = AFile.INDEXTRACK;
                string TrackAudioPath = AFile.AudioPath;
                string TrackExtension = "FLAC";

                //update class
                //AFile.
               
                MFD.trackDirectory = onlyDir;
                MFD.trackAudioPath = TrackAudioPath;
                MFD.trackName = TrackTitle;
                MFD.trackArtist = TrackArtist;
                MFD.trackAlbum = TrackAlbum;
                MFD.trackRating = TrackRating;
                MFD.trackModDateTag = TrackModTagDate;
                MFD.trackGenre = TrackGenre;
                MFD.trackIndex = TrackIndex;
                MFD.trackFileExtension = TrackExtension;
                MFD.pickedAFile = AFile;               
                
            }
        }
        static int ChangeModDate(string Filename, LazySW SW)
        {
            AudioFile AFile = null;

            // Open with ReadOnly as false - we are changing the files
            if (Helpers.JGetExtension(Filename) == "FLAC")
                AFile = new FLACFile(Filename, false);
            if (Helpers.JGetExtension(Filename) == "MP3")
                AFile = new MP3File(Filename, false);

            DateTime dateToday = DateTime.Today;

            AFile.MODTAGDATE = dateToday.ToString("d");
            AFile.Save(true);
            
            return 1;
        }
        static int ChangeTrackIndex(string Filename, LazySW SW)
        {
            AudioFile AFile = null;

            // Open with ReadOnly as false - we are changing the files
            if (Helpers.JGetExtension(Filename) == "FLAC")
                AFile = new FLACFile(Filename, false);
            if (Helpers.JGetExtension(Filename) == "MP3")
                AFile = new MP3File(Filename, false);

            // tu moze sie wypierdalac, bo nic nie sprawdza pliku czy jest flackiem
            List<SQLSeriesNoTable> queryGetLastIndex = new List<SQLSeriesNoTable>();
            DBFunctions db = new DBFunctions();
            queryGetLastIndex = db.GetSeriesLastIndex(10000);
            foreach (SQLSeriesNoTable TrackSeriesNo in queryGetLastIndex)
            {
                int NextIndexTrack = TrackSeriesNo.IndexNo + 1;
                AFile.INDEXTRACK = NextIndexTrack.ToString();
                AFile.Save(true);
                //save IndexNo in SQL
                db.UpdateIndexTrackInSeriesNo(10000, NextIndexTrack);
            }
            
            

            return 1;
        }
        


        
      
    }


    
}
