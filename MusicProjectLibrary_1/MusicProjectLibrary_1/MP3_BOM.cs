//========================================================================
// Name:     MP3_BOM.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  ID3 v 2.3 encodes strings using 'byte order marks' or BOMs.
//           This class provides static methods for working with these
//           BOMs.
// Comments: 
//========================================================================
using System;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;



namespace JAudioTags
{
    /// <summary>
    /// The different type of text encodings used with MP3/ID3 v 2.3 tags
    /// </summary>
    internal enum EncodingType
    {
        iso8859,
        utf16le,
        utf16be,
        Unknown
    }


    /// <summary>
    /// Static class of static methods for working with the encodings
    /// used in ID3 v2.3 tags
    /// </summary>
    internal class BOM
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "BOM:                       1.10";


        // The three encodings used
        static public Encoding iso8859 = Encoding.GetEncoding("iso-8859-1");
        static public Encoding utf16le = Encoding.GetEncoding("UTF-16LE");
        static public Encoding utf16be = Encoding.GetEncoding("UTF-16BE");


        /// <summary>
        /// Decodes a byte array into a string assuming the array
        /// uses a flag byte and a Byte-Order-Mark
        /// </summary>
        /// <param name="TheBytes">The array of bytes representing the string</param>
        /// <returns>The decoded string</returns>
        static public string DecodeWithFlag(byte[] TheBytes)
        {
            switch (TheBytes[0])
            {
                case 0x00:
                    // Strip null terminators
                    return iso8859.GetString(TheBytes.Skip(1).ToArray()).Trim('\0');
                case 0x01:
                    return DecodeUTF16WithBOM(TheBytes.Skip(1).ToArray());
                default:
                    return "Invalid flag byte.  Unable to decode.";
            }
        }


        /// <summary>
        /// Decodes a byte array into a string assuming the array
        /// has no flag byte but does have a pair of BOM bytes
        /// </summary>
        /// <param name="TheBytes">The array of bytes representing the string</param>
        /// <returns>The decoded string</returns>
        static public string DecodeUTF16WithBOM(byte[] TheBytes)
        {
            if (TheBytes.Length <= 2)
                return "";

            byte[] BOM = TheBytes.Take(2).ToArray();
            // This is opposite to what I have read on-line
            // but this seems to work and other way around does not.
            if (BOM.SequenceEqual(new byte[] { 0xFF, 0xFE }))
                // Strip null terminators
                return utf16le.GetString(TheBytes.Skip(2).ToArray()).Trim('\0');
            else
            if (BOM.SequenceEqual(new byte[] { 0xFE, 0xFF }))
                // Strip null terminators
                return utf16be.GetString(TheBytes.Skip(2).ToArray()).Trim('\0');
            else
                return "Invalid BOM.  Unable to decode.";
        }


        /// <summary>
        /// Encodes a string as an array of bytes using the encoding
        /// type specified in the second argument.  Prepends a flag
        /// byte and BOM bytes as appropriate.
        /// </summary>
        /// <param name="TheString">The string to be encoded</param>
        /// <param name="Encoder">Which encoding to use</param>
        /// <returns>The array of bytes</returns>
        static public byte[] EncodeWithFlagAndBOM(string TheString, EncodingType Encoder)
        {
            switch (Encoder)
            {
                case EncodingType.iso8859:
                    return Helpers.JoinByteArrays(new List<byte[]>() { new byte[] { 0x00 }, iso8859.GetBytes(TheString) });
                case EncodingType.utf16le:
                    return Helpers.JoinByteArrays(new List<byte[]>() { new byte[] { 0x01, 0xFF, 0xFE }, utf16le.GetBytes(TheString) });
                case EncodingType.utf16be:
                    return Helpers.JoinByteArrays(new List<byte[]>() { new byte[] { 0x01, 0xFE, 0xFF }, utf16be.GetBytes(TheString) });
                default:
                    throw new InvalidEnumArgumentException("Internal error in BOM.EncodeWithFlagAndBOM().");
            }
        }


        /// <summary>
        /// Encodes a string as an array of bytes using the encoding
        /// type specified in the second argument.
        /// </summary>
        /// <param name="TheString">The string to be encoded</param>
        /// <param name="Encoder">Which encoding to use</param>
        /// <returns>The array of bytes</returns>
        static public byte[] EncodeNoFlagNoBOM(string TheString, EncodingType Encoder)
        {
            switch (Encoder)
            {
                case EncodingType.iso8859:
                    return iso8859.GetBytes(TheString);
                case EncodingType.utf16le:
                    return utf16le.GetBytes(TheString);
                case EncodingType.utf16be:
                    return utf16be.GetBytes(TheString);
                default:
                    throw new InvalidEnumArgumentException("Internal error in BOM.EncodeNoFlagNoBOM().");
            }
        }


        /// <summary>
        /// Enclodes a string using the encoding type specified.
        /// Prepends BOM bytes if appropriate, but no flag byte.
        /// </summary>
        /// <param name="TheString"></param>
        /// <param name="Encoder"></param>
        /// <returns>The array of bytes</returns>
        static public byte[] EncodeNoFlagWithBOM(string TheString, EncodingType Encoder)
        {
            switch (Encoder)
            {
                case EncodingType.iso8859:
                    return iso8859.GetBytes(TheString);
                case EncodingType.utf16le:
                    return Helpers.JoinByteArrays(new List<byte[]>() { new byte[] { 0xFF, 0xFE }, utf16le.GetBytes(TheString) });
                case EncodingType.utf16be:
                    return Helpers.JoinByteArrays(new List<byte[]>() { new byte[] { 0xFE, 0xFF }, utf16be.GetBytes(TheString) });
                default:
                    throw new InvalidEnumArgumentException("Internal error in BOM.EncodeNoFlagWithBOM().");
            }
        }
    }
}