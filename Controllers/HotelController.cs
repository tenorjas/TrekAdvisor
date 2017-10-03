using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrekAdvisor.Data;
using TrekAdvisor.Models;

namespace TrekAdvisor.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hotel
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.HotelModel.Include(i => i.Reviews).ThenInclude(u => u.ApplicationUser)
                .SingleOrDefaultAsync(m => m.HotelID == id);
            if (hotelModel == null)
            {
                return NotFound();
            }

            return View(hotelModel);
        }

        // GET: Hotel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.HotelModel
                .SingleOrDefaultAsync(m => m.HotelID == id);
            if (hotelModel == null)
            {
                return NotFound();
            }

            return View(hotelModel);
        }

        // GET: Hotel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelID,HotelName,StreetAddress,StreetName,City,State,Country,PostalCode,StarRating,OutsidePhoto,InsidePhoto,OutsideWidth,OutsideHeight,OutsideContentType,InsideWidth,InsideHeight,InsideContentType")] HotelModel hotelModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotelModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotelModel);
        }

        // GET: Hotel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.HotelModel.SingleOrDefaultAsync(m => m.HotelID == id);
            if (hotelModel == null)
            {
                return NotFound();
            }
            return View(hotelModel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelID,HotelName,StreetAddress,StreetName,City,State,Country,PostalCode,StarRating,OutsidePhoto,InsidePhoto,OutsideWidth,OutsideHeight,OutsideContentType,InsideWidth,InsideHeight,InsideContentType")] HotelModel hotelModel)
        {
            if (id != hotelModel.HotelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelModelExists(hotelModel.HotelID))
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
            return View(hotelModel);
        }

        // GET: Hotel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.HotelModel
                .SingleOrDefaultAsync(m => m.HotelID == id);
            if (hotelModel == null)
            {
                return NotFound();
            }

            return View(hotelModel);
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotelModel = await _context.HotelModel.SingleOrDefaultAsync(m => m.HotelID == id);
            _context.HotelModel.Remove(hotelModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelModelExists(int id)
        {
            return _context.HotelModel.Any(e => e.HotelID == id);
        }
    }
}
