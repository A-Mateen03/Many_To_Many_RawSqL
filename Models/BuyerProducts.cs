using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Many_To_Many_RawSqL.Models
{
    public class BuyerProducts
    {
        [Key]
        public int BuyerProductId { get; set; }

        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }

        [ForeignKey("Products")]
        public int ProductP_ID { get; set; }

        public Buyer? Buyer { get; set; }

        public Products? Products { get; set; }
    }
}
