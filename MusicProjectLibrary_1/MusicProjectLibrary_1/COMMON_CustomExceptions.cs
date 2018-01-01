//========================================================================
// Name:     COMMON_CustomExceptions,c s
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Imeplements a 'BadAudiofileException' exception.
// Comments: 
//========================================================================
using System;


namespace JAudioTags
{
    /// <summary>
    /// An exception to flag internal errors in the audio file
    /// </summary>
    public class BadAudioFileException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BadAudioFileException() { }
        /// <summary>
        /// Constructor
        /// </summary>
        public BadAudioFileException(string message)
            : base(message) { }
        /// <summary>
        /// Constructor
        /// </summary>
        public BadAudioFileException(string message, Exception inner)
            : base(message, inner) { }
    }
}