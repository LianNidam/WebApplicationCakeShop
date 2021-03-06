using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationCakeShop.Models
{
    public class Cart
    { 
            [Key]
            public int Id { get; set; }

            public int UserId { get; set; }

            public User User { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public double TotalPrice { get; set; }

            public List<Cake> Cakes { get; set; }

            internal static object Index()
            {
                throw new NotImplementedException();
            }

        
    }
}
