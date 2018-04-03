using System;

namespace dotnetcore_formatterdemo.Data
{
    public class Appointment
    {
        public User Organizer { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}