using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    class mgt_Tracks
    {
        private static string setDate;

        public static void deleteOneStars(string primaryPath, string dateString, ListBox.ObjectCollection boxListConsole)
        {
            setDate = dateString;
            MusicFileDetails MFD = new MusicFileDetails(); // deklaruj klase
            try { mgt_HddAnalyzer.QuickRead(primaryPath, MFD); }
            catch { return; }

            try
            {
                int TrackIndex = Convert.ToInt32(MFD.trackIndex);
                if (TrackIndex > 1)
                {
                    try
                    {
                        File.Delete(primaryPath); // delete all one star files   
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"file does not exist while attempting to delete - path: {primaryPath} /n The process will continue...");
                    }

                    SetAsDeletedInDatabase(TrackIndex);
                    boxListConsole.Add($"...[File deleted (one star reason): {primaryPath}]!");
                }
                else
                    boxListConsole.Add($"...[No index file - error while deleting.): {primaryPath}]!");
            }
            catch (Exception e)
            {
                boxListConsole.Add($"...[No index file - error while deleting.): {primaryPath}]!");
            }
        }
        private static void SetAsDeletedInDatabase(int trackIndex)
        {
            mgt_SQLDatabase db = new mgt_SQLDatabase();
            db.UpdateTrackFileStatusByIndexLib(trackIndex, "DELETED");
            db.UpdateTrackFileDateProceed(trackIndex, setDate);
            db.UpdateTrackDirectoryPathByIndexLib(trackIndex, "");
        }
    }
}
