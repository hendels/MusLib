using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class SQLDataValidate
    {
        public static List<string> ErrorPicker = new List<string>();
        public static void ReadDataGrid(DataGridView DGV, ListBox.ObjectCollection boxListConsole, TextBox tbxPurgPath, TextBox tbxGeneralPath, int AlbumRowIndex)
        {
            ErrorPicker.Clear();
            /////////////////////////////////////
            int expectedPoints = 8;
            /////////////////////////////////////
            int colIndexAlbum = 0;
            int colAbumReleaseYear = 1;
            int colAlbumDirectory = 2;
            int colArtistName = 3;
            int colAlbumName = 4;
            int colDirectoryGenre = 7;
            int colArtistCheck = 10;
            int colAlbumCheck = 11;
            int colGenreCheck = 12;
            int colRatingCheck = 13;
            int colIndexCheck = 14;

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
                //validate errors when record is selected on Data Grid View
                if (DGV.Rows[rows].Selected)
                {
                    for (int col = 0; col < DGV.Rows[rows].Cells.Count; col++)
                    {
                        if (col == colIndexAlbum)
                        {
                            CurrentIndex = Convert.ToInt32(DGV.Rows[rows].Cells[col].Value);
                        }
                        else if (col == colAbumReleaseYear)
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
                        else if (col == colAlbumDirectory) // check if exist
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
                        else if (col == colDirectoryGenre) // check if directory Genre is filled
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
                        else if (col == colArtistCheck || col == colAlbumCheck || col == colGenreCheck || col == colRatingCheck || col == colIndexCheck)
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
                                if (col == colArtistCheck)
                                {
                                    ArtistName = DGV.Rows[rows].Cells[colArtistName].Value.ToString();
                                    validationPoints += 1;
                                }
                                else if (col == colAlbumCheck)
                                {
                                    AlbumName = DGV.Rows[rows].Cells[colAlbumName].Value.ToString();
                                    validationPoints += 1;
                                }
                                else
                                    validationPoints += 1;
                            }
                                
                            else
                            {
                                if (col == colArtistCheck)
                                    ErrorPicker.Add($"error [ArtistCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colAlbumCheck)
                                    ErrorPicker.Add($"error [AlbumCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colGenreCheck)
                                    ErrorPicker.Add($"error [GenreCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colRatingCheck)
                                    ErrorPicker.Add($"error [RatingCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colIndexCheck)
                                    ErrorPicker.Add($"error [IndexCheck test FAILED]: Album ID: {CurrentIndex}");
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
            if( validationPoints == expectedPoints)
            {
                //tworz foldery gatunkowe - jeżeli ich nie ma na dysku docelowym
                
                //przepchnij pliki z purga do folderu utworzonego
                if (Directory.Exists(tbxGeneralPath.Text))
                {
                    string buildPath = tbxGeneralPath.Text + @"\" + FolderGenre;
                    if (Directory.Exists(buildPath))
                    {                        
                        boxListConsole.Add($"folder exist in General Path: " + buildPath);
                        DirectoryManagement.CreateDirectoryForAlbum(buildPath, ArtistName, AlbumName, boxListConsole, tbxPurgPath, tbxGeneralPath, CurrentIndex, AlbumDirectory, AlbumRowIndex);
                    }
                    else
                    {                        
                        boxListConsole.Add($"...creating new folder in General Path: {buildPath}");
                        Directory.CreateDirectory(buildPath);
                        DirectoryManagement.CreateDirectoryForAlbum(buildPath, ArtistName, AlbumName, boxListConsole, tbxPurgPath, tbxGeneralPath, CurrentIndex, AlbumDirectory, AlbumRowIndex);
                    }
                    //pod gatunkami tworz foldery artystów - sprawdź czy istnieją
                    //ARTIST_AlbumName

                }
                else
                {
                    
                }
            }
            else
                boxListConsole.Add($"=========[Test Failed IDX: {CurrentIndex}]=========");
        }
        public static void ReadDataGridForAll(DataGridView DGV, ListBox.ObjectCollection boxListConsole, TextBox tbxPurgPath, TextBox tbxGeneralPath, int AlbumRowIndex)
        {
            ErrorPicker.Clear();
            /////////////////////////////////////
            int expectedPoints = 8;
            /////////////////////////////////////
            int colIndexAlbum = 0;
            int colAbumReleaseYear = 1;
            int colAlbumDirectory = 2;
            int colArtistName = 3;
            int colAlbumName = 4;
            int colDirectoryGenre = 7;
            int colArtistCheck = 10;
            int colAlbumCheck = 11;
            int colGenreCheck = 12;
            int colRatingCheck = 13;
            int colIndexCheck = 14;

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
                    if (col == colIndexAlbum)
                    {
                        CurrentIndex = Convert.ToInt32(DGV.Rows[rows].Cells[col].Value);
                    }
                    else if (col == colAbumReleaseYear)
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
                        //else
                            //ErrorPicker.Add($"error [no Album Release Year]: Album ID: {CurrentIndex}");
                    }
                    else if (col == colAlbumDirectory) // check if exist
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
                        //else
                            //ErrorPicker.Add($"error [FAILED to find PATH]: Album ID: {CurrentIndex}");
                    }
                    else if (col == colDirectoryGenre) // check if directory Genre is filled
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
                            
                        //else
                            //ErrorPicker.Add($"error [no Directory Genre declared]: Album ID: {CurrentIndex}");
                    }
                    else if (col == colArtistCheck || col == colAlbumCheck || col == colGenreCheck || col == colRatingCheck || col == colIndexCheck)
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
                            if (col == colArtistCheck)
                            {
                                ArtistName = DGV.Rows[rows].Cells[colArtistName].Value.ToString();
                                validationPoints += 1;
                            }
                            else if (col == colAlbumCheck)
                            {
                                AlbumName = DGV.Rows[rows].Cells[colAlbumName].Value.ToString();
                                validationPoints += 1;
                            }
                            else if (col == colRatingCheck)
                                validationPoints += 1;
                            else if (col == colGenreCheck)
                                validationPoints += 1;
                            else if (col == colIndexCheck)
                                validationPoints += 1;

                        }
                            /*
                            else
                            {
                                if (col == colArtistCheck)
                                    ErrorPicker.Add($"error [ArtistCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colAlbumCheck)
                                    ErrorPicker.Add($"error [AlbumCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colGenreCheck)
                                    ErrorPicker.Add($"error [GenreCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colRatingCheck)
                                    ErrorPicker.Add($"error [RatingCheck test FAILED]: Album ID: {CurrentIndex}");
                                if (col == colIndexCheck)
                                    ErrorPicker.Add($"error [IndexCheck test FAILED]: Album ID: {CurrentIndex}");
                            }
                            */
                        }
                    }
                if (validationPoints == expectedPoints)
                {
                    DGV.Rows[rows].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if(withoutGenre & validationPoints == expectedPoints - 1)
                {
                    DGV.Rows[rows].DefaultCellStyle.BackColor = Color.PeachPuff;
                }
                else
                    boxListConsole.Add($"=========[Test Failed IDX: {CurrentIndex}]=========");
            }
            
            //[stage two]
            //
            
        }
    }
}
