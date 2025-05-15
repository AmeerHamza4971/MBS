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
        var amount = await _db.Billings.ToListAsync();
        ViewBag.SepentAmount = amount.Where(x => x.Status == "Paid").Sum(x => x.SepentAmount);
        ViewBag.RemaingAmount = amount.Where(x => x.Status != "Pending").Sum(x => x.RemaingAmount);
        ViewBag.CurrentMonthAmount = amount
            .Where(x => x.FromDate.Month == DateTime.Now.Month && x.FromDate.Year == DateTime.Now.Year)
            .Sum(x => x.SepentAmount + x.RemaingAmount);

        ViewBag.TotalAmount = ViewBag.SepentAmount + ViewBag.RemaingAmount;
        return View();
    }

}
