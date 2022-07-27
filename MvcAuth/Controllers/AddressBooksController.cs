using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcAuth.Models;

namespace MvcAuth.Controllers
{
    public class AddressBooksController : Controller
    {
        private readonly aspnetMvcAuth2B182E742B6748EE95E062A11832A6A1Context _context;

        public AddressBooksController(aspnetMvcAuth2B182E742B6748EE95E062A11832A6A1Context context)
        {
            _context = context;
        }

        // GET: AddressBooks ========================================================================
        //[HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string? SearchPhrase)
        {

            var context = new HttpContextAccessor();
            var principal = context.HttpContext.User;
            if (principal == null) { return NotFound(); }
            var userid =principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var contact = _context.AddressBooks.Include(a => a.IdNavigation).Where(a => a.Id == userid);

            if (!string.IsNullOrEmpty(SearchPhrase))
            {
                contact = contact.Where(p => p.Fname.ToLower().Contains(SearchPhrase.ToLower()));
            }

            return View(await contact.ToListAsync());
            //return View(await _context.AddressBooks.Where(a => a.Id == userid).ToListAsync());
            // return Ok(All);
        }
        //[Authorize]
        //public async Task<IActionResult> IndexSearch(string SearchPhrase)
        //{
        //    var context = new HttpContextAccessor();
        //    var principal = context.HttpContext.User;
        //    if (principal == null) { return NotFound(); }
        //    var userid = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var contact = _context.AddressBooks.Include(a => a.IdNavigation).Where(a => a.Id == userid);

        //    if (!string.IsNullOrEmpty(SearchPhrase))
        //    {
        //        contact = contact.Where(p => p.Fname.ToLower().Contains(SearchPhrase.ToLower()));
        //    }

        //    return View(await contact.ToListAsync());

        //}

        // GET: AddressBooks/Details/5 =================================================================
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AddressBooks == null)
            {
                return NotFound();
            }

            var addressBook = await _context.AddressBooks
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync(m => m.AddBookId == id);
            if (addressBook == null)
            {
                return NotFound();
            }

            return View(addressBook);
        }

        // GET: AddressBooks/Create ======================================================================
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Id"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        // POST: AddressBooks/Create ==================================================================
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("AddBookId,Fname,Lname,Address,Nickname,PhoneNo,FaxNo,Email,Notes,Id")] AddressBook addressBook)
        {
            ModelState.Remove("IdNavigation");
            if (ModelState.IsValid)
            {
                _context.Add(addressBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", addressBook.Id);
            TempData["success"] = "Contact added successfully !!";
            return View(addressBook);
        }

        // GET: AddressBooks/Edit/5 =======================================================================
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AddressBooks == null)
            {
                return NotFound();
            }

            var addressBook = await _context.AddressBooks.FindAsync(id);
            if (addressBook == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", addressBook.Id);
            return View(addressBook);
        }

        // POST: AddressBooks/Edit/5 ========================================================================
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("AddBookId,Fname,Lname,Address,Nickname,PhoneNo,FaxNo,Email,Notes,Id")] AddressBook addressBook)
        {
            if (id != addressBook.AddBookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addressBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressBookExists(addressBook.AddBookId))
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
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", addressBook.Id);
            return View(addressBook);
        }

        // GET: AddressBooks/Delete/5 ===========================================================
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AddressBooks == null)
            {
                return NotFound();
            }

            var addressBook = await _context.AddressBooks
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync(m => m.AddBookId == id);
            if (addressBook == null)
            {
                return NotFound();
            }

            return View(addressBook);
        }

        // POST: AddressBooks/Delete/5 ==================================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AddressBooks == null)
            {
                return Problem("Entity set 'aspnetMvcAuth2B182E742B6748EE95E062A11832A6A1Context.AddressBooks'  is null.");
            }
            var addressBook = await _context.AddressBooks.FindAsync(id);
            if (addressBook != null)
            {
                _context.AddressBooks.Remove(addressBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressBookExists(int id)
        {
          return (_context.AddressBooks?.Any(e => e.AddBookId == id)).GetValueOrDefault();
        }
    }
}
