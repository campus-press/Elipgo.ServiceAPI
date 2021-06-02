using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Elipgo.DAO.Models
{
    public class PermissionDAO
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsDAO> RoleClaims { get; set; }
    }

    public class RoleClaimsDAO
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
