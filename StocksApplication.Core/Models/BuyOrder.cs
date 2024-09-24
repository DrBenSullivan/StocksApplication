using System.ComponentModel.DataAnnotations;

namespace StocksApplication.Core.Models
{
    public class BuyOrder : Order
    {
        [Key]
        public Guid BuyOrderID { get; set; } = Guid.NewGuid();
        [Required]
        public override string TradeType => "BuyOrder";
    }
}
