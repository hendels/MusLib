//========================================================================
// Name:     MP3_SyncSafe.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  MP3/ID3 v 2.3 encodes certain value and data blocks to
//           prevent too many consective 1s appearing when the file is
//           viewed as binary.  Apparently this used to cause
//           'synchronisation' read errors with early renderers.  This
//           class implements a set of methods for encoding and decoding
//           data so as to make them 'Sync' 'Safe'.
// Comments:
//========================================================================
using System;
using System.Linq;


namespace JAudioTags
{
    /// <summary>
    /// Implements unsynchronization and deunsynchronization 
    /// methods for use in ID3 tags.
    /// </summary>
    internal class SyncSafe
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "SyncSafe:                  1.01";


        /// <summary>
        /// Takes an array of bytes and 'unsynchronises' it.
        /// Additional zero bytes are added whenever a run
        /// of more than eleven consecutive 1s are read.
        /// This prevents tag data incorrectly being interpreted 
        /// as synchronisation bits indicating the start of the 
        /// music data.
        /// 
        /// USED TO MAKE A TEST FILE TO CHECK THAT
        /// DEUNSYNCHRONISATION CODE WORKS.
        /// 
        /// </summary>
        /// <param name="OldTag">The original, unmodified tag data.</param>
        /// <returns>The data with additional zero btes inserted
        /// to break long strings of 1s.</returns>
        static public byte[] Unsynchronize(byte[] OldTag)
        {
            if (OldTag == null)
                throw new ArgumentNullException("Null argument passed to SyncSafe.Unsynchronize().");
            byte[] NewTag = new byte[OldTag.Length * 2];
            int i = 0;
            int j = 0;
            byte Mask = 128 + 64 + 32;

            for (i = 0; i < OldTag.Length - 1; i++)
            {
                NewTag[j++] = OldTag[i];
                // Replace all '11111111 111xxxxx' sequences with '11111111 00000000 111xxxxx'
                if (OldTag[i] == 0xff && ((OldTag[i + 1] & Mask) == Mask))
                    NewTag[j++] = 0x00;
                // Replace any "0xff,0x00" with "0xff,0x00,0x00"
                if (OldTag[i] == 0xff && OldTag[i + 1] == 0x00)
                    NewTag[j++] = 0x00;
            }

            // If last byte is 0xff append 0x00
            if (OldTag[i] == 0xff)
            {
                NewTag[j++] = 0xff;
                NewTag[j++] = 0x00;
            }
            else
                NewTag[j++] = OldTag[i];

            return NewTag.Take(j).ToArray();
        }


        /// <summary>
        /// Undoes the transformation implemented by the
        /// UnSynchronize() method above.  Removes all the 
        /// inserted zeros rendering the data parseable again.
        /// </summary>
        /// <param name="OldTag">The data with embedded Unsynchronization bytes.</param>
        /// <returns>The data with unsynchronisation bits removed.</returns>
        static public byte[] DeUnscynchronize(byte[] OldTag)
        {
            if (OldTag == null)
                throw new ArgumentNullException("Null argument passed to SyncSafe.DeUnsynchronize().");
            byte[] NewTag = new byte[OldTag.Length];
            int j = 0;

            NewTag[j++] = OldTag[0];

            for (int i = 1; i < OldTag.Length; i++)
                if (OldTag[i - 1] == 0xFF && OldTag[i] == 0x00)
                { } // Do nothing
                else
                    NewTag[j++] = OldTag[i];

            return NewTag.Take(j).ToArray();
        }


        /// <summary>
        /// Generates test arrays to debug Unsynchronize and
        /// DeUnsynchronize.
        /// Use for DEBUGGING ONLY.
        /// </summary>
        /// <param name="Size">How big should the test array be</param>
        /// <returns>The test array</returns>
        static public byte[] Generate(int Size)
        {
            byte[] Temp = new byte[Size];
            Random r = new Random();

            for (int i = 0; i < Size; i++)
                Temp[i] = (byte)r.Next(0, 256);

            for (int i = 0; i < Size / 10; i++)
            {
                Temp[r.Next(0, 256)] = 0xff;
                Temp[r.Next(0, 256)] = 128 + 64 + 32;
                Temp[r.Next(0, 256)] = 0x00;
            }

            return Temp;
        }


        /// <summary>
        /// Converts a normal 32-bit integer to a SyncSafe integer.
        /// SyncSafe integers have the most significant bit of each
        /// byte set to zero.  This leaves only 28 (4 x 7) bits
        /// available to represent the numeric value.
        /// This prevents integers in tags being misintereted as
        /// synchronization bits indicating the start of the actual
        /// music.
        /// </summary>
        /// <param name="i">The 'normal' integer</param>
        /// <returns>The corresponding SyncSafe integer</returns>
        static public byte[] ToBigEndianSyncSafe(int i)
        {
            // Check this int is suitable for conversion - 
            // Left most four bits must be zero.
            if (i < 0)
                throw new ArgumentException("Negative integer passed to ToSyncSafe.ToBigEndianSyncSafe().  i = " + i);
            if (i > 0x0FFFFFFF)
                throw new ArgumentException("Value passed to ToSyncSafe.ToBigEndianSyncSafe() is too big for conversion to "
                    + "a SyncSafe integer.  i = " + i);

            // Now build a SyncSafe int
            int i0, i1, i2, i3, result;

            //  7654 3210  7654 3210  7654 3210  7654 3210
            //  .... abcd  efgh ijkl  mnop qrst  uvwx yz12

            i0 = (i & 0x0FE00000) << 3;
            //  0abc defg  0000 0000  0000 0000  0000 0000

            i1 = (i & 0x001FC000) << 2;
            //  0000 0000  0hij klmn  0000 0000  0000 0000

            i2 = (i & 0x00003F80) << 1;
            //  0000 0000  0000 0000  0opq rstu  0000 0000

            i3 = i & 0x0000007F;
            //  0000 0000  0000 0000  0000 0000  0vwx yz12

            result = i0 | i1 | i2 | i3;

            // Convert to bytes
            byte[] LittleEndian = BitConverter.GetBytes(result);

            // Return it with the bytes re-ordered
            return new byte[] { LittleEndian[3], LittleEndian[2], LittleEndian[1], LittleEndian[0] };
        }


        /// <summary>
        /// Converts a SyncSafe integer to a normal integer.
        /// </summary>
        /// <param name="TheBytes">The SyncSafe integer.</param>
        /// <returns>The corresponding 'normal' integer.</returns>
        static public int FromSyncSafe(byte[] TheBytes)
        {
            if (TheBytes == null)
                throw new ArgumentNullException("null argument passed to Helpers.FromSyncSafe()");
            if (TheBytes.Length != 4)
                throw new ArgumentException("ScynSafe array passed to Helpers.FromSyncSafe() has incrorrect size: " + TheBytes.Length);
            if (Helpers.IsBitSet(TheBytes[0], 7) ||
                Helpers.IsBitSet(TheBytes[1], 7) ||
                Helpers.IsBitSet(TheBytes[2], 7) ||
                Helpers.IsBitSet(TheBytes[3], 7))
                throw new ArgumentException("Value is not a proper SyncSafe value in Helpers.FromSyncSafe().");

            // Create a 32 bit uint from each byte.
            //
            // An mp3 file stores sizes in bigendian format.  
            // i.e. the first byte read (byte[0] is the most significant
            // and the last (byte[3]) is the least significant.
            int Int0 = TheBytes[0];
            int Int1 = TheBytes[1];
            int Int2 = TheBytes[2];
            int Int3 = TheBytes[3];

            // Shuffle bits to the left.
            //          76543210 76543210 76543210 76543210
            // Int0    ...0xxxx xxx..... ........ ........ 21 bits to the left.
            // Int1    ........ ..0xxxxx xx...... ........ 14 bits to the left.
            // Int2    ........ ........ .0xxxxxx x.......  7 bits to left.
            // Int3    ........ ........ ........ 0xxxxxxx

            // Each zero bit from left of Int now moved to left of 32 bit int.
            return (int)(Int0 << 21 | Int1 << 14 | Int2 << 7 | Int3);
        }


        ///// <summary>
        ///// Returns a string representing a 32-bit integer displayed in binary.
        ///// For debugging purposes only.
        ///// </summary>
        ///// <param name="Input">The integer to be displayed</param>
        ///// <returns>The int as a string</returns>
        //static public string Show(int Input)
        //{
        //    string Temp = "";

        //    byte[] TheBytes = BitConverter.GetBytes(Input);

        //    // Windows is little-endian so byte[0] is
        //    // the least significant.  
        //    // So show byte[3] first and byte[0] last.
        //    for (int i = 3; i > -1; i--)
        //    {
        //        for (int j = 7; j > -1; j--)
        //        {
        //            if (Helpers.IsBitSet(TheBytes[i], j))
        //                Temp += "1 ";
        //            else
        //                Temp += "0 ";
        //        }
        //        if (i != 0)
        //            Temp += "  ";
        //    }
        //    return Temp;
        //}


        ///// <summary>
        ///// Returns a string representing a 32-bit integer displayed in binary.
        ///// The integer is passed in as an array of bytes.
        ///// For debugging purposes only.
        ///// </summary>
        ///// <param name="TheBytes"></param>
        ///// <returns>String representing the bytes</returns>
        //static public string Show(byte[] TheBytes)
        //{
        //    string Temp = "";

        //    for (int i = 3; i > -1; i--)
        //    {
        //        for (int j = 7; j > -1; j--)
        //        {
        //            if (Helpers.IsBitSet(TheBytes[i], j))
        //                Temp += "1 ";
        //            else
        //                Temp += "0 ";
        //        }
        //        if (i != 0)
        //            Temp += "  ";
        //    }
        //    return Temp;
        //}
    }
}