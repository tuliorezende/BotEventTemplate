﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotEventManagement.Models.Database
{
    public class Activity
    {
        [Key]
        public string ActivityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("EventId")]
        public string EventId { get; set; }
        public virtual Event Event { get; set; }

        [ForeignKey("SpeakerId")]
        public string SpeakerId { get; set; }
        public virtual Speaker Speaker { get; set; }
        [ForeignKey("Stageid")]
        public string StageId { get; set; }
        public virtual Stage Stage { get; set; }

        public virtual List<GuestUserTalks> UserTalks { get; set; }
    }
}
