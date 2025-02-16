namespace MBS.Models;

public class Billing
{
    public long Id { get; set; }
    public string SpentName { get; set; } = default!;
    public decimal SepentAmount { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Reason { get; set; } = default!;
    public string Remaing { get; set; } = default!;
    public string Status {  get; set; } = default!;  
}
