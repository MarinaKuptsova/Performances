using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Performances.Model;

namespace Performances.Api.Models
{
    public class CreateCreativeTeamParameters
    {
        public CreativeTeam creativeTeam;
        public byte[] photo;
        public string filename;
    }
}