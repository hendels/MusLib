using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    public class SearchAlbumParameters
    {
        public string searchAlbumsString { get; set; }
        public bool processedAlbums { get; set; }
        public bool fullRated { get; set; }
        public bool showAll { get; set; }
        public int showAlbumLimiter { get; set; }
        
    }
    public class SearchTrackParameters
    {
        public string searchTracksString { get; set; }
        public int minTrackRating { get; set; }
        public int maxTrackRating { get; set; }
        public int showTracksLimiter { get; set; }
    }
    public class SearchArtistParameters
    {
        public string searchArtistsString { get; set; }
    }
    class mgt_SearchAlbums
    {
        public void MainSearch(DataGridView dgvAlbums, DataGridView dgvTracks, DataGridView dgvArtists, SearchAlbumParameters searchAlbumsParameters, SearchTrackParameters searchTracksParameters, SearchArtistParameters searchArtistsParameters, ListBox consoleListBox)
        {
            RefreshSpecificTable(1, dgvAlbums, dgvTracks, dgvArtists, searchAlbumsParameters, searchTracksParameters, searchArtistsParameters);
            mgt_SQLValidation.ReadDataGridForAll(dgvAlbums, consoleListBox.Items);
        }

        public static int RefreshSpecificTable(int RefreshTableNo, DataGridView dgvAlbums, DataGridView dgvTracks, DataGridView dgvArtists, SearchAlbumParameters searchAlbumsParameters, SearchTrackParameters searchTracksParameters, SearchArtistParameters searchArtistsParameters)
        {
            int counter = 0;
            int subcounter = 0;
            int subcounter2 = 0;
            int x = 0;
            switch (RefreshTableNo)
            {
                case 1: // if something exist in search box (album ID)           
                    if (Int32.TryParse(searchAlbumsParameters.searchAlbumsString, out x))
                    {
                        /*
                         = show Specific Album & Tracks Case - by Album ID
                        */
                        subcounter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(x, dgvAlbums,
                            1, 0, 0, 0, searchAlbumsParameters.processedAlbums, searchAlbumsParameters.fullRated, searchAlbumsParameters.searchAlbumsString);
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
                        if (searchAlbumsParameters.showAll)
                        {
                            counter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(0, dgvAlbums,
                                1, 0, 0, 0, false, false, searchAlbumsParameters.searchAlbumsString);
                        }
                        else
                        {
                            int AlbumCount = searchAlbumsParameters.showAlbumLimiter;
                            //if (searchAlbumsParameters.showAlbumLimiter != 0)
                            //    AlbumCount = searchAlbumsParameters.showAlbumLimiter;
                            if (Int32.TryParse(searchAlbumsParameters.searchAlbumsString, out x))
                            {

                            }
                            else if(searchAlbumsParameters.searchAlbumsString != "")
                            {
                                counter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(0, dgvAlbums,
                                2, AlbumCount, searchTracksParameters.minTrackRating, searchTracksParameters.maxTrackRating, searchAlbumsParameters.processedAlbums, searchAlbumsParameters.fullRated, searchAlbumsParameters.searchAlbumsString);
                            }
                            else
                                counter = mgt_SQLDatabase.AutoSearchDatabaseAlbums(0, dgvAlbums,
                                0, AlbumCount, searchTracksParameters.minTrackRating, searchTracksParameters.maxTrackRating, searchAlbumsParameters.processedAlbums, searchAlbumsParameters.fullRated, searchAlbumsParameters.searchAlbumsString);
                        }

                    }

                    return counter;
                case 2:
                    if (Int32.TryParse(searchTracksParameters.searchTracksString, out x))
                        counter = mgt_SQLDatabase.AutoSearchDatabaseTracksByTrackIndex(x, dgvTracks, 0, 0, 0);
                    else
                        counter = mgt_SQLDatabase.AutoSearchDatabaseTracksByTrackIndex(0, dgvTracks, searchTracksParameters.showTracksLimiter, searchTracksParameters.minTrackRating, searchTracksParameters.maxTrackRating);
                    return counter;
                case 3:
                    mgt_SQLDatabase.AutoSearchDatabaseArtists(searchArtistsParameters.searchArtistsString, dgvArtists);
                    return 0;
            }
            return counter;
        }
    }
}
