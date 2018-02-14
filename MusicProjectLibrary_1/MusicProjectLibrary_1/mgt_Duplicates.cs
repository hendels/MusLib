using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class mgt_Duplicates
    {
        public static void OpenPairOfFolders(DataGridView DGV, int AlbumRowIndex)
        {
            dataGridColumnsDuplicates DGCD = new dataGridColumnsDuplicates();

            DGV.MultiSelect = false;
            string GridValuePath1 = DGV.Rows[AlbumRowIndex].Cells[DGCD.colFirstPath].Value.ToString();
            string GridValuePath2 = DGV.Rows[AlbumRowIndex].Cells[DGCD.colSecondPath].Value.ToString();
            Process.Start(GridValuePath1);
            Process.Start(GridValuePath2);
        }
    }
}
