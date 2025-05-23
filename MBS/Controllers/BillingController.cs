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
        // Remove the Id validation error if it's the only issue
        if (!ModelState.IsValid && ModelState.ErrorCount == 1 && ModelState.ContainsKey("Id"))
        {
            ModelState.Clear();
            dto.Id = 0; // Set Id to 0 for new records
        }

        if (ModelState.IsValid)
        {
            if (dto.Id == 0)
            {
                // New record
                dto.CreatedDate = DateTime.Now;
                await _context.AddAsync(dto);
            }
            else
            {
                // Existing record
                var existingBilling = await _context.Billings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == dto.Id);
                if (existingBilling == null)
                {
                    return NotFound();
                }

                // Update the existing billing
                dto.CreatedDate = existingBilling.CreatedDate; // Keep the original created date
                dto.UpdatedDate = DateTime.Now; // Keep the original created date
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

    [HttpPost]
    public async Task<IActionResult> DeleteBill(long id)
    {
        var billing = await _context.Billings.FindAsync(id);
        if (billing == null)
        {
            return NotFound();
        }

        _context.Billings.Remove(billing);
        await _context.SaveChangesAsync();
        return Ok(); // Return a success response
    }


}
