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


namespace MusicProjectLibrary_1
{
    class DiscogsManagement
    {
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
                //release_title = "The Shape Of Jazz To Come"
            };
            
            //Retrieve observable result from search
            var observable = discogsClient.Search(discogsSearch);
            IObserver<int> obsvr = Observer.Create<int>();
            List <SearchResultsDiscogs> LSRD = new List<SearchResultsDiscogs>();
            //SearchResultsDiscogs SRD = new SearchResultsDiscogs();

            //IDisposable sub1 = observable.Subscribe(x => SRD.title = x.title);
            using (observable.Subscribe(x => listBoxConsole.Add($"{x.title}{x.style}{x.type}")))
            {
                
                SearchResultsDiscogs SRD = new SearchResultsDiscogs();
                SRD.type = x.type;
            }


            IDisposable sub2 = observable.Subscribe(x => listBoxConsole.Add(x.title));
            
        }
    }
}
