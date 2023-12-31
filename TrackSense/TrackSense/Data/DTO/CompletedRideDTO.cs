﻿using TrackSense.Entities;

namespace TrackSense.Data.DTO;

public class CompletedRideDTO
{
    public Guid CompletedRideId { get; set; }
    public Guid PlannedRideId { get; set; }
    public List<CompletedRidePointDTO> CompletedRidePoints { get; set; }
    public CompletedRideStatisticsDTO Statistics { get; set; }

    public CompletedRideDTO()
    {
        ;
    }

    public CompletedRideDTO(CompletedRide entite)
    {
        if (entite is null)
        {
            throw new ArgumentNullException(nameof(entite));
        }

        this.CompletedRideId = entite.CompletedRideId;
        this.PlannedRideId = entite.PlannedRideId;
        this.CompletedRidePoints = entite.CompletedRidePoints.Select(entite => new CompletedRidePointDTO(entite)).ToList();
        this.Statistics = new CompletedRideStatisticsDTO(entite.Statistics);
    }

    public CompletedRide ToEntity()
    {
        List<CompletedRidePoint> listPoints = this.CompletedRidePoints.Select(pointDTO
            => pointDTO.ToEntity()).ToList();

        return new CompletedRide()
        {
            CompletedRideId = this.CompletedRideId,
            PlannedRideId = this.PlannedRideId,
            CompletedRidePoints = listPoints,
            Statistics = this.Statistics.ToEntity()
        };
    }

}
