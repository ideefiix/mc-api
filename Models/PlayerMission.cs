﻿using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class PlayerMission
{
    [Key]
    public Guid PlayerMissionId { get; set; }
    public virtual Mission Mission { get; set; }
    public virtual Player Player { get; set; }
    public virtual ICollection<Item> FoundItems { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public int HpThreshold { get; set; } // TODO
    public virtual Event CompletionEvent { get; set; }
    public virtual Event ItemSpawnEvent { get; set; }

}