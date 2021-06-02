using System;
using System.Collections.Generic;
using System.Text;
using Examen.Elipgo.DAL.Entity;

namespace Examen.Elipgo.DAL.Models
{
    public class Article : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int TotalInShelf { get; set; }
        public int TotalInVault { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
