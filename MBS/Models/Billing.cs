namespace MBS.Models;

public class Billing
{
    public long Id { get; set; }
    public string SpentName { get; set; } = default!;
    public decimal SpentAmount { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Reason { get; set; } = default!;
    public decimal RemaingAmount { get; set; } = default!;
    public string Status {  get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
