using MBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MBS.Controllers;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext context)
    {
        _db=context;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _db.Billings.ToListAsync();

        // Current Unpaid
        ViewBag.CurrentUnpaidSpent = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Unpaid")
            .Sum(x => x.SpentAmount);
        ViewBag.CurrentUnpaidRemaining = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Unpaid")
            .Sum(x => x.RemaingAmount);

        // Current Paid
        ViewBag.CurrentPaidSpent = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Paid")
            .Sum(x => x.SpentAmount);
        ViewBag.CurrentPaidRemaining = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Paid")
            .Sum(x => x.RemaingAmount);


        // Pending
        ViewBag.PendingSpent = list.Where(x => x.Status == "Pending").Sum(x => x.SpentAmount);
        ViewBag.PendingRemaing = list.Where(x => x.Status == "Pending").Sum(x => x.RemaingAmount);

        // Unpaid
        ViewBag.UnpaidSpent = list.Where(x => x.Status == "Unpaid").Sum(x => x.SpentAmount);
        ViewBag.UnpaidRemaining = list.Where(x => x.Status == "Unpaid").Sum(x => x.RemaingAmount);
       
        // Paid
        ViewBag.PaidSpent = list.Where(x => x.Status == "Paid").Sum(x => x.SpentAmount);
        ViewBag.PaidRemaining = list.Where(x => x.Status == "Paid").Sum(x => x.RemaingAmount);

        // InProgress
        ViewBag.InProgressSpent = list.Where(x => x.Status == "InProgress").Sum(x => x.SpentAmount);
        ViewBag.InProgressRemaining = list.Where(x => x.Status == "InProgress").Sum(x => x.RemaingAmount);


        // Current Received
        ViewBag.CurrentReceivedAmountToPay = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Received")
            .Sum(x => x.SpentAmount);
        ViewBag.CurrentReceivedRemaining = ViewBag.CurrentReceivedAmountToPay - ViewBag.CurrentPaidSpent;


        // Received
        ViewBag.ReceivedAmountToPay = list.Where(x => x.Status == "Received").Sum(x => x.SpentAmount);
        ViewBag.ReceivedRemaining = ViewBag.ReceivedAmountToPay - ViewBag.PaidSpent;

        return View(list.Where(x => x.CreatedDate > DateTime.Today.AddDays(-7)));
    }

}
