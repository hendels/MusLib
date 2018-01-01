//========================================================================
// Name:     FLAC_Flacfile.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements a class to represent the metadata of a FLAC file.
// Comments: 
//========================================================================
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;



namespace JAudioTags
{
    /// <summary>
    /// Class to represent a flac file
    /// </summary>
    public class FLACFile : AudioFile
    {
        /// <summary>
        /// Version string
        /// </summary>
        public new const string _Version = "FLACFile:                     1.03";


        /// <summary>
        /// All FLAC files start with these 4 bytes
        /// </summary>
        private byte[] FileHeader = { (byte)'f', (byte)'L', (byte)'a', (byte)'C' };


        /// <summary>
        /// List of metadata blocks within the file.
        /// </summary>
        private BlockListType TheBlocks = new BlockListType();


        /// <summary>
        /// These are parts of the comment block of any FLAC file.
        /// Store them globally to facilitate the creation of a new comment block.
        /// </summary>
        private uint vendor_length;
        private string vendor_string;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FilePath">The path to the audio file.</param>
        /// <param name="ReadOnly">Should the file be opened in ReadOnly mode?
        /// In this mode all tag data is read but no additional data is read.
        /// This means that the file cannot be be rewritten after tag changes.
        /// It makes reading happen more quickly.</param>
        public FLACFile(string FilePath, bool ReadOnly)
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("File " + FilePath +
                    "\nDoes not exist in FLACFile constructor().");

            // Set the ReadOnly flag
            this.ReadOnly = ReadOnly;

            // Are we looking at the last metadata block?
            bool IsLastBlock;

            // Store the current path      
            AudioPath = FilePath;

            // The main tag list
            PrimaryTagList = new TagList(AudioFileTypes.flac);

            // Read the blocks from the file
            using (ByteSource TheReader = new ByteSource(AudioPath))
            {
                // Record the file length
                OriginalTotalFileLength = TheReader.Length;

                // Read the file header bytes
                byte[] HeaderBytes;
                try
                {
                    HeaderBytes = TheReader.GetBytes(4);
                }
                catch (Exception Ex)
                {
                    throw new BadAudioFileException("Error reading header from file in FLACFile constructor.\n" + Ex.Message);
                }

                // Check it has the correct file header
                if (!HeaderBytes.SequenceEqual(FileHeader))
                    throw new BadAudioFileException("FLAC file does not have the correct header in FLACFile constructor.");

                try
                {
                    // Iterate through the file reading the metadata blocks
                    do
                    {
                        // Read the header
                        MetadataBlockHeader TheHeader = new MetadataBlockHeader(TheReader.GetBytes(4));

                        // How much data do we need to read?
                        int BlockDataLength = TheHeader.DataLength;

                        // Is it the last block
                        IsLastBlock = TheHeader.IsLast;

                        byte[] Temp = new byte[] { };

                        if (!ReadOnly || TheHeader.BlockType == FlacMetadataBlockType.VORBIS_COMMENT)
                            Temp = TheReader.GetBytes(BlockDataLength);
                        else
                            // Move on to next block
                            TheReader.MoveTo(BlockDataLength, SeekOrigin.Current);
                        // Instantiate a new block and add it to the list
                        TheBlocks.AddBlock(new MetadataBlock(TheHeader, Temp));
                    } while (!IsLastBlock);
                }
                catch (Exception Ex)
                {
                    throw new BadAudioFileException("Error reading metadata blocks in FLACFile constructor.\n" + Ex.Message);
                }

                // Note where the metadata ends and the music starts
                StartOfMusic = TheReader.CurrentSeekPosition;
                NumberOfMusicBytes = OriginalTotalFileLength - StartOfMusic;
            } // Using

            // Check for illegal block counts
            if (TheBlocks.BlockCounts.CommentCount != 1)
                throw new BadAudioFileException(
                    "File has " + TheBlocks.BlockCounts[FlacMetadataBlockType.VORBIS_COMMENT]
                    + " VORBIS_COMMENT blocks.  So file is bad.");
            if (TheBlocks[0].BlockType != FlacMetadataBlockType.STREAMINFO)
                throw new BadAudioFileException("First metadata block is not of type STREAMINFO.  " +
                    "So file is bad.");

            // Parse the comment block's data and populate the list of tags
            PopulateTagList();
            HasEmbeddedGraphic = TheBlocks.BlockCounts.PictureCount > 0;
        } // Constructor



        /// <summary>
        /// Parse the data block of the comment block and populate the list of tags
        /// </summary>
        private void PopulateTagList()
        {
            using (ByteSource BlockBS = new ByteSource(TheBlocks[TheBlocks.IndexOfCommentBlock].DataBlock))
            {
                uint CommentLength;         // Holds the length of each comment in turn
                byte[] CommentRawArray;     // Hold the tag bytes prior to parsing

                // These are a mandatory part of a flac file's metadata
                vendor_length = BitConverter.ToUInt32(BlockBS.GetBytes(4), 0);
                vendor_string = Encoding.UTF8.GetString(BlockBS.GetBytes(Helpers.LongToInt(vendor_length)));

                // How many tags are there?
                int user_comment_list_length = Helpers.LongToInt(BitConverter.ToUInt32(BlockBS.GetBytes(4), 0));

                // Iterate through the tags, parsing each one
                for (int i = 0; i < user_comment_list_length; i++)
                {
                    CommentLength = BitConverter.ToUInt32(BlockBS.GetBytes(4), 0);
                    CommentRawArray = BlockBS.GetBytes(Helpers.LongToInt(CommentLength));
                    // Instantiate a new tag and add it to the list
                    TagType newTag = new TagType(CommentRawArray);
                    PrimaryTagList.AddTag(newTag);
                }
            }
        }


        /// <summary>
        /// Display debugging information on console.
        /// </summary>
        /// <param name="IncludeDetail">Should we display internal details?</param>
        /// <param name="IncludeTags">Should we also display tags?</param>
        public override void DebugPrint(bool IncludeDetail, bool IncludeTags)
        {
            // Refresh counts
            TheBlocks.UpdateBlockCounts();

            Console.WriteLine("===============================================================================");
            Console.WriteLine("File:                      " + Path.GetFileName(AudioPath));
            Console.WriteLine("Folder:                    " + Path.GetDirectoryName(AudioPath));
            Console.WriteLine("Time:                      " + DateTime.Now.ToString("h:mm:ss tt"));
            Console.WriteLine(string.Format("Original file length:     {0,11}", OriginalTotalFileLength.ToString("#,##0")));
            Console.WriteLine("===============================================================================");
            Console.WriteLine(string.Format("Number of blocks:                  {0,3}", TheBlocks.Count));
            Console.WriteLine(string.Format("Number of tags:                    {0,3}", PrimaryTagList.CountTags()));
            Console.WriteLine(string.Format("Start of music:            {0,11}", StartOfMusic.ToString("#,##0")));
            Console.WriteLine(string.Format("Length of music:           {0,11}", NumberOfMusicBytes.ToString("#,##0")));
            Console.WriteLine("Index of STREAMINFO block:           " + TheBlocks.IndexOfCommentBlock);
            Console.WriteLine("Index of padding block:              " + TheBlocks.IndexOfPaddingBlock);
            Console.WriteLine(string.Format("Size of STREAMINFO         {0,11}",
                (TheBlocks[TheBlocks.IndexOfCommentBlock].DataBlockLength + 4).ToString("#,##0")));
            Console.WriteLine(string.Format("Size of padding:           {0,11}",
                (TheBlocks[TheBlocks.IndexOfPaddingBlock].DataBlockLength + 4).ToString("#,##0")));
            Console.WriteLine(string.Format("STREAMINFO + padding:      {0,11}",
                (TheBlocks[TheBlocks.IndexOfCommentBlock].DataBlockLength + 4
                + TheBlocks[TheBlocks.IndexOfPaddingBlock].DataBlockLength + 4).ToString("#,##0")));
            Console.WriteLine(string.Format("Has embedded graphic:            {0,5}", HasEmbeddedGraphic));
            Console.WriteLine();

            int Count = 0;
            long PositionInFile = 4;        // Start after file header
            int TotalLength = 0;

            Console.WriteLine("          Pos'n     Length   Type");
            foreach (var MetadataBlock in TheBlocks)
            {
                string Temp = "";
                Temp += string.Format("   {0,2}  {1,8}   {2,8}   {3}",
                    Count++.ToString("D2"),
                    PositionInFile.ToString("#,##0"),
                    // Add 4 for block header
                    (MetadataBlock.DataBlockLength + 4).ToString("#,##0"),
                    MetadataBlock.BlockType);
                PositionInFile += MetadataBlock.DataBlockLength + 4;
                TotalLength += MetadataBlock.DataBlockLength + 4;
                Console.WriteLine(Temp);
            }
            Console.WriteLine(string.Format("   Total:        {0,9}", TotalLength.ToString("#,##0")));
            Console.WriteLine();

            if (IncludeDetail)
            {
                foreach (var MetadataBlock in TheBlocks)
                {
                    Console.Write(string.Format("{0,2} - ", Count++));
                    Console.WriteLine(MetadataBlock);
                    Console.WriteLine("     Start of header: " + string.Format("{0,8}", PositionInFile.ToString("#,##0")));
                    PositionInFile += 4;                                        // Step over block header
                    Console.WriteLine("     Start of data:   " + string.Format("{0,8}\n", PositionInFile.ToString("#,##0")));
                    PositionInFile += MetadataBlock.DataBlockLength;
                }
                Console.WriteLine();
            }

            if (IncludeTags)
            {
                foreach (var Tag in PrimaryTagList)
                    Console.WriteLine(Tag);
                Console.WriteLine();
            }

            Helpers.PressAnyKeyToContinue();
        }


        /// <summary>
        /// Walks through the current tag list and uses this to make a new
        /// Vorbis comment block.
        /// </summary>
        /// <param name="IsLast">Will the new block be last in list?</param>
        /// <returns>A new COMMENT block</returns>
        private MetadataBlock BuildNewCommentBlock(bool IsLast)
        {
            byte[] Temp;
            byte[] TagLengthBytes;      // Length of tag converted to an array of bytes
            uint LengthOfCurrentTag;    // Length of current tag

            // Start block with length of vendor_string followed by the string itself
            Temp = Helpers.JoinByteArrays(new List<byte[]>() { BitConverter.GetBytes(vendor_length), Encoding.UTF8.GetBytes(vendor_string) });

            // Append a uint representing how many tags to follow
            Temp = Helpers.JoinByteArrays(new List<byte[]>() { Temp, BitConverter.GetBytes((uint)PrimaryTagList.CountTags()) });

            // Look at each tag in turn and add its length and string to DataBlock
            foreach (TagType Tag in PrimaryTagList)
            {
                LengthOfCurrentTag = (uint)Tag.ToVorbisByteArray().Length;
                TagLengthBytes = BitConverter.GetBytes(LengthOfCurrentTag);
                Temp = Helpers.JoinByteArrays(new List<byte[]>() { Temp, TagLengthBytes });
                Temp = Helpers.JoinByteArrays(new List<byte[]>() { Temp, Tag.ToVorbisByteArray() });
            }
            MetadataBlockHeader Header = new MetadataBlockHeader(FlacMetadataBlockType.VORBIS_COMMENT, IsLast, Temp.Length);
            return new MetadataBlock(Header, Temp);
        }


        /// <summary>
        /// Build a new padding block.
        /// (All the bytes in a padding block are zero)
        /// </summary>
        /// <param name="IsLast">Will this block be the last metadata block?</param>
        /// <param name="DataSize">The size of the data (all null)</param>
        /// <returns>The new block</returns>
        private MetadataBlock BuildNewPaddingBlock(bool IsLast, int DataSize)
        {
            MetadataBlockHeader Temp = new MetadataBlockHeader(FlacMetadataBlockType.PADDING, IsLast, DataSize);
            return new MetadataBlock(Temp, Enumerable.Repeat((byte)0x00, DataSize).ToArray());
        }


        /// <summary>
        /// 1. Uses the amended tag list to generate a comment block.
        /// 2. If there is no padding block, it adds one.
        /// 3. If there is an existing padding block, adjust its size to
        ///    reflect new tag size.
        /// 4. Sets global 'MustRewriteWholeFile' flag accordingly
        /// </summary>
        protected override void InMemoryUpdate()
        {
            // Record the length of the current comment block
            int OldCommentBlockLength = TheBlocks[TheBlocks.IndexOfCommentBlock].DataBlockLength;
            // Generate a new comment block from the updated tag list
            TheBlocks[TheBlocks.IndexOfCommentBlock] = BuildNewCommentBlock(TheBlocks[TheBlocks.IndexOfCommentBlock].IsLast);

            // Calculate the increase (or decrease) in size of comments
            int ChangeInCommentSize = TheBlocks[TheBlocks.IndexOfCommentBlock].DataBlockLength - OldCommentBlockLength;

            // In my collection of 29000 flacs, none had more than one padding block, 
            // so taking no action to deal with this. 

            // If no padding block, then add one
            if (TheBlocks.BlockCounts.PaddingCount == 0)
            {
                // Add a padding block
                int NewpaddingBlockSize = 1024;
                TheBlocks.AddBlock(BuildNewPaddingBlock(true, NewpaddingBlockSize));
                MustRewriteWholeFile = true;
            }
            else
            {   // There is at least one padding block

                // Did comment block shrink?
                if (ChangeInCommentSize <= 0)
                {
                    // Grow padding block to take up slack 
                    // Subtract ChangeInCommentSize because it is negative
                    TheBlocks[TheBlocks.IndexOfPaddingBlock] =
                        BuildNewPaddingBlock(TheBlocks[TheBlocks.IndexOfPaddingBlock].IsLast,
                        TheBlocks[TheBlocks.IndexOfPaddingBlock].DataBlockLength - ChangeInCommentSize);
                    MustRewriteWholeFile = false;
                }
                else
                {
                    // Comment block grew (and there is a pre-existing padding block)
                    if (ChangeInCommentSize <= TheBlocks[TheBlocks.IndexOfPaddingBlock].DataBlockLength)
                    {
                        // Shrink existing padding block to make space
                        TheBlocks[TheBlocks.IndexOfPaddingBlock] =
                        BuildNewPaddingBlock(TheBlocks[TheBlocks.IndexOfPaddingBlock].IsLast,
                        TheBlocks[TheBlocks.IndexOfPaddingBlock].DataBlockLength - ChangeInCommentSize);
                        MustRewriteWholeFile = false;
                    }
                    else
                    {
                        // Comment block grew too much just to shrink existing padding block.
                        // So rewrite whole file.
                        MustRewriteWholeFile = true;
                    }
                }
            }
        }


        /// <summary>
        /// Called if necessary to rewrite the whole file.
        /// i.e.  New metadata is too big to fit over top of old metadata.
        /// </summary>
        /// <param name="SavePath">Path and file name to which the file will be written.</param>
        protected override void RewriteWholeFile(string SavePath)
        {
            try
            {
                using (ByteSource TheReader = new ByteSource(AudioPath))
                using (BinaryWriter TheWriter =
                        new BinaryWriter(File.Open(SavePath, FileMode.Create, FileAccess.Write)))
                {
                    // Write 'FLAC' file header
                    TheWriter.Write(FileHeader);

                    // Write all of the blocks
                    TheBlocks.WriteAllBlocksToFile(TheWriter, true);

                    // Now append the music
                    TheReader.MoveTo(StartOfMusic);
                    StartOfMusic = TheWriter.BaseStream.Position;
                    TheWriter.Write(TheReader.GetRestOfBytes());
                    OriginalTotalFileLength = TheWriter.BaseStream.Position;
                }
            }
            catch (Exception Ex)
            {
                throw new IOException("Error doing full over-write of file in FlacFile.OverwriteFile()."
                    + Ex.Message);
            }
        }


        /// <summary>
        /// Called if we are able to overwrite just part of the metadata.
        /// i.e.  Modified metadata will fit over old metadata.
        /// </summary>
        protected override void OverwriteWindow()
        {
            try
            {
                // Now overwrite all blocks in the overwrite window
                using (BinaryWriter TheWriter =
                    new BinaryWriter(File.Open(AudioPath, FileMode.Open, FileAccess.Write)))
                {
                    TheBlocks.OverwriteSubsetOfBlocksInFile(TheWriter);
                } // using
            } // try
            catch (Exception Ex)
            {
                throw new IOException("Error overwriting data inside FLAC file in FlacFile.OverwriteWindow()."
                    + Ex.Message);
            }
        }


        /// <summary>
        /// Used for testing purposes only.
        /// Used to create Flac files with unusual internal layouts.
        /// Offers user a chance to delete or add, or re-order metadata blocks.
        /// Then writes to file without any further internal adjustemnts.
        /// </summary>
        public override void SaveTestFile()
        {
            int a, x, y;
            do
            {
                Console.WriteLine("  1. To delete a block");
                Console.WriteLine("  2. Add an additional Padding Block");
                Console.WriteLine("  3. To swap blocks");
                Console.WriteLine("  4. To resize a padding block");
                Console.WriteLine("  5. To exit and save");
                Console.WriteLine("  6. To exit without saving.");
                a = Helpers.readInt("Enter choice: ", 1, 5);
                switch (a)
                {
                    case 1:
                        x = Helpers.readInt("Enter position of first block to be deleted (-1 to exit): ", -1, TheBlocks.Count);
                        if (x != -1)
                            TheBlocks.RemoveBlockAt(x);
                        break;
                    case 2:
                        x = Helpers.readInt("Enter size for new padding block: ", 0, 256 * 256);
                        TheBlocks.AddBlock(BuildNewPaddingBlock(TheBlocks[TheBlocks.IndexOfPaddingBlock].IsLast, x));
                        break;
                    case 3:
                        do
                        {
                            x = Helpers.readInt("Enter position of first block to be swapped (-1 to exit):  ", -1, TheBlocks.Count);
                            y = Helpers.readInt("Enter position of second block to be swapped (-1 to exit): ", -1, TheBlocks.Count);
                        } while (x == y);
                        if (x != -1 && y != -1)
                            TheBlocks.SwapBlocks(x, y);
                        break;
                    case 4:
                        x = Helpers.readInt("Enter new size for padding block (Currently " + TheBlocks[TheBlocks.IndexOfPaddingBlock].DataBlockLength +
                            "): ", 0, 256 * 256);
                        TheBlocks[TheBlocks.IndexOfPaddingBlock] = BuildNewPaddingBlock(TheBlocks[TheBlocks.IndexOfPaddingBlock].IsLast, x);
                        break;
                    default:
                        break;
                } // Switch
            } while (a < 5);

            TheBlocks.UpdateBlockCounts();

            if (a != 6)
            {
                string Name = Helpers.readString("Enter name for test file: ");
                // Build path of temp file
                string Folder = Path.GetDirectoryName(AudioPath);
                string TempPath = Folder + "\\" + Name;

                if (File.Exists(TempPath))
                    File.Delete(TempPath);

                try
                {
                    using (ByteSource TheReader = new ByteSource(AudioPath))
                    using (BinaryWriter TheWriter =
                            new BinaryWriter(File.Open(TempPath, FileMode.Create, FileAccess.Write)))
                    {
                        // Write 'FLAC' file header
                        TheWriter.Write(FileHeader);

                        // Write all of the blocks
                        TheBlocks.WriteAllBlocksToFile(TheWriter, false);

                        // Now append the music
                        TheReader.MoveTo(StartOfMusic);
                        TheWriter.Write(TheReader.GetRestOfBytes());
                    }
                }
                catch (Exception Ex)
                {
                    throw new IOException("Error creating test file.\n" + Ex.Message);
                }
            } // if
        }


        // Static instance member for use with GetDumpDetails() method.
        static private bool AleadyDone = false;


        /// <summary>
        /// This is an 'Func' that can be passed to
        /// AudioFile.FileWalk() to dump file info about a set of flac files
        /// </summary>
        /// <param name="FilePath">The path to the mp3 files</param>
        /// <param name="TheWriter">path for a log file</param>
        /// <returns>Not used in this case - so 1 returned each time.</returns>
        static public int GetDumpDetails(string FilePath, LazySW TheWriter)
        {
            // Write a header line (just once)
            if (!AleadyDone)
            {
                string Header = "File\tAPPLICATION\tCUESHEET\tINVALID\tPADDING\tSEEKTABLE\tSTREAMINFO\tVORBIS_COMMENT";
                if (TheWriter != null)
                    TheWriter.WriteLine(Header);
                Console.WriteLine(Header);
                AleadyDone = true;
                return 1;
            }

            // Now write details of current file.
            FLACFile Flac = new FLACFile(FilePath, true);

            string Temp = FilePath;
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.APPLICATION];
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.CUESHEET];
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.INVALID];
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.PADDING];
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.SEEKTABLE];
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.STREAMINFO];
            Temp += "\t" + Flac.TheBlocks.BlockCounts[FlacMetadataBlockType.VORBIS_COMMENT];
            Temp += "\t";

            if (TheWriter != null)
                TheWriter.WriteLine(Temp);
            Console.WriteLine(Temp);
            return 1;
        }


        /// <summary>
        /// Adds a new tag to the tag list
        /// </summary>
        /// <param name="Name">Name field of the new tag</param>
        /// <param name="Value">Value field of the new tag</param>
        public void AddTag(string Name, string Value) => PrimaryTagList.AddTag(Name, Value);


        /// <summary>
        /// Counts how many tags have the specified name.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        /// <returns>How many are found</returns>
        public int CountTags(string Name) => PrimaryTagList.CountTags(Name);


        /// <summary>
        /// Sees whether any tags with the given name exist.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        /// <returns>WHether or not any are found</returns>
        public bool Exists(string Name) => PrimaryTagList.Exists(Name);


        /// <summary>
        /// Returns the value field of the first tag in the list 
        /// with the name as specified
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        /// <returns>The value of the first matching tag</returns>
        public string First(string Name) => PrimaryTagList.First(Name);


        /// <summary>
        /// Removes all tags from the list which have name equal
        /// to the value specified.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        public void RemoveAll(string Name) => PrimaryTagList.RemoveAll(Name);


        /// <summary>
        /// Removes the first tag from the list which has name
        /// equal to the value specified.
        /// </summary>
        /// <param name="Name">The name to be searched for.</param>
        public void RemoveFirst(string Name) => PrimaryTagList.RemoveFirst(Name);


        /// <summary>
        /// Removes any tags with name and value matching those specified
        /// </summary>
        /// <param name="Name">Name to match</param>
        /// <param name="Value">Value to match</param>
        public void RemoveExact(string Name, string Value) => PrimaryTagList.RemoveExact(Name, Value);


        /// <summary>
        /// Remove all tags with the same name and adds a new tag
        /// with the name and value supplied
        /// </summary>
        /// <param name="Name">Name to be removed then added</param>
        /// <param name="Value">Value for the new tag to be added</param>
        public void ReplaceAll(string Name, string Value) => PrimaryTagList.ReplaceAll(Name, Value);


        /// <summary>
        /// Version property
        /// </summary>
        static public string Version
        {
            get
            {
                string Temp = _Version + "\n";
                Temp += "   " + AudioFile._Version + "\n";
                Temp += "   " + MetadataBlockTypeCounter._Version + "\n";
                Temp += "   " + BlockListType._Version + "\n";
                Temp += "   " + MetadataBlockHeader._Version + "\n";
                Temp += "   " + MetadataBlock._Version + "\n";
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