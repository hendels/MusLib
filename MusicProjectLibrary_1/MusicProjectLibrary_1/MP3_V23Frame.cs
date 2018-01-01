//========================================================================
// Name:     MP3_V23Frame.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  An MP3/ID3 v 2.3 file stores its metadata in a block of data
//           a tag.  Inside this tag is a  set of components called 
//           frames. This implements a class to represent a frame.. 
// Comments: 
//========================================================================
using System;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// An ID3 v 2.3 file has a block of bytes called a 'Tag'.
    /// This tag consists of a set of 'frames'.  Each frame holds
    /// a piece of metadata, e.f Artist="Abba".
    /// This class represents a frame.
    /// </summary>
    internal class V23Frame
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "V23Frame:                  1.03";


        /// <summary>
        /// Just one 'real' property.
        /// An array of bytes holding the data read from disc.
        /// </summary>
        public byte[] TheBytes { get; private set; }


        /// <summary>
        /// A calculated property tells whether this frame
        /// is a type that can hold text.  Either its name
        /// begins with a 'T' - i.e. it is a text frame,
        /// or its name is COMM and the first data byte
        /// is 0x01 - indicating a textual COMM frame.
        /// </summary>
        public bool IsText
        {
            get
            {
                if (TheBytes.Count() <= 10)
                    return false;
                if (TheBytes[0] == 'T'
                    || (TheBytes[0] == (byte)'C'
                        && TheBytes[1] == (byte)'O'
                        && TheBytes[2] == (byte)'M'
                        && TheBytes[3] == (byte)'M'
                        && TheBytes[10] == 0x01))
                    return true;
                else
                    return false;
            }
        }


        /// <summary>
        /// Calculated property to return the ID string
        /// </summary>
        public string Name
        {
            get
            {
                return Encoding.ASCII.GetString(TheBytes.Take(4).ToArray());
            }
        }


        /// <summary>
        /// Calculated property to return the value string
        /// </summary>
        public string Value
        {
            get
            {
                if (IsText && (Name == "COMM"))
                    return BOM.DecodeUTF16WithBOM(CommTags.AfterTwoNulls(TheBytes.Skip(10).ToArray()));
                if (IsText && (Name[0] == 'T'))
                    return BOM.DecodeWithFlag(TheBytes.Skip(10).ToArray());
                return "Not text: " + Encoding.ASCII.GetString(TheBytes.Skip(10).ToArray());
            }
        }


        /// <summary>
        /// Property returns the total length of frame.
        /// </summary>
        public int FrameSize
        {
            get
            {
                return TheBytes.Length;
            }
        }


        /// <summary>
        /// Constructor - to build a frame using bytes read from disc
        /// </summary>
        /// <param name="theBytes">The bytes from disc representing the frame</param>
        public V23Frame(byte[] theBytes)
        {
            if (theBytes == null)
                throw new ArgumentNullException("Null argument passed to V23Frame constructor.");
            if (theBytes.Length == 0)
                throw new ArgumentOutOfRangeException("Zero length byte array passed to V23Frame constructor.");
            TheBytes = theBytes;
        }


        /// <summary>
        /// Constructor - Generates a frame from a TagType
        /// </summary>
        /// <param name="TheTag">The tag used to make the frame</param>
        public V23Frame(TagType TheTag)
        {
            if (TheTag.Name[0] != 'T' && TheTag.Name != "COMM")
                throw new ArgumentException("V23Frame constructor only accepts text or COMM tags.");

            if (TheTag.Name.Length != 4)
                throw new ArgumentException("V23Frame constructor tags with name 4 chars long.");

            byte[] IDBytes = BOM.EncodeNoFlagNoBOM(TheTag.Name, EncodingType.iso8859);
            byte[] DataSize;
            byte[] Flags = new byte[] { 0x00, 0x00 };
            byte[] Data = new byte[] { };

            if (TheTag.Name[0] == 'T')
            {
                EncodingType TextEncoding;
                // This reflects what I found in my pre-existing mp3 collection.
                if (TheTag.Name == "TPOS" || TheTag.Name == "TRCK" || TheTag.Name == "TYER")
                    TextEncoding = EncodingType.iso8859;
                else
                    TextEncoding = EncodingType.utf16le;
                Data = BOM.EncodeWithFlagAndBOM(TheTag.Value, TextEncoding);
            }

            if (TheTag.Name == "COMM")
            {
                byte[] Flag = new byte[] { 0x01 };
                byte[] LanguageCode = Encoding.ASCII.GetBytes(CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName);
                byte[] Description = new byte[] { 0xFF, 0xFE };
                byte[] Seperator = new byte[] { 0x00, 0x00 };
                byte[] Payload = BOM.EncodeNoFlagWithBOM(TheTag.Value, EncodingType.utf16le);
                Data = Helpers.JoinByteArrays(new List<byte[]>() { Flag, LanguageCode, Description, Seperator, Payload });
            }

            DataSize = Helpers.IntToFourByteBigEndian(Data.Length);

            TheBytes = Helpers.JoinByteArrays(new List<byte[]>() { IDBytes, DataSize, Flags, Data });
        }


        /// <summary>
        /// Overwrite of ToString()
        /// </summary>
        /// <returns>A string representation of a frame</returns>
        public override string ToString()
        {
            string ID = "ID:         " + Name;
            string Val = "Value:      " + Value;
            string Size = "Total size: " + FrameSize;
            string Flags = String.Format("Flags:      0x{0:X02}:0x{1:X02}", TheBytes[8], TheBytes[9]);
            return ID + "\n" + Size + "\n" + Flags + "\n" + Val + "\n";
        }
    }
}