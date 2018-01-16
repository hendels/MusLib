using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using System.Data;
using System.Windows.Forms;

namespace MusicProjectLibrary_1
{
    public class DBFunctions
    {
        //[ALBUMS TABLE]
            //[GET from Albums table]
        public List<SQLAlbumTable> GetAllAlbums(int RecordCountP, int selectAllP, int sortByP, int rangeValidateMinP, int rangeValidateMaxP, bool showProceedP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                
                var output = connection.Query<SQLAlbumTable>($"dbo.spAlbums_GetAll @RecordCount, @selectAll, @sortBy, @rangeValidateMin, @rangeValidateMax, @showProceed", new
                {
                    RecordCount = RecordCountP,
                    selectAll = selectAllP,
                    sortBy = sortByP,
                    rangeValidateMin = rangeValidateMinP,
                    rangeValidateMax = rangeValidateMaxP,
                    showProceed = showProceedP
                }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLAlbumTable> GetAlbumsGenre()
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {

                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAllGenres").ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLAlbumTable> GetAlbum(string ArtistName) 
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetByAlbumName @Artist", new { Artist = ArtistName }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLAlbumTable> GetAlbumById(int idAlbumP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAlbumById @idAlbum", new { idAlbum = idAlbumP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        
        public List<SQLAlbumTable> GetAlbumDirectory(string AlbumDirectoryP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAlbumDirectory @AlbumDirectory", new { AlbumDirectory = AlbumDirectoryP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLAlbumTable> GetAlbumIDQuery(string AlbumDirectoryP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAlbumId @AlbumDirectory", new { AlbumDirectory = AlbumDirectoryP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        private int GetAlbumID(string AlbumDirectoryP)
        {
            List<SQLAlbumTable> queryAlbumID = new List<SQLAlbumTable>(); //sqlprzemy
            queryAlbumID = GetAlbumIDQuery(AlbumDirectoryP);
            foreach (SQLAlbumTable itemAlbumId in queryAlbumID)
            {
                int AlbumID = itemAlbumId.idAlbum;
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;
            }
            return 0;
        }
        
        public List<SQLAlbumTable> GetAllAlbumWrittenGenresByArtist(string AlbumArtistP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAllWrittenGenresByArtist @AlbumArtist", new { AlbumArtist = AlbumArtistP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLAlbumTable> GetAllRelatedGenres(string SelectedGenreP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAllRelatedGenres @SelectedGenre", new { SelectedGenre = SelectedGenreP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        
        public List<SQLAlbumTable> GetAlbumArtists()
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {

                var output = connection.Query<SQLAlbumTable>("dbo.spAlbums_GetAlbumsArtists").ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        //[INSERT Albums table]
        public void InsertAlbum(string AlbumNameP, string AlbumDir, string ArtistName, int releaseYear, string AlbumGenreP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                List<SQLAlbumTable> SqlAlbum = new List<SQLAlbumTable>();
                SqlAlbum.Add(new SQLAlbumTable { AlbumName = AlbumNameP, AlbumDirectory = AlbumDir, AlbumArtist = ArtistName, AlbumReleaseYear = releaseYear, AlbumGenre = AlbumGenreP }); 
                connection.Execute("dbo.spAlbums_InsertAlbum @AlbumName, @AlbumDirectory, @AlbumArtist, @AlbumReleaseYear, @AlbumGenre", SqlAlbum);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }    
            //[update Albums table]
        public int UpdateAlbumCheck(string AlbumDirectoryP, bool AlbumCheckP, string AlbumNameP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateAlbumCheck @AlbumDirectory, @AlbumCheck, @AlbumName", new { AlbumDirectory = AlbumDirectoryP, AlbumCheck = AlbumCheckP, AlbumName = AlbumNameP });
                int AlbumID = GetAlbumID(AlbumDirectoryP);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;                
            }
            return 0;
        }
        public int UpdateArtistCheck(string AlbumDirectoryP, bool ArtistCheckP, string AlbumArtistP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateArtistCheck @AlbumDirectory, @ArtistCheck, @AlbumArtist", new { AlbumDirectory = AlbumDirectoryP, ArtistCheck = ArtistCheckP, AlbumArtist = AlbumArtistP });
                int AlbumID = GetAlbumID(AlbumDirectoryP);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;
            }
            return 0;
        }
        public int UpdateGenreCheck(string AlbumDirectoryP, bool GenreCheckP, string AlbumGenreP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateGenreCheck @AlbumDirectory, @GenreCheck, @AlbumGenre", new { AlbumDirectory = AlbumDirectoryP, GenreCheck = GenreCheckP, AlbumGenre = AlbumGenreP });
                int AlbumID = GetAlbumID(AlbumDirectoryP);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;
            }
            return 0;
        }
        public int UpdateIndexCheck(string AlbumDirectoryP, bool IndexCheckP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateIndexCheck @AlbumDirectory, @IndexCheck", new { AlbumDirectory = AlbumDirectoryP, IndexCheck = IndexCheckP });
                int AlbumID = GetAlbumID(AlbumDirectoryP);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;
            }
            return 0;
        }
        public int UpdateRatingCheck(string AlbumDirectoryP, bool RatingCheckP, decimal AlbumRatingP, string AlbumRateCounterP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateRatingCheck @AlbumDirectory, @RatingCheck, @AlbumRating, @AlbumRateCounter", new { AlbumDirectory = AlbumDirectoryP, RatingCheck = RatingCheckP, AlbumRating = AlbumRatingP, AlbumRateCounter = AlbumRateCounterP});
                int AlbumID = GetAlbumID(AlbumDirectoryP);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;
            }
            return 0;
        }
        public void UpdateAlbumIndexCheckByAlbumID(int idAlbumP, bool AlbumIndexCheckP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateAlbumIndexCheckByAlbumID @idAlbum, @AlbumIndexCheck", new { idAlbum = idAlbumP, AlbumIndexCheck = AlbumIndexCheckP});
                
                GlobalChecker.TestSqlAlbumIdQuery += 1;                
            }
        }
        public void UpdateDirectoryGenreByAlbumID(int idAlbumP, string DirectoryGenreP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateByAlbumID @idAlbum, @DirectoryGenre", new { idAlbum = idAlbumP, DirectoryGenre = DirectoryGenreP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
            
        }
        public void UpdateAlbumValidationPointsByAlbumID(int idAlbumP, int ValidationPointsP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateValidationPointsByAlbumID @idAlbum, @ValidationPoints", new { idAlbum = idAlbumP, ValidationPoints = ValidationPointsP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public void UpdateAlbumGenreByAlbumID(int idAlbumP, string AlbumGenreP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateAlbumGenre @idAlbum, @AlbumGenre", new { idAlbum = idAlbumP, AlbumGenre = AlbumGenreP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public void UpdateAlbumDirectoryPathByAlbumID(int idAlbumP, string AlbumDirectoryP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.UpdateDirectoryPathByAlbumID @idAlbum, @AlbumDirectory", new { idAlbum = idAlbumP, AlbumDirectory = AlbumDirectoryP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public int UpdateAlbumReleaseYear(string AlbumDirectoryP, int AlbumReleaseYearP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateAlbumReleaseYear @AlbumDirectory, @AlbumReleaseYear", new { AlbumDirectory = AlbumDirectoryP, AlbumReleaseYear = AlbumReleaseYearP });
                int AlbumID = GetAlbumID(AlbumDirectoryP);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return AlbumID;
            }
            return 0;
        }
        public void UpdateAlbumProceedDate(int AlbumIdP, string ProceedDateP, bool ProceedP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateDateProceed @AlbumId, @ProceedDate, @Proceed", new { AlbumId = AlbumIdP, ProceedDate = ProceedDateP, Proceed = ProceedP });                
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }
        public void UpdateWriteIndex(int AlbumIdP, bool writeIndexP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_UpdateWriteIndexByAlbumID @AlbumId, @writeIndex", new { AlbumId = AlbumIdP, writeIndex = writeIndexP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }
        
        //[Delete Albums table]
        public void DeleteAlbumTableContents()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_DeleteAlbumTableContent");
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }
        public void DeleteAlbumTableContent_Parameter(string ArtistNameP)
        {
            if (ArtistNameP == "")
            {
                MessageBox.Show("Wpisz artyste do usuniecia!");
                return;
            }

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_DeleteAlbumTableContent_Parameter @ArtistName", new { ArtistName = ArtistNameP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }
        
        //[TRACKS TABLE]
            //[GET from Tracks table]
        public List<SQLTrackTable> GetAllTracks(int RecordCountP ,int selectAllP ,int sortyByP ,int rangeValidateMinP ,int rangeValidateMaxP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {

                var output = connection.Query<SQLTrackTable>("dbo.spTracks_GetAll @RecordCount, @selectAll, @sortyBy, @rangeValidateMin, @rangeValidateMax", 
                    new { RecordCount= RecordCountP, selectAll = selectAllP, sortyBy = sortyByP, rangeValidateMin= rangeValidateMinP, rangeValidateMax = rangeValidateMaxP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }

        public List<SQLTrackTable> GetTrackByAlbumId(int IdAlbumP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLTrackTable>("dbo.spTracks_GetByAlbumId @IdAlbum", new { IdAlbum = IdAlbumP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLTrackTable> GetTrackByTrackIndex(int IndexLibP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLTrackTable>("dbo.spTracks_GetByIndexLib @IndexLib", new { IndexLib = IndexLibP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLTrackTable> GetTrackByArtist(string TrackArtistP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLTrackTable>("dbo.spTracks_GetByArtist @TrackArtist", new { TrackArtist = TrackArtistP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        //[update Track table]
        public void UpdateTrackByIndexLib(int IdAlbumP, string TrackDirectoryP, string TrackGenreP,string TrackNameP,string TrackArtistP, int TrackRatingP,string FileExtensionP, int IndexLibP, string FileStatusP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {

                connection.Execute("dbo.spTracks_UpdateByIndexLib @IdAlbum, @TrackDirectory, @TrackGenre, @TrackName, @TrackArtist, @TrackRating, @FileExtension, @IndexLib, @FileStatus",
                    new { IdAlbum = IdAlbumP, TrackDirectory = TrackDirectoryP, TrackGenre = TrackGenreP, TrackName = TrackNameP, TrackArtist = TrackArtistP, TrackRating = TrackRatingP, IndexLib = IndexLibP, FileExtension = FileExtensionP, FileStatus = FileStatusP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public void UpdateTrackDirectoryPathByIndexLib(int IndexLibP, string AlbumDirectoryP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spTracks_UpdateTrackDirectoryPathByIndexLib @IndexLib, @AlbumDirectory", new { IndexLib = IndexLibP, AlbumDirectory = AlbumDirectoryP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public void UpdateTrackFileStatusByIndexLib (int IndexLibP, string FileStatusP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spTracks_UpdateTrackFileStatusByIndexLib @IndexLib, @FileStatus", new { IndexLib = IndexLibP, FileStatus = FileStatusP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }        
        public void UpdateTrackFileDateProceed(int IndexLibP, string DateProceedP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spTracks_UpdateTrackFileDateProceed @IndexLib, @DateProceed", new { IndexLib = IndexLibP, DateProceed = DateProceedP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public void UpdateTrackGenreByAlbumID(int idAlbumP, string TrackGenreP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spTracks_UpdateGenreByAlbumID @idAlbum, @TrackGenre", new { idAlbum = idAlbumP, TrackGenre = TrackGenreP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        public void UpdateTrackAlbumIndexByAlbumID(int idAlbumP, int IdAlbumIndexP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spTracks_UpdateAlbumIndexByAlbumID @idAlbum, @IdAlbumIndex", new { idAlbum = idAlbumP, IdAlbumIndex = IdAlbumIndexP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        //[Insert Track table]
        public void InsertTrack(int IdAlbumP, string TrackDirectoryP, string TrackGenreP, string TrackNameP, string TrackArtistP, int TrackRatingP, string TrackFileExtensionP, int IndexLibP, string FileStatusP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                List<SQLTrackTable> SqlTracks = new List<SQLTrackTable>();
                SqlTracks.Add(new SQLTrackTable { IdAlbum = IdAlbumP, TrackDirectory = TrackDirectoryP, TrackGenre = TrackGenreP, TrackName = TrackNameP, TrackArtist = TrackArtistP, TrackRating = TrackRatingP, FileExtension = TrackFileExtensionP, IndexLib = IndexLibP, FileStatus = FileStatusP }); // to idzie z pierwszej linijki wykomentowanej

                connection.Execute("dbo.spTracks_InsertTracks @IdAlbum, @TrackDirectory, @TrackGenre, @TrackName, @TrackArtist, @TrackRating, @FileExtension, @IndexLib, @FileStatus", SqlTracks);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }
        //[SERIES TABLE]
            //[GET from SeriesNO table]
        public List<SQLSeriesNoTable> GetSeriesLastIndex(int IdSeriesNoP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                var output = connection.Query<SQLSeriesNoTable>("dbo.spSeriesNo_GetLastTrackIndex @IdSeriesNo", new { IdSeriesNo = IdSeriesNoP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
            //[update SeriesNo table]
        public void UpdateIndexTrackInSeriesNo(int IdSeriesNoP, int IndexNoP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spSeriesNo_UpdateIndexNo @IdSeriesNo, @IndexNo", new { IdSeriesNo = IdSeriesNoP, IndexNo = IndexNoP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        // [ARTIST TABLE]
        //[GET from Albums table]
        public List<SQLArtistTable> GetAllByArtists(string ArtistNameP)
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {

                var output = connection.Query<SQLArtistTable>("dbo.spArtists_GetAllByArtist @ArtistName", new { ArtistName = ArtistNameP }).ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        public List<SQLArtistTable> GetAllArtists()
        {
            //throw new NotImplementedException();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {

                var output = connection.Query<SQLArtistTable>("dbo.spArtists_GetAll").ToList();
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return output;
            }
        }
        // [insert Artist table]
        public void InsertArtist(string ArtistNameP, string AlbumCountP, string ProceedToTotalP, decimal ArtistLibraryPercentP, decimal ProceedPercentP, int RatedSongsP, int GeneralRankingP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                List<SQLArtistTable> SqlArtist = new List<SQLArtistTable>();
                SqlArtist.Add(new SQLArtistTable { ArtistName = ArtistNameP, AlbumCount = AlbumCountP, ProceedToTotal = ProceedToTotalP, ArtistLibraryPercent = ArtistLibraryPercentP, ProceedPercent = ProceedPercentP, RatedSongs = RatedSongsP, GeneralRanking = GeneralRankingP });
                connection.Execute("dbo.spArtists_InsertArtist @ArtistName, @AlbumCount, @ProceedToTotal, @ArtistLibraryPercent, @ProceedPercent, @RatedSongs, @GeneralRanking", SqlArtist);
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }
        // [UPDATE Artist table]
        public void UpdateRatedAlbums(string ArtistNameP, string AlbumCountP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spArtists_UpdateRatedAlbums @ArtistName, @AlbumCount", new { ArtistName = ArtistNameP, AlbumCount = AlbumCountP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }

        }
        // [misc functions]
        public static int AutoSearchDatabaseAlbums(int IdAlbum, DataGridView DGV, int showAll, int AlbumCount, int PointsMin, int PointsMax, bool ShowProcedure)
        {
            List<SQLAlbumTable> AlbumList = new List<SQLAlbumTable>(); //sqlprzemy - table : Albums
            DBFunctions db = new DBFunctions();
            if (IdAlbum != 0)
                AlbumList = db.GetAlbumById(IdAlbum);
            else
                AlbumList = db.GetAllAlbums(AlbumCount, showAll, 2, PointsMin, PointsMax, ShowProcedure); //RecordCountP, selectAllP, sortByP, rangeValidateMinP, rangeValidateMaxP

            UpdateBindingAlbums(DGV, AlbumList);
            int countRecord = AlbumList.Count;
            return countRecord;
        }
        private static void UpdateBindingAlbums(DataGridView DGV, List<SQLAlbumTable> AlbumList)
        {
            DGV.DataSource = AlbumList;
        }
        public static int AutoSearchDatabaseArtists(string ArtistName, DataGridView DGV)
        {
            List<SQLArtistTable> ArtistList = new List<SQLArtistTable>(); //sqlprzemy - table : Albums
            DBFunctions db = new DBFunctions();
            if (ArtistName != "")
                ArtistList = db.GetAllByArtists(ArtistName);
            else
                ArtistList = db.GetAllArtists();

            UpdateBindingAlbums(DGV, ArtistList);
            int countRecord = ArtistList.Count;
            return countRecord;
        }
        private static void UpdateBindingAlbums(DataGridView DGV, List<SQLArtistTable> ArtistList)
        {
            DGV.DataSource = ArtistList;
        }                
        public static int AutoSearchDatabaseTracks(int IdTrack, DataGridView DGV, int AlbumCount, int RateMin, int RateMax)
        {
            List<SQLTrackTable> TrackList = new List<SQLTrackTable>(); //sqlprzemy - table : Albums
            DBFunctions db = new DBFunctions();
            if (IdTrack != 0)
                TrackList = db.GetTrackByTrackIndex(IdTrack);
            else
                TrackList = db.GetAllTracks(AlbumCount, 0, 1, RateMin, RateMax);

            UpdateBindingTracks(DGV, TrackList);
            int countRecord = TrackList.Count;
            return countRecord;
        }
        private static void UpdateBindingTracks(DataGridView DGV, List<SQLTrackTable> TrackList)
        {
            DGV.DataSource = TrackList;
        }
        public static void DeleteAlbumByAlbumID(int idAlbumP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                connection.Execute("dbo.spAlbums_DeleteAlbumTableByAlbumID @idAlbum", new { idAlbum = idAlbumP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
            }
        }

        public static int DeleteTracksByAlbumID(int IdAlbumP)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("MusicLibDB")))
            {
                DBFunctions db = new DBFunctions();
                List<SQLTrackTable> LTT = new List<SQLTrackTable>();
                LTT = db.GetTrackByAlbumId(IdAlbumP);
                connection.Execute("dbo.spTracks_DeleteTracksByAlbumID @IdAlbum", new { IdAlbum = IdAlbumP });
                GlobalChecker.TestSqlAlbumIdQuery += 1;
                return LTT.Count;
            }
            return 0;
        }

    }
    public class SQLAlbumTable
    {
        public int idAlbum { get; set; }
        public bool writeIndex { get; set; }

        public int AlbumReleaseYear { get; set; }

        public string AlbumDirectory { get; set; }
        public int idArtist { get; set; }
        public string AlbumArtist { get; set; }  
        public string AlbumName { get; set; }
        public string AlbumGenre { get; set; }        
        public string AlbumRateCounter { get; set; }
        public string DirectoryGenre { get; set; }
        public bool Proceed { get; set; }
        public string DateProceed { get; set; }

        public int ValidationPoints { get; set; }
        public decimal AlbumRating { get; set; }
        public bool ArtistCheck { get; set; }
        public bool AlbumCheck { get; set; }
        public bool GenreCheck { get; set; }
        public bool RatingCheck { get; set; }
        public bool IndexCheck { get; set; }
        public bool AlbumIndexCheck { get; set; }

        public string FullInfoAlbums
        {
            get
            {
                return $"{idAlbum} {writeIndex} {AlbumName}{AlbumDirectory} {ValidationPoints} {AlbumArtist} {idArtist} {AlbumReleaseYear} {AlbumGenre} {AlbumRateCounter} {AlbumRating} {DirectoryGenre} {DateProceed} " +
                    $"{ArtistCheck}{AlbumCheck} {GenreCheck} {RatingCheck}{IndexCheck} {AlbumIndexCheck} {Proceed}"; // [pobiera sie tu z sql'a - nazwy musza byc takie same jak w sql tabeli inaczej nic nie pokaze
            }

        }

    }
    public class SQLTrackTable
    {
        public int IdAlbum { get; set; }
        public string TrackDirectory { get; set; }
        public string TrackGenre { get; set; }
        public string TrackName { get; set; }
        public string TrackArtist { get; set; }
        public string FileExtension { get; set; }
        public string FileStatus { get; set; }
        public string DateProceed { get; set; }

        public int TrackRating { get; set; }
        public int IndexLib { get; set; }
        public int IdAlbumIndex { get; set; }


        public string FullInfoAlbums
        {
            get
            {
                return $"{IdAlbum} {TrackDirectory} ({TrackGenre})  {TrackName} {TrackArtist} {TrackRating} {FileExtension} {IndexLib} {FileStatus} {DateProceed} {IdAlbumIndex}"; // [pobiera sie tu z sql'a - nazwy musza byc takie same jak w sql tabeli inaczej nic nie pokaze
            }

        }
    }
    public class SQLSeriesNoTable
    {
        public int IdSeriesNo { get; set; }
        public string SeriesDescription { get; set; }
        public int IndexNo { get; set; }
  
        public string FullInfoAlbums
        {
            get
            {
                return $"{IdSeriesNo} {SeriesDescription} {IndexNo}"; // [pobiera sie tu z sql'a - nazwy musza byc takie same jak w sql tabeli inaczej nic nie pokaze
            }

        }
    }
    public class SQLArtistTable
    {
        public int IdArtist { get; set; }
        public int RatedSongs { get; set; }
        public int GeneralRanking { get; set; }

        public string ArtistName { get; set; }
        public string AlbumCount { get; set; }
        public string ProceedToTotal { get; set; }

        public decimal ArtistLibraryPercent { get; set; }
        public decimal ProceedPercent { get; set; }
        public string FullInfoArtists
        {
            get
            {
                return $"{IdArtist} {RatedSongs} {GeneralRanking} {ArtistName} {ArtistName} {AlbumCount} {ProceedToTotal}{ArtistLibraryPercent}{ProceedPercent}"; // [pobiera sie tu z sql'a - nazwy musza byc takie same jak w sql tabeli inaczej nic nie pokaze
            }

        }
    }

}
