using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using dotnetcore_formatterdemo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace dotnetcore_formatterdemo.Formatters
{
    public class IcsOutputFormatter : TextOutputFormatter
    {
        public IcsOutputFormatter()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-ical"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/ical"));
        }

        protected override bool CanWriteType(Type type)
        {
            // this should only be available for appointments
            if (typeof(Appointment).IsAssignableFrom(type) || typeof(IEnumerable<Appointment>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<IcsOutputFormatter>)) as ILogger;

            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<Appointment>)
            {
                foreach (Appointment appointment in context.Object as IEnumerable<Appointment>)
                {
                    FormatIcs(buffer, appointment, logger);
                }
            }
            else
            {
                var appointment = context.Object as Appointment;
                FormatIcs(buffer, appointment, logger);
            }
            return response.WriteAsync(buffer.ToString());
        }

        private void FormatIcs(StringBuilder buffer, Appointment appointment, ILogger logger)
        {
            buffer.AppendLine("BEGIN:VCALENDAR");
            buffer.AppendLine("VERSION:2.0");
            buffer.AppendLine("PRODID:https://github.com/norberteder/dotnetcore_formatterdemo");
            buffer.AppendLine("METHOD:PUBLISH");
            buffer.AppendLine("BEGIN:VEVENT");
            buffer.AppendLine("UID:--");
            buffer.AppendLine($"ORGANIZER;CN=\"{appointment.Organizer.FirstName} {appointment.Organizer.LastName}, {appointment.Organizer.Organization}\":MAILTO:{appointment.Organizer.Email}");
            buffer.AppendLine($"LOCATION:{appointment.Location}");
            buffer.AppendLine($"SUMMARY:{appointment.Title}");
            buffer.AppendLine($"DESCRIPTION:{appointment.Description}");
            buffer.AppendLine("CLASS:PUBLIC");
            buffer.AppendLine($"DTSTART:{appointment.Start:yyyyMMddTHHmmzzzZ}");
            buffer.AppendLine($"DTEND:{appointment.End:yyyyMMddTHHmmzzzZ}");
            buffer.AppendLine($"DTSTAMP:{DateTime.Now:yyyyMMddTHHmmzzzZ}");
            buffer.AppendLine("END:VEVENT");
            buffer.AppendLine("END:VCALENDAR");
        }
    }
}