using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string? Title {get; set;}
        public TimeSpan? ScheduleTime { get; set; }
        public int Nop {get; set;}
        public int DocId {get; set;}
    }
}
