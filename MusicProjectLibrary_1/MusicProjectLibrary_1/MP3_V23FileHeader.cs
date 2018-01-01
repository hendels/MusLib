//========================================================================
// Name:     MP3_V23FileHeader.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Provides a class to represent the header of an MP3/ID3 v 2.3
//           ausio file.
// Comments: 
//========================================================================
using System.Linq;
using System.Collections.Generic;



namespace JAudioTags
{
    /// <summary>
    /// Represents the header of an ID3 v 2.3 audio file
    /// </summary>
    internal class V23FileHeader
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "V23FileHeader:             1.02";


        /// <summary>
        /// Does this file start with the characters I D 3
        /// </summary>
        public bool IsID3 { get; private set; } = false;


        /// <summary>
        /// Does it state in this file that the ID3 version is 2.3?
        /// </summary>
        public bool VersionIs23 { get; private set; } = false;


        /// <summary>
        /// Does this file use Unsynchronisation?
        /// </summary>
        public bool UnsynchronisationUsed { get; private set; } = false;


        /// <summary>
        /// Does thsi file have an extended header
        /// </summary>
        public bool HasExtendedHeader { get; private set; } = false;


        /// <summary>
        /// How long does it say the tag is?
        /// </summary>
        public long TagLength { get; private set; }


        /// <summary>
        /// At what position within the file does the tag start?
        /// </summary>
        public int StartOfTag { get; private set; }


        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="TheReader">A ByteSource connected to the MP3 file on disc</param>
        public V23FileHeader(ByteSource TheReader)
        {
            byte[] TheBytes = TheReader.GetBytes(10);
            if (TheBytes.Take(3).ToArray().SequenceEqual(new byte[] { (byte)'I', (byte)'D', (byte)'3' }))
                IsID3 = true;
            if (TheBytes.Skip(3).Take(1).ToArray()[0] == 0x03)
                VersionIs23 = true;

            byte Flags = TheBytes.Skip(5).Take(1).ToArray()[0];
            UnsynchronisationUsed = Helpers.IsBitSet(Flags, 7);
            HasExtendedHeader = Helpers.IsBitSet(Flags, 6);
            TagLength = SyncSafe.FromSyncSafe(TheBytes.Skip(6).Take(4).ToArray());
            StartOfTag = Helpers.LongToInt(TheReader.CurrentSeekPosition);

            // If there is an extended header, move past it
            if (HasExtendedHeader)
            {
                int ExtendedHeaderSize = SyncSafe.FromSyncSafe(TheReader.GetBytes(4));
                TheReader.GetBytes(ExtendedHeaderSize);
                StartOfTag = Helpers.LongToInt(TheReader.CurrentSeekPosition);
            }
        }


        /// <summary>
        /// Return the header as a byte array
        /// </summary>
        /// <param name="TagSize">Hoe big is the current tag?</param>
        /// <returns>A byte array</returns>
        public byte[] ToBytes(int TagSize)
        {
            byte[] Signature = new byte[] { (byte)'I', (byte)'D', (byte)'3' };
            byte[] Version = new byte[] { 0x02, 0x03 };   // Version 2.3
            byte[] Flags = new byte[] { 0x00 };           // No unsynchronization and no extended header.
            byte[] TagLength = Helpers.IntToFourByteBigEndian(TagSize);

            return Helpers.JoinByteArrays(new List<byte[]>() { Signature, Version, Flags, TagLength });
        }


        /// <summary>
        /// Generates a string representation of the file header
        /// </summary>
        /// <returns>A string representation of the file header</returns>
        public override string ToString()
        {
            string Temp = "Signature:              'ID3'\n";
            Temp += "Version:               v2.3\n";
            Temp += "UnsynchronisationUsed: " + UnsynchronisationUsed + "\n";
            Temp += "HasExtendedHeader:     " + HasExtendedHeader + "\n";
            Temp += "TagLength:             " + TagLength + "\n";
            Temp += "StartOfTag:            " + StartOfTag + "\n";
            return Temp;
        }
    }
}