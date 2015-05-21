using System;

namespace PIFitness.GPX
{
    /// <summary>
    /// Holds simplified version of track point (time, position)
    /// </summary>
    public struct TrackPoint
    {
        private DateTime m_time;
        private double m_longitude;
        private double m_latitude;
        private double m_elevation;

        #region Properties

        public DateTime Time
        {
            get { return m_time; }
            private set { m_time = value; }
        }


        public double Longitude
        {
            get { return m_longitude; }
            private set { m_longitude = value; }
        }


        public double Latitude
        {
            get { return m_latitude; }
            private set { m_latitude = value; }
        }


        public double Elevation
        {
            get { return m_elevation; }
            private set { m_elevation = value; }
        }

        #endregion Properties


        public TrackPoint(DateTime time, double longitude, double latitude, double elevation)
        {
            m_time = time;
            m_longitude = longitude;
            m_latitude = latitude;
            m_elevation = elevation;
        }

    }
}
