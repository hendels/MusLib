//========================================================================
// Name:     MP3_V1Tags.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Provides a couple of methods for working with leagacy v1 MP3
//           tags.
// Comments: 
//========================================================================
using System;
using System.Text;


namespace JAudioTags
{
    /// <summary>
    /// Static class with just two static methods.
    /// Used to work with MP3 ID3 v1 Tag block
    /// </summary>
    static internal class V1Tags
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "V1Tags:                    1.01";


        // Postions and lengths of fields within tag block.
        const int Headerlength = 3;
        const int TitleOffset = 3;
        const int TitleMaxLength = 30;
        const int ArtistOffset = 33;
        const int ArtistMaxLength = 30;
        const int AlbumOffset = 63;
        const int AlbumMaxLength = 30;
        const int YearOffset = 93;
        const int YearMaxLength = 4;
        const int CommentOffset = 97;
        const int CommentMaxLength = 30;
        const int TrackOffset = 126;
        const int TrackFlagOffsetWithinComment = 28;
        const int TrackOffsetWithinComment = 29;
        const int TrackMaxLength = 1;
        const int GenreOffset = 127;
        const int GenreMaxLength = 1;


        /// <summary>
        /// Feed it the 128-byte v1 tag block from the end of a MP3 file
        /// and it returns a populated tag list.
        /// </summary>
        /// <param name="TheArray">A 128 byte array</param>
        /// <returns>A new TagList containing the ID3 V1 tags</returns>
        public static TagList ByteArrayToV1TagList(byte[] TheArray)
        {
            // Check the array
            if (TheArray == null)
                throw new ArgumentNullException("Null array passed in to 'ByteArrayToV1TagList'.");
            if (TheArray.Length != 128)
                throw new ArgumentException("Array passed in to 'ByteArrayToV1TagList' is " +
                    "not 128 bytes long.");

            // Create a new, empty tag list
            TagList TempList = new TagList(AudioFileTypes.mp3);

            // Wrap a byte source around the array, for ease or reading.
            ByteSource BS = new ByteSource(TheArray);

            // Skip over the first three bytes - they say 'TAG'.
            BS.GetBytes(Headerlength);

            // Now read the tag data
            TempList.AddTag(new TagType("TIT2", Encoding.ASCII.GetString(BS.GetBytes(TitleMaxLength))));
            TempList.AddTag(new TagType("TPE1", Encoding.ASCII.GetString(BS.GetBytes(ArtistMaxLength))));
            TempList.AddTag(new TagType("TALB", Encoding.ASCII.GetString(BS.GetBytes(AlbumMaxLength))));
            TempList.AddTag(new TagType("TYER", Encoding.ASCII.GetString(BS.GetBytes(YearMaxLength))));
            byte[] RawComment = BS.GetBytes(CommentMaxLength);
            TempList.AddTag(new TagType("COMM", Encoding.ASCII.GetString((RawComment))));
            // The penultimate byte (28) of the 30 byte comment is a zero
            // then byte 29 is a track number.
            if (RawComment[TrackFlagOffsetWithinComment] == 0)
                TempList.AddTag(new TagType("TRCK", (RawComment[TrackOffsetWithinComment]).ToString()));
            else
                TempList.AddTag(new TagType("TRCK", "0"));

            TempList.AddTag(new TagType("TCON", TagMappings.Genre(BS.GetBytes(GenreMaxLength)[0])));

            return TempList;
        }


        /// <summary>
        /// Pass it a tag list and it returns a 128 byte array containing those tags.
        /// </summary>
        /// <param name="TheList">A list of tags</param>
        /// <returns>A 128-byte array</returns>
        public static byte[] V1TagListToByteArray(TagList TheList)
        {
            if (TheList == null)
                throw new ArgumentNullException("Null TagList passed in to 'V1TagListToByteArray'.");

            // Create a 128 byte null array
            byte[] Temp = new byte[128];

            // Set the first three bytes to 'TAG'
            Temp[0] = (byte)'T'; Temp[1] = (byte)'A'; Temp[2] = (byte)'G';

            Helpers.OverwriteASCIIStringIntoArray(Temp, TheList.First("TIT2"), TitleOffset, TitleMaxLength);
            Helpers.OverwriteASCIIStringIntoArray(Temp, TheList.First("TPE1"), ArtistOffset, ArtistMaxLength);
            Helpers.OverwriteASCIIStringIntoArray(Temp, TheList.First("TALB"), AlbumOffset, AlbumMaxLength);
            Helpers.OverwriteASCIIStringIntoArray(Temp, TheList.First("TYER"), YearOffset, YearMaxLength);
            Helpers.OverwriteASCIIStringIntoArray(Temp, TheList.First("COMM"), CommentOffset, CommentMaxLength);

            int TempTrack;
            byte Track = 0;
            if (TheList.First("TRCK") != "")
            {
                if (int.TryParse(TheList.First("TRCK"), out TempTrack))
                    if (TempTrack >= 0 && TempTrack <= 255)
                        Track = Convert.ToByte(TempTrack);
            }
            Buffer.BlockCopy(new byte[] { Track }, 0, Temp, TrackOffset, TrackMaxLength);

            byte Genre = TheList.First("TCON").ToV1GenreCode();
            Buffer.BlockCopy(new byte[] { Genre }, 0, Temp, GenreOffset, GenreMaxLength);

            return Temp;
        }
    }
}