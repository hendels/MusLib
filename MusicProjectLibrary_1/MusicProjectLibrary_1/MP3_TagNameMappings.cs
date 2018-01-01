//========================================================================
// Name:     MP3_TagNameMappings.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements an enum and a set of dictionaries for mapping tag 
//           names between different audio file formats.
// Comments: 
//========================================================================
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// Implements a set of dictionaries mapping one set of tag
    /// names to another.
    /// </summary>
    internal static class TagMappings
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "TagMappings:               1.02";

        // Maps from common tag names to ID3 v2.3 names
        public static Dictionary<string, string> ToID3v23 = new Dictionary<string, string>();

        // Maps from ID3 v2.3 tag names to common names
        public static Dictionary<string, string> FromID3v23 = new Dictionary<string, string>();

        // Maps from common tag names to ID3 v2.4 names - Not currently used
        public static Dictionary<string, string> ToID3v24 = new Dictionary<string, string>();

        // Maps from ID3 v2.4 tag names to common names - Not currently used
        public static Dictionary<string, string> FromID3v24 = new Dictionary<string, string>();

        // Maps ID3 v1 genre codes to strings
        public static Dictionary<byte, string> ID3v1Genre = new Dictionary<byte, string>();

        // Maps ID3 v1 genre strings to codes 
        public static Dictionary<string, byte> ID3V1GenreToByte = new Dictionary<string, byte>();

        // Maps ID3 v2.x embedded image code to description
        public static Dictionary<byte, string> ImageDescription = new Dictionary<byte, string>();

        // Maps ID3 v2.x embedded immage description to code
        public static Dictionary<string, byte> ImageCode = new Dictionary<string, byte>();


        /// <summary>
        /// Constructor
        /// </summary>
        static TagMappings()
        {
            // Add Common to ToID3v23 and ToID3v23
            ToID3v23.Add("ALBUM", "TALB");
            ToID3v23.Add("ALBUMSORT", "TSOA");
            ToID3v23.Add("ALBUMARTIST", "TPE2");
            ToID3v23.Add("ALBUMARTISTSORT", "TSO2");
            ToID3v23.Add("ARTIST", "TPE1");
            ToID3v23.Add("ARTISTSORT", "TSOP");
            ToID3v23.Add("BPM", "TBPM");
            ToID3v23.Add("COMMENT", "COMM");
            ToID3v23.Add("COMPILATION", "TCMP");
            ToID3v23.Add("COMPOSER", "TCOM");
            ToID3v23.Add("COMPOSERSORT", "TSOC");
            ToID3v23.Add("CONDUCTOR", "TPE3");
            ToID3v23.Add("CONTENTGROUP", "TIT1");
            ToID3v23.Add("COPYRIGHT", "TCOP");
            ToID3v23.Add("DISCNUMBER", "TPOS");
            ToID3v23.Add("ENCODEDBY", "TENC");
            ToID3v23.Add("ENCODERSETTINGS", "TSSE");
            ToID3v23.Add("FILEOWNER", "TOWN");
            ToID3v23.Add("FILETYPE", "TFLT");
            ToID3v23.Add("GENRE", "TCON");
            ToID3v23.Add("INITIALKEY", "TKEY");
            ToID3v23.Add("ISRC", "TSRC");
            ToID3v23.Add("LANGUAGE", "TLAN");
            ToID3v23.Add("LENGTH", "TLEN");
            ToID3v23.Add("LYRICIST", "TEXT");
            ToID3v23.Add("MEDIATYPE", "TMED");
            ToID3v23.Add("MIXARTIST", "TPE4");
            ToID3v23.Add("MOVEMENTNAME", "MVNM");
            ToID3v23.Add("MOVEMENT", "MVIN");
            ToID3v23.Add("MOVEMENTTOTAL", "MVIN");
            ToID3v23.Add("NETRADIOOWNER", "TRSO");
            ToID3v23.Add("NETRADIOSTATION", "TRSN");
            ToID3v23.Add("ORIGALBUM", "TOAL");
            ToID3v23.Add("ORIGARTIST", "TOPE");
            ToID3v23.Add("ORIGFILENAME", "TOFN");
            ToID3v23.Add("ORIGLYRICIST", "TOLY");
            ToID3v23.Add("PODCAST", "PCST");
            ToID3v23.Add("PODCASTCATEGORY", "TCAT");
            ToID3v23.Add("PODCASTDESC", "TDES");
            ToID3v23.Add("PODCASTID", "TGID");
            ToID3v23.Add("PODCASTKEYWORDS", "TKWD");
            ToID3v23.Add("PODCASTURL", "WFED");
            ToID3v23.Add("POPULARIMETER", "POPM");
            ToID3v23.Add("PUBLISHER", "TPUB");
            ToID3v23.Add("RATING MM", "POPM");
            ToID3v23.Add("RATING WMP", "POPM");
            ToID3v23.Add("RELEASETIME", "TDRL");
            ToID3v23.Add("SUBTITLE", "TIT3");
            ToID3v23.Add("TITLE", "TIT2");
            ToID3v23.Add("TITLESORT", "TSOT");
            // The one below modded by JR - because Vorbis uses "TRACKNUMBER"
            ToID3v23.Add("TRACKNUMBER", "TRCK");
            ToID3v23.Add("UNSYNCEDLYRICS", "USLT");
            ToID3v23.Add("WWW", "WXXX");
            ToID3v23.Add("WWWARTIST", "WOAR");
            ToID3v23.Add("WWWAUDIOFILE", "WOAF");
            ToID3v23.Add("WWWAUDIOSOURCE", "WOAS");
            ToID3v23.Add("WWWCOMMERCIALINFO", "WCOM");
            ToID3v23.Add("WWWCOPYRIGHT", "WCOP");
            ToID3v23.Add("WWWPAYMENT", "WPAY");
            ToID3v23.Add("WWWPUBLISHER", "WPUB");
            ToID3v23.Add("WWWRADIOPAGE", "WORS");
            ToID3v23.Add("OTHERFIELD", "TXXX");


            // Copy them to ToID3v24 - they're common
            foreach (var item in ToID3v23)
                FromID3v24.Add(item.Key, item.Value);


            // Add mappings unique to ID3v23
            ToID3v23.Add("ORIGYEAR", "TORY");
            ToID3v23.Add("INVOLVEDPEOPLE", "IPLS");
            ToID3v23.Add("DATE", "TYER");


            // Add mappings unique to ID3v24
            ToID3v24.Add("ENCODINGTIME", "TDEN");
            ToID3v24.Add("MOOD", "TMOO");
            ToID3v24.Add("MUSICIANCREDITS", "TMCL");
            ToID3v24.Add("SETSUBTITLE", "TSST");
            ToID3v24.Add("TAGGINGTIME", "TDTG");
            ToID3v24.Add("ORIGYEAR", "TDOR");
            ToID3v24.Add("INVOLVEDPEOPLE", "TIPL");
            ToID3v24.Add("DATE", "TDRC");


            // Populate reverse mapping - FromID3v23
            foreach (var item in ToID3v23)
                if (!FromID3v23.ContainsKey(item.Value))
                    FromID3v23.Add(item.Value, item.Key);


            // Populate reverse mapping - Populate FromID3v24
            foreach (var item in ToID3v24)
                if (!FromID3v24.ContainsKey(item.Value))
                    FromID3v24.Add(item.Value, item.Key);


            // Populate ID3v1Genre
            ID3v1Genre.Add(0, "Blues");
            ID3v1Genre.Add(1, "Classic Rock");
            ID3v1Genre.Add(2, "Country");
            ID3v1Genre.Add(3, "Dance");
            ID3v1Genre.Add(4, "Disco");
            ID3v1Genre.Add(5, "Funk");
            ID3v1Genre.Add(6, "Grunge");
            ID3v1Genre.Add(7, "Hip-Hop");
            ID3v1Genre.Add(8, "Jazz");
            ID3v1Genre.Add(9, "Metal");
            ID3v1Genre.Add(10, "New Age");
            ID3v1Genre.Add(11, "Oldies");
            ID3v1Genre.Add(12, "Other");
            ID3v1Genre.Add(13, "Pop");
            ID3v1Genre.Add(14, "Rhythm and Blues");
            ID3v1Genre.Add(15, "Rap");
            ID3v1Genre.Add(16, "Reggae");
            ID3v1Genre.Add(17, "Rock");
            ID3v1Genre.Add(18, "Techno");
            ID3v1Genre.Add(19, "Industrial");
            ID3v1Genre.Add(20, "Alternative");
            ID3v1Genre.Add(21, "Ska");
            ID3v1Genre.Add(22, "Death Metal");
            ID3v1Genre.Add(23, "Pranks");
            ID3v1Genre.Add(24, "Soundtrack");
            ID3v1Genre.Add(25, "Euro-Techno");
            ID3v1Genre.Add(26, "Ambient");
            ID3v1Genre.Add(27, "Trip-Hop");
            ID3v1Genre.Add(28, "Vocal");
            ID3v1Genre.Add(29, "Jazz & Funk");
            ID3v1Genre.Add(30, "Fusion");
            ID3v1Genre.Add(31, "Trance");
            ID3v1Genre.Add(32, "Classical");
            ID3v1Genre.Add(33, "Instrumental");
            ID3v1Genre.Add(34, "Acid");
            ID3v1Genre.Add(35, "House");
            ID3v1Genre.Add(36, "Game");
            ID3v1Genre.Add(37, "Sound Clip");
            ID3v1Genre.Add(38, "Gospel");
            ID3v1Genre.Add(39, "Noise");
            ID3v1Genre.Add(40, "Alternative Rock");
            ID3v1Genre.Add(41, "Bass");
            ID3v1Genre.Add(42, "Soul");
            ID3v1Genre.Add(43, "Punk");
            ID3v1Genre.Add(44, "Space");
            ID3v1Genre.Add(45, "Meditative");
            ID3v1Genre.Add(46, "Instrumental Pop");
            ID3v1Genre.Add(47, "Instrumental Rock");
            ID3v1Genre.Add(48, "Ethnic");
            ID3v1Genre.Add(49, "Gothic");
            ID3v1Genre.Add(50, "Darkwave");
            ID3v1Genre.Add(51, "Techno-Industrial");
            ID3v1Genre.Add(52, "Electronic");
            ID3v1Genre.Add(53, "Pop-Folk");
            ID3v1Genre.Add(54, "Eurodance");
            ID3v1Genre.Add(55, "Dream");
            ID3v1Genre.Add(56, "Southern Rock");
            ID3v1Genre.Add(57, "Comedy");
            ID3v1Genre.Add(58, "Cult");
            ID3v1Genre.Add(59, "Gangsta");
            ID3v1Genre.Add(60, "Top 40");
            ID3v1Genre.Add(61, "Christian Rap");
            ID3v1Genre.Add(62, "Pop/Funk");
            ID3v1Genre.Add(63, "Jungle");
            ID3v1Genre.Add(64, "Native US");
            ID3v1Genre.Add(65, "Cabaret");
            ID3v1Genre.Add(66, "New Wave");
            ID3v1Genre.Add(67, "Psychedelic");
            ID3v1Genre.Add(68, "Rave");
            ID3v1Genre.Add(69, "Showtunes");
            ID3v1Genre.Add(70, "Trailer");
            ID3v1Genre.Add(71, "Lo-Fi");
            ID3v1Genre.Add(72, "Tribal");
            ID3v1Genre.Add(73, "Acid Punk");
            ID3v1Genre.Add(74, "Acid Jazz");
            ID3v1Genre.Add(75, "Polka");
            ID3v1Genre.Add(76, "Retro");
            ID3v1Genre.Add(77, "Musical");
            ID3v1Genre.Add(78, "Rock ’n’ Roll");
            ID3v1Genre.Add(79, "Hard Rock");
            ID3v1Genre.Add(80, "Folk");
            ID3v1Genre.Add(81, "Folk-Rock");
            ID3v1Genre.Add(82, "National Folk");
            ID3v1Genre.Add(83, "Swing");
            ID3v1Genre.Add(84, "Fast Fusion");
            ID3v1Genre.Add(85, "Bebop");
            ID3v1Genre.Add(86, "Latin");
            ID3v1Genre.Add(87, "Revival");
            ID3v1Genre.Add(88, "Celtic");
            ID3v1Genre.Add(89, "Bluegrass");
            ID3v1Genre.Add(90, "Avantgarde");
            ID3v1Genre.Add(91, "Gothic Rock");
            ID3v1Genre.Add(92, "Progressive Rock");
            ID3v1Genre.Add(93, "Psychedelic Rock");
            ID3v1Genre.Add(94, "Symphonic Rock");
            ID3v1Genre.Add(95, "Slow Rock");
            ID3v1Genre.Add(96, "Big Band");
            ID3v1Genre.Add(97, "Chorus");
            ID3v1Genre.Add(98, "Easy Listening");
            ID3v1Genre.Add(99, "Acoustic");
            ID3v1Genre.Add(100, "Humour");
            ID3v1Genre.Add(101, "Speech");
            ID3v1Genre.Add(102, "Chanson");
            ID3v1Genre.Add(103, "Opera");
            ID3v1Genre.Add(104, "Chamber Music");
            ID3v1Genre.Add(105, "Sonata");
            ID3v1Genre.Add(106, "Symphony");
            ID3v1Genre.Add(107, "Booty Bass");
            ID3v1Genre.Add(108, "Primus");
            ID3v1Genre.Add(109, "Porn Groove");
            ID3v1Genre.Add(110, "Satire");
            ID3v1Genre.Add(111, "Slow Jam");
            ID3v1Genre.Add(112, "Club");
            ID3v1Genre.Add(113, "Tango");
            ID3v1Genre.Add(114, "Samba");
            ID3v1Genre.Add(115, "Folklore");
            ID3v1Genre.Add(116, "Ballad");
            ID3v1Genre.Add(117, "Power Ballad");
            ID3v1Genre.Add(118, "Rhythmic Soul");
            ID3v1Genre.Add(119, "Freestyle");
            ID3v1Genre.Add(120, "Duet");
            ID3v1Genre.Add(121, "Punk Rock");
            ID3v1Genre.Add(122, "Drum Solo");
            ID3v1Genre.Add(123, "A cappella");
            ID3v1Genre.Add(124, "Euro-House");
            ID3v1Genre.Add(125, "Dance Hall");
            ID3v1Genre.Add(126, "Goa");
            ID3v1Genre.Add(127, "Drum & Bass");
            ID3v1Genre.Add(128, "Club-House");
            ID3v1Genre.Add(129, "Hardcore Techno");
            ID3v1Genre.Add(130, "Terror");
            ID3v1Genre.Add(131, "Indie");
            ID3v1Genre.Add(132, "BritPop");
            ID3v1Genre.Add(133, "Negerpunk");
            ID3v1Genre.Add(134, "Polsk Punk");
            ID3v1Genre.Add(135, "Beat");
            ID3v1Genre.Add(136, "Christian Gangsta Rap");
            ID3v1Genre.Add(137, "Heavy Metal");
            ID3v1Genre.Add(138, "Black Metal");
            ID3v1Genre.Add(139, "Crossover");
            ID3v1Genre.Add(140, "Contemporary Christian");
            ID3v1Genre.Add(141, "Christian Rock");
            ID3v1Genre.Add(142, "Merengue");
            ID3v1Genre.Add(143, "Salsa");
            ID3v1Genre.Add(144, "Thrash Metal");
            ID3v1Genre.Add(145, "Anime");
            ID3v1Genre.Add(146, "Jpop");
            ID3v1Genre.Add(147, "Synthpop");
            ID3v1Genre.Add(148, "Abstract");
            ID3v1Genre.Add(149, "Art Rock");
            ID3v1Genre.Add(150, "Baroque");
            ID3v1Genre.Add(151, "Bhangra");
            ID3v1Genre.Add(152, "Big Beat");
            ID3v1Genre.Add(153, "Breakbeat");
            ID3v1Genre.Add(154, "Chillout");
            ID3v1Genre.Add(155, "Downtempo");
            ID3v1Genre.Add(156, "Dub");
            ID3v1Genre.Add(157, "EBM");
            ID3v1Genre.Add(158, "Eclectic");
            ID3v1Genre.Add(159, "Electro");
            ID3v1Genre.Add(160, "Electroclash");
            ID3v1Genre.Add(161, "Emo");
            ID3v1Genre.Add(162, "Experimental");
            ID3v1Genre.Add(163, "Garage");
            ID3v1Genre.Add(164, "Global");
            ID3v1Genre.Add(165, "IDM");
            ID3v1Genre.Add(166, "Illbient");
            ID3v1Genre.Add(167, "Industro-Goth");
            ID3v1Genre.Add(168, "Jam Band");
            ID3v1Genre.Add(169, "Krautrock");
            ID3v1Genre.Add(170, "Leftfield");
            ID3v1Genre.Add(171, "Lounge");
            ID3v1Genre.Add(172, "Math Rock");
            ID3v1Genre.Add(173, "New Romantic");
            ID3v1Genre.Add(174, "Nu-Breakz");
            ID3v1Genre.Add(175, "Post-Punk");
            ID3v1Genre.Add(176, "Post-Rock");
            ID3v1Genre.Add(177, "Psytrance");
            ID3v1Genre.Add(178, "Shoegaze");
            ID3v1Genre.Add(179, "Space Rock");
            ID3v1Genre.Add(180, "Trop Rock");
            ID3v1Genre.Add(181, "World Music");
            ID3v1Genre.Add(182, "Neoclassical");
            ID3v1Genre.Add(183, "Audiobook");
            ID3v1Genre.Add(184, "Audio Theatre");
            ID3v1Genre.Add(185, "Neue Deutsche Welle");
            ID3v1Genre.Add(186, "Podcast");
            ID3v1Genre.Add(187, "Indie Rock");
            ID3v1Genre.Add(188, "G-Funk");
            ID3v1Genre.Add(189, "Dubstep");
            ID3v1Genre.Add(190, "Garage Rock");
            ID3v1Genre.Add(191, "Psybient");

            // Populate reverse mapping - Populate FromID3v1
            foreach (var item in ID3v1Genre)
                if (!ID3V1GenreToByte.ContainsKey(item.Value))
                    ID3V1GenreToByte.Add(item.Value, item.Key);


            // Populate mappings from image code to image description
            ImageDescription.Add(0x00, "Other");
            ImageDescription.Add(0x01, "32×32 pixels ‘file icon’ (PNG only)");
            ImageDescription.Add(0x02, "Other file icon");
            ImageDescription.Add(0x03, "Cover (front)");
            ImageDescription.Add(0x04, "Cover (back)");
            ImageDescription.Add(0x05, "Leaflet page");
            ImageDescription.Add(0x06, "Media (e.g. label side of CD)");
            ImageDescription.Add(0x07, "Lead artist/lead performer/soloist");
            ImageDescription.Add(0x08, "Artist/performer");
            ImageDescription.Add(0x09, "Conductor");
            ImageDescription.Add(0x0A, "Band/Orchestra");
            ImageDescription.Add(0x0B, "Composer");
            ImageDescription.Add(0x0C, "Lyricist/text writer");
            ImageDescription.Add(0x0D, "Recording Location");
            ImageDescription.Add(0x0E, "During recording");
            ImageDescription.Add(0x0F, "During performance");
            ImageDescription.Add(0x10, "Movie/video screen capture");
            ImageDescription.Add(0x11, "A bright coloured fish");
            ImageDescription.Add(0x12, "Illustration");
            ImageDescription.Add(0x13, "Band/artist logotype");
            ImageDescription.Add(0x14, "Publisher/Studio logotype");

            // Populate image reverse mapping dictionary
            foreach (var Code in ImageDescription)
                ImageCode.Add(Code.Value, Code.Key);
        }


        /// <summary>
        /// For use with V1 genres.
        /// A static method to map a byte to its genre
        /// </summary>
        /// <param name="b">The byte genre code</param>
        /// <returns>The genre string</returns>
        public static string Genre(byte b)
        {
            if (b > ID3v1Genre.Count)
                return "Undefined value.";
            else
                return ID3v1Genre[b];
        }
    }


    /// <summary>
    /// Extension methods.  
    /// Adds new methods to strings.
    /// </summary>
    public static class MappingExtensionMethods
    {
        /// <summary>
        /// Maps the common name of a tag to its v2.3 name
        /// </summary>
        /// <param name="Input">The tag common name</param>
        /// <returns>The tag common name</returns>
        public static string ToID3v23(this string Input)
        {
            if (TagMappings.ToID3v23.ContainsKey(Input))
                return TagMappings.ToID3v23[Input];
            else
                return Input;
        }


        /// <summary>
        /// Maps the common name of a tag to its v2.4 name
        /// </summary>
        /// <param name="input">The tag common name</param>
        /// <returns>The v2.4 tag code</returns>
        public static string ToID3v24(this string input)
        {
            if (TagMappings.ToID3v24.ContainsKey(input))
                return TagMappings.ToID3v24[input];
            else
                return input;
        }


        /// <summary>
        /// Maps the string representation of a v2.3 tag code to its common name
        /// </summary>
        /// <param name="input">the v2.3 tag code</param>
        /// <returns>The common name</returns>
        public static string FromID3v23(this string input)
        {
            if (TagMappings.FromID3v23.ContainsKey(input))
                return TagMappings.FromID3v23[input];
            else
                return input;
        }


        /// <summary>
        /// Maps the string representation of a v2.4 tag code to its common name
        /// </summary>
        /// <param name="input">the v2.4 tag code</param>
        /// <returns>The common name</returns>
        public static string FromID3v24(this string input)
        {
            if (TagMappings.FromID3v24.ContainsKey(input))
                return TagMappings.FromID3v24[input];
            else
                return input;
        }


        /// <summary>
        /// Maps a V1 genre name to its byte code.
        /// </summary>
        /// <param name="input">The v1 genre name string</param>
        /// <returns>The corresponding byte code</returns>
        public static byte ToV1GenreCode(this string input)
        {
            const int InvalidGenreFlag = 255;

            if (TagMappings.ID3V1GenreToByte.ContainsKey(input))
                return TagMappings.ID3V1GenreToByte[input];
            else
                return InvalidGenreFlag;
        }
    }
}