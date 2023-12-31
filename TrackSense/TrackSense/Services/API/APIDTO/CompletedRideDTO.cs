﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using TrackSense.Entities;

namespace TrackSense.Services.API.APIDTO;

public class CompletedRideDTO
{
    public string UserLogin { get; set; }
    public string CompletedRideId { get; set; }
    [JsonIgnore]
    public PlannedRideDTO PlannedRide {get;set ;}
    public  List<CompletedRidePointDTO> CompletedRidePoints { get; set; }
    public CompletedRideStatisticsDTO Statistics { get; set; }
    public CompletedRideDTO()
    {
        ;
    }

    public CompletedRideDTO(Entities.CompletedRide p_completedRide)
    {
        if (p_completedRide == null)
        {
            throw new NullReferenceException(nameof(p_completedRide));
        }
        if (p_completedRide.UserLogin == null)
        {
            throw new NullReferenceException(nameof(p_completedRide.UserLogin));
        }
        if (p_completedRide.CompletedRideId == Guid.Empty)
        {
            throw new InvalidOperationException("Id du CompletedRide ne doit pas être null ni vide");
        }

        this.UserLogin = p_completedRide.UserLogin;
        this.CompletedRideId = p_completedRide.CompletedRideId.ToString();
       
        this.CompletedRidePoints = p_completedRide.CompletedRidePoints.Select(entite => 
                                                                                new CompletedRidePointDTO(entite) 
                                                                                { 
                                                                                    CompletedRideId =this.CompletedRideId
                                                                                })
                                                                       .ToList();
    }

    public Entities.CompletedRide ToEntity()
    {
        return new CompletedRide()
        {
            CompletedRideId = new Guid(this.CompletedRideId!),
            CompletedRidePoints = this.CompletedRidePoints.Select(p => p.ToEntity()).ToList(),
            UserLogin = this.UserLogin,
            Statistics = this.Statistics?.ToEntity(),
        };
    }

}
