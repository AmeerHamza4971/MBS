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

        ViewBag.CurrentUnpaid = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Unpaid")
            .Sum(x => x.RemaingAmount);
        ViewBag.CurrentPaid = list
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year && x.Status == "Paid")
            .Sum(x => x.SpentAmount);
        

        ViewBag.TotalPending = list.Where(x => x.Status == "Pending").Sum(x => x.RemaingAmount);
        ViewBag.TotalUnpaid = list.Where(x => x.Status == "Unpaid").Sum(x => x.RemaingAmount);
        ViewBag.TotalPaid = list.Where(x => x.Status == "Paid").Sum(x => x.SpentAmount);


        ViewBag.TotalAmount = ViewBag.TotalPaid + ViewBag.TotalUnpaid;
        return View(list.Where(x => x.CreatedDate > DateTime.Today.AddDays(-7)));
    }

}
