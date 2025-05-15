using MBS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var res = await _context.Billings.ToListAsync();
        return View(res);
    }

    public async Task<IActionResult> AddNewBill(long? id)
    {
        // If id is provided, it's an edit operation
        if (id.HasValue)
        {
            var billing = await _context.Billings.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }
            ViewBag.IsEdit = true;
            ViewBag.Title = "Edit Bill";
            return View(billing);
        }

        // Otherwise it's an add operation
        ViewBag.IsEdit = false;
        ViewBag.Title = "Add New Bill";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNewBill(Billing dto)
    {
        if (ModelState.IsValid)
        {
            // If ID is 0, it's a new record, otherwise update existing
            if (dto.Id == 0)
            {
                await _context.AddAsync(dto);
            }
            else
            {
                _context.Update(dto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("BillingList");
        }

        // Set ViewBag based on whether it's an edit or add
        ViewBag.IsEdit = dto.Id != 0;
        ViewBag.Title = dto.Id != 0 ? "Edit Bill" : "Add New Bill";
        return View(dto);
    }

    // Add this method to your BillingController class
    public async Task<IActionResult> DeleteBill(long id)
    {
        var billing = await _context.Billings.FindAsync(id);
        if (billing == null)
        {
            return NotFound();
        }

        _context.Billings.Remove(billing);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(BillingList));
    }

}
