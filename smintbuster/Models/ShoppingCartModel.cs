using smintbuster.Modals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smintbuster.Models
{
    public class ShoppingCartModel
    {
        [Key]
        public int ShoppingCartId { get; set; }
        public bool CartStatus { get; set; }    
        public string User { get; set; }
        // public ICollection<ProductModel> products { get; set; }
    }
}
