using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class CategoryCollection : IModelCollection<Category>
    {
        public List<Category> Categories { set; get; }
        public int Count => Categories.Count;
        public IEnumerable<Category> Data => Categories;
    }
}
