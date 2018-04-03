using System;
using dotnetcore_formatterdemo.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcore_formatterdemo.Controllers
{
    [Route("api/1/appointment"), EnableCors("AllowAllOrigins")]
    public class AppointmentController : Controller
    {
        [FormatFilter]
        [HttpGet("{id}"), HttpGet("{id}.{format}")]
        public Appointment Get(int id)
        {
            var organizer = new User()
            {
                FirstName = "Norbert",
                LastName = "Eder",
                Organization = "NE",
                Email = "thisismyemail@herewego"
            };
            return new Appointment()
            {
                IsPublic = true,
                Organizer = organizer,
                Description = "Wall of text",
                Title = "So important",
                Location = "Vienna",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2)
            };
        }
    }
}