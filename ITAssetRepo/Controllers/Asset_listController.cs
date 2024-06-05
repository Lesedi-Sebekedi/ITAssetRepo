using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITAssetRepo.Data;
using ITAssetRepo.Models;
using System.Text;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using ITAssetRepo.Data.Pagination;


namespace ITAssetRepo.Controllers
{
    //[Authorize]
    public class Asset_listController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Asset_listController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Asset_list
        public async Task<IActionResult> Index(string searchString, int? PageNumber)
        {
            ViewData["CurrentFilter"] = searchString;

            var AssetList = from a in _context.Asset_list select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                AssetList = AssetList.Where(a => a.Asset_Number.Contains(searchString)
                || a.Custodian.Contains(searchString));
                return View(AssetList);
            }


                int pageSize = 5;
            return View(await PaginatedList<Asset_list>.CreateAsync(_context.Asset_list.AsNoTracking()
                , PageNumber ?? 1
                , pageSize));
            //return View(await _context.Asset_list.ToListAsync());
        }

        // GET: Asset_list/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset_list = await _context.Asset_list
                .FirstOrDefaultAsync(m => m.Asset_Number == id);
            if (asset_list == null)
            {
                return NotFound();
            }

            return View(asset_list);
        }

        // GET: Asset_list/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Asset_list/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Asset_Number" +
            ",Description" +
            ",Catergory,Acq_Date" +
            ",Location" +
            ",Label" +
            ",Custodian" +
            ",Condition" +
            ",PO_Number" +
            ",Model" +
            ",Serial_Number" +
            ",Asset_Cost")] Asset_list asset_list)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asset_list);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asset_list);
        }

        // GET: Asset_list/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset_list = await _context.Asset_list.FindAsync(id);
            if (asset_list == null)
            {
                return NotFound();
            }
            return View(asset_list);
        }

        // POST: Asset_list/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Asset_Number,Description,Catergory,Acq_Date,Location,Label,Custodian,Condition,PO_Number,Model,Serial_Number,Asset_Cost")] Asset_list asset_list)
        {
            if (id != asset_list.Asset_Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset_list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Asset_listExists(asset_list.Asset_Number))
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
            return View(asset_list);
        }

        // GET: Asset_list/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset_list = await _context.Asset_list
                .FirstOrDefaultAsync(m => m.Asset_Number == id);
            if (asset_list == null)
            {
                return NotFound();
            }

            return View(asset_list);
        }

        // POST: Asset_list/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var asset_list = await _context.Asset_list.FindAsync(id);
            if (asset_list != null)
            {
                _context.Asset_list.Remove(asset_list);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Asset_listExists(string id)
        {
            return _context.Asset_list.Any(e => e.Asset_Number == id);
        }

        public IActionResult UploadExcel()
        {
            return View();
        }

        //Upload the excel file and save the data to the database
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (file != null && file.Length > 0) //check if the file is empty or not
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filepath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //Read the data from the excel
                using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
                {

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                Asset_list e = new Asset_list
                                {
                                    Asset_Number = reader.GetValue(0).ToString(),
                                    Description = reader.GetValue(1).ToString(),
                                    Catergory = reader.GetValue(2).ToString(),
                                    Acq_Date = reader.GetValue(3).ToString(),
                                    Location = reader.GetValue(4).ToString(),
                                    Label = reader.GetValue(5).ToString(),
                                    Custodian = reader.GetValue(6).ToString(),
                                    Condition = reader.GetValue(7).ToString(),
                                    PO_Number = reader.GetValue(8).ToString(),
                                    Model = reader.GetValue(9).ToString(),
                                    Serial_Number = reader.GetValue(10).ToString(),
                                    Asset_Cost = reader.GetValue(11).ToString()
                                };

                                //Save the data in the database
                                _context.Add(e);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "Success";
                    }
                }
            }

            else
                ViewBag.Message = "Empty";
            return View();
        }
    }
}
