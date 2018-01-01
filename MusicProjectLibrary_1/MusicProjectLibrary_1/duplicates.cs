using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    public partial class duplicates : Form
    {
        public duplicates()
        {
            InitializeComponent();
        }
        class MyStruct
        {
            public string Path1 { get; set; }
            public string Path2 { get; set; }


            public MyStruct(string p1, string p2)
            {
                Path1 = p1;
                Path2 = p2;
            }
        }
        private void duplicates_Load(object sender, EventArgs e)
        {
            List<string> uniqueArtists = new List<string>();
            List<string> uniquePartializedTitles = new List<string>();
            List<DuplicatesPaths> LDP = new List<DuplicatesPaths>();

            

            Functions.createArtistsList(uniqueArtists);
            Functions.checkTrackDuplicates(uniqueArtists, uniquePartializedTitles, LDP);

            var source = new BindingSource();
            List<MyStruct> list = new List<MyStruct>();
            foreach(DuplicatesPaths DP in LDP)
            {
                list.Add(new MyStruct(DP.firstPath,DP.secondPath));                
            }            
            
            source.DataSource = list;
            dgvPaths.DataSource = source;
            dgvPaths.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvPaths.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void dgvPaths_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvPaths.Rows[dgvPaths.SelectedCells[0].RowIndex].Selected = true;
        }

        private void dgvPaths_DoubleClick(object sender, EventArgs e)
        {
            DirectoryManagement.OpenPairOfFolders(dgvPaths, dgvPaths.SelectedCells[0].RowIndex);
        }
    }
}
