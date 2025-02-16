using MBS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MBS.Controllers;

public class BillingController : Controller
{
    private readonly ApplicationDbContext _context;

    public BillingController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> BillingList()
    {
        var res = _context.Billings.ToList();
        return View(res);
    }

    public IActionResult AddNewBill()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddNewBill(Billing dto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(dto);
            await _context.SaveChangesAsync();
            return RedirectToAction("BillingList");
        }
        return View(dto);
    }
}
