using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class mgt_SearchAlbums
    {
        public void MainSearch(DataGridView DGV, ListBox consoleListBox)
        {
            RefreshSpecificTable(1);
            mgt_SQLValidation.ReadDataGridForAll(DGV, consoleListBox.Items);
        }

        private int RefreshSpecificTable(int RefreshTableNo)
        {
            int counter = 0;
            int subcounter = 0;
            int subcounter2 = 0;
            int x = 0;
            switch (RefreshTableNo)
            {
                case 1: // if something exist in search box (album ID)           
                    if (Int32.TryParse(tbxSearchAlbums.Text, out x))
                    {
                        /*
                         = show all Case
                        */
                        subcounter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(x, dgvAlbums,
                            1, 0, 0, 0, chbProceed.Checked, chb_ShowExceptRating.Checked);
                        subcounter2 = mgt_SQLDatabase.AutoSearchDatabaseTracksByAlbumID(x, dgvTracks, 0, 0, 0);
                        counter = subcounter + subcounter2;
                    }
                    else
                    {
                        /*
                         * id album
                         * DGV
                         * par 1: showAll ; 1 = true, 0 = false
                         * par 2: Album count ; only for showAll = 0
                         * par 3: PointsMin ; only for showAll = 0
                         * par 4: PointsMax ; only for showAll = 0
                         * par 5: showProceed; only for showAll = 0
                         * par 6: showFullyRated; only forShowAll = 0
                        */
                        if (chbShowAll.Checked)
                        {
                            counter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(0, dgvAlbums,
                                1, 0, 0, 0, false, false);
                        }
                        else
                        {
                            int AlbumCount = 0;
                            if (tbxAlbumCount.Text != "")
                                AlbumCount = Convert.ToInt32(tbxAlbumCount.Text);

                            counter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(0, dgvAlbums,
                                0, AlbumCount, Convert.ToInt32(tbxPointsMin.Text), Convert.ToInt32(tbxPointsMax.Text), chbProceed.Checked, chb_ShowExceptRating.Checked);
                        }

                    }

                    return counter;
                case 2:
                    if (Int32.TryParse(tbxSearchTracks.Text, out x))
                        counter = mgt_SQLDatabase.AutoSearchDatabaseTracksByTrackIndex(x, dgvTracks, 0, 0, 0);
                    else
                        counter = mgt_SQLDatabase.AutoSearchDatabaseTracksByTrackIndex(0, dgvTracks, Convert.ToInt32(tbxTrackCount.Text), Convert.ToInt32(tbxTrackRatingMin.Text), Convert.ToInt32(tbxTrackRatingMax.Text));
                    return counter;
                case 3:
                    counter = mgt_SQLDatabase.AutoSearchDatabaseArtists(tbxSearchArtist.Text, dgvArtists);
                    return counter;
            }
            return counter;
        }
    }
}
