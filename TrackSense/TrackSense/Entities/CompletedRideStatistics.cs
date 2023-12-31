﻿//using Xamarin.Google.Crypto.Tink.Proto;

namespace TrackSense.Entities
{
    public class CompletedRideStatistics
    {
        public Guid CompletedRideId { get; set; } 
        public double AverageSpeed { get; set; }
        public double MaximumSpeed { get; set; }
        public double Distance { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfPoints { get; set; }
        public int Calories { get; set; }
        public int Falls { get; set; }

        public CompletedRideStatistics()
        {
            
        }
    }
}