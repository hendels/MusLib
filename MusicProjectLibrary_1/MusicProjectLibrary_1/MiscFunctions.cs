﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace MusicProjectLibrary_1
{
    public class MiscFunctions
    {
        public static void WriteFile(string Filename, string newPath)
        {
            File.WriteAllText(Filename, newPath);
        }
        public static string ReadFile(string Filename)
        {
            return File.ReadAllText(Filename);
        }
        public decimal PercentComplete { get; set; }
        /*
        public lookForDuplicatesInList()
        {
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
        */
    }

    
}
