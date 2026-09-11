namespace JPAzureFunctions.Models;

public class Bill
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string EbNumber { get; set; } = string.Empty;
    public int UnitsConsumed { get; set; }
    public decimal BillAmount { get; set; }

    public string BillStatus { get; set; } = "Pending";
    public DateTime BillDate { get; set; }

    public User User { get; set; } = null!;
}