//========================================================================
// Name:     COMMON_TagType.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements a Tag type that is simple a (Name, Value) pair of
//           strings.  Also implements methods to convert them to and 
//           from other class objects.
// Comments: 
//========================================================================
using System.Text;
using System;
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// Class to hold a (Name, Value) tag
    /// </summary>
    internal class TagType
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "TagType:                   1.03";


        /// <summary>
        /// The name of the tag - e.g. Artist
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// The value of the tag - e.g. Pink Floyd
        /// </summary>
        public string Value { get; private set; }


        /// <summary>
        /// Used to format ToString() output
        /// </summary>
        private static int LongestTagName = 0;


        /// <summary>
        /// Constructor.
        /// Takes two strings: Name and Value.
        /// </summary>
        /// <param name="name">String for tag 'name'</param>
        /// <param name="value">String for tag 'value'</param>
        public TagType(string name, string value)
        {
            Name = name; Value = value;
            if (Name.Length > LongestTagName) LongestTagName = Name.Length;
        }


        /// <summary>
        /// Constructor
        /// Makes a Tag from an V23 Frame
        /// </summary>
        /// <param name="TheFrame"></param>
        public TagType(V23Frame TheFrame)
        {
            if (!TheFrame.IsText)
                throw new Exception("Trying to instantiate a Tag with a non-text frame.");
            Name = TheFrame.Name;
            Value = TheFrame.Value;
        }


        /// <summary>
        /// Constructor
        /// Pass it an array of bytes representing a FLAC/Ogg/Vorgis comment
        /// and it extracts name and value to build a tag.
        /// </summary>
        /// <param name="TheBytes">Bytes representing the comment or frame</param>
        public TagType(byte[] TheBytes)
        {
            int CommentRawArrayLength;       // How long is the raw comment
            int EqualsPos;                   // Position of the equals symbol
            byte[] TagNameArray;             // Temp array to hold tag name array
            byte[] TagValueArray;            // Temp array to hold tag value array

            // Get length of the raw array
            CommentRawArrayLength = TheBytes.Length;

            // FindEquals position of equals symbol
            EqualsPos = FindEquals(TheBytes);

            // Extract the name part
            TagNameArray = new byte[EqualsPos];
            Array.ConstrainedCopy(TheBytes, 0, TagNameArray, 0, EqualsPos);
            Name = Encoding.ASCII.GetString(TagNameArray);
            if (Name.Length > LongestTagName) LongestTagName = Name.Length;

            // Extract the value part
            TagValueArray = new byte[CommentRawArrayLength - EqualsPos - 1];
            Array.ConstrainedCopy(TheBytes, EqualsPos + 1, TagValueArray, 0, CommentRawArrayLength - EqualsPos - 1);
            Value = Encoding.UTF8.GetString(TagValueArray);
        }


        /// <summary>
        /// Returns an array of bytes representing the fields of a tag
        /// encoded as a FLAC/Ogg/Vorbis comment, suitable for inserting 
        /// into a FLAC file.
        /// </summary>
        /// <returns>FLAC/Ogg/Vorbis comment byte array</returns>
        public byte[] ToVorbisByteArray()
        {
            byte[] TagName = Encoding.ASCII.GetBytes(Name);
            byte[] Equal = new byte[] { 0x3d };
            byte[] TagValue = Encoding.UTF8.GetBytes(Value);
            return Helpers.JoinByteArrays(new List<byte[]>() { TagName, Equal, TagValue });
        }


        /// <summary>
        /// Debugging method.
        /// Allows us to hand-generate some ID3 v2.3 frames, in order to
        /// then see whether we can re-read them.
        /// </summary>
        /// <param name="name">Name of the frame</param>
        /// <param name="value">Value of the frame</param>
        /// <returns>A new ID3v23Frame</returns>
        static public byte[] ToID3v23Frame(string name, string value)
        {
            byte[] ID = Encoding.ASCII.GetBytes(name);
            byte[] Flags = { 0x00, 0x00 };
            byte[] EncodingFlag = { 0x00 };
            byte[] PayLoad = Encoding.ASCII.GetBytes(value);
            byte[] DataLength = Helpers.IntToFourByteBigEndian(PayLoad.Length);

            return Helpers.JoinByteArrays(new List<byte[]>() { ID, DataLength, Flags, EncodingFlag, PayLoad });
        }


        /// <summary>
        /// Override of ToString()
        /// </summary>
        /// <returns>A string represention of a tag</returns>
        public override string ToString()
        {
            int MaxLength = 79;

            string Temp = Name.PadLeft(TagType.LongestTagName + 1) + " = " + Value;
            return Temp.Substring(0, (Temp.Length > MaxLength ? MaxLength : Temp.Length));
        }


        /// <summary>
        /// Returns a Tag containing a flag to indicate that this 
        /// library was used.
        /// </summary>
        /// <returns>A new tag</returns>
        static public TagType WaterMark()
        {
            return new TagType("JAudioTags", "Library used on " + DateTime.Now.Date.ToString("dd/MM/yyyy"));
        }


        /// <summary>
        /// Pass it a byte array and it returns the position of the 
        /// first '=' (0x3d). Otherwise throws an exception.
        /// Used with FLAC Vorbis tags.
        /// </summary>
        /// <param name="TheBytes">The byte array to be searched</param>
        /// <returns>Position of the first '='</returns>
        static private int FindEquals(byte[] TheBytes)
        {
            int i = 0;
            while (i < TheBytes.Length)
            {
                if (TheBytes[i] == 0x3d)
                    return i;
                else
                    i++;
            }
            throw new BadAudioFileException("No '=' symbol in comment in TagType.FindEquals().");
        }
    }
}