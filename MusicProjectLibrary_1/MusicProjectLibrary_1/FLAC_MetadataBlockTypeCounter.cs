//========================================================================
// Name:     FLAC_MetadataBlockTypeCounter.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  Implements an 'enum' of the types that a FLAC metadata block
//           may be.  Then implements a counter typr for keeping count of
//           how many of each different kind of block exist.
// Comments: 
//========================================================================
using System.Collections.Generic;


namespace JAudioTags
{
    /// <summary>
    /// Class to enable easy counting of how many of each block type 
    /// are found in a flac file.
    /// </summary>
    internal class MetadataBlockTypeCounter : Dictionary<FlacMetadataBlockType, int>
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "MetadataBlockTypeCounter:  1.01";


        /// <summary>
        /// Constructor
        /// </summary>
        public MetadataBlockTypeCounter()
        {
            this.Add(FlacMetadataBlockType.STREAMINFO, 0);
            this.Add(FlacMetadataBlockType.PADDING, 0);
            this.Add(FlacMetadataBlockType.APPLICATION, 0);
            this.Add(FlacMetadataBlockType.SEEKTABLE, 0);
            this.Add(FlacMetadataBlockType.VORBIS_COMMENT, 0);
            this.Add(FlacMetadataBlockType.CUESHEET, 0);
            this.Add(FlacMetadataBlockType.PICTURE, 0);
            this.Add(FlacMetadataBlockType.INVALID, 0);
        }


        /// <summary>
        /// Returns how many Padding blocks were counted
        /// </summary>
        public int PaddingCount
        {
            get { return this[FlacMetadataBlockType.PADDING]; }
        }


        /// <summary>
        /// Returns how many comment blocks were found
        /// </summary>
        public int CommentCount
        {
            get { return this[FlacMetadataBlockType.VORBIS_COMMENT]; }
        }


        /// <summary>
        /// Returns how many picture blocks were found
        /// </summary>
        public int PictureCount
        {
            get { return this[FlacMetadataBlockType.PICTURE]; }
        }


        /// <summary>
        /// Resets all counts to zero
        /// </summary>
        public void Reset()
        {
            this[FlacMetadataBlockType.STREAMINFO] = 0;
            this[FlacMetadataBlockType.PADDING] = 0;
            this[FlacMetadataBlockType.APPLICATION] = 0;
            this[FlacMetadataBlockType.SEEKTABLE] = 0;
            this[FlacMetadataBlockType.VORBIS_COMMENT] = 0;
            this[FlacMetadataBlockType.CUESHEET] = 0;
            this[FlacMetadataBlockType.PICTURE] = 0;
            this[FlacMetadataBlockType.INVALID] = 0;
        }
    }


    /// <summary>
    /// Enumerates the kind of metadata blocks that 
    /// can be found inside a FLAC file.
    /// </summary>
    public enum FlacMetadataBlockType
    {
        /// <summary>
        /// A STREAMINFO metadata block
        /// </summary>
        STREAMINFO = 0,
        /// <summary>
        /// A PADDING metadata block
        /// </summary>
        PADDING = 1,
        /// <summary>
        /// An APPLICATION metadata block
        /// </summary>
        APPLICATION = 2,
        /// <summary>
        /// A SEEKTABLE metadata block
        /// </summary>
        SEEKTABLE = 3,
        /// <summary>
        /// A VORBIS_COMMENT metadata block
        /// </summary>
        VORBIS_COMMENT = 4,
        /// <summary>
        /// A CUESHEET metadata block
        /// </summary>
        CUESHEET = 5,
        /// <summary>
        /// A PICTURE metadata block
        /// </summary>
        PICTURE = 6,
        /// <summary>
        /// An INAVLID metadata block
        /// </summary>
        INVALID = 127
    };
}