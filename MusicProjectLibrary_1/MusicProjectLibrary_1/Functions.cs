using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace MusicProjectLibrary_1
{
    public class Functions
    {
        public static string globalOldPath;
        public static string filenamePath;

        

        public static void xmlSave(object obj, string filename)
        {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            sr.Serialize(writer, obj);
            writer.Close();
        }

        public static void getPurgatoryPath(TextBox currentTextBox, Boolean updateTextbox)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Functions.filenamePath = startupPath + @"\purgatoryPath.txt";
            Functions.globalOldPath = MiscFunctions.ReadFile(Functions.filenamePath);


            if (updateTextbox)
            {
                currentTextBox.Text = Functions.globalOldPath;
            }
        }
        public static void getMainDirectoryPath(TextBox currentTextBox, Boolean updateTextbox)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Functions.filenamePath = startupPath + @"\mainDirectoryPath.txt";
            Functions.globalOldPath = MiscFunctions.ReadFile(Functions.filenamePath);


            if (updateTextbox)
            {
                currentTextBox.Text = Functions.globalOldPath;
            }
        }
        public static void getDriveGeneralPath(TextBox currentTextBox, Boolean updateTextbox)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Functions.filenamePath = startupPath + @"\generalDrivePath.txt";
            Functions.globalOldPath = MiscFunctions.ReadFile(Functions.filenamePath);


            if (updateTextbox)
            {
                currentTextBox.Text = Functions.globalOldPath;
            }
        }
        public static void pickPath(int pathCase, TextBox currentTextBox)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                switch (pathCase)
                {
                    case 1:
                        getPurgatoryPath(currentTextBox, false);
                        break;
                    case 2:
                        getMainDirectoryPath(currentTextBox, false);
                        break;
                    case 3:

                        break;
                }

                string newPath = FBD.SelectedPath;
                if (Functions.globalOldPath != newPath)
                {
                    string messageText = $"stara ścieżka główna to: {Functions.globalOldPath} czy na pewno chcesz zmienić na: {FBD.SelectedPath}?";

                    if (MessageBox.Show(messageText, "różne ścieżki", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        MiscFunctions.WriteFile(Functions.filenamePath, FBD.SelectedPath);
                        currentTextBox.Text = FBD.SelectedPath;
                    }
                }
            }
        }

        
        
               
        
        public static void getRelatedToArtistGenres()
        {
            List<SQLAlbumTable> queryGetAllGenres = new List<SQLAlbumTable>();
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            queryGetAllGenres = db.GetAlbumsGenre();
            //chlstSuggestedGenres
        }

        public static string findProhibitedSigns(string checkString, char changeToChar)
        {
            List<char> signs = new List<char>();
            signs.Add('|');
            signs.Add('"');
            signs.Add('/');
            signs.Add('<');
            signs.Add('>');
            signs.Add('\\');
            //signs.Add(@".");
            string modifiedString = checkString;
            foreach (char itemList in signs)
            {
                if (checkString.Contains(itemList))
                {
                    modifiedString = modifiedString.Replace(itemList, changeToChar);
                };

            }
            return modifiedString;
        }
        public static string findProhibitedSigns(string checkString)
        {
            List<string> signs = new List<string>();
            signs.Add("|");
            signs.Add(@"""");
            signs.Add(@"/");
            signs.Add(@"<");
            signs.Add(@">");
            //signs.Add(@".");
            string modifiedString = checkString;
            foreach (string itemList in signs)
            {
                if (checkString.Contains(itemList))
                {
                    modifiedString = modifiedString.Replace(itemList, string.Empty);
                };

            }
            return modifiedString;
        }
  
        
        public static void TreeFileSearch(string CurrentDirectory)
        {            
            foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
            {


                try
                {
                    
                }
                catch (Exception Ex)
                {
                    
                }

            }
            foreach (string ChildDirectory in Directory.GetDirectories(CurrentDirectory))
                TreeFileSearch(ChildDirectory);
        }
        public static string baseDirectory { get; set; }
        public static void FindFilesInTreeDirectory(bool dontMoveFile, string CurrentDirectory, string movedAlbumDirectory, ListBox.ObjectCollection boxListConsole) //[przemy knowledge - best tree search]
        {
            int foundIllegalFiles = 0;
            try
            {
                foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
                {
                    if (Path.GetExtension(CurrentFile) == ".flac" || Path.GetExtension(CurrentFile) == ".mp3")
                    {
                        foundIllegalFiles += 1;
                        boxListConsole.Add($"found illegal file in subfolder {CurrentFile} - manual action needed.");
                    }

                    if (!dontMoveFile)
                    {
                        if (Path.GetExtension(CurrentFile) == ".jpg" || Path.GetExtension(CurrentFile) == ".png" 
                            || Path.GetExtension(CurrentFile) == ".tif" || Path.GetExtension(CurrentFile) == ".jpeg") 
                        {
                            try
                            {
                                File.Move(CurrentFile, movedAlbumDirectory + @"\" + Path.GetFileName(CurrentFile));
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException ex1)
            {
                MessageBox.Show(ex1.ToString());
            }

            if (foundIllegalFiles == 0 & Directory.Exists(CurrentDirectory))
            {
                try
                {
                    if (!dontMoveFile)
                    {
                        foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
                        {
                            File.Delete(CurrentFile);
                            boxListConsole.Add($"deleting trash file: {CurrentFile}");
                        }
                    }   
                    else
                    {
                        Directory.Delete(CurrentDirectory, dontMoveFile);
                        try { Directory.Delete(Directory.GetParent(baseDirectory).ToString()); }
                        catch { }
                    }
                        
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            if (!dontMoveFile)
            {
                foreach (string ChildDirectory in Directory.GetDirectories(CurrentDirectory))
                {
                    GlobalVariables.IgnoreCurrentFolder += 1;
                    FindFilesInTreeDirectory(dontMoveFile, ChildDirectory, movedAlbumDirectory, boxListConsole);
                }
                //take care of primaryFolder after all moving - delete it
                Directory.Delete(baseDirectory, true);
                try { Directory.Delete(Directory.GetParent(baseDirectory).ToString()); }
                catch { }
            }     
        }
    }
}
