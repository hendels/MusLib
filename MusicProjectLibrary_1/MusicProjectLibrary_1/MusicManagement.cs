using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

///
using System.IO;
using MusicProjectLibrary_1;
using MaterialSkin;
/// <summary>
/// 
/// </summary>
namespace MusicProjectLibrary_1
{
    public partial class AlbumWindow : MaterialSkin.Controls.MaterialForm
    {
        List<SQLTestGet> AlbumList = new List<SQLTestGet>();
        public AlbumWindow()
        {

            InitializeComponent();
            lstAlbums.DataSource = AlbumList;
            lstAlbums.DisplayMember = "FullInfo";


            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'albumDataSet.Album' table. You can move, or remove it, as needed.
            //this.albumTableAdapter.Fill(this.albumDataSet.Album);
            // TODO: This line of code loads data into the 'musicLibraryDataSet.Track' table. You can move, or remove it, as needed.
            //this.trackTableAdapter.Fill(this.musicLibraryDataSet.Track);

            ///
            Functions.getPurgatoryPath(BoxPickedPath,true);
            GlobalVariables globalProcCatalog = new GlobalVariables();

            
            //musicLibraryDataSetBindingSource.AllowRemove;
            ///
        }

        


        private void idAlbumLabel_Click(object sender, EventArgs e)
        {

        }

        private void release_DateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void idAlbumTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void favourite_Tracks_NoTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void nameLabel_Click(object sender, EventArgs e)
        {

        }

        private void favourite_Tracks_NoLabel_Click(object sender, EventArgs e)
        {

        }

        private void release_DateLabel_Click(object sender, EventArgs e)
        {

        }

        private void albumListBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.albumListBindingSource.EndEdit();


        }

        private void siemanoBox_TextChanged(object sender, EventArgs e)
        {

        }
        /// <functions>
        

        
        /// </functions>
        /// <param name="Filename"></param>

        private void updateButton_Click(object sender, EventArgs e)
        {
            
        }

        private void moveFileButt_Click(object sender, EventArgs e)
        {
            string source = @"D:\muz\CSharp\Move1\sharp_move.txt";

            string destination = @"D:\muz\CSharp\Move2\sharp_move.txt";

            File.Move(source, destination);
            MessageBox.Show("File moved");
        }

        public void checkFilesInDirectory_Click(object sender, EventArgs e)
        {
            Functions.pickPath(BoxPickedPath);
            Functions.showFolderBrowserDialog(ListBoxItems.Items);
        }

        private void tagTest_Click(object sender, EventArgs e)
        {
            string Root = @"C:\prywata\CSharp";
            MusicFileMgt.changeFiles(0, Root, BoxListConsole.Items);
        }

        private void ButtonReadTag_Click(object sender, EventArgs e)
        {
            MusicFileMgt.readFiles(BoxListConsole.Items);
        }

        private void ButtonReadDir_Click(object sender, EventArgs e)
        {
            
        }

        private void CheckBoxProcessCatalogs_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxProcessCatalogs.Checked == true)
                GlobalVariables.globalProcessCatalog = true;            
            else            
                GlobalVariables.globalProcessCatalog = false;
            
        }

        private void CheckBoxModifyFIles_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxProcessCatalogs.Checked == true)
                GlobalVariables.globalModifyFIles = true;
            else
                GlobalVariables.globalModifyFIles = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {          
        }
        

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                musicLibraryDataSet.Track.AddTrackRow(musicLibraryDataSet.Track.NewTrackRow());
                musicLibraryDataSetBindingSource.MoveLast();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                musicLibraryDataSet.RejectChanges();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                Validate();
                musicLibraryDataSetBindingSource.EndEdit();
                trackTableAdapter.Update(musicLibraryDataSet.Track);
                dataGridView.Refresh();
                MessageBox.Show("updated");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                musicLibraryDataSet.RejectChanges();
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //musicLibraryDataSetBindingSource.AllowRemove = true;
            //musicLibraryDataSetBindingSource.EndEdit();
            trackBindingSource.RemoveCurrent();
        }

        private void dataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                trackBindingSource.RemoveCurrent();
            }
        }

        private void btnFindAlbums_Click(object sender, EventArgs e)
        {
            DBFunctions db = new DBFunctions();

            AlbumList = db.GetAlbum(tbxSearchAlbums.Text);

            lstAlbums.DataSource = AlbumList;
            lstAlbums.DisplayMember = "FullInfo";
        }
    }
}
