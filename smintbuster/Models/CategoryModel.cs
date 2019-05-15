using smintbuster.Modals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smintbuster.Modals
{
    public class CategoryModel
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryImage { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public ICollection<ProductModel> Products { get; set; }
    }
}
