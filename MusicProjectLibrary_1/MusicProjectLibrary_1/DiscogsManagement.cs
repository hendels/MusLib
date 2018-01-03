using DiscogsClient.Data.Query;
using DiscogsClient.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscogsClient;
using RestSharpHelper.OAuth1;
using System.Reactive;

namespace MusicProjectLibrary_1
{
    class DiscogsManagement
    {
        public void go()
        {

        }
        public class SearchResultsDiscogs
        {
            public string title { get; set; }
            public string type { get; set; }
            public string genre { get; set; }
            public int year { get; set; }
        }
        public static void startDiscogs(ListBox.ObjectCollection listBoxConsole, string SearchArtist, string SearchRelease, int AlbumID)
        {
            var tokenInformation = new TokenAuthenticationInformation("QWgIjbVsUoULLGCvqwpQZUvsTEFZlmRALsOJJrLv");
            var discogsClient = new DiscogsClient.DiscogsClient(tokenInformation);

            var discogsSearch = new DiscogsSearch()
            {
                artist = SearchArtist,
                release_title = SearchRelease
            };

            //Retrieve observable result from search
            var observable = discogsClient.Search(discogsSearch);
            //IObserver<DiscogsSearch> obsvr = Observer.Create<DiscogsSearch>(x => listBoxConsole.Add(x.type));
            //obsvr.OnCompleted
            List<SearchResultsDiscogs> LSRD = new List<SearchResultsDiscogs>();
            //SearchResultsDiscogs SRD = new SearchResultsDiscogs();
            //observable.Subscribe(SearchResultsDiscogs, ex => { throw ex; });
            //using (observable.Subscribe(x => listBoxConsole.Add(x.title)))
            //{
                //listBoxConsole.Add(LSRD.Count.ToString());
            //}
            
            IDisposable sub1 = observable.Subscribe(x =>                       
            {
                SearchResultsDiscogs SRD = new SearchResultsDiscogs();
                SRD.title = x.title;
                SRD.type = x.type.ToString();
                SRD.year = Convert.ToInt32(x.year);

                string[] test = new string[2];
                var array = x.genre;                
                SRD.genre = string.Join(",", array);
                LSRD.Add(SRD);
                //listBoxConsole.Add(LSRD.Count.ToString());
                //throw LSRD();
            }
            );
            
            //sub1.
            //sub1.Dispose();

            MessageBox.Show("click ok...");
            int masterReleases = 0;
            foreach (SearchResultsDiscogs itemSRD in LSRD)
            {
                if (itemSRD.type == "master")
                {
                    masterReleases += 1;
                    listBoxConsole.Add(itemSRD.title + "/" + itemSRD.year + "/" + itemSRD.genre);
                    DialogResult res = MessageBox.Show("Downloaded genres:" + "\n" + itemSRD.genre, "Discogs API", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (res == DialogResult.OK)
                    {
                        List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();
                        DBFunctions db = new DBFunctions();
                        queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(AlbumID);
                        MusicFileDetails MFD = new MusicFileDetails();
                        foreach (SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
                        {
                            MusicFileMgt.QuickRead(itemTrack.TrackDirectory, MFD);
                            MFD.pickedAFile.GENRE = itemSRD.genre;
                            MFD.pickedAFile.Save(true);
                            listBoxConsole.Add("......changed Genre in file:" + itemTrack.TrackDirectory);
                        }


                        db.UpdateAlbumGenreByAlbumID(AlbumID, itemSRD.genre);
                        db.UpdateTrackGenreByAlbumID(AlbumID, itemSRD.genre);

                        listBoxConsole.Add("...updated Album & Related Tracks");
                    }
                }                
            }
            if (masterReleases == 0)
            {
                foreach (SearchResultsDiscogs itemSRD in LSRD)
                {
                    masterReleases += 1;
                    listBoxConsole.Add(itemSRD.title + "/" + itemSRD.year + "/" + itemSRD.genre);
                    DialogResult res = MessageBox.Show("NO MASTER genres:" + "\n" + itemSRD.genre, "Discogs API", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (res == DialogResult.OK)
                    {
                        List<SQLTrackTable> queryGetAllTracksByAlbumID = new List<SQLTrackTable>();
                        DBFunctions db = new DBFunctions();
                        queryGetAllTracksByAlbumID = db.GetTrackByAlbumId(AlbumID);
                        MusicFileDetails MFD = new MusicFileDetails();
                        foreach(SQLTrackTable itemTrack in queryGetAllTracksByAlbumID)
                        {
                            MusicFileMgt.QuickRead(itemTrack.TrackDirectory, MFD);
                            MFD.pickedAFile.GENRE = itemSRD.genre;
                            MFD.pickedAFile.Save(true);
                            listBoxConsole.Add("......changed Genre in file:" + itemTrack.TrackDirectory);
                        }
                        

                        db.UpdateAlbumGenreByAlbumID(AlbumID, itemSRD.genre);
                        db.UpdateTrackGenreByAlbumID(AlbumID, itemSRD.genre);
                        
                        listBoxConsole.Add("...updated Album & Related Tracks");
                    }
                    
                }
                if (LSRD.Count == 0)
                    listBoxConsole.Add("...Discogs Search = null, attempting another cycle...");
            }


            //IDisposable sub2 = observable.Subscribe(x => listBoxConsole.Add(x.title));

        }
        
    }

}
