﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BotEventManagement.Services.Model.Database
{
    public class Speaker
    {

        public string SpeakerId { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string UploadedPhoto { get; set; }
        [ForeignKey("EventId")]
        public string EventId { get; set; }
        public virtual Event Event { get; set; }
        public virtual List<Activity> Activity { get; set; }
    }
}
