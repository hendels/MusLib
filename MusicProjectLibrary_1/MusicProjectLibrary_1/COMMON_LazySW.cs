using System;
using System.IO;

namespace JAudioTags
{
    /// <summary>
    /// Implements a 'lazy' StreamWriter.  
    /// It only creates a file when/if you ask 
    /// it to actually write to the file.
    /// </summary>
    public class LazySW : IDisposable
    {
        /// <summary>
        /// A lazy object wrapping a StreamWriter
        /// </summary>
        Lazy<StreamWriter> LSW;


        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "LazySW:                    1.00";


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path"></param>
        public LazySW(string Path)
        {
            LSW = new Lazy<StreamWriter>(() => new StreamWriter(Path));
        }


        /// <summary>
        /// To implement IDisposable
        /// </summary>
        public void Dispose()
        {
            if (LSW.IsValueCreated)
                LSW.Value.Dispose();
        }


        /// <summary>
        /// Write a string
        /// </summary>
        /// <param name="S"></param>
        public void Write(string S)
            => LSW.Value.Write(S);


        /// <summary>
        /// Write a string then move to a new line
        /// </summary>
        /// <param name="S"></param>
        public void WriteLine(string S)
            => LSW.Value.WriteLine(S);


        /// <summary>
        /// Write a char
        /// </summary>
        /// <param name="C"></param>
        public void Write(char C)
            => LSW.Value.Write(C);


        /// <summary>
        /// Flush output to the stream
        /// </summary>
        public void Flush()
            => LSW.Value.Flush();


        /// <summary>
        /// Close the stream
        /// </summary>
        public void Close()
            => LSW.Value.Close();
    }
}