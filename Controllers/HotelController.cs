using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using TrekAdvisor.Data;
using TrekAdvisor.Models;

namespace TrekAdvisor.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public HotelController(ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _environment = appEnvironment;
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
        public async Task<IActionResult> Create(HotelModel hotelModel)
        {
            if (ModelState.IsValid)
            {
                // UPLOAD: grabs the files from the incoming form
                var files = HttpContext.Request.Form.Files;
                // UPLOAD: processes each file
                foreach (var _image in files)
                {
                    if (_image != null && _image.Length > 0)
                    {
                        var file = _image;

                        // UPLOAD: sets the path of the where the file is stored on the server
                        var uploads = Path.Combine(_environment.WebRootPath, "uploads/images");

                        if (file.Length > 0)
                        {
                            // UPLOAD: creates a new unique file name to store in the uploads folder 
                            var fileName = Guid.NewGuid().ToString() + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                            var _filePath = Path.Combine(uploads, fileName);


                            // UPLOAD: Saves file to local server
                            using (var fileStream = new FileStream(_filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);

                                // UPLOAD: sets properties on the new model
                                if (file.Name == "outsidepic")
                                {
                                    hotelModel.OutsidePhoto = fileName;
                                }
                                else if (file.Name == "insidepic")
                                {
                                    hotelModel.InsidePhoto = fileName;
                                }
                            }
                        }
                    }
                }

                _context.Add(hotelModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
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
        public async Task<IActionResult> Edit(int id, HotelModel hotelModel)
        {
            if (id != hotelModel.HotelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // UPLOAD: grabs the files from the incoming form
                    var files = HttpContext.Request.Form.Files;
                    Console.WriteLine(files.Count());
                    // UPLOAD: processes each file
                    foreach (var _image in files)
                    {
                        if (_image != null && _image.Length > 0)
                        {
                            var file = _image;

                            // UPLOAD: sets the path of the where the file is stored on the server
                            var uploads = Path.Combine(_environment.WebRootPath, "uploads/images");

                            if (file.Length > 0)
                            {
                                // UPLOAD: creates a new unique file name to store in the uploads folder 
                                var fileName = Guid.NewGuid().ToString() + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                                var _filePath = Path.Combine(uploads, fileName);


                                // UPLOAD: Saves file to local server
                                using (var fileStream = new FileStream(_filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                
                                    // UPLOAD: sets properties on the new model
                                    if (file.Name == "outsidepic")
                                    {
                                        hotelModel.OutsidePhoto = fileName;
                                    }
                                    else if (file.Name == "insidepic")
                                    {
                                        hotelModel.InsidePhoto = fileName;
                                    }
                                }
                            }
                        }
                    }


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
            return View("Index");
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
