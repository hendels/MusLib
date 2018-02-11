using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class mgt_PickGenre
    {
        public static List<string> SortedList = new List<string>();

        public static void createGenreList(CheckedListBox checkedListGenre)
        {
            List<SQLAlbumTable> queryGetAllGenres = new List<SQLAlbumTable>();
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllGenres = db.GetAlbumsGenre();

            if (queryGetAllGenres.Count > 0)
            {
                List<string> uniqueGenre = new List<string>();
                foreach (SQLAlbumTable itemGenre in queryGetAllGenres)
                {
                    List<string> splitString = new List<string>();
                    //szukaj , / 
                    for (int x = 0; x <= 1; x++)
                    {
                        char specialSign = ' ';
                        switch (x)
                        {
                            case 0:
                                specialSign = ',';
                                break;
                            case 1:
                                specialSign = '/';
                                break;
                        }
                        if (itemGenre.AlbumGenre.Contains(specialSign))
                        {
                            int count = 0;
                            int specialAppear = 0;
                            string toExtract = itemGenre.AlbumGenre;
                            string extractedString;
                            toExtract = toExtract.Replace(" ", string.Empty);

                            foreach (char special in itemGenre.AlbumGenre)
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

                                if (uniqueGenre.Any(uGenre => uGenre == extractedString)) // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie
                                {
                                    //globalBoxListConsole.Add("catalog exist for: " + item.trackDirectory);
                                }
                                else
                                {
                                    uniqueGenre.Add(extractedString);

                                }
                            }
                        }
                    }
                }
                SortedList = uniqueGenre.OrderByDescending(o => o).ToList(); //[przemy knowledge - sorting list]
                foreach (string itemGenre in SortedList)
                {
                    checkedListGenre.Items.Add(itemGenre);
                }
            }

        }
        public static string pickedGenre(TextBox tbxPickedGenre, CheckedListBox checkedListGenre)
        {
            string allGenres = "";
            int counter = 0;
            foreach (Object item in checkedListGenre.CheckedItems)
            {
                counter += 1;
                if (checkedListGenre.Items.Count == 1)
                    allGenres = item.ToString();
                else if (counter == 1)
                    allGenres = item.ToString();
                else
                    allGenres = allGenres + "-" + item.ToString();
            }
            tbxPickedGenre.Text = allGenres;
            return allGenres;
        }
        public static void createSecondaryGenreList(string SelectedArtist, CheckedListBox checkedSuggestedGenre)
        {
            List<SQLAlbumTable> queryGetAllWrittenGenres = new List<SQLAlbumTable>();
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllWrittenGenres = db.GetAllAlbumWrittenGenresByArtist(SelectedArtist);
            if (queryGetAllWrittenGenres.Count != 0 & GlobalVariables.SelectedArtist != "")
            {
                List<string> uniqueGenre = new List<string>();
                foreach (SQLAlbumTable itemList in queryGetAllWrittenGenres)
                {
                    if (itemList.DirectoryGenre != null & itemList.DirectoryGenre != "")
                    {
                        if (uniqueGenre.Any(uGenre => uGenre == itemList.DirectoryGenre)) // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie
                        {                            
                        }
                        else
                        {
                            uniqueGenre.Add(itemList.DirectoryGenre);
                            checkedSuggestedGenre.Items.Add(itemList.DirectoryGenre);
                        }                        
                    }
                    else
                    {
                        //checkedSuggestedGenre.Items.Add("<nothing found in DB>");
                        //checkedSuggestedGenre.Enabled = false;
                    }
                }
            }
            else
            {
                checkedSuggestedGenre.Items.Add("<nothing found in DB>");
                checkedSuggestedGenre.Enabled = false;
            }
        }
        public static void findRelatedGenres(string selectedGenre, CheckedListBox chlstRelatedGenresOnHDD)
        {
            chlstRelatedGenresOnHDD.Items.Clear();
            List<SQLAlbumTable> queryGetAllRelatedGenres = new List<SQLAlbumTable>();
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllRelatedGenres = db.GetAllRelatedGenres(selectedGenre);
            if (queryGetAllRelatedGenres.Count != 0 & selectedGenre != "")
            {
                List<string> uniqueGenre = new List<string>();
                int enableChlst = 0;
                
                foreach (SQLAlbumTable itemList in queryGetAllRelatedGenres)
                {
                    if (itemList.DirectoryGenre != null & itemList.DirectoryGenre != "")
                    {
                        if (uniqueGenre.Any(uGenre => uGenre == itemList.DirectoryGenre)) // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie
                        {
                        }
                        else
                        {
                            enableChlst += 1;
                            uniqueGenre.Add(itemList.DirectoryGenre);
                            chlstRelatedGenresOnHDD.Items.Add(itemList.DirectoryGenre);
                            if (enableChlst == 1) chlstRelatedGenresOnHDD.Enabled = true;
                        }                        
                    }
                    else
                    {
                        //chlstRelatedGenresOnHDD.Items.Add("<nothing found in DB>");
                        //chlstRelatedGenresOnHDD.Enabled = false;
                    }
                }
            }
            else
            {
                chlstRelatedGenresOnHDD.Items.Add("<nothing found in DB>");
                chlstRelatedGenresOnHDD.Enabled = false;
            }
        }
    }
}
