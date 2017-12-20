using Performances.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Performances.Api.Models
{
    public class CreateEventParameters
    {
        public Event newEvent;
        public byte[] photo;
        public string filename;
        public Guid creativeTeamId;
    }
}