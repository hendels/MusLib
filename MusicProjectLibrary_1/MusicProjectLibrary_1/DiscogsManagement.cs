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
        public static void startDiscogs()
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
            var enumerable = discogsClient.SearchAsEnumerable(discogsSearch);

            foreach (DiscogsClient.Data.Result.DiscogsSearchResult itemD in enumerable)
            {
                MessageBox.Show("sup");


            }
            
        }
    }
}
