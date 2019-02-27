using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class CategoryCollection : ModelCollection<Category>
    {
        public List<Category> Categories { set; get; }
        public override IEnumerable<Category> Data => Categories;
    }
}
