//========================================================================
// Name:     MP3_CommTags.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  ID3 v 2.3 COMM frames are structured differently from other 
//           frames.  This class provides methods for working with those 
//           frames.
// Comments: 
//========================================================================
using System;
using System.Linq;


namespace JAudioTags
{
    /// <summary>
    /// Class of static methods for processing ID3 v2.3 COMM tags.
    /// Much of this involves locating nulls within byte arrays.
    /// </summary>
    static internal class CommTags
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "CommTags:                  1.01";


        /// <summary>
        /// Searches for a byte array inside another.
        /// If found, it returns the starting position.
        /// Otherwise it returns -1.
        /// </summary>
        /// <param name="TheArray">The array to be searched.</param>
        /// <param name="Target">The array to be searched for</param>
        /// <returns>The position if the embedded array</returns>
        static private int IndexOf(byte[] TheArray, byte[] Target)
        {
            if (TheArray == null)
                throw new ArgumentNullException("First argument null in CommTags.IndexOf().");
            if (Target==null)
                throw new ArgumentNullException("Second argument null in CommTags.IndexOf().");
            bool Found;

            if (Target.Length > TheArray.Length)
                return -1;
            else
            {
                for (int i = 0; i <= TheArray.Length - Target.Length; i++)
                {
                    Found = true;
                    for (int j = 0; j < Target.Length; j++)
                    {
                        if (TheArray[i + j] != Target[j])
                            Found = false;
                        if (Found) return i;
                    }
                }
                return -1;
            }
        }


        /// <summary>
        /// Searches the input array for two consecutive null bytes.
        /// If found, returns all the bytes after the nulls.
        /// If not, returns an empty byte array.
        /// </summary>
        /// <param name="Input">The source array</param>
        /// <returns>The bytes after the nulls</returns>
        static public byte[] AfterTwoNulls(byte[] Input)
        {
            int Pos = IndexOf(Input, new byte[] { (byte)0x00, (byte)0x00 });
            if (Pos == -1)
                return new byte[] { };
            else
                return Input.Skip(Pos + 2).Take(Input.Length - Pos - 2).ToArray();
        }


        ///// <summary>
        ///// Prints out a byte array, interpreting each byte as a char.
        ///// FOR DEBUGGING USE.
        ///// </summary>
        ///// <param name="Input"></param>
        //static public string AsString(byte[] Input)
        //{
        //    string Temp = "";
        //    for (int i = 0; i < Input.Length; i++)
        //        // Temp += (char)Input[i];
        //        Temp += String.Format("0x{0:X02} ", Input[i]);
        //    return Temp;
        //}
    }
}