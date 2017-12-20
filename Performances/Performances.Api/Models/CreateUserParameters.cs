using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Performances.Model;

namespace Performances.Api.Models
{
    public class CreateUserParameters
    {
        public User user;
        public File photo;
    }
}