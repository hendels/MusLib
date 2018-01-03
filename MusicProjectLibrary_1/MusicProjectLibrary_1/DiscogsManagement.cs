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
            public int year { get; set; }
        }
        public async static void startDiscogs(ListBox.ObjectCollection listBoxConsole)
        {
            var tokenInformation = new TokenAuthenticationInformation("QWgIjbVsUoULLGCvqwpQZUvsTEFZlmRALsOJJrLv");
            var discogsClient = new DiscogsClient.DiscogsClient(tokenInformation);

            var discogsSearch = new DiscogsSearch()
            {
                artist = "wisp",
                release_title = "Building Dragons"
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
                LSRD.Add(SRD);
                //listBoxConsole.Add(LSRD.Count.ToString());
                //throw LSRD();
            }
            );
            
            //sub1.
            //sub1.Dispose();

            MessageBox.Show("click ok...");
            foreach (SearchResultsDiscogs itemSRD in LSRD)
            {
                if (itemSRD.type == "master")
                {
                    listBoxConsole.Add(itemSRD.title + "/" + itemSRD.year);
                }
                
            }


            //IDisposable sub2 = observable.Subscribe(x => listBoxConsole.Add(x.title));

        }
        
    }

}
