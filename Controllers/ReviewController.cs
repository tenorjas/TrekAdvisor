using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrekAdvisor.Data;
using TrekAdvisor.Models;

namespace TrekAdvisor.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> um)
        {
            _context = context;
            _userManager = um;
        }

        // GET: Review
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReviewModel.Include(r => r.ApplicationUser).Include(r => r.Hotel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewModel = await _context.ReviewModel
                .Include(r => r.ApplicationUser)
                .Include(r => r.Hotel)
                .SingleOrDefaultAsync(m => m.ReviewID == id);
            if (reviewModel == null)
            {
                return NotFound();
            }

            return View(reviewModel);
        }

        // GET: Review/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["HotelID"] = new SelectList(_context.HotelModel, "HotelID", "HotelID");
            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromRoute] int id, [FromForm] string title, [FromForm] string dateofstay, [FromForm] string body)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var newReview = new ReviewModel {HotelID = id, Title = title, DateOfStay = dateofstay, Body = body, ApplicationUserId = user.Id};
            _context.ReviewModel.Add(newReview);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Hotel",new{id});
        }

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewModel = await _context.ReviewModel.SingleOrDefaultAsync(m => m.ReviewID == id);
            if (reviewModel == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", reviewModel.ApplicationUserId);
            ViewData["HotelID"] = new SelectList(_context.HotelModel, "HotelID", "HotelID", reviewModel.HotelID);
            return View(reviewModel);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,Title,Body,DatePosted,DateOfStay,HotelID,ApplicationUserId")] ReviewModel reviewModel)
        {
            if (id != reviewModel.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewModelExists(reviewModel.ReviewID))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", reviewModel.ApplicationUserId);
            ViewData["HotelID"] = new SelectList(_context.HotelModel, "HotelID", "HotelID", reviewModel.HotelID);
            return View(reviewModel);
        }

        // GET: Review/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewModel = await _context.ReviewModel
                .Include(r => r.ApplicationUser)
                .Include(r => r.Hotel)
                .SingleOrDefaultAsync(m => m.ReviewID == id);
            if (reviewModel == null)
            {
                return NotFound();
            }

            return View(reviewModel);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviewModel = await _context.ReviewModel.SingleOrDefaultAsync(m => m.ReviewID == id);
            _context.ReviewModel.Remove(reviewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewModelExists(int id)
        {
            return _context.ReviewModel.Any(e => e.ReviewID == id);
        }
    }
}
