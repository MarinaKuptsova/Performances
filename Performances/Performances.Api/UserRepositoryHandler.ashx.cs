using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JsonServices;
using JsonServices.Web;
using Perfomances.DataLayer;
using Performances.DataLayer.PostgreSQL;

namespace Performances.Api
{
    /// <summary>
    /// Сводное описание для UserRepositoryHandler
    /// </summary>
    public class UserRepositoryHandler : JsonHandler
    {
        public UserRepositoryHandler()
        {
            this.service.Name = "Performances.Api";
            this.service.Description = "";
            InterfaceConfiguration IConfig = new InterfaceConfiguration("UserAPI", typeof(IUserRepository), typeof(UserRepository));
            this.service.Interfaces.Add(IConfig);
        }
    }
}