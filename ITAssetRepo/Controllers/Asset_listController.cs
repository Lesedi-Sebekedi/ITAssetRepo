using ExcelDataReader;
using ITAssetRepo.Data;
using ITAssetRepo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System.Text;


namespace ITAssetRepo.Controllers
{
    [Authorize]
    public class Asset_listController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Asset_listController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Asset_list
        public async Task<IActionResult> Index(
            string searchString
            , string currentFilter
            , int? pageNumber)
        {

            //Paging
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // Ensure pageNumber is valid
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            //Searching
            ViewData["CurrentFilter"] = searchString;
            var assets = from a in _context.Assets select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                assets = assets.Where(a => a.Asset_Number.Contains(searchString)
                || a.Custodian.Contains(searchString));
            }

            //paging
            int pageSize = 15;
            var paginatedAssets = await PaginatedList<Models.Asset>.CreateAsync(
                assets.AsNoTracking(), // Use AsNoTracking without type argument
                                       pageNumber ?? 1,    pageSize);

            return View(paginatedAssets);
        }

        // GET: Asset_list/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset_list = await _context.Assets
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Asset_Number,Description,Category,Acq_Date,Location,Label,Custodian,Condition,PO_Number,Model,Serial_Number,Asset_Cost")]
                                        NuGet.ContentModel.Asset asset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asset);
        }


        // GET: Asset_list/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset_list = await _context.Assets.FindAsync(id);
            if (asset_list == null)
            {
                return NotFound();
            }
            return View(asset_list);
        }

        // POST: Asset_list/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Asset_Number" +
                                                               ",Description" +
                                                               ",Catergory" +
                                                               ",Acq_Date" +
                                                               ",Location" +
                                                               ",Label" +
                                                               ",Custodian" +
                                                               ",Condition" +
                                                               ",PO_Number" +
                                                               ",Model" +
                                                               ",Serial_Number" +
                                                               ",Asset_Cost")]
                                                            Models.Asset asset_list)
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

            var asset_list = await _context.Assets
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
            var asset_list = await _context.Assets.FindAsync(id);
            if (asset_list != null)
            {
                _context.Assets.Remove(asset_list);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Asset_listExists(string id)
        {
            return _context.Assets.Any(e => e.Asset_Number == id);
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

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filepath = Path.Combine(uploadsFolder, file.FileName);

                try
                {
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Check if file is empty
                    if (new FileInfo(filepath).Length == 0)
                    {
                        ViewBag.Message = "The file is empty.";
                        return View();
                    }

                    // Read data from the Excel file
                    using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                Models.Asset e = new Models.Asset
                                {
<<<<<<< HEAD
                                    Asset_Number = reader.GetValue(0)?.ToString(),
                                    Description = reader.GetValue(1)?.ToString(),
                                    Catergory = reader.GetValue(2)?.ToString(),
                                    Acq_Date = reader.GetValue(3) != null && DateTime.TryParse(reader.GetValue(3).ToString(), out DateTime acqDate) ? acqDate : DateTime.MinValue,
                                    Location = reader.GetValue(4)?.ToString(),
                                    Label = reader.GetValue(5)?.ToString(),
                                    Custodian = reader.GetValue(6)?.ToString(),
                                    Condition = reader.GetValue(7)?.ToString(),
                                    PO_Number = reader.GetValue(8)?.ToString(),
                                    Model = reader.GetValue(9)?.ToString(),
                                    Serial_Number = reader.GetValue(10)?.ToString(),
                                    Asset_Cost = reader.GetValue(11) != null && decimal.TryParse(reader.GetValue(11).ToString(), out decimal assetCost) ? assetCost : 0m
                                };

=======
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
>>>>>>> parent of 976e7e1 (partial upload)
                                _context.Add(e);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    ViewBag.Message = "Success";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "No file uploaded or file is empty.";
            }

            return View();
        }

    }
}
