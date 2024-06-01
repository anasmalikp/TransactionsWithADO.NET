namespace Transactions.Models
{
    public class TransferFundDto
    {
        public int? senderId { get; set; }
        public int? receiverId { get; set; }
        public int? amount { get; set; }
    }
}
