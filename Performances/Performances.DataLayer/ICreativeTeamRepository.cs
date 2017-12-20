using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performances.Model;

namespace Performances.DataLayer
{
    public interface ICreativeTeamRepository
    {
        CreativeTeam CreateCreativeTeam(CreativeTeam creativeTeam, byte[] photo, string filename);
        void DeleteCreativeTeam(CreativeTeam creativeTeam);
        void DeleteCreativeTeam(Guid creativeTeamId);
        CreativeTeam UpdateCreativeTeam(CreativeTeam newCreativeTeam);
        CreativeTeam GetCreativeTeamById(Guid creativeTeamId);
    }
}
