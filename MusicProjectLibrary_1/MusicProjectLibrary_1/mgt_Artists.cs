using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectLibrary_1
{
    class mgt_Artists
    {
        public static List<string> autoMatchArtists(string albumName, int minimumStringLen)
        {
            // find chars > change it to space
            string changedAlbumName = Functions.findProhibitedSigns(albumName, ' ');
            // find spaces and cut album name string to separate words and add them to RETURN List
            int count = 0;
            char specialSign = ' ';
            int specialAppear = 0;
            string extractedString;
            List<string> ItemList = new List<string>();

            foreach (char special in changedAlbumName)
                if (special == specialSign) count++;
            for (int i = 0; i <= count; i++)
            {
                int firstSign = changedAlbumName.IndexOf(specialSign, specialAppear);
                if (firstSign == -1)
                    extractedString = changedAlbumName;
                else                
                    extractedString = changedAlbumName.Substring(specialAppear, firstSign);

                changedAlbumName = changedAlbumName.Substring(firstSign + 1, changedAlbumName.Length - firstSign - 1);
                if (extractedString.Length >= minimumStringLen)
                {
                    ItemList.Add(extractedString);
                }
            }
            foreach (string item in ItemList)
            {

            }
            return ItemList;
        }
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
        public static void createArtistsList(List<string> uniqueArtists)
        {
            //List<string> uniqueArtists = new List<string>();
            List<SQLAlbumTable> queryGetAllArtists = new List<SQLAlbumTable>();

            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllArtists = db.GetAlbumArtists();
            foreach (SQLAlbumTable artist in queryGetAllArtists)
            {
                if (uniqueArtists.Any(uArtist => uArtist == artist.AlbumArtist)) // [przemy knowledge] szukanie duplikatów w liście > jeżeli istnieje duplikat nie dodawaj do listy ponownie
                {

                }
                else
                {
                    uniqueArtists.Add(artist.AlbumArtist);
                }
            }
        }
    }

}
