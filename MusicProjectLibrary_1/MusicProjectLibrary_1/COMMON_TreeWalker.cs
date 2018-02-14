//========================================================================
// Name:     COMMON_TreeWalker.cs   
// Author:   Jim Roberts 
// Date:     February 2017
// Purpose:  A general purpose class providing methods for walking a 
//           directory tree.  There are constructors and methods to access
//           either every matching file in the tree or every matching 
//           directory.  A method with an appropriate signature is passed
//           in to do the actual processing of the nodes.
// Comments: 
//========================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;


//// ===========================================================================================================
////   E X A M P L E   O F   U S A G E
//// ===========================================================================================================
//using System;
//using System.IO;
//using System.Collections.Generic;


//namespace JAudioTags
//{
//    class Program
//    {
//        /// Method to process directories.
//        /// </summary>
//        /// <param name="CurrentDirectory">The current directories</param>
//        /// <param name="Extensions">A list of file extensions - to select files to be looked at</param>
//        /// <param name="TheWriter">A StreamWriter optionally attached to a results file</param>
//        /// <returns></returns>
//        static private int ProcessDirectory(string CurrentDirectory, List<string> Extensions, StreamWriter TheWriter)
//        {
//            int Accumulator = 0;

//            foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
//            {
//                string Extension = Helpers.JGetExtension(CurrentFile);
//                if (Extensions.Contains(Extension) || Extensions.Contains("*"))
//                    Accumulator++;
//            }
//            return Accumulator;
//        }


//        /// <summary>
//        /// Method to process files
//        /// </summary>
//        /// <param name="CurrentFile">The name of the file to be processed</param>
//        /// <param name="TheWriter">A StreamWriter optionally attached to a results file</param>
//        /// <returns></returns>
//        static private int ProcessFile(string CurrentFile, StreamWriter TheWriter)
//        {
//            FileInfo F = new FileInfo(CurrentFile);
//            Console.WriteLine("Length of file " + F.Name + " is " + F.Length);
//            return 0;
//        }


//        static void Main(string[] args)
//        {
//            string TestPath = @"O:\TestData\TestFolder";

//            List<string> Extensions = new List<string> { "flac" };

//            TreeWalker TW1 = new TreeWalker(TestPath, ProcessDirectory, null, null, null);

//            Console.WriteLine("DirWalk 1:  " + TW1.DirWalk());

//            TreeWalker TW2 = new TreeWalker(TestPath, ProcessFile, Extensions, null, null);

//            Console.WriteLine("FileWalk 2: " + TW2.FileWalk());

//            Helpers.PressAnyKeyToContinue();
//        }
//    }
//}


namespace JAudioTags
{
    /// <summary>
    /// Class for an object that recursively walks through a directory tree.
    /// </summary>
    public class TreeWalker : IDisposable
    {
        /// <summary>
        /// Version string
        /// </summary>
        public const string _Version = "TreeWalker:                1.02";


        /// <summary>
        /// An integer that can be returned by the treewalker methods.
        /// For example, can be used to count files in a directory
        /// </summary>
        private int ReturnValue = 0;


        /// <summary>
        /// Flag to indicate if already disposed
        /// </summary>
        private bool IsDisposed { get; set; } = false;


        /// <summary>
        /// Entry point of recursive tree walk
        /// </summary>
        private string StartPath;


        /// <summary>
        /// A method that will process an individual file, and,
        /// optionally, log results to a file.
        /// 
        /// string is the name of the file.
        /// StreamWriter is used for results logging.
        /// Returns an int.
        /// </summary>
        private Func<string, LazySW, int> ProcessFile;


        /// <summary>
        /// A method that will process a directory and, optionally, write results 
        /// to a log file.
        /// 
        /// string is the path to the file.
        /// List:string: is a list of extensions of files to be looked at in the directory
        /// StreamWriter is used for results logging.
        /// Returns an int.
        /// </summary>
        private Func<string, List<string>, LazySW, int> ProcessDirectory;


        /// <summary>
        /// The file extensions of the files to be processed.
        /// Defaults to *
        /// </summary>
        private List<string> Extensions = new List<string>() { "*" };


        /// <summary>
        /// A StreamWriter to write the output of the process to a log file
        /// </summary>
        private static LazySW TheWriter = null;


        /// <summary>
        /// StreamWriter to log errors to.
        /// </summary>
        private static LazySW ErrorLog = null;


        /// <summary>
        /// Constructor for a walk visiting FILES
        ///   - Able to log results and errors
        ///   - Files amy be filtered by extension
        /// 
        /// Any of the last three argumenst can be null:
        ///   - If FileExtensions is null, all files are matched.
        ///   - If either of the log paths are null the process
        ///   -    must not use them
        /// </summary>
        /// <param name="Root">The starting point for the recursive walk.</param>
        /// <param name="ProcessFile">A method to process and log each file.</param>
        /// <param name="FileExtensions">List of file extensions of files to be processed.</param>
        /// <param name="ResultsPath">Path for file to receive results</param>
        /// <param name="ErrorLogPath">Path to error log</param>
        /// 
        

        //
        public TreeWalker(string Root,
                  Func<string, LazySW, int> ProcessFile,
                  List<string> FileExtensions,
                  string ResultsPath,
                  string ErrorLogPath)
        {
            string Temp;

            StartPath = Root;
            this.ProcessFile = ProcessFile;
            ProcessDirectory = null;

            if (FileExtensions != null)
            {
                Extensions.Clear();
                foreach (var Extension in FileExtensions)
                    // If there is a leading period, remove it.
                    if (Extension[0] == '.')
                    {
                        Temp = Extension.Substring(1, Extension.Length - 1).ToUpper();
                        Extensions.Add(Temp);
                    }
                    else
                        Extensions.Add(Extension.ToUpper());
            }

            if (ResultsPath != null)
                TheWriter = new LazySW(ResultsPath);

            if (ErrorLogPath != null)
                ErrorLog = new LazySW(ErrorLogPath);
        }


        /// <summary>
        /// Constructor for a walk visiting DIRECTORIES
        ///   - Able to log results and errors
        ///   - Able to filter by files in directories by extension
        /// 
        /// Any of the last three argumenst can be null:
        ///   - If FileExtensions is null, all files are matched.
        ///   - If either of the log paths are null the process
        ///   -    must not use them
        /// </summary>
        /// <param name="Root">The starting point for the recursive walk.</param>
        /// <param name="ProcessDirectory">A method to process and log each directory.</param>
        /// <param name="FileExtensions">List of file extensions of files to be processed.</param>
        /// <param name="ResultsPath">Path for file to hold output</param>
        /// <param name="ErrorLogPath">Path to error log</param>
        public TreeWalker(string Root,
                          Func<string, List<string>, LazySW, int> ProcessDirectory,
                          List<string> FileExtensions,
                          string ResultsPath,
                          string ErrorLogPath)
        {
            string Temp;

            StartPath = Root;
            ProcessFile = null;
            this.ProcessDirectory = ProcessDirectory;

            if (FileExtensions != null)
            {
                Extensions.Clear();
                foreach (var Extension in FileExtensions)
                    // If there is a leading period, remove it.
                    if (Extension[0] == '.')
                    {
                        Temp = Extension.Substring(1, Extension.Length - 1).ToUpper();
                        Extensions.Add(Temp);
                    }
                    else
                        Extensions.Add(Extension.ToUpper());
            }

            if (ResultsPath != null)
                TheWriter = new LazySW(ResultsPath);

            if (ErrorLogPath != null)
                ErrorLog = new LazySW(ErrorLogPath);
        }


        /// <summary>
        /// Call this to start the walk processing every matching file
        /// </summary>
        public int FileWalk()
        {
            // First call.  Calls itself recursively after this.
            FileSearch(StartPath);

            // Make sure output gets written
            if (!(TheWriter == null))
                TheWriter.Flush();
            if (!(ErrorLog == null))
                ErrorLog.Flush();

            return ReturnValue;
        }


        /// <summary>
        /// Private method called recursively to walk tree
        /// </summary>
        /// <param name="CurrentDirectory">The current directory</param>
        /// 
        
        public void FileSearch(string CurrentDirectory)
        {
            if (Directory.Exists(CurrentDirectory))
            {
                foreach (string CurrentFile in Directory.GetFiles(CurrentDirectory))
                {
                    string CurrentExtension = Helpers.JGetExtension(CurrentFile);
                    if (Extensions.Contains("*") || Extensions.Contains(CurrentExtension))
                    {
                        try
                        {
                            if (ProcessFile != null)
                                ReturnValue += ProcessFile(CurrentFile, TheWriter);
                        }
                        catch (Exception Ex)
                        {
                            string Message = Ex.Message + "\n   " + CurrentDirectory + "\n   "
                                    + Path.GetFileName(CurrentFile);
                            if (!(ErrorLog == null))
                                ErrorLog.WriteLine(Message);
                            else
                                Console.WriteLine(Message);
                        }
                    }
                }
                foreach (string ChildDirectory in Directory.GetDirectories(CurrentDirectory))
                    FileSearch(ChildDirectory);
            }
            else
                MessageBox.Show($"Directory didn't exist {CurrentDirectory}");
        }


        /// <summary>
        /// Call this to start the walk looking at sub-directory
        /// </summary>
        public int DirWalk()
        {
            // First call.  Calls itself recursively after this.
            DirSearch(StartPath);

            // Make sure output gets written
            if (!(TheWriter == null))
                TheWriter.Flush();
            if (!(ErrorLog == null))
                ErrorLog.Flush();

            return ReturnValue;
        }


        /// <summary>
        /// Private method called recursively to walk tree.
        /// </summary>
        /// <param name="CurrentDirectory">The current directory</param>
        private void DirSearch(string CurrentDirectory)
        {
            try
            {
                if (ProcessDirectory != null)
                    ReturnValue += ProcessDirectory(CurrentDirectory, Extensions, TheWriter);
            }
            catch (Exception Ex)
            {
                string Message = CurrentDirectory + "\t" + Ex.Message;
                if (!(ErrorLog == null))
                    ErrorLog.WriteLine(Message);
                else
                    Console.WriteLine(Message);
            }
            foreach (string ChildDirectory in Directory.GetDirectories(CurrentDirectory))
                DirSearch(ChildDirectory);
        }


        /// <summary>
        /// To implement iDisposable
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// To implement iDisposable
        /// </summary>
        /// <param name="areDisposing"></param>
        protected virtual void Dispose(bool areDisposing)
        {
            try
            {
                // If you have already used this method
                // do not use it again.
                if (!IsDisposed)
                {
                    if (areDisposing)
                    {
                        // If we are disposing _not_ finalizing
                        // dispose of the managed resource.
                        if (TheWriter != null)
                            TheWriter.Close();
                        TheWriter = null;
                    }
                    // Free up the unmanaged resource - there are none.
                }
            }
            finally
            {
                // Set flag so method not used again.
                this.IsDisposed = true;
            }
        }


        /// <summary>
        /// To implement iDisposable.
        /// </summary>
        ~TreeWalker()
        {
            this.Dispose(false);
        }
    } // class 
}
