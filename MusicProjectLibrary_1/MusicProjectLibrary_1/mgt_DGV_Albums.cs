using MusicProjectLibrary_1.AppForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class mgt_DGV_Albums
    {
        public static bool DoubleClickOnGridCallBack(DataGridView DGV, ListBox boxListConsole, int AlbumRowIndex, int AlbumColIndex)
        {
            DGV.MultiSelect = false;
            mgt_SQLValidation.dataGridColumns DGC = new mgt_SQLValidation.dataGridColumns();

            //int AlbumColIndex = DGV.CurrentCell.ColumnIndex; // [przemy knowledge - select column in data grid view]
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
            else if (AlbumColIndex == DGC.colDirectoryGenre) // show pick Genre Form
            {
                PickGenre pickGenreForm = new PickGenre();
                string ArtistGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colArtistName].Value.ToString();
                GlobalVariables.SelectedArtist = ArtistGrid;
                pickGenreForm.ShowDialog();
                if (pickGenreForm.GeneratedGenreString != "")
                {
                    int countRecord = mgt_SQLDatabase.AutoSearchDatabaseAlbums(1, DGV, 1, 0, 0, 0, false, false);
                    boxListConsole.Items.Add("Album table updated: " + countRecord.ToString());
                    boxListConsole.SelectedIndex = boxListConsole.Items.Count - 1;
                    return true;
                }

            }
            else if (AlbumColIndex == DGC.colAlbumGeneralGenre)
            {

                string ArtistGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colArtistName].Value.ToString();
                string ReleaseGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colAlbumName].Value.ToString();
                int AlbumID = Convert.ToInt32(DGV.Rows[AlbumRowIndex].Cells[DGC.colIndexAlbum].Value);

                PickAlbumGeneralGenre pickAlbumGenerealGenreForm = new PickAlbumGeneralGenre();
                pickAlbumGenerealGenreForm.ShowDialog();
                if (pickAlbumGenerealGenreForm.selectedDiscogs)
                {
                    mgt_Discogs.startDiscogs(1, boxListConsole.Items, ArtistGrid, ReleaseGrid, AlbumID);
                }

                return true;

            }
            else if (AlbumColIndex == DGC.colAbumReleaseYear)
            {
                string ArtistGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colArtistName].Value.ToString();
                string ReleaseGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colAlbumName].Value.ToString();
                int AlbumID = Convert.ToInt32(DGV.Rows[AlbumRowIndex].Cells[DGC.colIndexAlbum].Value);

                PickAlbumYear pickAlbumYearForm = new PickAlbumYear();
                pickAlbumYearForm.ShowDialog();

                if (pickAlbumYearForm.selectedDiscogs == true)
                {
                    mgt_Discogs.startDiscogs(2, boxListConsole.Items, ArtistGrid, ReleaseGrid, AlbumID);
                }

                return true;
            }
            else if (AlbumColIndex == DGC.colArtistName)
            {
                GlobalVariables.runPickArtist = true;
                PickArtist pickArtistForm = new PickArtist();
                pickArtistForm.ShowDialog();
                GlobalVariables.runPickArtist = false;

                if (pickArtistForm.ArtistSelected == true)
                {
                    int countRecord = mgt_SQLDatabase.AutoSearchDatabaseAlbums(1, DGV, 1, 0, 0, 0, false, false);
                    boxListConsole.Items.Add("Album table updated: " + countRecord.ToString());
                    boxListConsole.SelectedIndex = boxListConsole.Items.Count - 1;
                    return true;
                }
            }
            else if (AlbumColIndex == DGC.colAlbumName)
            {

                PickAlbumName pickAlbumName = new PickAlbumName();
                pickAlbumName.ShowDialog();
                if (pickAlbumName.ArtistNameFilled == true)
                {
                    int countRecord = mgt_SQLDatabase.AutoSearchDatabaseAlbums(1, DGV, 1, 0, 0, 0, false, false);
                    boxListConsole.Items.Add("Album table updated: " + countRecord.ToString());
                    boxListConsole.SelectedIndex = boxListConsole.Items.Count - 1;
                    return true;
                }

                string ReleaseGrid = DGV.Rows[AlbumRowIndex].Cells[DGC.colAlbumName].Value.ToString();

            }
            return false;
        }
        public static bool SingleClickOnGridCallBack(DataGridView DGV, int AlbumRowIndex)
        {
            mgt_SQLValidation.dataGridColumns DGC = new mgt_SQLValidation.dataGridColumns();
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            int AlbumColIndex = DGV.CurrentCell.ColumnIndex;
            bool GridValueBool;

            if (AlbumColIndex == DGC.colWriteIndex)
            {
                GridValueBool = Convert.ToBoolean(DGV.Rows[AlbumRowIndex].Cells[AlbumColIndex].Value);
                int AlbumID = Convert.ToInt32(DGV.Rows[AlbumRowIndex].Cells[DGC.colIndexAlbum].Value);
                if (GridValueBool)
                {
                    db.UpdateWriteIndex(AlbumID, true);
                }
                else
                {
                    db.UpdateWriteIndex(AlbumID, false);
                }

            }

            return false;
        }
    }
}
