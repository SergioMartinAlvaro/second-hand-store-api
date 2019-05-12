using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smintbuster.Modals
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDetails { get; set; }

        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
    }
}
