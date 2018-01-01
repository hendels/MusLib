using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// Enumeration of the types of line found in a M3U playlist file.
    /// </summary>
    enum LineType
    {
        Blank,      // Line is blank
        Comment,    // Line is a comment
        Absolute,   // Line holds an absolute path
        Relative,   // Line holds a relative path
        URL,        // Line holds a URL
        Format,     // Line is a format indicator: #EXTM3U
        Marker,     // Line is a marker: #EXTINF
        Unknown     // Unable to recognise line as any known type
    }


    /// <summary>
    /// Class to represent and check an M3U playlist.
    /// It checks that:
    ///  - URLs point to a site that can be contacted.
    ///  - Relative paths point to a file.
    ///  - Absolute paths point to a file.  (This is problematic
    ///    as if the Playlist file being checked resides on a 
    ///    different computer, the absolute path with not be
    ///    valid on the checking machine.  I have not attempted 
    ///    to resolve this problem.)
    ///  - In a extended file, the filename, track number and
    ///    track names are correct or blank.
    /// It does not check:
    ///  - That the duration of the file is correct.
    ///    (I was unable to find a simple way to get the 
    ///     music duration of an audio file from a .Net
    ///     program.)
    /// </summary>
    public class M3UFile
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "M3UFile:                   0.91";

        /// <summary>
        /// The path to the playlist file
        /// </summary>
        private string AudioPath;

        /// <summary>
        /// An array of strings to hold the lines of the M3U file.
        /// (Lines holding relative paths are updated to hold the
        /// corresponding absolute paths as part of the checking
        /// process.)
        /// </summary>
        private string[] Lines;

        /// <summary>
        /// A LazySW to write the output of the checking.
        /// i.e. Information about any error found.
        /// </summary>
        private LazySW SW;

        /// <summary>
        /// Is the current M3U and 'extended' M3U file?
        /// i.e one with a #EXTM3U format indicator and 
        /// #EXTINF marker lines.
        /// </summary>
        private bool IsExtended = false;

        /// <summary>
        /// The rest of my code can only process FLAC or MP3 files.
        /// This lists lets you specify what kind of audio file are
        /// valid in a M3U playlist.
        /// </summary>
        private List<string> ValidExtensions = new List<string>() { "FLAC", "MP3" };


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AudioPath">The path to the M3U file.</param>
        /// <param name="ResultsFile">The LazySW that will 
        /// record the output - i.e. errors in the file.</param>
        public M3UFile(string AudioPath, LazySW ResultsFile)
        {
            if (!File.Exists(AudioPath) || Helpers.JGetExtension(AudioPath) != "M3U")
                throw new Exception("File does not exist or is not MU3 in MU3 constructor.");

            this.AudioPath = AudioPath;
            SW = ResultsFile;

            // Populate the lines array with the lines of the file.
            Lines = File.ReadAllLines(AudioPath);

            // Clear any unwanted spaces.
            for (int i = 0; i < Lines.Length; i++)
                Lines[i] = Lines[i].Trim();

            // See if it is an 'Extended' M3U file.
            if (Lines.Count() > 0 && Lines[0].ToUpper() == "#EXTM3U")
                IsExtended = true;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AudioPath">The path to the M3U file.</param>
        /// <param name="ResultsFile">The path to a file that will be used to 
        /// record the output - i.e. any errors in the file.</param>
        public M3UFile(string AudioPath, string ResultsFile)
            : this(AudioPath, new LazySW(ResultsFile))
        {
        }


        /// <summary>
        /// Looks at line number 'i' in the string array then returns
        /// what kind of line it is.
        /// </summary>
        /// <param name="i">The line number of the line in the array.</param>
        /// <returns>What kind of file this is</returns>
        private LineType Categorise(int i)
        {
            // Possible drive letters
            string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Take a copy of the current line
            string L = Lines[i];

            // Record length of trimmed line
            int Length = L.Length;

            // Is it blank?
            if (Length == 0)
                return LineType.Blank;

            // If it starts with # there are three kinds it could be
            if (L[0] == '#')
            {
                if (Length == 7 && L.Substring(0, 7).ToUpper() == "#EXTM3U")
                    return LineType.Format;
                if (Length >= 7 && L.Substring(0, 7).ToUpper() == "#EXTINF")
                    return LineType.Marker;
                return LineType.Comment;
            }

            // If it starts with a letter then a colon, it's an absolute path
            if (Length > 1 && Letters.Contains(Char.ToUpper(L[0])) && L[1] == ':')
                return LineType.Absolute;

            // If it starts with HTTP:// it's a URL
            if (Length >= 7 && L.Substring(0, 7).ToUpper() == "HTTP://")
                return LineType.URL;

            // If it starts with \, /, or . it's a relative path
            if (Length > 1 && (L[0] == '/' || L[0] == '\\' || L[0] == '.'))
                return LineType.Relative;

            // Otherwise we don'y know what it is.
            return LineType.Unknown;
        }


        /// <summary>
        /// Check the file for errors
        /// </summary>
        /// <returns>True if no errors</returns>
        public bool CheckFile()
        {
            if (Lines.Count() == 0)
            {
                SW.WriteLine(AudioPath + "\tPlaylist file is empty.");
                SW.Flush();
                return false;
            }

            bool Result;
            if (IsExtended)
                Result = CheckExtendedFile();

            else
                Result = CheckSimpleFile();

            SW.Flush();
            return Result;
        }


        /// <summary>
        /// Checks a 'simple' m3U playlist for errors.
        /// A simple file has only comments and paths.
        /// </summary>
        /// <returns>True if no errors found.</returns>
        private bool CheckSimpleFile()
        {
            bool NoErrors = true;

            for (int i = 0; i < Lines.Length; i++)
            {
                switch (Categorise(i))
                {
                    case LineType.Blank:
                        SW.WriteLine(AudioPath + "\tLine " + (i + 1) + " is blank.");
                        NoErrors = false;
                        break;
                    case LineType.Comment:
                        // Do nothing
                        break;
                    case LineType.Absolute:
                        if (!CheckAbsolutePath(i))
                            NoErrors = false;
                        break;
                    case LineType.Relative:
                        if (!CheckRelativePath(i))
                            NoErrors = false;
                        break;
                    case LineType.URL:
                        if (!CheckURL(i))
                            NoErrors = false;
                        break;
                    case LineType.Format:
                        SW.WriteLine(AudioPath + "\tFormat string found at line " + (i + 1) + " in 'simple' m3u file.");
                        NoErrors = false;
                        break;
                    case LineType.Marker:
                        SW.WriteLine(AudioPath + "\tMarker string found at line " + (i + 1) + " in 'simple' m3u file.");
                        NoErrors = false;
                        break;
                    default:
                        SW.WriteLine(AudioPath + "\tUnknown line type on line " + (i + 1));
                        NoErrors = false;
                        break;
                }
            }
            return NoErrors;
        }


        /// <summary>
        /// Check an 'extended' M3U playlist file.
        /// This should start with a #EXTM3U string
        /// then have pairs of lines: #EXTINF followed
        /// by a path.
        /// </summary>
        /// <returns>True if no errors.</returns>
        private bool CheckExtendedFile()
        {
            bool NoErrors;

            // See how many lines there are in the file.
            int Count = Lines.Count();

            // If none, it's an empty file
            if (Count == 0)
            {
                SW.WriteLine(AudioPath + "\tFile is empty.");
                return false;
            }

            // Check it starts with #EXTM3U
            if (Lines[0] != "#EXTM3U")
            {
                SW.WriteLine(AudioPath + "\tFile does not start with #EXTM3U string.");
                return false;
            }

            // Check at least three lines and an odd number of lines
            if (Count < 3 || Count % 2 == 0)
            {
                SW.WriteLine(AudioPath + "\tFile does not contain lines in marker/path pairs.");
                return false;
            }

            // Check each pair of lines
            NoErrors = true;
            for (int i = 1; i < Count; i += 2)
                if (!ProcessExtendedPair(i))
                    NoErrors = false;

            return NoErrors;
        }


        /// <summary>
        /// Check a pair of lines from an extended M3U file.
        /// The firest should be a #EXTINF marker and the
        /// second a path.
        /// The Marker should contain information from the 
        /// tags within the file or blank info instead.
        /// This code DOES NOT verify the duration of the music.
        /// </summary>
        /// <param name="i">The line number of the first line
        /// of the pair to be checked</param>
        /// <returns>True if no errors found.</returns>
        private bool ProcessExtendedPair(int i)
        {
            // The audio file pointed to by the second line;
            AudioFile AF;

            // The text in the first linbe of the pair
            string TagInfoInM3UFile;

            // The tag values retrieved from the file.
            string ActualTagInfo;

            // What type is the line being examined?
            LineType LT;

            // Exit if the first of the pair is not a marker
            LT = Categorise(i);
            if (LT != LineType.Marker)
            {
                SW.WriteLine(AudioPath + "\tLine " + (i + 1) + " should be a marker (#EXTINF) but is not.");
                return false;
            }

            // See what kind of line the second of the pair is
            LT = Categorise(i + 1);

            // If it is a path, check it.
            switch (LT)
            {
                case LineType.Absolute:
                    if (!CheckAbsolutePath(i + 1))
                        return false;
                    break;
                case LineType.Relative:
                    if (!CheckRelativePath(i + 1))
                        return false;
                    break;
                case LineType.URL:
                    if (!CheckURL(i + 1))
                        return false;
                    break;
                default:
                    SW.WriteLine(AudioPath + "\tLine " + (i + 2) + " should be a path but is not.");
                    return false;
            }

            // If we get here, path is good.

            // If it's a URL no checking to do, else need
            // to check tag info is correct
            if (LT != LineType.URL)
            {
                // Extract the text after the duration field.
                TagInfoInM3UFile = Lines[i].Substring(Lines[i].IndexOf(',') + 1);

                // If it's blank there is nothing to check
                // (Remove trailing space as line has been trimmed)
                if (TagInfoInM3UFile == " -  -")
                    return true;

                // Otherwise we must compare with the tag info inside the file
                string Ext = Helpers.JGetExtension(Lines[i + 1]);
                if (Ext == "FLAC")
                    AF = new FLACFile(Lines[i + 1], true);
                else if (Ext == "MP3")
                    AF = new MP3File(Lines[i + 1], true);
                else
                {
                    SW.WriteLine(AudioPath + "\tPath points to invalid file type at line " + (i + 2));
                    return false;
                }
                // Read and concatenate the three tags.
                ActualTagInfo = AF.ALBUM + " - " + AF.TRACKNUMBER + " - " + AF.TITLE;

                // Check they match
                if (ActualTagInfo != TagInfoInM3UFile)
                {
                    SW.WriteLine(AudioPath + "\tActual tag data does not match data in marker " +
                        "on line " + (i + 2));
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Checks that a format line is OK.
        /// In other words, that it is the first line in the file.
        /// </summary>
        /// <param name="i">The position of the line within the file.</param>
        /// <returns>True if position is zero</returns>
        private bool CheckFormat(int i)
        {
            if (i != 0)
            {
                SW.WriteLine(AudioPath + "\t#EXTM3U should only appear at start of file.  Found on line " + (i + 1));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Checks that a URL path is valid.
        /// This is the only code that I do not really understand.
        /// I cobbled it together from various sources on the Internet,
        /// </summary>
        /// <param name="i">The position of the lien in the file.</param>
        /// <returns>True is URL is valid</returns>
        private bool CheckURL(int i)
        {
            string Line = Lines[i];
            try
            {
                HttpWebRequest Request = HttpWebRequest.Create(Line) as HttpWebRequest;
                Request.Timeout = 5000;
                Request.Method = "HEAD";

                using (HttpWebResponse Response = Request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)Response.StatusCode;

                    if (statusCode >= 100 && statusCode < 400)
                        return true;
                    else
                    {
                        SW.WriteLine(AudioPath + "\tURL no response on line " + (i + 1));
                        return false;
                    }
                }
            }
            catch (Exception Ex)
            {
                SW.WriteLine(AudioPath + "\tURL not valid on line " + (i + 1) + " (" + Ex.Message + ")");
                return false;
            }
        }


        /// <summary>
        /// Checks that an absolute path is valid
        /// </summary>
        /// <param name="i">The position of the line within the file.</param>
        /// <returns>True if path is valid</returns>
        private bool CheckAbsolutePath(int i)
        {
            // Check that it exists
            if (!File.Exists(Lines[i]))
            {
                SW.WriteLine(AudioPath + "\tCannot resolve absolute path on line " + (i + 1));
                return false;
            }
            // Check that it points to a valid file type.
            if (!ValidExtensions.Contains(Helpers.JGetExtension(Lines[i])))
            {
                SW.WriteLine(AudioPath + "\tPath points to unsupported file type on line " + (i + 1));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Checks that a realtive path exists
        /// </summary>
        /// <param name="i">The position of the line within the file</param>
        /// <returns>True if relative path is valid</returns>
        private bool CheckRelativePath(int i)
        {
            // Get the directory holding the M3U file
            string CurrentDir = Path.GetDirectoryName(AudioPath);

            // Calculate an absolute path
            string AbsPath;
            if (!Helpers.AbsolutePath(Lines[i], CurrentDir, out AbsPath))
            {
                SW.WriteLine(AudioPath + "\tRelative path is incompatible with starting path at line " + (i + 1));
                return false;
            }

            // Check that it exists
            if (!File.Exists(AbsPath))
            {
                SW.WriteLine(AudioPath + "\tCannot resolve relative path on line " + (i + 1));
                return false;
            }
            // Check that it points to a valid file type.
            if (!ValidExtensions.Contains(Helpers.JGetExtension(AbsPath)))
            {
                SW.WriteLine(AudioPath + "\tPath points to unsupported file type on line " + (i + 1));
                return false;
            }

            // Record the absolute path
            Lines[i] = AbsPath;

            return true;
        }
    }
}