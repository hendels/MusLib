//========================================================================
// Name:     ByteSource.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Puts a wrapper around a BinaryReader.  
//           Allows Binary readers built on files and on byte arrays to 
//           used interchangeably.
// Comments: I am unsure if this class is needed.  I could probably work
//           directly with the underlying BinaryReader class.
//========================================================================
using System;
using System.IO;


namespace JAudioTags
{
    /// <summary>
    /// Class: ByteSource
    /// Wraps a BinaryReader so that it can get bytes from a file 
    /// or from an in-memory byte array.
    /// We may then access either using the same kind of object.
    /// </summary>
    internal class ByteSource : IDisposable
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "ByteSource:                1.01";


        /// <summary>
        /// Refers to the underlying stream
        /// </summary>
        private FileStream TheFile;


        /// <summary>
        /// The wrapped reader
        /// </summary>
        private BinaryReader TheReader;


        /// <summary>
        /// Is it built on a file or byte array? Used in disposing.
        /// </summary>
        private bool IsFileBased;              
        

        /// <summary>
        /// Then total number of bytes in the file stream.
        /// </summary>
        public long Length { get; private set; }               


        /// <summary>
        /// Current seek position within stream
        /// </summary>
        public long CurrentSeekPosition { get; private set; }


        /// <summary>
        /// How many bytes remaining after the current seek position
        /// </summary>
        public long BytesRemaining                              
        {
            get
            {
                return Length - CurrentSeekPosition;
            }
        }


        /// <summary>
        /// Are we at the end of the file yet?
        /// </summary>
        public bool EOF                       
        {
            get
            {
                return CurrentSeekPosition >= Length;
            }
        }


        /// <summary>
        /// Constructor for file based source
        /// </summary>
        /// <param name="Path">Path to the file to be read</param>
        public ByteSource(string Path)
        {
            try
            {
                TheFile = File.Open(Path, FileMode.Open, FileAccess.Read);
                TheReader = new BinaryReader(TheFile);
                CurrentSeekPosition = 0;
                Length = TheReader.BaseStream.Length;
                IsFileBased = true;
            }
            catch (Exception Ex)
            {
                throw new IOException("Error in disc ByteSource constructor trying to connect to " 
                    + Path + "\n" + Ex.Message);
            }
        }


        /// <summary>
        /// Constructor for in-memory byte array based source
        /// </summary>
        /// <param name="TheBytes">A pre-populated array of bytes</param>
        public ByteSource(byte[] TheBytes)
        {
            try
            {
                TheReader = new BinaryReader(new MemoryStream(TheBytes));
                CurrentSeekPosition = 0;
                Length = TheReader.BaseStream.Length;
                IsFileBased = false;
            }
            catch (Exception Ex)
            {
                throw new IOException("Error in memory block memory stream constructor.\n"
                    + Ex.Message);
            }
        }


        /// <summary>
        /// Advance seek position to NewSeekPosition in stream
        /// </summary>
        /// <param name="NewSeekPosition">Position in file</param>
        public void MoveTo(long NewSeekPosition)
        {
            if (NewSeekPosition < 0 || NewSeekPosition > Length)
                throw new EndOfStreamException("Trying to set new seek position outside of file in ByteSource.MoveTo().");
            TheReader.BaseStream.Seek(NewSeekPosition, SeekOrigin.Begin);
            CurrentSeekPosition = NewSeekPosition;
        }


        /// <summary>
        /// Nove the Seek point to a new position
        /// </summary>
        /// <param name="Offset">The new position (Relative to offset argument)</param>
        /// <param name="Origin">Origin against which offset is used.</param>
        public void MoveTo(long Offset, SeekOrigin Origin)
        {
            switch (Origin)
            {
                case SeekOrigin.Begin:
                    if (Offset < 0 || Offset > Length)
                        throw new EndOfStreamException("Trying to set new seek position outside of file in ByteSource.MoveTo().");
                    break;
                case SeekOrigin.Current:
                    if (Offset < 0 && CurrentSeekPosition + Offset < 0)
                        throw new EndOfStreamException("Trying to read before beginning of file in ByteSource.MoveTo");
                    if (Offset >= 0 && CurrentSeekPosition + Offset > Length)
                        throw new EndOfStreamException("Trying to set new seek position outside of file in ByteSource.MoveTo().");
                    break;
                case SeekOrigin.End:
                    if (Offset > 0 || Offset > Length)
                        throw new EndOfStreamException("Trying to read before beginning of file in ByteSource.MoveTo()");
                    break;
                default:
                    break;
            }
            TheReader.BaseStream.Seek(Offset, Origin);
            CurrentSeekPosition = TheReader.BaseStream.Position;
        }


        /// <summary>
        /// Read and return bytes from the source
        /// </summary>
        /// <param name="HowMany">How many byte to return</param>
        /// <param name="Message">Optional message to be passed to LongToInt()</param>
        /// <returns>The Bytes returned</returns>
        public byte[] GetBytes(long HowMany, string Message)
        {
            if (CurrentSeekPosition + HowMany > Length)
                throw new EndOfStreamException(
                    "Tried to read beyond end of stream in ByteSource.GetBytes().");
            else
            {
                int IntHowMany = Helpers.LongToInt(HowMany, Message);
                CurrentSeekPosition += HowMany;
                return TheReader.ReadBytes(IntHowMany);
            }
        }


        /// <summary>
        /// Read and return bytes from the source
        /// </summary>
        /// <param name="HowMany">How many byte to return</param>
        /// <returns>The bytes requested</returns>
        public byte[] GetBytes(long HowMany) => GetBytes(HowMany, "");


        /// <summary>
        /// Look ahead at the next bytes without moving the seek position.
        /// </summary>
        /// <param name="HowMany">How many bytes to look at</param>
        /// <returns>The bytes 'peeked' at</returns>
        public byte[] PeekBytes(long HowMany)
        {
            byte[] Temp = GetBytes(HowMany);
            MoveTo(-HowMany, SeekOrigin.Current);
            return Temp;
        }


        /// <summary>
        /// Return all bytes still unread
        /// </summary>
        /// <returns>All bytes still unread.</returns>
        public byte[] GetRestOfBytes()
        {
            if (BytesRemaining > 0)
                return TheReader.ReadBytes(Helpers.LongToInt(BytesRemaining,
                    "Unable to cast long to int in GetSource.GetRestOfBytes()"));
            else
                return new byte[] { };
        }


        /// <summary>
        /// To implement iDisposable interface
        /// Flag to indicate if already disposed
        /// </summary>
        private bool IsDisposed { get; set; }  // Defaults to false


        /// <summary>
        /// To implement iDisposable interface
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// To implement iDisposable interfac
        /// </summary>
        /// <param name="areDisposing">areDisposing</param>
        protected virtual void Dispose(bool areDisposing)
        {
            try
            {
                // If you have already used this method
                // do not use it again.
                if (!this.IsDisposed)
                {
                    if (areDisposing)
                    {
                        // If we are disposing _not_ finalizing
                        // dispose of the managed resource.
                        if (IsFileBased)
                        {
                            TheFile.Dispose();
                            TheFile = null;
                            TheReader.Dispose();
                            TheReader = null;
                        }

                    }
                    // Free up any unmanaged resource.

                }
            }
            finally
            {
                // Set flag so method not used again.
                this.IsDisposed = true;
            }
        }


        /// <summary>
        /// To implement iDisposable interface
        /// Filaniser
        /// </summary>
        ~ByteSource()
        {
            this.Dispose(false);
        }
    } // class ByteSource 
}