using System;
using System.Collections.Generic;
using System.Text;
using Examen.Elipgo.DAL.Entity;

namespace Examen.Elipgo.DAL.Models
{
    public class Store : EntityBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
