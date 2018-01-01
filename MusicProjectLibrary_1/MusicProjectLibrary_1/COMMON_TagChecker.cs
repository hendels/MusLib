//========================================================================
// Name:     TagChecker.cs
// Author:   Jim Roberts 
// Date:     March 2017
// Purpose:  Performs a set of checks on the tags in a tree of FLAC, MP3
//           and M3U files. 
// Comments: 
//========================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace JAudioTags
{
    /// <summary>
    /// Implements a set of checks that are applied against a collection
    /// of audio files.
    /// </summary>
    public class TagChecker
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string Version = "TagChecker:                0.13";


        /// <summary>
        /// Counts the number of files examined.
        /// </summary>
        private int FilesProcessed = 1;


        /// <summary>
        /// Regular expression to match a 'full' date : yyyy-mm-dd
        /// </summary>
        static private Regex RegExFull = new Regex(@"^(19|20)\d\d-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$");


        /// <summary>
        /// Regular expression to match a year-only date : yyyy
        /// </summary>
        static private Regex RegExYear = new Regex(@"^(19|20)\d\d$");


        /// <summary>
        /// Array containing all the characters that are acceptable
        /// in a file name - for compatiblity with Windows and Unix.
        /// </summary>
        private char[] GoodCharsList = ("abcdefghijklmnopqrstuvwxyz" +
                                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" +
                                        " .,-_()[]'").ToCharArray();


        /// <summary>
        /// List of tags that must be present in ALL audio files.
        /// </summary>
        private List<string> MandatoryTagsList = new List<string>() { "TITLE", "ARTIST", "ALBUMARTIST", "TRACKNUMBER", "DATE", "ALBUM", "GENRE", "DISCNUMBER" };


        /// <summary>
        /// Holds a list of file types that should be omitted
        /// from checking - populated via constructor.
        /// </summary>
        private List<string> IgnoreList = new List<string>() { };


        /// <summary>
        /// A list of tags that must be the same for all tracks in an album/folder.
        /// </summary>
        private List<string> InvariantTagsList = new List<string>() { "ALBUMARTIST", "DATE", "ALBUM", "GENRE", "DISCNUMBER" };


        /// <summary>
        /// A list of tags that should be entirely numeric.
        /// Will check that they are.
        /// </summary>
        private List<string> NumericTags = new List<string>() { "TRACKNUMBER", "DISCNUMBER" };


        /// <summary>
        /// Used to hold the mandatory tag information for all files in a directory.
        /// The tags in each file are put into an array.
        /// Each new array is added to this list.
        /// </summary>
        private List<string[]> TagList = new List<string[]>();


        /// <summary>
        /// MandatoryTagsList lists all mandatory tags.  
        /// InvariantTagsList lists tags that must be the same for all 
        /// tracks in an album/folder.
        /// This lists lists the positions of the latter within the former.
        /// i.e. The numbers of the columns whose tag values must be invariant.
        /// </summary>
        private List<int> InvariantColumns = new List<int>();


        /// <summary>
        /// NumericTags holds the names of tag which should be numeric.
        /// This is loaded with the corresponding column numbers.
        /// </summary>
        private List<int> NumericColumns = new List<int>() { };


        /// <summary>
        /// Should we check the number of digits in the track numbers
        /// </summary>
        private bool CheckTrackDigits = false;


        /// <summary>
        /// Should we check that all files in a folder have the same genre?
        /// </summary>
        private bool CheckInvariantGenres = false;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IList">Optional list of file types to be ignored</param>
        /// <param name="CheckTrackDigits">Should we check how many digits in track numbers?</param>
        /// <param name="CheckInvariantGenres">Should we check if all tracks in an album have the same genre?</param>
        /// <param name="CheckInvariantDates">Should we check if all the tracks in an album have the same date</param>
        public TagChecker(List<string> IList = null,
            bool CheckTrackDigits = false,
            bool CheckInvariantGenres = false,
            bool CheckInvariantDates = false)
        {
            // Store the ignore list
            if (IList != null)
            {
                for (int i = 0; i < IList.Count; i++)
                    IList[i] = IList[i].ToUpper();
                IgnoreList = IList;
            }

            // Store whether or not we should check digits, genres and dates?
            this.CheckTrackDigits = CheckTrackDigits;
            this.CheckInvariantGenres = CheckInvariantGenres;
            this.CheckInvariantGenres = CheckInvariantDates;

            // Populate the invariant columns list.
            // Leave off DATE and GENRE unless switches present.
            foreach (var Tag in InvariantTagsList)
            {
                int Pos = MandatoryTagsList.FindIndex(t => t == Tag);

                if (Pos < 0 || Pos >= MandatoryTagsList.Count())
                    throw new Exception("Invalid invariant position in TagChecker constructor().");

                if ((Tag == "DATE" && !CheckInvariantDates) || (Tag == "GENRE" && !CheckInvariantGenres))
                    continue;

                InvariantColumns.Add(Pos);
            }

            foreach (string Tag in NumericTags)
            {
                int Pos = MandatoryTagsList.FindIndex(t => t == Tag);

                if (Pos < 0 || Pos >= MandatoryTagsList.Count())
                    throw new Exception("Invalid numeric position in TagChecker constructor().");

                NumericColumns.Add(Pos);
            }
        }


        /// <summary>
        /// This is the key method of the class.
        /// It is called for each directory in the tree being checked.
        /// It does a few checks itself and calls other methods for other
        /// checks.
        /// </summary>
        /// <param name="CurrentDir">The directory to be checked</param>
        /// <param name="Exts">Not used in this application.  
        /// (Reuired by DIrWalk method)</param>
        /// <param name="ResultsFile">A 'lazy' stream writer to record
        /// any tag errors found.</param>
        /// <returns>1 - Because the DirWalk method requires 
        /// a method that returns an int.</returns>
        private int ProcessDirectory(string CurrentDir, List<string> Exts, LazySW ResultsFile)
        {
            // Does this directory hold a FLAC file?
            bool DirHasFLAC = false;

            // Does this directory hold an MP3 file? 
            bool DirHasMP3 = false;

            // Does this directory hold an M3U file?
            bool DirHasM3U = false;

            // Does this directory hold a PNG file - only playlist folders
            bool DirHasPNG = false;

            // Does this folder hold a JPG file?
            bool DirHasJpg = false;

            // Does this directory hold a folder.jpg file
            bool DirHasFolderJpg = false;

            // String to accumulate 'bad' characters found in a file name.
            string BadChars = "";

            // The name of the durrent file.
            string Name;

            // The extension of the current file.
            string Extension;

            // An array of strings - the names of all of the 
            // files within the current directory.
            string[] FileSet = Directory.GetFiles(CurrentDir);

            // An AudioFile - used to represent any FLAC or MP3
            // files encountered so that its tags can be read.
            AudioFile AF;

            // An M3U file - used to represent any M3U
            // encountered so that it can be checked.
            M3UFile M3U;

            // We are in a processing a new directoy, so clear the 
            // Tag list ready for a new set.
            TagList.Clear();

            // Look at each file in the directory in turn
            for (int i = 0; i < FileSet.Count(); i++)
            {
                // Write its name to the screen
                Console.WriteLine(FilesProcessed++ + " - " + FileSet[i]);

                // Get name and extension
                Name = Path.GetFileName(FileSet[i]);
                Extension = Helpers.JGetExtension(FileSet[i]);

                // Skip this file if its type is on the ignore list
                if (IgnoreList.Contains(Extension))
                    continue;

                // Flag any 'bad' characters in its file name
                bool BadCharsFound = false;
                foreach (char Ch in Name)
                    if (!GoodCharsList.Contains(Ch))
                    {
                        BadChars += Ch;
                        BadCharsFound = true;
                    }
                if (BadCharsFound)
                    ResultsFile.WriteLine(FileSet[i] + "\tBad chars " + "(" + BadChars + ")" + " in file name.");

                // Instantiate file types that will be read 
                // and check file compinations
                if (Extension == "FLAC")
                {
                    AF = new FLACFile(FileSet[i], true);
                    CheckDateAndGraphic(AF, Extension, ResultsFile);
                    AddToTagList(AF, ResultsFile);
                    DirHasFLAC = true;
                }
                else if (Extension == "MP3")
                {
                    AF = new MP3File(FileSet[i], true);
                    CheckDateAndGraphic(AF, Extension, ResultsFile);
                    AddToTagList(AF, ResultsFile);
                    DirHasMP3 = true;
                }
                else if (Extension == "M3U")
                {
                    M3U = new M3UFile(FileSet[i], ResultsFile);
                    M3U.CheckFile();
                    DirHasM3U = true;
                }
                else if (Extension == "PNG")
                    DirHasPNG = true;
                else if (Extension == "JPG")
                {
                    DirHasJpg = true;
                    if (Name.ToUpper() == "FOLDER.JPG")
                        DirHasFolderJpg = true;
                }
                else
                    ResultsFile.WriteLine(FileSet[i] + "\tUnexpected file encountered.");
            }  // for


            // Check for invalid file type combinations
            if (DirHasFLAC && DirHasMP3)
                ResultsFile.WriteLine(CurrentDir + "\tDirectory has both FLAC and MP3 files.");

            if ((DirHasMP3 || DirHasFLAC) && DirHasM3U)
                ResultsFile.WriteLine(CurrentDir + "\tDirectory has both music and playlist files.");

            if ((DirHasMP3 || DirHasFLAC) && DirHasPNG)
                ResultsFile.WriteLine(CurrentDir + "\tPNG file in music folder.");

            if ((DirHasFLAC || DirHasMP3) && !DirHasFolderJpg)
                ResultsFile.WriteLine(CurrentDir + "\tNo graphic file in music folder.");

            if ((DirHasFLAC || DirHasMP3) && DirHasJpg && !DirHasFolderJpg)
                ResultsFile.WriteLine(CurrentDir + "\tExtra JPG file(s) in music folder.");

            // We have added tag information from each file to the TagList list
            // Now analyse that information.

            if (FileSet.Count() > 0)
                VerifyTagsAreNumeric(CurrentDir, ResultsFile);

            if (FileSet.Count() > 1)
                VerifyTagsAreInvariant(CurrentDir, ResultsFile);

            // Use the TagList list to check tag numbers are OK
            if (CheckTrackDigits && FileSet.Count() > 1)
                CheckTrackNumbers(CurrentDir, ResultsFile);

            return 1;
        }


        /// <summary>
        /// Reads the tags from an audio file and adds them to
        /// the TagList list so that they can be checked later.
        /// </summary>
        /// <param name="AF">The AudioFile representation of the current file</param>
        /// <param name="SW">A 'lazy' StreamWriter to record
        /// any tagging problems found.</param>
        private void AddToTagList(AudioFile AF, LazySW SW)
        {
            string Temp;

            // Make a 1-D array to hold the tags of this file
            string[] TempArray = new string[MandatoryTagsList.Count()];

            // For each mandatory tag, check it is not blank and add
            // it to the new array.
            for (int i = 0; i < MandatoryTagsList.Count(); i++)
            {
                Temp = GetVal(AF, MandatoryTagsList[i]);
                if (Temp == "")
                    SW.WriteLine(AF.AudioPath + "\tTag " + MandatoryTagsList[i] + "' is empty.");
                TempArray[i] = Temp;
            }
            // Add this file's array to the TagList list
            TagList.Add(TempArray);
        }


        /// <summary>
        /// Checks the date format of the current music file.
        /// </summary>
        /// <param name="AF">The current audio file.</param>
        /// <param name="Extension">The extenion of the current file name</param>
        /// <param name="SW">'Lazy' StreamWriter to record errors.</param>
        private void CheckDateAndGraphic(AudioFile AF, string Extension, LazySW SW)
        {
            // Check dates
            if (Extension == "FLAC")
                if (!RegExFull.IsMatch(AF.DATE))
                    SW.WriteLine(AF.AudioPath + "\tDate has incorrect format.");
            if (Extension == "MP3")
                if (!RegExYear.IsMatch(AF.DATE))
                    SW.WriteLine(AF.AudioPath + "\tDate has incorrect format.");
        }


        ///// <summary>
        ///// Debugging method.  Dumps TagList to console.
        ///// </summary>
        //private void DumpTagList()
        //{
        //    Console.WriteLine();
        //    foreach (var Row in TagList)
        //    {
        //        foreach (var Item in Row)
        //            Console.Write(String.Format("{0}   ", Item.Length > 8 ? Item.Substring(0, 8) : Item));
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //}


        /// <summary>
        /// Checks that all tags that should not vary do not.
        /// For example, all ALBUM tags should be the same for all
        /// files in the same diretory.
        /// </summary>
        /// <param name="CurDir">The current directory</param>
        /// <param name="SW">'Lazy' StreamWriter to record errors.</param>
        private void VerifyTagsAreInvariant(string CurDir, LazySW SW)
        {
            List<string> BadCols = new List<string>() { };

            if (TagList.Count() > 1)
                foreach (var Col in InvariantColumns)
                    for (int Row = 0; Row < TagList.Count() - 1; Row++)
                        if (TagList[Row][Col] != TagList[Row + 1][Col])
                            if (!BadCols.Contains(MandatoryTagsList[Col]))
                                BadCols.Add(MandatoryTagsList[Col]);
            if (BadCols.Count > 0)
            {
                string Temp = "";
                foreach (var Col in BadCols)
                    Temp += Col + " ";
                SW.WriteLine(CurDir + "\tInconsistent tags in columns: " + Temp);
            }
        }


        /// <summary>
        /// Checks track numbers in a folder:
        /// - No duplicates.
        /// - If > 9 tracks 2-digit numbers are used.
        /// - If > 99 tracks 3-digit numbers are used.
        /// </summary>
        /// <param name="CurDir">The current directory</param>
        /// <param name="SW">'Lazy' StreamWriter to record errors.</param>
        private void CheckTrackNumbers(string CurDir, LazySW SW)
        {
            // WHich column holds TRACKNUMBERs?
            int TrackCol = MandatoryTagsList.FindIndex(t => t == "TRACKNUMBER");

            // How many fils in this folder?
            int HowMany = TagList.Count();

            // Used to check for duplicate track numbers
            List<string> Duplicates = new List<string>() { };

            // Accumulates error messages
            List<string> Msgs = new List<string>() { };

            // Two error messages
            string InsuffcientMsg = "Track number has insufficient digits.";
            string DuplicateMsg = "Duplicate track numbers found.";

            // Look at each file in turn
            for (int Row = 0; Row < HowMany; Row++)
            {
                string Current = TagList[Row][TrackCol];

                if ((HowMany > 99 && Current.Length < 3)
                    || (HowMany > 9 && Current.Length < 2))
                    if (!Msgs.Contains(InsuffcientMsg))
                        Msgs.Add(InsuffcientMsg);

                if (Duplicates.Contains(Current))
                {
                    if (!Msgs.Contains(DuplicateMsg))
                        Msgs.Add(DuplicateMsg);
                }
                else
                    Duplicates.Add(Current);
            }

            if (Msgs.Count > 0)
                foreach (string Msg in Msgs)
                    SW.WriteLine(CurDir + "\t" + Msg);
        }


        /// <summary>
        /// Checks that tags that should be purely numeric
        /// are purely numeric.
        /// </summary>
        /// <param name="CurDir">The current directory</param>
        /// <param name="SW">StreamWriter for results</param>
        private void VerifyTagsAreNumeric(string CurDir, LazySW SW)
        {
            int Dummy;
            int HowMany = TagList.Count;
            bool ProblemFound = false;

            foreach (int Column in NumericColumns)
            {
                for (int Row = 0; Row < HowMany; Row++)
                    if (!int.TryParse(TagList[Row][Column], out Dummy))
                        ProblemFound = true;
            }
            if (ProblemFound)
                SW.WriteLine(CurDir + "\t" + "Invalid numeric tag.");
        }


        /// <summary>
        /// Takes a tag name from the mandatory tags list
        /// and retrieves the corresponding value from
        /// within the audio file.
        /// </summary>
        /// <param name="AF">The Audiofile</param>
        /// <param name="Tag">The name of the tag</param>
        /// <returns></returns>
        private string GetVal(AudioFile AF, string Tag)
        {
            if (Tag == "ALBUM")
                return AF.ALBUM;
            else if (Tag == "ALBUMARTIST")
                return AF.ALBUMARTIST;
            else if (Tag == "ARTIST")
                return AF.ARTIST;
            else if (Tag == "COMMENT")
                return AF.COMMENT;
            else if (Tag == "COMPOSER")
                return AF.COMPOSER;
            else if (Tag == "DATE")
                return AF.DATE;
            else if (Tag == "DISCNUMBER")
                return AF.DISCNUMBER;
            else if (Tag == "GENRE")
                return AF.GENRE;
            else if (Tag == "TITLE")
                return AF.TITLE;
            else if (Tag == "TRACKNUMBER")
                return AF.TRACKNUMBER;
            else
                throw new Exception("Attempting to read non-supported tag: " + Tag);
        }


        /// <summary>
        /// This is the method called by client code to perform the checking.
        /// Does a 'directory walk' and calls ProcessDirectory() against
        /// each directory encountered.
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="ResultsFile"></param>
        /// <param name="ErrorFile"></param>
        public void CheckFiles(string Root, string ResultsFile, string ErrorFile)
        {
            if (!Directory.Exists(Root))
            {
                Console.WriteLine("Path to files does not exist.");
                return;
            }
            Stopwatch Watch = new Stopwatch();
            Watch.Start();
            AudioFile.DirWalk(Root, ProcessDirectory, null, ResultsFile, ErrorFile);
            Watch.Stop();
            Console.WriteLine("Time elapsed: {0}", Watch.Elapsed);
        }
    }
}