//========================================================================
// Name:     COMMON_TagList.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  A FLAC file stores its metadata as a list of 'blocks'.  This
//           class implements a List<> to manage these metadata blocks.
// Comments: 
//========================================================================
using System;
using System.Collections.Generic;
using System.IO;


namespace JAudioTags
{
    /// <summary>
    /// Wraps a List of metadata blocks.
    /// Provides additional methods to manipulate the list.
    /// </summary>
    internal class BlockListType : List<MetadataBlock>
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "BlockListType:             1.01";


        /// <summary>
        /// Structure to hold counts of block types
        /// </summary>
        public MetadataBlockTypeCounter BlockCounts { get; private set; }


        /// <summary>
        /// Where are the two key blocks at this time?
        /// </summary>
        public int IndexOfCommentBlock { get; private set; }
        public int IndexOfPaddingBlock { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public BlockListType()
        {
            BlockCounts = new MetadataBlockTypeCounter();
        }


        /// <summary>
        /// Write a selected number of blocks to file
        /// </summary>
        /// <param name="TheWriter">An opened BinaryWriter</param>
        /// <param name="First">The index of the first block</param>
        /// <param name="Last">The index of the last block</param>
        private void WriteBlocksToFile(BinaryWriter TheWriter, int First, int Last)
        {
            if (First < 0 || Last >= this.Count || First >= Last)
                throw new ArgumentOutOfRangeException("Invalid 'First' or 'Last' value passed to BlockListType:WriteToFile()");
            
            // Step over file header
            int StartAddress = 4;

            // Step over each block before first
            for (int i = 0; i < First; i++)
                StartAddress += 4 + this[i].DataBlockLength;

            // Move to the start of the overwrite window
            TheWriter.Seek(StartAddress, SeekOrigin.Begin);

            // Write each block in turn
            for (int i = First; i <= Last; i++)
            {
                if (i == this.Count - 1)
                    // Write header with 'IsLast' set
                    TheWriter.Write(new MetadataBlockHeader(this[i].BlockType, true,
                         this[i].DataBlock.Length).TheBytes);
                else
                    // Write header with 'IsLast' not set
                    TheWriter.Write(new MetadataBlockHeader(this[i].BlockType, false,
                        this[i].DataBlock.Length).TheBytes);
                // Now write data
                TheWriter.Write(this[i].DataBlock);
            }
        }


        /// <summary>
        /// Write all blocks in the list to file
        /// </summary>
        /// <param name="TheWriter">An opened BinaryWriter</param>
        /// <param name="ReOrderBlocks">Should we change the order of the blocks before writing?</param>
        public void WriteAllBlocksToFile(BinaryWriter TheWriter, bool ReOrderBlocks)
        {
            if (ReOrderBlocks)
                MoveCommentAndPaddingBlocks(1, this.Count - 1); 
            
            WriteBlocksToFile(TheWriter, 0, this.Count - 1);
        }


        /// <summary>
        /// Puts comment first, followed by padding.
        /// Then writes this subset of blocks to disc.
        /// </summary>
        /// <param name="TheWriter">An opened BinaryWriter</param>
        public void OverwriteSubsetOfBlocksInFile(BinaryWriter TheWriter)
        {
            int First, Last;

            // Record size of block
            if (IndexOfCommentBlock < IndexOfPaddingBlock)
            {
                First = IndexOfCommentBlock;
                Last = IndexOfPaddingBlock;
            }
            else
            {
                First = IndexOfPaddingBlock;
                Last = IndexOfCommentBlock;
            }

            // Re-order blocks
            MoveCommentAndPaddingBlocks(First, Last);
            
            // Overwrite the window of the file
            WriteBlocksToFile(TheWriter, First, Last);
        }


        /// <summary>
        /// Moves the VORBIS_COMMENT block to the first position in the window
        /// and the PADDING block into the second position.
        /// </summary>
        /// <param name="FirstBlockInWindow">The first position of the window</param>
        /// <param name="LastBlockInWindow">The last position of the window</param>
        private void MoveCommentAndPaddingBlocks(int FirstBlockInWindow, int LastBlockInWindow)
        {
            if (IndexOfCommentBlock < FirstBlockInWindow ||
                IndexOfPaddingBlock < FirstBlockInWindow ||
                IndexOfCommentBlock > LastBlockInWindow ||
                IndexOfPaddingBlock > LastBlockInWindow)
                throw new ArgumentException("Invalid parameters passed to MoveCommentAndPaddingBlocks().");

            // Move comment block into FirstBlockInWindow
            // (STREAMINFO should be in position 0)
            if (this[FirstBlockInWindow].BlockType != FlacMetadataBlockType.VORBIS_COMMENT)
                SwapBlocks(FirstBlockInWindow, IndexOfCommentBlock);
            UpdateBlockCounts();

            // Move padding block into next position
            if (this[FirstBlockInWindow + 1].BlockType != FlacMetadataBlockType.PADDING)
                SwapBlocks(FirstBlockInWindow + 1, IndexOfPaddingBlock);
            UpdateBlockCounts();
        }


        /// <summary>
        /// Counts how many blocks of each type and set pointers. 
        /// </summary>
        public void UpdateBlockCounts()
        {
            // Set counts back to zero
            BlockCounts.Reset();

            // Count backwards so padding block is first in file, not last.
            for(int i = Count - 1; i >= 0; i--)
            {
                BlockCounts[this[i].BlockType]++;  // Update counts
                if (this[i].BlockType == FlacMetadataBlockType.VORBIS_COMMENT)
                    IndexOfCommentBlock = i;
                if (this[i].BlockType == FlacMetadataBlockType.PADDING)
                    IndexOfPaddingBlock = i;
            }
        }


        /// <summary>
        /// Swaps blocks within the metadata block list.
        /// </summary>
        /// <param name="x">MetadataBlock to swap</param>
        /// <param name="y">MetadataBlock to swap</param>
        public void SwapBlocks(int x, int y)
        {
            MetadataBlock temp = this[x];
            this[x] = this[y];
            this[y] = temp;

            // Recount
            UpdateBlockCounts();
        }


        /// <summary>
        /// Add a block to the list and update counts
        /// </summary>
        /// <param name="TheBlock">The block to be added</param>
        public void AddBlock(MetadataBlock TheBlock)
        {
            this.Add(TheBlock);
            UpdateBlockCounts();
        }


        /// <summary>
        /// Remove a block from the list at the specified position
        /// and update the counts.
        /// </summary>
        /// <param name="n">The block position to delete.</param>
        public void RemoveBlockAt(int n)
        {
            if (n < 0 || n > this.Count - 1)
                throw new ArgumentException("Trying to remove non-existant block in BlockListType:RemoveBlockAt().\n"
                    + "n = " + n);
            this.RemoveAt(n);
            UpdateBlockCounts();
        }


        /// <summary>
        /// Show the current block list as a string.
        /// </summary>
        /// <returns>String representation of the block list</returns>
        public override string ToString()
        {
            String s = "";
            for (int i = 0; i < this.Count; i++)
                s += i + ".  " + this[i].BlockType + "\n";
            s += "\n";
            return s;
        }
    }
}