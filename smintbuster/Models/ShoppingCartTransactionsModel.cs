using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smintbuster.Models
{
    public class ShoppingCartTransactionsModel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
