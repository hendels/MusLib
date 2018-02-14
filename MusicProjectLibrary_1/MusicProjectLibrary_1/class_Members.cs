using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JAudioTags;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    public class MusicFileDetails
    {
        public string trackDirectory { set; get; }
        public string trackAudioPath { set; get; }
        public string trackName { set; get; }
        public string trackArtist { set; get; }
        public string trackAlbum { set; get; }
        public string trackRating { set; get; }
        public string trackModDateTag { set; get; }
        public string trackGenre { set; get; }
        public string trackIndex { set; get; }
        public string trackFileExtension { set; get; }
        public string trackIdAlbumIndex { get; set; }
        public string trackStyle { get; set; }

        public AudioFile pickedAFile { set; get; }
    }
    public class RatingInformation
    {
        public string trackDirectory { set; get; }
        public int trackRating { set; get; }
  
        public AudioFile pickedAFile { set; get; }
    }
    public class TagInformation
    {
        public string trackName { set; get; }
        public string trackAlbum { set; get; }
        public string trackArtist { set; get; }
        public string trackGenre { set; get; }
        public string trackIndexLib { set; get; }
        public string trackFileExtension { set; get; }
        public AudioFile pickedAFile { set; get; }
    }
    public class GlobalRating
    {
        private static int howManyRate;
        public static int globalhowManyRate
        {
            get { return howManyRate; }
            set { howManyRate = value; }
        }
        private static int howManyUnrate;
        public static int globalhowManyUnrate
        {
            get { return howManyUnrate; }
            set { howManyUnrate = value; }
        }
    }
    public class GlobalVariables
    {
        private static bool ProcessCatalog = true;
        public static bool globalProcessCatalog
        {
            get { return ProcessCatalog; }
            set { ProcessCatalog = value; }
        }
        private static bool ModifyFIles = false;
        public static bool globalModifyFIles
        {
            get { return ModifyFIles; }
            set { ModifyFIles = value; }
        }
        public static bool writeValidationPoints = false;
        public static bool checkGeneralPath = false;
        public static bool runPickArtist = false;
        private static bool writeIndexes = false;
        public static bool globalwriteIndexes
        {
            get { return writeIndexes; }
            set { writeIndexes = value; }
        }
        private static int selectedGridAlbumID = 0;
        public static int globalSelectedGridAlbumID
        {
            get { return selectedGridAlbumID; }
            set { selectedGridAlbumID = value; }
        }
        private static bool CreateBackup = false;
        public static bool globalCreateBackup
        {
            get { return CreateBackup; }
            set { CreateBackup = value; }
        }
        public static int IgnoreCurrentFolder = 0;
        public static string SelectedArtist = "";
        public static string LastUsedGenre = "";
        public static string SelectedAlbum = "";
    }
    public class uniqueCatalogs
    {
        public string uniqeDirectory { set; get; }
    }
    public class TrackDuplicates
    {
        public string partTrackName { set; get; }
        public string fullTrackName { set; get; }
        public string TrackPath { set; get; }
        public MusicFileDetails MFD { set; get; }
    }
    public class DuplicatesPaths
    {
        public string firstPath { set; get; }
        public string secondPath { set; get; }
        public int ratingStarsFirstFile { get; set; }
        public int ratingStarsSecondFile { get; set; }
        
        public int firstIDAlbum { get; set; }
        public int firstIDTrack { get; set; }
        public int secondIDAlbum { get; set; }
        public int secondIDTrack { get; set; }
    }
    public class PassConsole
    {
        public ListBox listboxConsole { set; get; }
    }
    public class GlobalChecker
    {
        private static int CheckerTags;
        public static int globalCheckerTags
        {
            get { return CheckerTags; }
            set { CheckerTags = value; }
        }

        public static int CheckerAlbum = 0;
        public static int globalCheckerAlbum
        {
            get { return CheckerAlbum; }
            set { CheckerAlbum = value; }
        }
        public static int TestSqlAlbumIdQuery = 0;
        //public static int globalCheckerPhoto
        //{
        //    get { return TestSqlAlbumIdQuery; }
        //    set { TestSqlAlbumIdQuery = value; }
        //}
        public static int CheckerRating = 0;
        public static int globalCheckerRating
        {
            get { return CheckerRating; }
            set { CheckerRating = value; }
        }
        
    }
    public class dataGridColumnsDuplicates
    {

        public int colFirstPath = 0;
        public int colFirstIdAlbum = 1;
        public int colFirstIdTrack = 2;
        public int colFirstRating = 3;

        public int colSecondPath = 4;
        public int colSecondIdAlbum = 5;
        public int colSecondIdTrack = 6;
        public int colSecondRating = 7;

    }
}
