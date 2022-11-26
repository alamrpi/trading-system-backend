using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartshop.Entities.ProductManagement
{
    public class Category
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual Group? Group { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        public Category(string name)
        {
            Name = name;
        }
        public Category(string name, int groupId) : this(name)
        {
            GroupId = groupId;
        }
    }
}
