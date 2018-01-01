using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace MusicProjectLibrary_1
{
    public class MiscFunctions
    {
        public static void WriteFile(string Filename, string newPath)
        {
            File.WriteAllText(Filename, newPath);
        }
        public static string ReadFile(string Filename)
        {
            return File.ReadAllText(Filename);
        }
        public decimal PercentComplete { get; set; }
        
    }
    
}
