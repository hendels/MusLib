//========================================================================
// Name:     MP3V23Tag.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  An MP3/ID3 v2.3 file stores its metadata in a large block 
//           called a tag.  Inside the tag is a sequence of 'frames'.
//           Each frame holds an individual item of metadata.  This file
//           implements a class to process a tag.
//
// Comments: Beware!  'Tag' as used in this sense is different to its 
//           normal use.  In everyday use (and it the rest of this code)
//           a tag is an item of metadata, for example ARTIST=ABBA.  Here
//           'Tag' refers to a block of bytes representing a sequence of
//           frames, each of which contains an item such as ARTIST=ABBA.
//========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;


namespace JAudioTags
{
    /// <summary>
    /// Represents an ID3 v2.3 Tag.
    /// It is a list of Frames with a handful of methods added.
    /// </summary>
    internal class V23Tag : List<V23Frame>
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "V23Tag:                    1.20";


        /// <summary>
        /// Is there an APIC frame in this tag?
        /// </summary>
        public bool HasEmbeddedGraphic { get; private set; }


        /// <summary>
        /// What is the total length of all the frames.
        /// By subtracting this from the frame length
        /// we calculate how much padding there is.
        /// </summary>
        public long SumOfFrameLengths
        {
            get
            {
                long Temp = 0;
                foreach (var Frame in this)
                    Temp += Frame.FrameSize;
                return Temp;
            }
        }


        /// <summary>
        /// Returns the Tag as an array of bytes.
        /// </summary>
        /// <returns>An array of bytes representing the tag </returns>
        public byte[] ToBytes()
        {
            byte[] Temp = new byte[SumOfFrameLengths];

            int StartPos = 0;
            int FrameSize;
            foreach (V23Frame Frame in this)
            {
                FrameSize = Helpers.LongToInt(Frame.FrameSize);
                Buffer.BlockCopy(Frame.TheBytes, 0, Temp, StartPos, FrameSize);
                StartPos += FrameSize;
            }
            return Temp;
        }


        /// <summary>
        /// Read through the tag section of the file.  
        /// Add each frame to the list.
        /// </summary>
        /// <param name="TheReader">A byte source attached to the file</param>
        /// <param name="ReadOnly">Was the file opened in ReadOnly mode?</param>
        /// <returns></returns>
        public void PopulateFrameList(ByteSource TheReader, bool ReadOnly)
        {
            byte[] Header;
            byte[] ID;
            byte[] Data;

            V23Tag Temp = new V23Tag();

            bool EOF = false;

            uint DataSize;

            try
            {
                using (TheReader)
                {
                    do
                    {
                        // Read enough to re-write
                        if (TheReader.BytesRemaining >= 10 && !Helpers.BytesAreAll(TheReader.PeekBytes(4), 0))
                        {
                            Data = new byte[] { };

                            Header = TheReader.GetBytes(10);
                            ID = Header.Take(4).ToArray();
                            DataSize = Helpers.ConvertFromBigEndian(Header.Skip(4).Take(4).ToArray());

                            if (Helpers.Equal(ID, "APIC"))
                                HasEmbeddedGraphic = true;

                            if (ReadOnly)
                            {
                                if (ID[0] == (byte)'T'
                                    || (!TheReader.EOF && Helpers.Equal(ID, "COMM") && TheReader.PeekBytes(1)[0] == 0x01))
                                    Data = TheReader.GetBytes(DataSize);
                                else
                                    TheReader.MoveTo(DataSize, SeekOrigin.Current);
                            }
                            else
                                Data = TheReader.GetBytes(DataSize);

                            this.Add(new V23Frame(Helpers.JoinByteArrays(new List<byte[]>() { Header, Data })));
                        }
                        else
                            EOF = true;
                    } while (!EOF);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error extracting tags from frame.\n" + ex.Message);
            }
        }


        /// <summary>
        /// Returns a string representation of the tag
        /// </summary>
        /// <param name="StartPosOfTag">The start position of the tag</param>
        /// <returns>A string represnetation </returns>
        public string ToString(long StartPosOfTag)
        {
            string Temp = "    Name Pos'n   Length  Value\n";
            string Line = "";
            int MaxLength = 79;

            int i = 1;
            long StartPos = StartPosOfTag;

            foreach (V23Frame Frame in this)
            {
                Line = String.Format("{0,3} {1} {2,5}   {3,6}  {4}",
                    i++,
                    Frame.Name,
                    StartPos,
                    Frame.FrameSize,
                    Frame.Value);
                Temp += Line.Substring(0, (Line.Length > MaxLength ? MaxLength : Line.Length)) + "\n";
                StartPos += Frame.FrameSize;
            }
            Temp += string.Format("Sum of frame lengths:    {0,9}\n", SumOfFrameLengths.ToString("#,##0"));
            return Temp;
        }


        /// <summary>
        /// Removes a non-text frame from the frame list.
        /// Use to delete unwanted PRIV or COMM tags
        /// </summary>
        /// <param name="TargetName">The name of the tag to remove</param>
        public void BinaryFrameTypeRemoveAll(string TargetName)
        {
            this.RemoveAll(
                delegate (V23Frame TheFrame)
                {
                    return TheFrame.Name.ToUpper() == TargetName.ToUpper() && !(TheFrame.IsText);
                }
                );
        }
    }
}