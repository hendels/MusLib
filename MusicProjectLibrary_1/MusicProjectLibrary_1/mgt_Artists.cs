using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectLibrary_1
{
    class mgt_Artists
    {
        public static int getArtistIdByName(string artistName)
        {
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            List<SQLArtistTable> queryGetSpecificArtist = new List<SQLArtistTable>();
            queryGetSpecificArtist = db.GetAllByArtists(artistName);
            if (queryGetSpecificArtist.Count == 1)
            {
                foreach (SQLArtistTable itemArtist in queryGetSpecificArtist)
                {
                    return itemArtist.IdArtist;
                }
            }
            return 0;
        }
    }
}
