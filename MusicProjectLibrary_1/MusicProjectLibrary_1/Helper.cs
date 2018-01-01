using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    public static class Helper
    {
        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        public static void DoWork(IProgress<int> progress, int processed, int maxValue, Label progressLabel, string progressString)
        {
            // This method is executed in the context of
            // another thread (different than the main UI thread),
            // so use only thread-safe code
            
            if (progress != null)
            {
                progress.Report((processed + 1) * 100 / maxValue); // 100k MAX VALUE   
                decimal progressDecimal = (processed + 1) * 100 / maxValue;
                //progressLabel.Text = progressString + ": " + progressDecimal.ToString();
            }
                 
            
        }
    }
}
