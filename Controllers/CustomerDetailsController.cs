
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelProject.Models;
using HotelProject.Data;

public class CustomerDetailsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerDetailsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CUSTOMERDETAILSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.CustomerDetails.ToListAsync());
    }

    // GET: CUSTOMERDETAILSS/Details/5
    public async Task<IActionResult> Details(int? customerid)
    {
        if (customerid == null)
        {
            return NotFound();
        }

        var customerdetails = await _context.CustomerDetails
            .FirstOrDefaultAsync(m => m.CustomerID == customerid);
        if (customerdetails == null)
        {
            return NotFound();
        }

        return View(customerdetails);
    }

    // GET: CUSTOMERDETAILSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CUSTOMERDETAILSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CustomerID,FirstName,LastName,Email,PhoneNumber,Address1,Address2,GST,City,State,PinCode,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] CustomerDetails customerdetails)
    {
        if (ModelState.IsValid)
        {
            _context.Add(customerdetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(customerdetails);
    }

    // GET: CUSTOMERDETAILSS/Edit/5
    public async Task<IActionResult> Edit(int? customerid)
    {
        if (customerid == null)
        {
            return NotFound();
        }

        var customerdetails = await _context.CustomerDetails.FindAsync(customerid);
        if (customerdetails == null)
        {
            return NotFound();
        }
        return View(customerdetails);
    }

    // POST: CUSTOMERDETAILSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? customerid, [Bind("CustomerID,FirstName,LastName,Email,PhoneNumber,Address1,Address2,GST,City,State,PinCode,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] CustomerDetails customerdetails)
    {
        if (customerid != customerdetails.CustomerID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(customerdetails);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerDetailsExists(customerdetails.CustomerID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(customerdetails);
    }

    // GET: CUSTOMERDETAILSS/Delete/5
    public async Task<IActionResult> Delete(int? customerid)
    {
        if (customerid == null)
        {
            return NotFound();
        }

        var customerdetails = await _context.CustomerDetails
            .FirstOrDefaultAsync(m => m.CustomerID == customerid);
        if (customerdetails == null)
        {
            return NotFound();
        }

        return View(customerdetails);
    }

    // POST: CUSTOMERDETAILSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? customerid)
    {
        var customerdetails = await _context.CustomerDetails.FindAsync(customerid);
        if (customerdetails != null)
        {
            _context.CustomerDetails.Remove(customerdetails);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CustomerDetailsExists(int? customerid)
    {
        return _context.CustomerDetails.Any(e => e.CustomerID == customerid);
    }
}
