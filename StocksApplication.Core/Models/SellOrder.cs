using System.ComponentModel.DataAnnotations;

namespace StocksApplication.Core.Models
{
    public class SellOrder : Order
    {
        [Key]
        public Guid SellOrderID { get; set; } = Guid.NewGuid();
        [Required]
        public override string TradeType => "SellOrder";
    }
}
