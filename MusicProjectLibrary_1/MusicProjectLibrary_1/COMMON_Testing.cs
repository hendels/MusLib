//========================================================================
// Name:     COMMON_Testing.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements a class with a couple of methods enabling 
//           rudimentary testing of the rest of the code.  The key method
//           (Test()) simply accesses each audio file in a folder in turn, 
//           and for each one adds and removes tags, saves and reads, 
//           then checks that some of basic properties of the file are 
//           still OK. What tag manipulations are done is set in the 
//           method TestFiles().
// Comments: These methods do not prove that there are no bugs.  All they
//           do is raise confidence that basic functionality is working.
//========================================================================
using System;
using System.IO;
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// A class to allow some rudimentary testing of the rest of the
    /// code.
    /// </summary>
    static public class Testing
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "Testing:                   2.10";


        /// <summary>
        /// Information about the current audio file
        /// </summary>
        static private string CurrentFilePath;
        static private long InitialLengthOfMusic;
        static private int InitialTagCount;

        // An audio file object made from disc file.
        static private AudioFile MusicFile;

        // MP3 or FLAC?
        static private AudioFileTypes Type;

        // Were the properties OK after being changed?
        static private bool PropertiesOK;

        // Streamwriter used for writing results file.
        static private LazySW TheWriter;

        /// <summary>
        /// List of tags added during testing.
        /// Used to avoid errors trying to delete tags
        /// that have not been added.
        /// </summary>
        static private List<FLACTag> FLACTags = new List<FLACTag>();
        static private List<MP3Tag> MP3Tags = new List<MP3Tag>();

        /// <summary>
        /// String used as value of tags added
        /// </summary>
        static private string Payload = "abcdefghijklmnopqrstuvwxyz";


        /// <summary>
        /// This is the method we call from outside.
        /// It performs a rudimentary test routine.
        /// Point it at a test directory containing a bunch of flac and mp3 
        /// files and it:
        ///  1.  Makes a backup of each file
        ///  2.  Makes some changes to the file
        ///  3.  Checks that all still seems OK with the file
        ///  4.  Restores the file from backup
        ///  
        /// The method above (TestFiles()) controls what tests are done.
        /// 
        /// This does not prove there are no bugs, it just increases confidence.
        /// </summary>
        /// <param name="RootPath">Path to the test directory</param>
        /// <param name="ResultsPath">Path to a results file</param>
        /// <param name="ErrorLogPath">Path to an error log file</param>
        static public void Test(string RootPath, string ResultsPath, string ErrorLogPath)
        {
            int Result;

            if (Directory.Exists(RootPath))
            {
                Result = AudioFile.FileWalk(RootPath, TestFiles, new List<string> { "flac", "mp3" }, ResultsPath, ErrorLogPath);
                Console.WriteLine("Files processed: " + Result);
            }
            else
                Console.WriteLine("Root path not found: " + RootPath);
        }


        /// <summary>
        /// This is key method.  Modify it to change the tests performed.
        /// </summary>
        /// <param name="FileName">The path to the file to be tested</param>
        /// <param name="SW">A lazy StreamWriter to log results</param>
        /// <returns></returns>
        static private int TestFiles(string FileName, LazySW SW)
        {
            // Write file name to screen and log
            Console.WriteLine(Path.GetFileName(FileName));
            SW.WriteLine(Path.GetFileName(FileName));

            // Store arguments
            CurrentFilePath = FileName;
            TheWriter = SW;

            // See which kind of audio file it is.
            if (Helpers.JGetExtension(CurrentFilePath) == "FLAC")
                Type = AudioFileTypes.flac;
            else
                Type = AudioFileTypes.mp3;

            // Make a backup of the file before we change it.
            Helpers.MakeBackup(CurrentFilePath);

            // Instantiate an object
            MusicFile = MakeNewFile();

            // Capture initial sizes
            InitialLengthOfMusic = MusicFile.NumberOfMusicBytes;
            InitialTagCount = MusicFile.NumberOfTags;


            // Make an arbitrary set of edits to the file.  
            // Check basic parameters are still OK afterwards.
            RecordTagCount("Before changes:");
            ChangeProperties();
            RecordTagCount("After changes:");
            AddTags(1, false);
            AddTags(1, true);
            RecordTagCount("After first adds (1):");

            MusicFile = MakeNewFile();
            RecordTagCount("After first reload:");
            AddTags(1, false);
            AddTags(150, true);
            RecordTagCount("After 150 Add:");

            MusicFile = MakeNewFile();
            RecordTagCount("After second reload:");
            RemoveAll(false);
            RecordTagCount("After first remove:");
            AddTags(1, true);
            RemoveFirst(false);
            AddTags(3, true);
            ReplaceAll(true);
            RecordTagCount("After final mods:");

            MusicFile = MakeNewFile();
            RecordTagCount("After third reload:");

            RemoveAnyRemainingNewTags(true);
            MusicFile = MakeNewFile();
            RecordTagCount("After clear up reload:");

            PropertiesOK = CheckProperties();
            CheckAndPrint();

            // Restore from backup copy
            Helpers.RestoreFromBackup(CurrentFilePath);

            // Caller requires an int to be returned.
            // Used to count how many files looked at.
            return 1;
        }


        /// <summary>
        /// Choose a newly added tag type and remove the first
        /// instance of that tag. 
        /// </summary>
        /// <param name="Save">Should we save the file to disc?</param>
        static private void RemoveFirst(bool Save)
            => RemoveTag(RemoveType.First, Save);


        /// <summary>
        /// Choose a newly added tag type and remove all
        /// instances of that tag. 
        /// </summary>
        /// <param name="Save">Should we save the file to disc?</param>
        static private void RemoveAll(bool Save)
            => RemoveTag(RemoveType.All, Save);


        /// <summary>
        /// Choose a newly added tag type and remove one
        /// instance by specifying name and value.
        /// </summary>
        /// <param name="Save">Should we save the file to disc?</param>
        static private void RemoveExact(bool Save)
            => RemoveTag(RemoveType.Exact, Save);


        /// <summary>
        /// Choose a newly added tag type, remove all,
        /// then add a new tag.
        /// </summary>
        /// <param name="Save"></param>
        static private void ReplaceAll(bool Save)
            => RemoveTag(RemoveType.All, Save);


        /// <summary>
        /// Writes Tag count info to the results log file.
        /// </summary>
        /// <param name="Message">Message to indicate where in change list we are.</param>
        static private void RecordTagCount(string Message)
        {
            int FieldWidth = 30;
            Message = "   " + Message
                + new string(' ', FieldWidth - Message.Length)
                + string.Format("{0,3}", MusicFile.NumberOfTags);
            string Temp = "";

            int Total = 0;
            if (Type == AudioFileTypes.flac)
                foreach (var Tag in FLACTags)
                {
                    Temp += Tag.Name + "(" + Tag.Count + ")  ";
                    Total += Tag.Count;
                }
            else
                foreach (var Tag in MP3Tags)
                {
                    Temp += Tag.Type.ToString() + "(" + Tag.Count + ")  ";
                    Total += Tag.Count;
                }
            TheWriter.WriteLine(Message + "   [" + string.Format("{0,3}", Total) + "]\t" + Temp);
        }


        /// <summary>
        /// Checks that basic parameters are OK.
        /// Throws an exception if not, which gets caught and recorded
        /// in the errors log.
        /// </summary>
        static private void CheckAndPrint()
        {
            if (!PropertiesOK)
                throw new Exception("Properties not as expected.");
            if (MusicFile.NumberOfMusicBytes != InitialLengthOfMusic)
                throw new Exception("Number of music bytes has changed.");
            if (MusicFile.NumberOfTags != InitialTagCount)
                throw new Exception("Number of tags has changed. "
                    + InitialTagCount + " -> " + MusicFile.NumberOfTags);
        }


        /// <summary>
        /// Removes any tags added in testing that have not yet been removed.
        /// </summary>
        /// <param name="Save">Shoule we save the file to disc?</param>
        static private void RemoveAnyRemainingNewTags(bool Save)
        {
            int Count;
            if (Type == AudioFileTypes.flac)
            {
                // File is FLAC
                Count = FLACTags.Count;
                for (int i = 0; i < Count; i++)
                    RemoveTag(RemoveType.All, Save);
                FLACTags.Clear();
            }
            else
            {
                // File is MP3
                Count = MP3Tags.Count;
                for (int i = 0; i < Count; i++)
                    RemoveTag(RemoveType.All, Save);
                MP3Tags.Clear();
            }
            if (Save)
                MusicFile.Save(false);
            return;
        }


        /// <summary>
        /// Removes one or more instances of a newly added tag.
        /// </summary>
        /// <param name="RType">Remove the first, all, or an exact match?</param>
        /// <param name="Save">Should we save the file to disc?</param>
        static private void RemoveTag(RemoveType RType, bool Save)
        {
            Random r = new Random();
            int HowMany = 0;

            if (Type == AudioFileTypes.flac)
            {
                // File is FLAC

                // Select a random element in the tags added list
                int Pos = r.Next(0, FLACTags.Count - 1);

                switch (RType)
                {
                    case RemoveType.First:
                        HowMany = 1;
                        ((FLACFile)MusicFile).RemoveFirst(FLACTags[Pos].Name);
                        break;
                    case RemoveType.All:
                        // See how many tags were added by that entry
                        HowMany = FLACTags[Pos].Count;
                        ((FLACFile)MusicFile).RemoveAll(FLACTags[Pos].Name);
                        break;
                    case RemoveType.Exact:
                        HowMany = 1;
                        ((FLACFile)MusicFile).RemoveExact(FLACTags[Pos].Name, Payload);
                        break;
                    case RemoveType.Replace:
                        // Many removed and one added.
                        HowMany = FLACTags[Pos].Count - 1;
                        ((FLACFile)MusicFile).ReplaceAll(FLACTags[Pos].Name, Payload);
                        break;
                    default:
                        break;
                }
                // Edit the tags added list
                if ((FLACTags[Pos].Count -= HowMany) == 0)
                    FLACTags.RemoveAt(Pos);
            }
            else
            {
                // File is MP3

                // Select a random element in the tags added list
                int Pos = r.Next(0, MP3Tags.Count - 1);

                switch (RType)
                {
                    case RemoveType.First:
                        HowMany = 1;
                        ((MP3File)MusicFile).RemoveFirst(MP3Tags[Pos].Type);
                        break;
                    case RemoveType.All:
                        // See how many tags were added by that entry
                        HowMany = MP3Tags[Pos].Count;
                        ((MP3File)MusicFile).RemoveAll(MP3Tags[Pos].Type);
                        break;
                    case RemoveType.Exact:
                        HowMany = 1;
                        ((MP3File)MusicFile).RemoveExact(MP3Tags[Pos].Type, Payload);
                        break;
                    case RemoveType.Replace:
                        // Many removed and one added.
                        HowMany = MP3Tags[Pos].Count - 1;
                        ((MP3File)MusicFile).ReplaceAll(MP3Tags[Pos].Type, Payload);
                        break;
                    default:
                        break;
                }
                // Edit the tags added list
                if ((MP3Tags[Pos].Count -= HowMany) == 0)
                    MP3Tags.RemoveAt(Pos);
            }
            // Resave the file if requested
            if (Save)
                MusicFile.Save(false);
        }


        /// <summary>
        /// Adds one or more instances of a tag. 
        /// </summary>
        /// <param name="HowMany">How many instances to add.</param>
        /// <param name="Save">Should we save the file to disc?</param>
        static private void AddTags(int HowMany, bool Save)
        {
            if (HowMany < 0)
                throw new Exception("Requesting adding a negative amount of tags "
                    + "in Testing.AddTags().");

            string TempName;
            V23TextTags TempType;

            if (Type == AudioFileTypes.flac)
            {
                // File is FLAC
                TempName = GenerateName();
                for (int i = 0; i < HowMany; i++)
                    ((FLACFile)MusicFile).AddTag(TempName, Payload);
                FLACTags.Add(new FLACTag(TempName, HowMany));
                if (Save)
                    MusicFile.Save(false);
            }
            else
            {
                // File is MP3
                TempType = FindUnusedTag();
                for (int i = 0; i < HowMany; i++)
                    ((MP3File)MusicFile).AddTag(TempType, Payload);
                MP3Tags.Add(new MP3Tag(TempType, HowMany));
                if (Save)
                    MusicFile.Save(false);
            }
        }


        /// <summary>
        /// Make a new audiofile instance from the underlying disc file.
        /// </summary>
        /// <returns>The newly created file.</returns>
        static private AudioFile MakeNewFile()
        {
            if (Type == AudioFileTypes.flac)
                return new FLACFile(CurrentFilePath, false);
            else
                return new MP3File(CurrentFilePath, false);
        }


        /// <summary>
        /// Selects a type of ID3 text tag that is not currently in use 
        /// in the file.  We will add this.
        /// </summary>
        /// <returns>AN unused type.</returns>
        static private V23TextTags FindUnusedTag()
        {
            foreach (V23TextTags Tag in Enum.GetValues(typeof(V23TextTags)))
                if (!((MP3File)MusicFile).Exists(Tag))
                    return Tag;
            throw new Exception("No unused tags in Testing.FindUnusedTag()");
        }


        /// <summary>
        /// Generates a random 4 character string.
        /// Used to make test name for FLAC Tags.
        /// </summary>
        /// <returns></returns>
        static private string GenerateName()
        {
            string s = "";

            Random r = new Random();
            for (int i = 0; i < 4; i++)
                s += ((char)r.Next(65, 90)).ToString();
            return s;
        }


        /// <summary>
        /// Change each of the ten 'standard' properties 
        /// of an audio file.
        /// </summary>
        static private void ChangeProperties()
        {

            if (Type == AudioFileTypes.flac)
            {
                // File is FLAC
                FLACFile FFile = (FLACFile)MusicFile;
                if (FFile.CountTags("ALBUM") == 1)
                    FFile.ALBUM = "Test album name";
                if (FFile.CountTags("ALBUMARTIST") == 1)
                    FFile.ALBUMARTIST = "Test album artist name";
                if (FFile.CountTags("ARTIST") == 1)
                    FFile.ARTIST = "Test artist name";
                if (FFile.CountTags("COMMENT") == 1)
                    FFile.COMMENT = "Test Comment";
                if (FFile.CountTags("DATE") == 1)
                    FFile.DATE = "Test Date";
                if (FFile.CountTags("DISCNUMBER") == 1)
                    FFile.DISCNUMBER = "Test disc number";
                if (FFile.CountTags("GENRE") == 1)
                    FFile.GENRE = "Test Genre";
                if (FFile.CountTags("TITLE") == 1)
                    FFile.TITLE = "Test Title";
                if (FFile.CountTags("TRACKNUMBER") == 1)
                    FFile.TRACKNUMBER = "Test Track";
                if (FFile.CountTags("COMPOSER") == 1)
                    FFile.COMPOSER = "Test Composer";
                FFile.Save(false);
            }
            else
            {
                // File is MP3
                MP3File MFile = (MP3File)MusicFile;
                if (MFile.CountTags(V23TextTags.TALB) == 1)
                    MFile.ALBUM = "Test album name";
                if (MFile.CountTags(V23TextTags.TPE2) == 1)
                    MFile.ALBUMARTIST = "Test album artist name";
                if (MFile.CountTags(V23TextTags.TPE1) == 1)
                    MFile.ARTIST = "Test artist name";
                if (MFile.CountTags(V23TextTags.COMM) == 1)
                    MFile.COMMENT = "Test Comment";
                if (MFile.CountTags(V23TextTags.TYER) == 1)
                    MFile.DATE = "Test Date";
                if (MFile.CountTags(V23TextTags.TPOS) == 1)
                    MFile.DISCNUMBER = "Test disc number";
                if (MFile.CountTags(V23TextTags.TCON) == 1)
                    MFile.GENRE = "Test Genre";
                if (MFile.CountTags(V23TextTags.TIT2) == 1)
                    MFile.TITLE = "Test Title";
                if (MFile.CountTags(V23TextTags.TRCK) == 1)
                    MFile.TRACKNUMBER = "Test Track";
                if (MFile.CountTags(V23TextTags.TCOM) == 1)
                    MFile.COMPOSER = "Test Composer";
                MFile.Save(false);
            }
        }


        /// <summary>
        /// Check the ten standard properties are as expected after 
        /// they were changed.
        /// </summary>
        /// <returns>Are they OK?</returns>
        static private bool CheckProperties()
        {
            bool Temp = true;
            if (Type == AudioFileTypes.flac)
            {
                // File is FLAC
                FLACFile FFile = (FLACFile)MusicFile;
                if (FFile.CountTags("ALBUM") == 1)
                    Temp = Temp && (FFile.ALBUM == "Test album name");
                if (FFile.CountTags("ALBUMARTIST") == 1)
                    Temp = Temp && (FFile.ALBUMARTIST == "Test album artist name");
                if (FFile.CountTags("ARTIST") == 1)
                    Temp = Temp && (FFile.ARTIST == "Test artist name");
                if (FFile.CountTags("COMMENT") == 1)
                    Temp = Temp && (FFile.COMMENT == "Test Comment");
                if (FFile.CountTags("DATE") == 1)
                    Temp = Temp && (FFile.DATE == "Test Date");
                if (FFile.CountTags("DISCNUMBER") == 1)
                    Temp = Temp && (FFile.DISCNUMBER == "Test disc number");
                if (FFile.CountTags("GENRE") == 1)
                    Temp = Temp && (FFile.GENRE == "Test Genre");
                if (FFile.CountTags("TITLE") == 1)
                    Temp = Temp && (FFile.TITLE == "Test Title");
                if (FFile.CountTags("TRACKNUMBER") == 1)
                    Temp = Temp && (FFile.TRACKNUMBER == "Test Track");
                if (FFile.CountTags("COMPOSER") == 1)
                    Temp = Temp && (FFile.COMPOSER == "Test Composer");
                return Temp;
            }
            else
            {
                // FIle is MP3
                MP3File MFile = (MP3File)MusicFile;
                if (MFile.CountTags(V23TextTags.TALB) == 1)
                    Temp = Temp && (MFile.ALBUM == "Test album name");
                if (MFile.CountTags(V23TextTags.TPE2) == 1)
                    Temp = Temp && (MFile.ALBUMARTIST == "Test album artist name");
                if (MFile.CountTags(V23TextTags.TPE1) == 1)
                    Temp = Temp && (MFile.ARTIST == "Test artist name");
                if (MFile.CountTags(V23TextTags.COMM) == 1)
                    Temp = Temp && (MFile.COMMENT == "Test Comment");
                if (MFile.CountTags(V23TextTags.TYER) == 1)
                    Temp = Temp && (MFile.DATE == "Test Date");
                if (MFile.CountTags(V23TextTags.TPOS) == 1)
                    Temp = Temp && (MFile.DISCNUMBER == "Test disc number");
                if (MFile.CountTags(V23TextTags.TCON) == 1)
                    Temp = Temp && (MFile.GENRE == "Test Genre");
                if (MFile.CountTags(V23TextTags.TIT2) == 1)
                    Temp = Temp && (MFile.TITLE == "Test Title");
                if (MFile.CountTags(V23TextTags.TRCK) == 1)
                    Temp = Temp && (MFile.TRACKNUMBER == "Test Track");
                if (MFile.CountTags(V23TextTags.TCOM) == 1)
                    Temp = Temp && (MFile.COMPOSER == "Test Composer");
                return Temp;
            }
        }


        /// <summary>
        /// Used to select which of four kinds of tag remove will be performed.
        /// </summary>
        private enum RemoveType
        {
            First, All, Exact, Replace
        };
    }


    /// <summary>
    /// A class to record what kind of FLAC tag was added 
    /// and how many instances.
    /// Added to list as file is modified.
    /// </summary>
    class FLACTag
    {
        // The name of the tag added.
        public string Name { get; private set; }

        // How many instances were added.
        public int Count;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">Name of tag</param>
        /// <param name="c">How many instances</param>
        public FLACTag(string n, int c)
        {
            Name = n;
            Count = c;
        }


        /// <summary>
        /// Return as string
        /// </summary>
        /// <returns>Object as string</returns>
        public override string ToString() => Name + ": " + Count;
    }


    /// <summary>
    /// A class to record what kind of MP3 tag was added 
    /// and how many instances.
    /// Added to list as file is modified.
    /// </summary>
    class MP3Tag
    {
        // The type of the tag added.
        public V23TextTags Type { get; private set; }

        // How many instances were added.
        public int Count;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t">What type of tag was added</param>
        /// <param name="c">How many instances were added</param>
        public MP3Tag(V23TextTags t, int c)
        {
            Type = t;
            Count = c;
        }

        /// <summary>
        /// Returns as string
        /// </summary>
        /// <returns>Object as string</returns>
        public override string ToString() => Type + ": " + Count;
    }
}