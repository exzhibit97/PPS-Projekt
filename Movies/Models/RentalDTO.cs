using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Web.Models
{
    public class RentalDTO
    {        
        public decimal Price { get; set; }
        public string UserID { get; set; }
        public string MovieID { get; set; }
        public int RentLength { get; set; }
    }
}
