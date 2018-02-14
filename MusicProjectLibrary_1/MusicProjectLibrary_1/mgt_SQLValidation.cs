using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// przemy PC - DESKTOP-FKSTQC7\SQLEXPRESS
// work PC - DESKTOP-FS4HD9A\SQLEXPRESS
namespace MusicProjectLibrary_1
{
    class mgt_SQLValidation
    {
        public static List<string> ErrorPicker = new List<string>();

        public class dataGridColumns
        {
            /////////////////////////////////////
            public int expectedPoints = 9;
            /////////////////////////////////////
            public int colIndexAlbum = 0;
            public int colWriteIndex = 1;
            public int colAbumReleaseYear = 2;
            public int colAlbumDirectory = 3;
            public int colIdArtist = 4;
            public int colArtistName = 5;
            public int colAlbumName = 6;
            public int colAlbumGeneralGenre = 7;
            public int colDirectoryGenre = 9;
            public int colAlbumNumericRating = 13;
            public int colArtistCheck = 14;
            public int colAlbumCheck = 15;
            public int colGenreCheck = 16;
            public int colRatingCheck = 17;
            public int colIndexCheck = 18;
            public int colIndexAlbumCheck = 19;
        }
        public class ArtistDataGridColumns
        {
            public int colIdArtist = 0;
            public int colArtistName = 1;
            public int AlbumCount = 2;
            public int ProceedToTotal = 3;
            public int ArtistLibraryPercent = 4;
            public int ProceedPercent = 5;
            public int RatedSongs = 6;
            public int GeneralRanking = 7;
        }
        public static void ReadDataGrid(DataGridView DGV, ListBox.ObjectCollection boxListConsole, TextBox tbxPurgPath, TextBox tbxGeneralPath, int AlbumRowIndex)
        {
            ErrorPicker.Clear();
            dataGridColumns DGC = new dataGridColumns();
     
            int validationPoints = 0;            

            int CurrentIndex = 0;
            string FolderGenre = "";
            string ArtistName = "";
            string AlbumName = "";
            string AlbumDirectory = "";

            string GridValueString = "";
            int GridValueInt = 0;
            bool GridValueBool = false;
            bool ratingPositive = false;

            for (int rows = 0; rows < DGV.Rows.Count; rows++)
            {
                //validate errors when record is selected on Data Grid View
                if (DGV.Rows[rows].Selected)
                {
                    for (int col = 0; col < DGV.Rows[rows].Cells.Count; col++)
                    {
                        if (col == DGC.colIndexAlbum)
                        {
                            CurrentIndex = Convert.ToInt32(DGV.Rows[rows].Cells[col].Value);
                        }
                        else if (col == DGC.colAbumReleaseYear)
                        {
                            try
                            {
                                GridValueInt = Convert.ToInt32(DGV.Rows[rows].Cells[col].Value);
                            }
                            catch (Exception e)
                            {
                                GridValueInt = 0;
                            }
                            if (GridValueInt != 0)
                                validationPoints += 1;
                            else
                                ErrorPicker.Add($"error [no Album Release Year]: Album ID: {CurrentIndex}");
                        }
                        else if (col == DGC.colAlbumDirectory) // check if exist
                        {
                            try
                            {
                                GridValueString = DGV.Rows[rows].Cells[col].Value.ToString();
                            }
                            catch (NullReferenceException e)
                            {
                                GridValueString = "";
                            }
                            if (GridValueString != "" || Directory.Exists(GridValueString))
                            {
                                validationPoints += 1;
                                AlbumDirectory = GridValueString;
                            }
                            else
                                ErrorPicker.Add($"error [FAILED to find PATH]: Album ID: {CurrentIndex}");
                        }
                        else if (col == DGC.colDirectoryGenre) // check if directory Genre is filled
                        {
                            
                            try
                            {
                                GridValueString = DGV.Rows[rows].Cells[col].Value.ToString();
                            }                            
                            catch (NullReferenceException e)
                            {
                                GridValueString = "";  
                            }

                            if (GridValueString != "")
                            {
                                validationPoints += 1;
                                FolderGenre = GridValueString;
                            }                                    
                            else
                                ErrorPicker.Add($"error [no Directory Genre declared]: Album ID: {CurrentIndex}");
                        }
                        else if (col == DGC.colArtistCheck || col == DGC.colAlbumCheck || col == DGC.colGenreCheck || col == DGC.colRatingCheck || col == DGC.colIndexCheck || col == DGC.colIndexAlbumCheck)
                        {
                            try
                            {
                                GridValueBool = Convert.ToBoolean(DGV.Rows[rows].Cells[col].Value);
                            }
                            catch (Exception e)
                            {
                                GridValueBool = false;
                            }
                            if (GridValueBool != false)
                            {
                                if (col == DGC.colArtistCheck)
                                {
                                    ArtistName = DGV.Rows[rows].Cells[DGC.colArtistName].Value.ToString();
                                    validationPoints += 1;
                                }
                                else if (col == DGC.colAlbumCheck)
                                {
                                    AlbumName = DGV.Rows[rows].Cells[DGC.colAlbumName].Value.ToString();
                                    validationPoints += 1;
                                }
                                else if (col == DGC.colIndexAlbumCheck)
                                    validationPoints += 1;
                                else if (col == DGC.colRatingCheck)
                                {       
                                    decimal albumNumericRate = Convert.ToDecimal(DGV.Rows[rows].Cells[DGC.colAlbumNumericRating].Value);
                                    if (albumNumericRate > 0)
                                        ratingPositive = true;

                                    validationPoints += 1;
                                }
                                else
                                    validationPoints += 1;
                            }
                                
                            else
                            {
                                if (col == DGC.colArtistCheck)
                                    ErrorPicker.Add($"error [ArtistCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == DGC.colAlbumCheck)
                                    ErrorPicker.Add($"error [AlbumCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == DGC.colGenreCheck)
                                    ErrorPicker.Add($"error [GenreCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == DGC.colRatingCheck)
                                    ErrorPicker.Add($"error [RatingCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == DGC.colIndexCheck)
                                    ErrorPicker.Add($"error [IndexCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == DGC.colIndexAlbum)
                                    ErrorPicker.Add($"error [IndexAlbumCheck test FAILED]: Album ID: {CurrentIndex}");
                            }                                
                        }    
                    }
                }
            }
            //throw errors
            foreach (string itemError in ErrorPicker)
            {
                boxListConsole.Add(itemError);                
            }
            //
            //[stage two]
            //
            //add validation points to SQL -> used for album find
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            db.UpdateAlbumValidationPointsByAlbumID(CurrentIndex, validationPoints);
            if (validationPoints == DGC.expectedPoints & ratingPositive)
            {
                //tworz foldery gatunkowe - jeżeli ich nie ma na dysku docelowym
                
                //przepchnij pliki z purga do folderu utworzonego
                if (Directory.Exists(tbxGeneralPath.Text))
                {
                    string buildPath = tbxGeneralPath.Text + @"\" + FolderGenre;
                    if (Directory.Exists(buildPath))
                    {                        
                        boxListConsole.Add($"folder exist in General Path: " + buildPath);
                        mgt_Directory.CreateDirectoryForAlbum(buildPath, ArtistName, AlbumName, boxListConsole, tbxPurgPath, tbxGeneralPath, CurrentIndex, AlbumDirectory, AlbumRowIndex);
                    }
                    else
                    {                        
                        boxListConsole.Add($"...creating new folder in General Path: {buildPath}");
                        Directory.CreateDirectory(buildPath);
                        mgt_Directory.CreateDirectoryForAlbum(buildPath, ArtistName, AlbumName, boxListConsole, tbxPurgPath, tbxGeneralPath, CurrentIndex, AlbumDirectory, AlbumRowIndex);
                    }
                    //pod gatunkami tworz foldery artystów - sprawdź czy istnieją
                    //ARTIST_AlbumName

                }
                else
                {
                   
                }
            }
            //delete Album Folder from Purgatory and Tracks, keep information about deleted album & tracks in Database
            else if (validationPoints == DGC.expectedPoints & !ratingPositive) 
            {
                List<SQLTrackTable> LSQTT = new List<SQLTrackTable>();
                //mgt_SQLDatabase db = new mgt_SQLDatabase();
                LSQTT = db.GetTrackByAlbumId(CurrentIndex);

                DateTime dateNow = DateTime.Now;
                string dateString = String.Format("{0:MM/dd/yyyy}", dateNow);
                string primaryPath = "";
                foreach (SQLTrackTable item in LSQTT)
                {
                    string primarySongPath = item.TrackDirectory;                    
                    primarySongPath = Functions.findProhibitedSigns(primarySongPath);
                    mgt_Tracks.deleteOneStars(primarySongPath, dateString, boxListConsole);
                    primaryPath = Path.GetDirectoryName(primarySongPath);
                }
                Functions.FindFilesInTreeDirectory(true, primaryPath, "", boxListConsole);
                db.UpdateAlbumDirectoryPathByAlbumID(CurrentIndex, "deleted");
                db.UpdateAlbumProceedDate(CurrentIndex, dateString, true);
            }
            else
                boxListConsole.Add($"=========[Test Failed IDX: {CurrentIndex}]=========");
        }

        public static void bw_ReadDataGridForAll(object sender, DataGridView DGV, ListBox.ObjectCollection boxListConsole) //DoWorkEventArgs e
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            ReadDataGridForAll(DGV, boxListConsole);

        }
        public static void ReadDataGridForAll(DataGridView DGV, ListBox.ObjectCollection boxListConsole)
        {
            ErrorPicker.Clear();
            dataGridColumns DGC = new dataGridColumns();

            int validationPoints = 0;
            int CurrentIndex = 0;
            string FolderGenre = "";
            string ArtistName = "";
            string AlbumName = "";
            string AlbumDirectory = "";

            string GridValueString = "";
            int GridValueInt = 0;
            bool GridValueBool = false;

            for (int rows = 0; rows < DGV.Rows.Count; rows++)
            {
                validationPoints = 0;
                bool withoutGenre = false;
                for (int col = 0; col < DGV.Rows[rows].Cells.Count; col++)
                {
                    if (col == DGC.colIndexAlbum)
                    {
                        CurrentIndex = Convert.ToInt32(DGV.Rows[rows].Cells[col].Value);
                    }
                    else if (col == DGC.colAbumReleaseYear)
                    {
                        try
                        {
                            GridValueInt = Convert.ToInt32(DGV.Rows[rows].Cells[col].Value);
                        }
                        catch (Exception e)
                        {
                            GridValueInt = 0;
                        }
                        if (GridValueInt != 0)
                            validationPoints += 1;                        
                    }
                    else if (col == DGC.colAlbumDirectory) // check if exist
                    {
                        try
                        {
                            GridValueString = DGV.Rows[rows].Cells[col].Value.ToString();
                        }
                        catch (NullReferenceException e)
                        {
                            GridValueString = "";
                        }
                        if (GridValueString != "" || Directory.Exists(GridValueString))
                        {
                            validationPoints += 1;
                            AlbumDirectory = GridValueString;
                        }                        
                    }
                    else if (col == DGC.colDirectoryGenre) // check if directory Genre is filled
                    {

                        try
                        {
                            GridValueString = DGV.Rows[rows].Cells[col].Value.ToString();
                        }
                        catch (NullReferenceException e)
                        {
                            GridValueString = "";
                        }

                        if (GridValueString != "")
                        {
                            validationPoints += 1;
                            FolderGenre = GridValueString;
                        }
                        else
                        {
                            withoutGenre = true;
                        }  
                    }
                    else if (col == DGC.colArtistCheck || col == DGC.colAlbumCheck || col == DGC.colGenreCheck || col == DGC.colRatingCheck || col == DGC.colIndexCheck || col == DGC.colIndexAlbumCheck)
                    {
                        try
                        {
                            GridValueBool = Convert.ToBoolean(DGV.Rows[rows].Cells[col].Value);
                        }
                        catch (Exception e)
                        {
                            GridValueBool = false;
                        }
                        if (GridValueBool != false)
                        {
                            if (col == DGC.colArtistCheck)
                            {
                                ArtistName = DGV.Rows[rows].Cells[DGC.colArtistName].Value.ToString();
                                validationPoints += 1;
                            }
                            else if (col == DGC.colAlbumCheck)
                            {
                                AlbumName = DGV.Rows[rows].Cells[DGC.colAlbumName].Value.ToString();
                                validationPoints += 1;
                            }
                            else if (col == DGC.colRatingCheck)
                                validationPoints += 1;
                            else if (col == DGC.colGenreCheck)
                                validationPoints += 1;
                            else if (col == DGC.colIndexCheck)
                                validationPoints += 1;
                            else if (col == DGC.colIndexAlbumCheck)
                                validationPoints += 1;

                        }                           
                    }
                }
                //add validation points to SQL -> used for album find
                if (GlobalVariables.writeValidationPoints)
                {
                    mgt_SQLDatabase db = new mgt_SQLDatabase();
                    db.UpdateAlbumValidationPointsByAlbumID(CurrentIndex, validationPoints);
                }                

                if (validationPoints == DGC.expectedPoints)
                {
                    DGV.Rows[rows].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if(withoutGenre & validationPoints == DGC.expectedPoints - 1)
                {
                    DGV.Rows[rows].DefaultCellStyle.BackColor = Color.PeachPuff;
                }
                //else
                    //boxListConsole.Add($"=========[Test Failed IDX: {CurrentIndex}]=========");
            }            
        }
    }
}
