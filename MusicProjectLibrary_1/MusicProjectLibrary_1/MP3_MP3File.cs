//========================================================================
// Name:     MP3_MP3File.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements a class to represent and manipulate the metadata
//           of an ID3 v 2.3 MP3 audio file. 
// Comments: 
//========================================================================
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// Class to represent an MP3 file
    /// </summary>
    public class MP3File : AudioFile
    {
        /// <summary>
        /// Version string
        /// </summary>
        public new const string _Version = "MP3File:                      1.10";


        /// <summary>
        /// File header
        /// </summary>
        private V23FileHeader FileHeader;


        /// <summary>
        /// Tag as read - a set of frames
        /// </summary>
        private V23Tag TagAsRead = new V23Tag();


        /// <summary>
        /// Modified tag to be saved to disc
        /// </summary>
        private V23Tag NewTag = new V23Tag();


        /// <summary>
        /// Position in file where tag starts.
        /// (Depends on whether there is an extended header.)
        /// </summary>
        private long StartOfTag;


        /// <summary>
        /// Size of the tag as read - including any padding.
        /// </summary>
        private long OriginalTagSizeIncludingPadding;


        /// <summary>
        /// Does this file have a ID3 v1 tag block on the end?
        /// </summary>
        public bool HasV1Tags { get; private set; } = false;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FilePath">The path to the mp3 file on disc.</param>
        /// <param name="ReadOnly">Should the file be opened in ReadOnly mode?
        /// In this mode all tag data is read but no additional data is read.
        /// This means that the file cannot be be rewritten after tag changes.
        /// It makes reading happen more quickly.</param>
        public MP3File(string FilePath, bool ReadOnly)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("File " + FilePath +
                    "\nDoes not exist in MP3File constructor().");

            // Set ReadOnly value
            this.ReadOnly = ReadOnly;

            // Record the file path
            AudioPath = FilePath;

            // Tag list for v 2.3 tags
            PrimaryTagList = new TagList(AudioFileTypes.mp3);

            // Tag list for v1 tags
            SecondaryTagList = new TagList(AudioFileTypes.mp3);

            using (ByteSource TheReader = new ByteSource(AudioPath))
            {
                OriginalTotalFileLength = TheReader.Length;

                try
                {
                    FileHeader = new V23FileHeader(TheReader);
                }
                catch (Exception Ex)
                {
                    throw new BadAudioFileException("Error reading file header in MP3File constructor.\n" +
                        Ex.Message);
                }

                OriginalTagSizeIncludingPadding = FileHeader.TagLength;
                StartOfMusic = FileHeader.StartOfTag + FileHeader.TagLength;

                if (!FileHeader.IsID3)
                    throw new BadAudioFileException("Header information is incorrect in Mp3File constructor.\n");
                if (!FileHeader.VersionIs23)
                    throw new BadAudioFileException("File does not have v2.3 tags in MP3File constructor.\n");

                try
                {
                    // See if it has V1 tag
                    TheReader.MoveTo(-128, SeekOrigin.End);
                    if (TheReader.PeekBytes(3).SequenceEqual(new byte[] { (byte)'T', (byte)'A', (byte)'G' }))
                    {
                        HasV1Tags = true;
                        SecondaryTagList = V1Tags.ByteArrayToV1TagList(TheReader.GetBytes(128));
                    }

                    if (HasV1Tags)
                        NumberOfMusicBytes = OriginalTotalFileLength - StartOfMusic - 128;
                    else
                        NumberOfMusicBytes = OriginalTotalFileLength - StartOfMusic;

                    StartOfTag = FileHeader.StartOfTag;
                }
                catch (Exception Ex)
                {
                    throw new BadAudioFileException("Error reading V1 tag in MP3File constructor.\n"
                        + Ex.Message);
                }

                TheReader.MoveTo(StartOfTag, SeekOrigin.Begin);

                // Array to hold de-unsynced tag
                byte[] DeUnsyncedTag;

                if (FileHeader.UnsynchronisationUsed)
                {
                    byte[] UnSyncedTag = TheReader.GetBytes(OriginalTagSizeIncludingPadding);
                    DeUnsyncedTag = SyncSafe.DeUnscynchronize(UnSyncedTag);
                    TagAsRead.PopulateFrameList(new ByteSource(DeUnsyncedTag), ReadOnly);
                }
                else
                    TagAsRead.PopulateFrameList(TheReader, ReadOnly);

                HasEmbeddedGraphic = TagAsRead.HasEmbeddedGraphic;

                GenerateTagsFromFrames();
            } // using
        }


        /// <summary>
        /// Iterate through the tag block reading each frame in turn.
        /// Generate a new tag for each text frame.
        /// </summary>
        private void GenerateTagsFromFrames()
        {
            foreach (var Frame in TagAsRead)
                if (Frame.IsText)
                    PrimaryTagList.AddTag(new TagType(Frame));
        }


        /// <summary>
        /// Display debugging information on console.
        /// </summary>
        /// <param name="IncludeDetail">Should we display internal details?</param>
        /// <param name="IncludeTags">Should we also display tags?</param>
        public override void DebugPrint(bool IncludeDetail, bool IncludeTags)
        {
            Console.WriteLine("===============================================================================");
            Console.WriteLine("File:                            " + Path.GetFileName(AudioPath));
            Console.WriteLine("Folder:                          " + Path.GetDirectoryName(AudioPath));
            Console.WriteLine("Time:                            " + DateTime.Now.ToString("h:mm:ss tt"));
            Console.WriteLine("Original file length:            " + OriginalTotalFileLength.ToString("#,##0"));
            Console.WriteLine("===============================================================================");
            Console.WriteLine(string.Format("Has extended header:        {0,5}", FileHeader.HasExtendedHeader));
            Console.WriteLine(string.Format("Uses Unsynchronisation:     {0,5}", FileHeader.UnsynchronisationUsed));
            Console.WriteLine();
            Console.WriteLine(string.Format("Number of tags:         {0,9}", PrimaryTagList.CountTags().ToString("#,##0")));
            Console.WriteLine(string.Format("Number of frames:       {0,9}", TagAsRead.Count.ToString("#,##0")));
            Console.WriteLine();
            Console.WriteLine(string.Format("Start of tag:           {0,9}", StartOfTag.ToString("#,##0")));
            Console.WriteLine(string.Format("Total of frames sizes:  {0,9}", TagAsRead.SumOfFrameLengths.ToString("#,##0")));
            Console.WriteLine(string.Format("Padding size:           {0,9}", (OriginalTagSizeIncludingPadding - TagAsRead.SumOfFrameLengths).ToString("#,##0")));
            Console.WriteLine(string.Format("Tag size (enc padd'g):  {0,9}", OriginalTagSizeIncludingPadding.ToString("#,##0")));
            Console.WriteLine(string.Format("Padding verified OK:        {0,5}", VerifyPadding(StartOfTag + TagAsRead.SumOfFrameLengths, StartOfMusic - 1)));
            Console.WriteLine();
            Console.WriteLine(string.Format("Start of music:         {0,9}", StartOfMusic.ToString("#,##0")));
            Console.WriteLine(string.Format("Length of music:        {0,9}", NumberOfMusicBytes.ToString("#,##0")));
            Console.WriteLine();
            Console.WriteLine(string.Format("Has v1 tags:                {0,5}", HasV1Tags));
            Console.WriteLine(string.Format("Has embedded graphic:       {0,5}", HasEmbeddedGraphic));
            Console.WriteLine();

            if (IncludeDetail)
            {
                // Show the frames
                Console.WriteLine("FRAMES IN TAG BLOCK");
                Console.WriteLine("===================");
                Console.WriteLine(TagAsRead.ToString(StartOfTag));
            }

            if (IncludeTags)
            {
                // List the primary tags
                Console.WriteLine("Primary tags:");
                Console.WriteLine(PrimaryTagList);

                // Show the V1 tags
                if (HasV1Tags)
                {
                    Console.WriteLine("V1 tags:");
                    Console.WriteLine(SecondaryTagList);
                }
            }
            Helpers.PressAnyKeyToContinue();
        }


        /// <summary>
        /// A debugging method.
        /// Used to verify that the padding area at the end of the tag
        /// is indeed all nulls.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns>Is padding all null?</returns>
        private bool VerifyPadding(long start, long stop)
        {
            if (stop < start)
                throw new ArgumentOutOfRangeException("stop(" + stop + ") > start (" + start + ") in MP3File.VerifyPadding().");
            using (ByteSource TheReader = new ByteSource(AudioPath))
            {
                bool OK = true;

                TheReader.MoveTo(start);

                foreach (var Byte in TheReader.GetBytes(1 + stop - start))
                    if (Byte != 0)
                        OK = false;

                return OK;
            }
        }


        /// <summary>
        /// Build a new tag block from the PrimaryTagList
        /// and the pre-existing non-text frames.
        /// </summary>
        /// <returns>A new tag</returns>
        private V23Tag BuildNewTagBlock()
        {
            V23Tag Temp = new V23Tag();

            // Add each tag in the list
            foreach (TagType Tag in PrimaryTagList)
                Temp.Add(new V23Frame(Tag));

            // Then appened the unchanged non-text frames
            foreach (V23Frame Frame in TagAsRead)
                if (!Frame.IsText)
                    Temp.Add(Frame);

            return Temp;
        }


        /// <summary>
        /// Called once tags have been edited.
        /// Decides what to write file - all or just a window?
        /// </summary>
        protected override void InMemoryUpdate()
        {
            // If there is not this much free padding left then rewrite
            int MinimumPaddingSizeRequired = 256;

            NewTag = BuildNewTagBlock();

            if (NewTag.SumOfFrameLengths >= OriginalTagSizeIncludingPadding)
                // New tag bigger than old
                MustRewriteWholeFile = true;
            else
            {
                // New tag smaller than old
                if (OriginalTagSizeIncludingPadding - NewTag.Count < MinimumPaddingSizeRequired)
                    // Not much padding left
                    MustRewriteWholeFile = true;
                else
                    // Plenty of padding left
                    MustRewriteWholeFile = false;
            }
        }


        /// <summary>
        /// Rewrite the whole of the file.
        /// </summary>
        /// <param name="TempPath">Path to hold temporary file</param>
        protected override void RewriteWholeFile(string TempPath)
        {
            // Create a new padding block
            int NewPaddingSize = 1024;
            byte[] Padding = new byte[NewPaddingSize];

            // Join the new tag and the padding
            byte[] NewTagAndPadding = Helpers.JoinByteArrays(new List<byte[]>() { NewTag.ToBytes(), Padding });

            // Build a new header
            byte[] NewHeader = new byte[6];
            NewHeader[0] = (byte)'I'; NewHeader[1] = (byte)'D'; NewHeader[2] = (byte)'3';
            NewHeader[3] = 3;  // Version is 3 (ID3 v2.3)
            NewHeader[4] = 0;  // Revision is 0
            NewHeader[5] = 0;  // No unsynchronization and no extended header.
            byte[] TagSize = SyncSafe.ToBigEndianSyncSafe(NewTagAndPadding.Length);
            NewHeader = Helpers.JoinByteArrays(new List<byte[]>() { NewHeader, TagSize });

            try
            {
                using (ByteSource TheReader = new ByteSource(AudioPath))
                using (BinaryWriter TheWriter =
                        new BinaryWriter(File.Open(TempPath, FileMode.Create, FileAccess.Write)))
                {
                    // Write new file header.
                    TheWriter.Write(NewHeader);
                    // Update in-memory data ready for any further write
                    FileHeader = new V23FileHeader(new ByteSource(NewHeader));
                    StartOfTag = TheWriter.BaseStream.Position;

                    // Write the new tag (including padding).
                    TheWriter.Write(NewTagAndPadding);

                    // Update in-memory data ready for any further write
                    OriginalTagSizeIncludingPadding = NewTagAndPadding.Length;
                    TagAsRead.Clear();
                    TagAsRead.PopulateFrameList(new ByteSource(NewTagAndPadding), ReadOnly);

                    // Copy the music
                    TheReader.MoveTo(StartOfMusic);
                    StartOfMusic = TheWriter.BaseStream.Position;
                    TheWriter.Write(TheReader.GetBytes(NumberOfMusicBytes));

                    // Update any v1 tags
                    if (HasV1Tags)
                        TheWriter.Write(V1Tags.V1TagListToByteArray(PrimaryTagList));

                    OriginalTotalFileLength = TheWriter.BaseStream.Position;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Helpers.SplitPath(AudioPath) +
                    "\nError doing full over-write of MP3 file in MP3File.RewriteWholeFile().\n" +
                    Ex.Message);
            }
        }


        /// <summary>
        /// Rewrite just the changed subset within the file.
        /// Do not rewrite the whole file.
        /// </summary>
        protected override void OverwriteWindow()
        {
            try
            {
                // Overwrite the Tag block (and the V1 tag at the end)
                using (BinaryWriter TheWriter = new BinaryWriter(File.Open(AudioPath, FileMode.Open, FileAccess.Write)))
                {
                    // Convert the new tag to a byte array
                    byte[] NewTagBytes = NewTag.ToBytes();

                    // Make a new padding block
                    byte[] Padding = new byte[OriginalTagSizeIncludingPadding - NewTagBytes.Length];

                    // Add it to end of the tag
                    byte[] NewTagAndPadding = Helpers.JoinByteArrays(new List<byte[]>() { NewTagBytes, Padding });

                    // Move to start of the tag
                    TheWriter.Seek(Helpers.LongToInt(StartOfTag), SeekOrigin.Begin);

                    // Overwrite the tag
                    TheWriter.Write(NewTagAndPadding);
                    TagAsRead.Clear();
                    TagAsRead.PopulateFrameList(new ByteSource(NewTagAndPadding), ReadOnly);

                    if (HasV1Tags)
                    {
                        // Move to start of the V1 tag
                        TheWriter.Seek(-128, SeekOrigin.End);

                        // Copy the v1 Tag to the end of the file.
                        TheWriter.Write(V1Tags.V1TagListToByteArray(PrimaryTagList));
                    }
                } // using
            } // try
            catch (Exception Ex)
            {
                throw new Exception(Helpers.SplitPath(AudioPath) +
                    "Error overwriting data inside MP3 file in MP3File.OverwriteWindow().\n" + Ex.Message);
            }
        }


        /// <summary>
        /// Adds a new tag to the tag list
        /// </summary>
        /// <param name="Name">Name enumeration of the new tag</param>
        /// <param name="Value">Value field of the new tag</param>
        public void AddTag(V23TextTags Name, string Value)
            => PrimaryTagList.AddTag(Enum.GetName(typeof(V23TextTags), Name), Value);


        /// <summary>
        /// Counts how many tags have the specified name.
        /// </summary>
        /// <param name="Name">The name enumeration to be searched for.</param>
        /// <returns>How many are found</returns>
        public int CountTags(V23TextTags Name)
            => PrimaryTagList.CountTags(Enum.GetName(typeof(V23TextTags), Name));


        /// <summary>
        /// Sees whether any tags with the given name exist.
        /// </summary>
        /// <param name="Name">The name enumeration to be searched for.</param>
        /// <returns>Whether or not any are found</returns>
        public bool Exists(V23TextTags Name)
            => PrimaryTagList.Exists(Enum.GetName(typeof(V23TextTags), Name));


        /// <summary>
        /// Returns the value field of the first tag in the list 
        /// with the name as specified
        /// </summary>
        /// <param name="Name">The name enumeration to be searched for.</param>
        /// <returns>The value of the first matching tag</returns>
        public string First(V23TextTags Name)
            => PrimaryTagList.First(Enum.GetName(typeof(V23TextTags), Name));


        /// <summary>
        /// Removes all tags from the list which have name equal
        /// to the value specified.
        /// </summary>
        /// <param name="Name">The name enumeration to be searched for.</param>
        public void RemoveAll(V23TextTags Name)
            => PrimaryTagList.RemoveAll(Enum.GetName(typeof(V23TextTags), Name));


        /// <summary>
        /// Removes the first tag from the list which has name
        /// equal to the value specified.
        /// </summary>
        /// <param name="Name">The name enumeration to be searched for.</param>
        public void RemoveFirst(V23TextTags Name)
            => PrimaryTagList.RemoveFirst(Enum.GetName(typeof(V23TextTags), Name));


        /// <summary>
        /// Removes any tags with name and value matching those specified
        /// </summary>
        /// <param name="Name">Name enumeration to match</param>
        /// <param name="Value">Value to match</param>
        public void RemoveExact(V23TextTags Name, string Value)
            => PrimaryTagList.RemoveExact(Enum.GetName(typeof(V23TextTags), Name), Value);


        /// <summary>
        /// Remove all tags with the same name and adds a new tag
        /// with teh name and value supplied
        /// </summary>
        /// <param name="Name">Name enumeration to be removed then added</param>
        /// <param name="Value">Value for the new tag to be added</param>
        public void ReplaceAll(V23TextTags Name, string Value)
            => PrimaryTagList.ReplaceAll(Enum.GetName(typeof(V23TextTags), Name), Value);


        /// <summary>
        /// Returns a string representation of the file
        /// by listing the tags.
        /// </summary>
        /// <returns>String representation of the tag</returns>
        public override string ToString()
        {
            string Temp = "Primary Tag list:\n";
            Temp += "=================\n";
            Temp += PrimaryTagList;
            if (HasV1Tags)
            {
                Temp += "\nV1 Tag list:\n";
                Temp += "============\n";
                Temp += SecondaryTagList;
            }
            Temp += "\n";
            return Temp;
        }


        /// <summary>
        /// WARNING.  DOES NOT UPDATE IN-MEMORY DATA STRUCTURES.  
        /// SO DO NOT RESAVE AFTER USING THIS METHOD.
        /// Used for creating test files only.
        /// Takes and in-memory file and rewrites it with, optionally:
        ///  - A (dummy) extended header.  (Containing nothing meaningful.)
        ///  - An unsynchronised tag block
        /// </summary>
        public override void SaveTestFile()
        {
            bool AddExtendedHeader = false; ;
            bool Unsynchronise = false;

            string Temp = Helpers.readString("Add an extended header (y/n): ");
            if ((Temp.ToUpper())[0] == 'Y')
                AddExtendedHeader = true;
            Temp = Helpers.readString("Use Unsynchronisation (y/n): ");
            if ((Temp.ToUpper())[0] == 'Y')
                Unsynchronise = true;
            string TestFilePath = Path.GetDirectoryName(AudioPath) + "\\" +
                Helpers.readString("Enter name for text file: ");

            // Build a new tag
            byte[] NewTagBytes = TagAsRead.ToBytes();
            // Add some padding
            NewTagBytes = Helpers.JoinByteArrays(new List<byte[]>() { NewTagBytes, new byte[512] });

            // Build a new header
            byte[] NewHeader = new byte[6];
            NewHeader[0] = (byte)'I'; NewHeader[1] = (byte)'D'; NewHeader[2] = (byte)'3';
            NewHeader[3] = 3;  // Version is 3 (ID3 v2.3)
            NewHeader[4] = 0;  // Revision is 0
            NewHeader[5] = 0;
            if (AddExtendedHeader)
            {
                // Set flag bit
                NewHeader[5] += 64;
            }
            if (Unsynchronise)
            {
                // Set flag bit
                NewHeader[5] += 128;

                // Unsynchronise
                NewTagBytes = SyncSafe.Unsynchronize(NewTagBytes);
            }

            byte[] TagSize = SyncSafe.ToBigEndianSyncSafe(NewTagBytes.Length);
            NewHeader = Helpers.JoinByteArrays(new List<byte[]>() { NewHeader, TagSize });

            try
            {
                using (ByteSource TheReader = new ByteSource(AudioPath))
                using (BinaryWriter TheWriter =
                        new BinaryWriter(File.Open(TestFilePath, FileMode.Create, FileAccess.Write)))
                {
                    // Write new file header.
                    TheWriter.Write(NewHeader);

                    if (AddExtendedHeader)
                    {
                        TheWriter.Write(Helpers.IntToFourByteBigEndian(10));
                        TheWriter.Write(new byte[10]);
                    }

                    // Write the new tag (including padding).
                    TheWriter.Write(NewTagBytes);

                    // Copy the music
                    TheReader.MoveTo(StartOfMusic);
                    TheWriter.Write(TheReader.GetBytes(NumberOfMusicBytes));

                    // Update any v1 tags
                    if (HasV1Tags)
                        TheWriter.Write(V1Tags.V1TagListToByteArray(PrimaryTagList));
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Helpers.SplitPath(AudioPath) +
                    "\nError saving file in SaveTestFile().\n" +
                    Ex.Message);
            }
        }


        /// <summary>
        /// USE WITH CAUTION.  
        /// Removes a non-text frame from the frame list.
        /// Use to delete unwanted PRIV or COMM tags
        /// </summary>
        /// <param name="TargetName">The name of the tag to remove</param>
        public void BinaryFrameTypeRemoveAll(string TargetName)
        {
            TagAsRead.BinaryFrameTypeRemoveAll(TargetName);
        }


        /// <summary>
        /// Version property
        /// </summary>
        static public string Version
        {
            get
            {
                string Temp = _Version + "\n";
                Temp += "   " + AudioFile._Version + "\n";
                Temp += "   " + V23Tag._Version + "\n";
                Temp += "   " + V23Frame._Version + "\n";
                Temp += "   " + V23FileHeader._Version + "\n";
                Temp += "   " + V1Tags._Version + "\n";
                Temp += "   " + TagMappings._Version + "\n";
                Temp += "   " + SyncSafe._Version + "\n";
                Temp += "   " + CommTags._Version + "\n";
                Temp += "   " + BOM._Version + "\n";
                Temp += "   " + TreeWalker._Version + "\n";
                Temp += "   " + Testing._Version + "\n";
                Temp += "   " + TagType._Version + "\n";
                Temp += "   " + TagList._Version + "\n";
                Temp += "   " + Helpers._Version + "\n";
                Temp += "   " + ByteSource._Version + "\n";
                Temp += "   " + LazySW._Version + "\n";
                return Temp;
            }
        }
    }
}