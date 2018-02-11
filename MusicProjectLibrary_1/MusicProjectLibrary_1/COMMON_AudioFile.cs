//========================================================================
// Name:     COMMON_Audiofile.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements the AudioFile class.
//           This is the parent class for a set of classes to hold the 
//           metadata for audio files.
//           (Currently only flac and MP3/ID3 v2.3 are implemented.)
//           This class is intended to implement properties and methods 
//           common to all such files.
// Comments: 
//========================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace JAudioTags
{
    /// <summary>
    /// Used to indicate what kind of file we are working with?
    /// (mu3 not currently implemented.)
    /// </summary>
    public enum AudioFileTypes
    {
        /// <summary>
        /// File is a 'FLAC' audio file.
        /// </summary>
        flac,
        /// <summary>
        /// File is an 'MP3' audio file.
        /// </summary>
        mp3,
        /// <summary>
        /// File is a 'M3U' playlist file
        /// </summary>
        m3u
    }



    /// <summary>
    /// Parent class for audio files of all supported kinds: Flac; MP3.
    /// Attemps to capture all common elements.
    /// </summary>
    public abstract class AudioFile
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "AudioFile:                 1.05";


        /// <summary>
        /// The path to the audio file
        /// </summary>
        public string AudioPath { get; protected set; }


        /// <summary>
        /// If true the file can be read but cannot be 
        /// saved.
        /// </summary>
        protected bool ReadOnly;


        /// <summary>
        /// How long was the file before we made changes?
        /// </summary>
        protected long OriginalTotalFileLength;


        /// <summary>
        /// Where, within the file does the music start?
        /// </summary>
        protected long StartOfMusic;


        /// <summary>
        /// How long is the music section of the file?
        /// (This should not change as tags are edited.)
        /// </summary>
        public long NumberOfMusicBytes { get; protected set; }


        /// <summary>
        /// Is there an embedded image?
        /// </summary>
        public bool HasEmbeddedGraphic { get; protected set; }


        /// <summary>
        /// Do we need to rewrite the entire file?
        /// (Or can we get away with just overwriting the bits that 
        /// have changed?)
        /// </summary>
        protected bool MustRewriteWholeFile;


        /// <summary>
        /// Is tag name matching case sensitive?
        /// </summary>
        static public bool IsCaseSensitive = false;


        /// <summary>
        /// Holds the main list of tags.
        /// </summary>
        internal TagList PrimaryTagList;


        /// <summary>
        /// Mp3 files can have two sets of tags: V2.x and V1.
        /// This holds the deprecated v1 tags.
        /// </summary>
        internal TagList SecondaryTagList;


        /// <summary>
        /// Called to commit in-memory tag changes to file.
        /// </summary>
        /// <param name="MakeBackup">Should we make a backup?</param>
        public void Save(bool MakeBackup)
        {
            if (ReadOnly)
                throw new Exception("Attempting to save a file that was opened as ReadOnly.");

            // Use the amended tag list to make changes to the in-memory
            // representation of the file metadata.
            InMemoryUpdate();

            if (MustRewriteWholeFile)
            {
                // Rewrite entire file
                string TempFileName = "JAudioTags.tmp";

                // Build path of temp file
                string Folder = Path.GetDirectoryName(AudioPath);
                string TempPath = Folder + "\\" + TempFileName;

                RewriteWholeFile(TempPath);

                if (MakeBackup)
                    Helpers.MakeBackup(AudioPath);

                // Delete the orginal file
                File.Delete(AudioPath);
                // The rename the temp file as the original
                File.Move(TempPath, AudioPath);
            }
            else
            {
                // Just overwrite changed parts of file.
                if (MakeBackup)
                    Helpers.MakeBackup(AudioPath);
                OverwriteWindow();
            }
        }


        /// <summary>
        /// Walks a tree and visits every FILE.  Logs results and errors 
        /// to file.
        /// </summary>
        /// <param name="Root">Root of the tree to be traversed</param>
        /// <param name="ProcessFile">A method to process each file
        /// in turn.</param>
        /// <param name="FileExtensions">A list of file extensions to
        /// match: "mp3" and "flac"</param>
        /// <param name="LogPath">Path to results log file</param>
        /// <param name="ErrorLogPath">Path to error log file</param>
        static public int FileWalk(string Root,
            Func<string, LazySW, int> ProcessFile,
            List<string> FileExtensions, string LogPath, string ErrorLogPath)
        {
            int Result;
            using (TreeWalker DTW = new TreeWalker(Root, ProcessFile,
                                            FileExtensions, LogPath, ErrorLogPath))
            {
                Result = DTW.FileWalk();
            }
            return Result;
            
        }


        /// <summary>
        /// Walks a tree and visits every FILE.  DOES NOT log results and 
        /// errors to file.
        /// </summary>
        /// <param name="Root">Root of the tree to be traversed</param>
        /// <param name="ProcessFile">A method to process each file 
        /// in turn.</param>
        /// <param name="FileExtensions">A list of file extensions to
        /// match: "mp3" and "flac"</param>
        static public int FileWalk(string Root,
            Func<string, LazySW, int> ProcessFile,
            List<string> FileExtensions)
        {
            int Result;
            using (TreeWalker FW = new TreeWalker(Root, ProcessFile,
                FileExtensions, null, null))
            {
                Result = FW.FileWalk();
            }
            return Result;
        }


        /// <summary>
        /// Walks a tree and vists every DIRECTORY.  Logs results and errors to file.
        /// </summary>
        /// <param name="Root">Root of the tree to be traversed.</param>
        /// <param name="ProcessDirectory">A method to process each directory in 
        /// turn.</param>
        /// <param name="FileExtensions">A list of file extensions to 
        /// match: "mp3" and "flac"</param>
        /// <param name="ResultsPath">Path to results log file</param>
        /// <param name="ErrorLogPath">Path to error log file</param>
        static public int DirWalk(string Root,
            Func<string, List<string>, LazySW, int> ProcessDirectory,
            List<string> FileExtensions, string ResultsPath, string ErrorLogPath) //
        {
            int Result;
            using (TreeWalker DW = new TreeWalker(Root, ProcessDirectory,
                FileExtensions, ResultsPath, ErrorLogPath))
            {
                Result = DW.DirWalk();
            }
            return Result;
        }

        //==================================================================
        // Methods to be implemented by derived classes
        //==================================================================

        /// <summary>
        /// Display debugging information on console.
        /// </summary>
        /// <param name="IncludeDetail">Should we display internal 
        /// details?</param>
        /// <param name="IncludeTags">Should we also display tags?</param>
        public abstract void DebugPrint(bool IncludeDetail, bool IncludeTags);


        /// <summary>
        /// Rewrites the whole of the file.
        /// </summary>
        /// <param name="TempPath">Path to hold temporary file</param>
        protected abstract void RewriteWholeFile(string TempPath);


        /// <summary>
        /// Rewrites just the changed subset of the file.
        /// </summary>
        protected abstract void OverwriteWindow();


        /// <summary>
        /// Reflects tag changes in in-memory data structures.
        /// </summary>
        protected abstract void InMemoryUpdate();


        /// <summary>
        /// Debugging method.
        /// Generates a file with an unusual internal layout for test 
        /// purposes.
        /// </summary>
        public abstract void SaveTestFile();


        /// <summary>
        /// Property wrapping the ALBUM tag
        /// </summary>
        public string ALBUM
        {
            get { return ReturnTag("ALBUM"); }
            set { SetTag("ALBUM", value); }
        }


        /// <summary>
        /// Property wrapping the ALBUMARTIST tag
        /// </summary>
        public string ALBUMARTIST
        {
            get { return ReturnTag("ALBUMARTIST"); }
            set { SetTag("ALBUMARTIST", value); }
        }


        /// <summary>
        /// Property wrapping the ARTIST tag
        /// </summary>
        public string ARTIST
        {
            get { return ReturnTag("ARTIST"); }
            set { SetTag("ARTIST", value); }
        }


        /// <summary>
        /// Property wrapping the COMPOSER tag
        /// </summary>
        public string COMPOSER
        {
            get { return ReturnTag("COMPOSER"); }
            set { SetTag("COMPOSER", value); }
        }


        /// <summary>
        /// Property wrapping the DISCNUMBER tag
        /// </summary>
        public string DISCNUMBER
        {
            get { return ReturnTag("DISCNUMBER"); }
            set { SetTag("DISCNUMBER", value); }
        }


        /// <summary>
        /// Property wrapping the GENRE tag
        /// </summary>
        public string GENRE
        {
            get { return ReturnTag("GENRE"); }
            set { SetTag("GENRE", value); }
        }


        /// <summary>
        /// Property wrapping the TITLE tag
        /// </summary>
        public string TITLE
        {
            get { return ReturnTag("TITLE"); }
            set { SetTag("TITLE", value); }
        }


        /// <summary>
        /// Property wrapping the TRACKNUMBER tag
        /// </summary>
        public string TRACKNUMBER
        {
            get { return ReturnTag("TRACKNUMBER"); }
            set { SetTag("TRACKNUMBER", value); }
        }


        /// <summary>
        /// Property wrapping the DATE tag
        /// </summary>
        public string DATE
        {
            get { return ReturnTag("DATE"); }
            set { SetTag("DATE", value); }
        }


        /// <summary>
        /// Property wrapping the COMMENT tag
        /// </summary>
        public string COMMENT
        {
            get { return ReturnTag("COMMENT"); }
            set { SetTag("COMMENT", value); }
        }

        public string RATING
        {
            get { return ReturnTag("RATING"); }
            set { SetTag("RATING", value); }
        }
        public string INDEXTRACK
        {
            get { return ReturnTag("INDEXTRACK"); }
            set { SetTag("INDEXTRACK", value); }
        }
        public string INDEXALBUM
        {
            get { return ReturnTag("INDEXALBUM"); }
            set { SetTag("INDEXALBUM", value); }
        }
        public string MODTAGDATE
        {
            get { return ReturnTag("MODTAGDATE"); }
            set { SetTag("MODTAGDATE", value); }
        }
        public string STYLE
        {
            get { return ReturnTag("STYLE"); }
            set { SetTag("STYLE", value); }
        }
        /// <summary>
        /// If the current file is a FLAC file this searches the
        /// tag list for the name as passed in and returns the 
        /// corresponding value.
        /// If the file is MP3 it maps the common tag name
        /// (e.g. ARTIST) to the ID3 tag name (e.g. TPE1)
        /// </summary>
        /// <param name="Name">The name of the tag being queried</param>
        /// <returns></returns>
        private string ReturnTag(string Name)
        {
            if (PrimaryTagList.AudioFileType == AudioFileTypes.mp3)
            {
                // It's an MP3 file
                {
                    string T = TagMappings.ToID3v23[Name];
                    if (PrimaryTagList.Exists(T))
                        return PrimaryTagList.First(T);
                    else
                        return "";
                }
            }
            else
            {
                // It's a FLAC file
                if (PrimaryTagList.Exists(Name))
                    return PrimaryTagList.First(Name);
                else
                    return "";
            }
        }


        /// <summary>
        /// Removes all existing tags with name = Name
        /// and adds a new tag with name = Name and value = Value.
        /// If the file is a FLAC file name is unaltered.
        /// If the file is an ID3 file it is replaced with the
        /// IDE equivalent. (e.g. ALBUM is replaced with TALB)
        /// </summary>
        /// <param name="Name">The Tag Name</param>
        /// <param name="Value">The value of the new tag</param>
        private void SetTag(string Name, string Value)
        {
            if (PrimaryTagList.AudioFileType == AudioFileTypes.mp3)
                PrimaryTagList.ReplaceAll(TagMappings.ToID3v23[Name], Value);
            else
                PrimaryTagList.ReplaceAll(Name, Value);
        }


        /// <summary>
        /// Returns the number of primary tags
        /// </summary>
        public int NumberOfTags
        {
            get
            {
                return PrimaryTagList.CountTags();
            }
        }


        /// <summary>
        /// Makes this class enumerable by publishing the 
        /// enumerator of the Tag list member
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return PrimaryTagList.GetEnumerator();
        }


        /// <summary>
        /// Private method to be used by public Dump() method below.
        /// Walks a tree and dumps core file attributes to a
        /// tab separated file.
        /// </summary>
        /// <param name="FileName">The current file name</param>
        /// <param name="SW">The 'lazy' StreamWriter to write to</param>
        /// <returns>One for each file dumped.</returns>
        static private int GetBasicDetails(string FileName, LazySW SW)
        {
            AudioFile AF;

            if (Helpers.JGetExtension(FileName) == "MP3")
                AF = new MP3File(FileName, true);
            else if (Helpers.JGetExtension(FileName) == "FLAC")
                AF = new FLACFile(FileName, true);
            else
                return 0;

            StringBuilder SB = new StringBuilder();

            SB.Append(Path.GetDirectoryName(AF.AudioPath) + "\t");
            SB.Append(Path.GetFileName(AF.AudioPath) + "\t");
            SB.Append(AF.TITLE + "\t");
            SB.Append(AF.ALBUM + "\t");
            SB.Append(AF.ARTIST + "\t");
            SB.Append(AF.ALBUMARTIST + "\t");
            SB.Append(AF.GENRE + "\t");
            SB.Append(AF.DATE + "\t");
            SB.Append(AF.TRACKNUMBER + "\t");
            SB.Append(AF.COMMENT);

            SW.WriteLine(SB.ToString());
            Console.WriteLine(SB);
            return 1;
        }


        /// <summary>
        /// Static method to walk a tree and dump basic attributes
        /// of all FLAC and MP3 files to a file.
        /// </summary>
        /// <param name="Root">Roor of the tree</param>
        /// <param name="Results">Path of log file</param>
        /// <param name="Errors">Path of error file.</param>
        /// <returns>The number of files dumped.</returns>
        static public int Dump(string Root, string Results, string Errors)
        {
            int Count = 0;

            Count += AudioFile.FileWalk(Root, GetBasicDetails, new List<string>() { "MP3", "FLAC" }, Results, Errors);

            return Count;
        }
    }
}