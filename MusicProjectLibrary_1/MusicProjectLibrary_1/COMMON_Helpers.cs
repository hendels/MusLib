//========================================================================
// Name:     COMMON_Helpers.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  A class full of static 'utility' methods that do not fit 
//           anywhere else. 
// Comments: 
//========================================================================
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using MusicProjectLibrary_1;

namespace JAudioTags
{
    /// <summary>
    /// A bunch of utility methods used elsewhere in the code.
    /// </summary>
    static public class Helpers
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "Helpers:                   1.14";


        /// <summary>
        /// Displays "Press any key to continue..."
        /// and waits for a key press
        /// </summary>
        /// <returns>The character pressed</returns>
        public static char PressAnyKeyToContinue()
        {
            Console.Write("Press any key to continue... ");
            char c = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return c;
        } // PressAnyKeyToContinue


        /// <summary>
        /// Returns the value represented by the rightmost 'n' bits
        /// So ValueOfRightBits( 11110000, 5 ) would return 10000, or 16.
        /// </summary>
        /// <param name="Data">The byte conatining the bits</param>
        /// <param name="NumberOfBits">How many bits to look at</param>
        /// <returns>The value of the right most bits</returns>
        static public int ValueOfRightBits(byte Data, int NumberOfBits)
        {
            switch (NumberOfBits)
            {
                case 0: return 0;
                case 1: return Data & 1;
                case 2: return Data & 3;
                case 3: return Data & 7;
                case 4: return Data & 15;
                case 5: return Data & 31;
                case 6: return Data & 63;
                case 7: return Data & 127;
                case 8: return Data;
                default:
                    throw new Exception("Should never get here in Helpers.ValueOfRightBits().\n"
                        + "Data = " + Data + "\tNumberOfBits = " + NumberOfBits);
            }
        }


        /// <summary>
        /// Take an array of bytes representing an unsigned int with BigEndian encoding
        /// and return the unisgned int.
        /// </summary>
        /// <param name="input">An array of bytes</param>
        /// <returns>The bytes as a uint</returns>
        static public uint ConvertFromBigEndian(byte[] input)
        {
            // Most numbers are stored in a FLAC file as BigEndian unsigned integers.  
            // A Windows PC is LittleEndian.
            // The BitConverter.ToUInt32 method expects a 4 byte argument as its first parameter.
            //
            // This gives us three tasks
            // 1.  Left pad the input array up to 4 bytes in length if it is shorter than this
            // 2.  Reverse the order of the bytes
            // 3.  Convert the 4 byte array to a Uint

            // Check not null
            if (input == null)
                throw new ArgumentException("Null array passed to Helpers.ConvertFromBigEndian().");
            // If the length OK?
            int len = input.Length;
            if (len > 4)
                throw new ArgumentOutOfRangeException(
                    "Too big ByteArray passed to Helpers.ConvertFromBigEndian().  Len = " + len.ToString());

            // Arrays are passed by reference, so make a local copy as we do not want to alter
            // the original

            // Create a new local empty array
            byte[] local = new byte[4];

            // Populate it from the input, and left pad with zeros
            switch (len)
            {
                case 1:
                    {
                        local[0] = local[1] = local[2] = 0x00;
                        local[3] = input[0];
                        break;
                    }
                case 2:
                    {
                        local[0] = local[1] = 0x00;
                        local[2] = input[0];
                        local[3] = input[1];
                        break;
                    }
                case 3:
                    {
                        local[0] = 0x00;
                        local[1] = input[0];
                        local[2] = input[1];
                        local[3] = input[2];
                        break;
                    }
                case 4:
                    {
                        local[0] = input[0];
                        local[1] = input[1];
                        local[2] = input[2];
                        local[3] = input[3];
                        break;
                    }
            }
            // Now reverse it
            Array.Reverse(local);

            // No convert to a Uint
            return BitConverter.ToUInt32(local, 0);
        }


        /// <summary>
        /// Reports whether a bit is set in a byte
        /// </summary>
        /// <param name="TheByte">The byte</param>
        /// <param name="Pos">The bit position</param>
        /// <returns>Is the bit set?</returns>
        static public bool IsBitSet(byte TheByte, int Pos)
        {

            switch (Pos)
            {
                case 0: return (TheByte & 1) == 1;
                case 1: return (TheByte & 2) == 2;
                case 2: return (TheByte & 4) == 4;
                case 3: return (TheByte & 8) == 8;
                case 4: return (TheByte & 16) == 16;
                case 5: return (TheByte & 32) == 32;
                case 6: return (TheByte & 64) == 64;
                case 7: return (TheByte & 128) == 128;
                default:
                    throw new ArgumentException("Inappropriate bit position passed to Helpers.IsBitSet().\n"
                        + "TheByte = " + TheByte + "\tPos = " + Pos);
            }
        }


        /// <summary>
        /// Takes a standard Windows little-endian 4-byte uint
        /// and returns a 3-byte big-endian representation.
        /// Used with FLAC metadata block header.
        /// No error checking done to check it will fit.
        /// </summary>
        /// <param name="ui">The unsigned int to be converted</param>
        /// <returns>Three bytes reprsenting the uInt in Big Endian</returns>
        static public byte[] UIntToThreeByteBigEndian(uint ui)
        {
            byte[] temp = BitConverter.GetBytes(ui);
            return new byte[] { temp[2], temp[1], temp[0] };
        }


        /// <summary>
        /// Takes a standard Windows little-endian 4-byte int
        /// and returns a 4-byte big-endian representation.
        /// </summary>
        /// <param name="i">The int to be converted</param>
        /// <returns>Four bytes reprsenting the int in Big Endian</returns>
        static public byte[] IntToFourByteBigEndian(int i)
        {
            byte[] temp = BitConverter.GetBytes(i);
            return new byte[] { temp[3], temp[2], temp[1], temp[0] };
        }


        /// <summary>
        /// Tries to convert a long to an int.
        /// Throws an exception if this is not possible
        /// </summary>
        /// <param name="x">The long to be converted</param>
        /// <param name="Msg">Optional message to display</param>
        /// <returns>The long converted to an int</returns>
        static public int LongToInt(long x, string Msg = "")
        {
            if (x <= int.MaxValue)
                return (int)x;
            else
                throw new ArgumentOutOfRangeException(
                    "Long value too big to be converted to an int in Helpers.LongToInt().\n" +
                    "x = " + x + "\n" + Msg);
        }


        /// <summary>
        /// Splits a full file path into filename and folder.
        /// </summary>
        /// <param name="AudioPath"></param>
        /// <returns>The full file path</returns>
        static public string SplitPath(string AudioPath)
        {
            return "File:   " + Path.GetFileName(AudioPath)
            + "\nFolder: " + Path.GetDirectoryName(AudioPath);
        }


        /// <summary>
        /// Checks if all bytes in an array have the same specified value.
        /// </summary>
        /// <param name="TheBytes">The byte array to be searched</param>
        /// <param name="TargetValue">The value they must all have</param>
        /// <returns></returns>
        static public bool BytesAreAll(byte[] TheBytes, byte TargetValue)
        {
            if (TheBytes == null)
                throw new ArgumentNullException("Null array passed to Helpers.BytesAreAll.");

            foreach (var Byte in TheBytes)
                if (Byte != TargetValue)
                    return false;
            return true;
        }


        /// <summary>
        /// Overwrites part of one byte array with bytes encoded from a string.
		/// Uses ASCII encoding.
        /// </summary>
        /// <param name="Base">The array part of which will be overwritten.</param>
        /// <param name="Overlay">A string that will be converted to ASCII bytes and 
        /// then overwritten onto the base array.</param>
        /// <param name="Position">Position within the array where overwriting will start.</param>
        /// <param name="FieldLength">The maximum length of the field.  If overlay is longer
        /// than this it is truncated.</param>
        public static void OverwriteASCIIStringIntoArray(byte[] Base, string Overlay, int Position, int FieldLength)
        {
            if (Base == null)
                throw new ArgumentNullException("Null 'Base' array passed into Helpers.OverwriteASCIIStringIntoArray().");
            if (Position > Base.Length)
                throw new ArgumentOutOfRangeException("Position > Base.Lenth in Helpers.OverwriteASCIIStringIntoArray()."
                    + "Base.Length = " + Base.Length + "\tPosition = " + Position);

            byte[] Temp = Encoding.ASCII.GetBytes(Overlay);
            if (Temp.Length > FieldLength)
                Temp = Temp.Take(FieldLength).ToArray();
            Buffer.BlockCopy(Temp, 0, Base, Position, Temp.Length);
        }


        /// <summary>
        /// Display a prompt on the console then read and return a string.  
        /// Ignore empty strings
        /// </summary>
        /// <param name="prompt">A text promt</param>
        /// <returns>The string read.</returns>
        public static string readString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
            } while (result == "");
            return result;
        } // readString


        /// <summary>
        /// Read and return an int in a specified range
        /// </summary>
        /// <param name="prompt">A text promt</param>
        /// <param name="low">Lowest acceptable return value</param>
        /// <param name="high">Highest acceptable return value</param>
        /// <returns>The integer read from the keyboard</returns>
        public static int readInt(string prompt, int low, int high)
        {
            int result;
            string intString;

            do
            {
                do
                {
                    intString = readString(prompt);
                } while (!int.TryParse(intString, out result));
            } while ((result < low) || (result > high));
            return result;
        } // readInt


        /// <summary>
        /// Debugging method to print an array of bytes.
        /// </summary>
        /// <param name="TheBytes">The array of bytes to be printed</param>
        /// <param name="Vertical">Vertical (true) or horizontal (false)</param>
        static public void Print(byte[] TheBytes, bool Vertical)
        {
            int i = 0;
            foreach (var Byte in TheBytes)
                if (Vertical)
                    Console.WriteLine(String.Format("{0,2}  {1,3} : 0x{1:X02} : {2}", i++, Byte, (char)Byte));
                else
                    Console.Write((char)Byte + ":");
            Console.WriteLine();

        }


        /// <summary>
        /// Joins an arbitrary number of byte arrays.
        /// </summary>
        /// <param name="TheArrays">A List of byte arrays</param>
        /// <returns>New array made from argument arrays</returns>
        static public byte[] JoinByteArrays(List<byte[]> TheArrays)
        {
            int TotalLength = 0;

            for (int i = 0; i < TheArrays.Count; i++)
                if (TheArrays[i] == null)
                    throw new ArgumentNullException("Array number " + i + " is null in Helpers.JoinByteArrays().");
                else
                    TotalLength += TheArrays[i].Length;

            byte[] Temp = new byte[TotalLength];

            int DestOffSet = 0;
            foreach (byte[] Array in TheArrays)
            {
                Buffer.BlockCopy(Array, 0, Temp, DestOffSet, Array.Length);
                DestOffSet += Array.Length;
            }

            return Temp;
        }


        /// <summary>
        /// Open NotePad++ and view a text file
        /// </summary>
        /// <param name="TextFilePath">The file to view</param>
        static public void ViewTextFile(string TextFilePath)
        {
            string NotePadPP = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
            string NotePad = @"C:\Windows\system32\notepad.exe";
            string ViewerPath;

            if (File.Exists(TextFilePath))
            {
                if (File.Exists(NotePadPP))
                    ViewerPath = NotePadPP;
                else
                    ViewerPath = NotePad;
                Process Program = new System.Diagnostics.Process();
                Program.StartInfo.FileName = ViewerPath;
                Program.StartInfo.Arguments = TextFilePath;
                Program.Start();
            }
        }


        /// <summary>
        /// Pass it a file path and it returns the extension
        /// with the leading period removed and converted to upper case.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        static public string JGetExtension(string FileName)
        {
            string Extension = Path.GetExtension(FileName);
            if (Extension.Length > 0)
                Extension = Extension.Substring(1, Extension.Length - 1).ToUpper();
            return Extension;
        }


        /// <summary>
        /// Make a local backup copy of a file
        /// </summary>
        /// <param name="CurrentFile">The current full path and name of the file</param>
        /// <param name="BackupFileExtension">The file extension to give the backup.</param>
        static public void MakeBackup(string CurrentFile, string BackupFileExtension = "bak")
        {
            if (GlobalVariables.globalCreateBackup)
            {
                if (BackupFileExtension[0] != '.')
                    BackupFileExtension = "." + BackupFileExtension;

                string BackupPath = Path.GetDirectoryName(CurrentFile) + "\\" +
                    Path.GetFileNameWithoutExtension(CurrentFile) + BackupFileExtension;

                if (File.Exists(BackupPath))
                    File.Delete(BackupPath);

                File.Copy(CurrentFile, BackupPath);
            }

        }


        /// <summary>
        /// Restore a file from a local backup copy
        /// </summary>
        /// <param name="OriginalNameOfBackedUpFile">The name of the file that was backed up</param>
        /// <param name="BackupFileExtension">The file extension used for the backup</param>
        static public void RestoreFromBackup(string OriginalNameOfBackedUpFile,
                                            string BackupFileExtension = "bak")
        {
            if (BackupFileExtension[0] != '.')
                BackupFileExtension = "." + BackupFileExtension;

            string BackupPath = Path.GetDirectoryName(OriginalNameOfBackedUpFile) + "\\" +
                Path.GetFileNameWithoutExtension(OriginalNameOfBackedUpFile) + BackupFileExtension;

            if (File.Exists(BackupPath))
            {
                File.Delete(OriginalNameOfBackedUpFile);
                File.Copy(BackupPath, OriginalNameOfBackedUpFile);
                File.Delete(BackupPath);
            }
        }


        /// <summary>
        /// Compares a a byte array and a string
        /// by converting the string into a byte array.
        /// </summary>
        /// <param name="A">The byte array</param>
        /// <param name="S">The string</param>
        /// <returns>True if they are equal</returns>
        public static bool Equal(byte[] A, string S)
            => A.SequenceEqual(Encoding.ASCII.GetBytes(S));


        /// <summary>
        /// Pass it a relative path and the path to which that 
        /// path is relative, it returns a corresponding absolute path.
        /// </summary>
        /// <param name="RelativePath">The relative path</param>
        /// <param name="BasePath">The base path - the path where the relative
        /// path starts from.</param>
        /// <param name="AbsolutePath">An OUT Param.  The absolute path</param>
        /// <returns>True if the two input paths are compatible so we can
        /// calculate an absolute path.  False if they are not/</returns>
        public static bool AbsolutePath(string RelativePath, string BasePath, out string AbsolutePath)
        {
            // Checkargs exist
            if (RelativePath == null || RelativePath == "")
                throw new ArgumentException("Relative Path missing in Helpers.AbsolutePath()");
            if (BasePath == null || BasePath == "")
                throw new ArgumentException("Current Path missing in Helpers.AbsolutePath()");

            // Make any Unix paths into Windows paths
            RelativePath = RelativePath.Replace("/", "\\");
            
            // Check that paths are compatible

            // How many directories above current path
            int CurrentDepth = BasePath.Where(c => c == '\\').Count();

            // How many up steps in relative path
            int RelativeDepth = 0;
            string Temp = RelativePath;
            while (Temp.Substring(0, 3) == "..\\")
            {
                Temp = Temp.Substring(3);
                RelativeDepth++;
            }
            
            // It they are incompatible, return false;
            if (RelativeDepth > CurrentDepth)
            {
                AbsolutePath = null;
                return false;
            }

            // Otherwise, merge the paths
            for (int i = 0; i < RelativeDepth; i++)
            {
                RelativePath = RelativePath.Substring(3);
                BasePath = BasePath.Substring(0, BasePath.LastIndexOf('\\'));
            }
            AbsolutePath = BasePath + "\\" + RelativePath;
            return true;
        }


        /// <summary>
        /// Splits a string into parts.
        /// </summary>
        /// <param name="Target">The string that gets split</param>
        /// <param name="SplitChars">Characters used as split points</param>
        /// <param name="Trim">If true, sub-strings are trimmed of leading and trailing spaces</param>
        /// <returns>A lsit of substrings</returns>
        static public List<string> Split2(string Target, string SplitChars, bool Trim = false)
        {
            if (Target == null || SplitChars == null)
                throw new ArgumentNullException("Null argument in Helpers.Split()");

            if (Target == "" || SplitChars == "")
                return new List<string>() { Target };

            List<string> Temp = Target.Split(SplitChars.ToCharArray()).ToList();

            if (Trim)
                for (int i = 0; i < Temp.Count; i++)
                    Temp[i] = Temp[i].Trim();

            return Temp;
        }
    }


    /// <summary>
    /// An extension method to the in-built string class
    /// to implement a ToUpper() method that does nothing
    /// if CaseSensitive is true.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns an upper case version of its argument
        /// if the varibale AudioFile.IsCaseSensitive is true.
        /// If not, the argument is returend unchanged.
        /// </summary>
        /// <param name="Input">The string to be changed to upper case</param>
        /// <returns>The argument converted to upper case.</returns>
        public static string JToUpper(this string Input)
        {
            if (AudioFile.IsCaseSensitive)
                return Input;
            else
                return Input.ToUpper();
        }
    }
}