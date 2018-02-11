using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectLibrary_1
{

    public class mgt_XML
    {
        private string _pointsMin;
        private string _pointsMax;
        private string _rateMin;
        private string _rateMax;
        private bool _showproceed;
        private int _albumCount;
        private int _trackCount;

        public string PointsMin
        {
            get { return _pointsMin; }
            set { _pointsMin = value; }
        }
        public string PointsMax
        {
            get { return _pointsMax; }
            set { _pointsMax = value; }
        }
        public string RateMin
        {
            get { return _rateMin; }
            set { _rateMin = value; }
        }
        public string RateMax
        {
            get { return _rateMax; }
            set { _rateMax = value; }
        }
        public bool ShowProceed
        {
            get { return _showproceed; }
            set { _showproceed = value; }
        }
        public int AlbumCount
        {
            get { return _albumCount; }
            set { _albumCount = value; }
        }
        public int TrackCount
        {
            get { return _trackCount; }
            set { _trackCount = value; }
        }
    }
}
