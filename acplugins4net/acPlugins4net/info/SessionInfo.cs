﻿using acPlugins4net.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace acPlugins4net.info
{
    [DataContract]
    public class SessionInfo
    {
        public SessionInfo()
        {
            this.MissedSessionStart = true;
            this.SessionName = "Unknown";
            this.SessionType = 0;
            this.Timestamp = DateTime.UtcNow.Ticks;
            this.Weather = "Unknown";
            this.Drivers = new List<DriverInfo>();
            this.Laps = new List<LapInfo>();
            this.Incidents = new List<IncidentInfo>();
            this.MaxClients = 32;
        }

        [DataMember]
        public bool MissedSessionStart { get; set; }
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public string TrackName { get; set; }
        [DataMember]
        public string TrackConfig { get; set; }
        [DataMember]
        public string SessionName { get; set; }
        [DataMember]
        public byte SessionType { get; set; }
        [DataMember]
        public ushort SessionDuration { get; set; }
        [DataMember]
        public ushort LapCount { get; set; }
        [DataMember]
        public ushort WaitTime { get; set; }
        [DataMember]
        public long Timestamp { get; set; }
        [DataMember]
        public byte AmbientTemp { get; set; }
        [DataMember]
        public byte RoadTemp { get; set; }
        [DataMember]
        public string Weather { get; set; }
        [DataMember]
        public int MaxClients { get; set; }
        [DataMember]
        public int RealtimeUpdateInterval { get; set; }
        [DataMember]
        public List<DriverInfo> Drivers { get; set; }
        [DataMember]
        public List<LapInfo> Laps { get; set; }
        [DataMember]
        public List<IncidentInfo> Incidents { get; set; }


        public long GetLastLapTimestamp(DriverInfo driver)
        {
            LapInfo lapReport = this.Laps.FirstOrDefault(l => l.ConnectionId == driver.ConnectionId && l.LapNo == driver.LapCount);
            if (lapReport != null)
            {
                return lapReport.Timestamp;
            }
            return long.MaxValue;
        }

        /// <summary>
        /// Computes the distance to the closest opponent.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="opponent">The closest opponent.</param>
        /// <returns>The distance in meters.</returns>
        public float GetDistanceToClosestOpponent(DriverInfo driver, out DriverInfo opponent)
        {
            opponent = this.Drivers.Where(d => d != driver
                            && Math.Abs(d.LastPositionUpdate - driver.LastPositionUpdate) < 2 * RealtimeUpdateInterval)
                            .OrderBy(d => (d.LastPosition - driver.LastPosition).Length()).FirstOrDefault();
            if (opponent != null)
            {
                return (opponent.LastPosition - driver.LastPosition).Length();
            }
            else
            {
                return float.MaxValue;
            }
        }
    }
}