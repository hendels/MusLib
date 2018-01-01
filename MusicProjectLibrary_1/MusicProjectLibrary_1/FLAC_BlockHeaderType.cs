//========================================================================
// Name:     FLAC_BlockHeaderType.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  A FLAC file stores its metadata in a series of blocks.  Each
//           block has a header.  This file provides a class to represent 
//           these headers.
// Comments: 
//========================================================================
using System;
using System.Collections.Generic;


// A FLAC file is structured as follows:
//   A header: 4 bytes = "fLaC"
//   One or more METADATA_BLOCKs
//     Each METADATA block consist of
//       A Header, which consists of
//         1 bit:  1 if this the last block, else 0
//         7 bits: Indicating block type
//           0 = STREAMINFO
//           1 = PADDING
//           2 = APPLICATION
//           3 = SEEKTABLE
//           4 = VORBIS_COMMENT
//           5 = CUESHEET
//           6 = PICTURE
//      3 bytes = length of the data in this block
// One or more audio frames


namespace JAudioTags
{
    /// <summary>
    /// Class to represent the 4-byte header of a FLAC metadata block.
    /// </summary>
    internal class MetadataBlockHeader
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "MetadataBlockHeader:       1.03";


        /// <summary>
        /// Array to hold the 4-byte header
        /// </summary>
        public byte[] TheBytes { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="theBytes">The four byte header as read from disc</param>
        public MetadataBlockHeader(byte[] theBytes)
        {
            if (theBytes == null)
                throw new ArgumentNullException("Null byte array MetadataBlockHeader constructor.");
            if (theBytes.Length != 4)
                throw new ArgumentException("Header passed to MetadataBlockHeader constructor has incorrecr length.\n"
                    + "Length = " + theBytes.Length);
            TheBytes = theBytes;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Type">What kind of block is this a header for?</param>
        /// <param name="IsLast">Is it the last block?</param>
        /// <param name="LengthOfData">How long is the associated data?</param>
        public MetadataBlockHeader(FlacMetadataBlockType Type, bool IsLast, int LengthOfData)
        {
            byte FirstByte;

            // Set bit 7
            if (IsLast)
                FirstByte = 128;
            else
                FirstByte = 0;

            // Now set the other 7 bits
            switch (Type)
            {
                case FlacMetadataBlockType.STREAMINFO:
                    FirstByte |= 0;
                    break;
                case FlacMetadataBlockType.PADDING:
                    FirstByte |= 1;
                    break;
                case FlacMetadataBlockType.APPLICATION:
                    FirstByte |= 2;
                    break;
                case FlacMetadataBlockType.SEEKTABLE:
                    FirstByte |= 3;
                    break;
                case FlacMetadataBlockType.VORBIS_COMMENT:
                    FirstByte |= 4;
                    break;
                case FlacMetadataBlockType.CUESHEET:
                    FirstByte |= 5;
                    break;
                case FlacMetadataBlockType.PICTURE:
                    FirstByte |= 6;
                    break;
                case FlacMetadataBlockType.INVALID:
                    FirstByte |= 127;
                    break;
                default:
                    break;
            }
            byte[] Temp = new byte[] { FirstByte };
            TheBytes = Helpers.JoinByteArrays(new List<byte[]>() { Temp, Helpers.UIntToThreeByteBigEndian((uint)LengthOfData) });
        }


        /// <summary>
        /// Property to return the type of block this is header for.
        /// </summary>
        public FlacMetadataBlockType BlockType
        {
            get
            {
                int Value = Helpers.ValueOfRightBits(TheBytes[0], 7);
                switch (Value)
                {
                    case 0:
                        return FlacMetadataBlockType.STREAMINFO;
                    case 1:
                        return FlacMetadataBlockType.PADDING;
                    case 2:
                        return FlacMetadataBlockType.APPLICATION;
                    case 3:
                        return FlacMetadataBlockType.SEEKTABLE;
                    case 4:
                        return FlacMetadataBlockType.VORBIS_COMMENT;
                    case 5:
                        return FlacMetadataBlockType.CUESHEET;
                    case 6:
                        return FlacMetadataBlockType.PICTURE;
                    default:
                        throw new BadAudioFileException("Incorrect BlockType bits read in MetadataBlockHeader.BlockType().\n" +
                            "Value read = " + Value);
                }
            }
        }


        /// <summary>
        /// Property to return the length of the data in this block
        /// </summary>
        public int DataLength
        {
            get
            {
                return Helpers.LongToInt(Helpers.ConvertFromBigEndian(new byte[] { TheBytes[1], TheBytes[2], TheBytes[3] }));
            }
        }


        /// <summary>
        /// Property to return whether or not this is the last block.
        /// </summary>
        public bool IsLast
        {
            get
            {
                return Helpers.IsBitSet(TheBytes[0], 7);
            }
        }


        /// <summary>
        /// Returns a string representaion of a clock header.
        /// </summary>
        /// <returns>The string represnetation</returns>
        public override string ToString()
        {
            // Print the four bytes of the header
            string s1 = String.Format("{0,9} : {1,3} : {2,3} : {3,3}",
                TheBytes[0].ToString("D3"), TheBytes[1].ToString("D3"),
                TheBytes[2].ToString("D3"), TheBytes[3].ToString("D3"));

            // Convert the first byte to binary
            string Temp = Convert.ToString(TheBytes[0], 2).PadLeft(8, '0');
            // Add a space between bit 7 and the rest
            string s2 = "                    " + Temp.Substring(0, 1) + " " + Temp.Substring(1, 7);
            // Express the last three bytes as an int
            string s3 = Helpers.ConvertFromBigEndian(new byte[] { TheBytes[1], TheBytes[2], TheBytes[3] }).ToString();
            // Evaluate the first 7 bits of the first byte
            string s4 = "                    " + Temp.Substring(0, 1) + " " +
                        string.Format("{0,7}", Helpers.ValueOfRightBits(TheBytes[0], 7).ToString()) + " : " + s3;
            // Put it all together
            return s1 + "\n" + s2 + " : " + s3 + "\n" + s4;
        }
    }
}