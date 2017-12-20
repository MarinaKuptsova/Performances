using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Performances.Client.View;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class RegistrationViewModel
    {
        public User RegisterUser { get; set; }
        public CreativeTeam RegisterCreativeTeam { get; set; }
        
        public UserRegistrationView uRegistrationView { get; set; } 
        public CreativeTeamRegistrationView ctRegistrationView { get; set; }
        

        public RegistrationViewModel()
        {
            
        }


    }
}
