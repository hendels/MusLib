//========================================================================
// Name:     FLAC_MetadataBlocks.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  The metadata in a FLAC file is held in a series of 'blocks'.
//           This is a class to represent those blocks.
// Comments: 
//========================================================================
namespace JAudioTags
{
    /// <summary>
    /// Represents a FLAC metadata block
    /// </summary>
    internal class MetadataBlock
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "MetadataBlock:             1.01";


        /// <summary>
        /// A 4-byte header
        /// </summary>
        public MetadataBlockHeader Header { get; private set; }


        /// <summary>
        /// The metadata itself.
        /// </summary>
        public byte[] DataBlock { get; private set; }    


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="header">A pre-built header</param>
        /// <param name="data">A pre-populated data block</param>
        public MetadataBlock(MetadataBlockHeader header, byte[] data)
        {
            Header = header;
            DataBlock = data;
        }


        /// <summary>
        /// Property to return the block type
        /// </summary>
        public FlacMetadataBlockType BlockType
        {
            get
            {
                return Header.BlockType;
            }
        }


        /// <summary>
        /// Property to return whether the block is last
        /// </summary>
        public bool IsLast
        {
            get
            {
                return Header.IsLast;
            }
        }


        /// <summary>
        /// Property to return length of the data section of this block.
        /// </summary>
        public int DataBlockLength
        {
            get
            {
                return DataBlock.Length;
            }
        }


        /// <summary>
        /// Overwrite to Tostring()
        /// </summary>
        /// <returns>Block details as a string</returns>
        public override string ToString()
        {
            string Temp = BlockType.ToString() + "\n";
            Temp += "     Header:         " + Header + "\n";
            Temp += "     Length of data:   " + string.Format("{0,8}", DataBlockLength.ToString("#,##0") + "\n");
            Temp += "     Is Last:            " + string.Format("{0,5}", IsLast);
            return Temp;
        }
    }
}