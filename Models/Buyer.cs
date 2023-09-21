using System.ComponentModel.DataAnnotations;

namespace Many_To_Many_RawSqL.Models
{
    public class Buyer
    {
        [Key]
        public int BuyerId { get; set; }
        public required string Name { get; set; }

        public List<Products>? Product { get; set; }
    }
}
