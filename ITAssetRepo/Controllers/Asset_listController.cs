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
    //[Authorize]
    public class Asset_listController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Asset_listController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Asset_list
        public async Task<IActionResult> Index(
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            // Initialize page number to 1 if search string is provided
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // Ensure pageNumber is valid
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            // Store the current filter in ViewData for use in the search form
            ViewData["CurrentFilter"] = searchString;

            // Query the database for assets
            var assetsQuery = _context.Asset_list.AsQueryable();

            // Apply search filters if searchString is not empty
            if (!string.IsNullOrEmpty(searchString))
            {
                assetsQuery = assetsQuery.Where(a => a.Description.Contains(searchString)
                                                  || a.Catergory.Contains(searchString)
                                                  || a.Location.Contains(searchString));
            }

            // Define page size for pagination
            int pageSize = 15;

            // Retrieve paginated list of assets
            var paginatedAssets = await PaginatedList<Models.Asset_list>.CreateAsync(
                assetsQuery.AsNoTracking(), // Avoid tracking for read-only queries
                pageNumber ?? 1,
                pageSize);

            // Return the view with the paginated list of assets
            return View(paginatedAssets);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Asset_Number,Description,Category,Acq_Date,Location,Label,Custodian,Condition,PO_Number,Model,Serial_Number,Asset_Cost")] Asset_list asset_list)
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
                                                            Models.Asset_list asset_list)
        {
            if (id != asset_list.Asset_Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Handle file upload
                if (asset_list.BitlockerFile != null)
                {
                    var filePath = Path.Combine("wwwroot/files", asset_list.BitlockerFile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await asset_list.BitlockerFile.CopyToAsync(stream);
                    }

                    // Save the file path to the database
                    asset_list.BitlockerFilePath = filePath;
                }
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
            // Check if the file is null or empty
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file uploaded or file is empty.";
                return View();
            }

            // Register the encoding provider for Excel files
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Define the folder path where the file will be uploaded
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            // Ensure the directory exists
            Directory.CreateDirectory(uploadsFolder);

            // Create the full file path for the uploaded file
            var filePath = Path.Combine(uploadsFolder, Path.GetFileName(file.FileName));

            try
            {
                // Save the uploaded file to the specified path
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Check if the file saved is empty
                if (new FileInfo(filePath).Length == 0)
                {
                    ViewBag.Message = "The file is empty.";
                    return View();
                }

                // Open the saved Excel file for reading
                await using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        bool isHeaderSkipped = false;  // Track whether the header row is skipped
                        var assetList = new List<Models.Asset_list>();  // List to hold asset records

                        // Read each row in the Excel file
                        while (reader.Read())
                        {
                            // Skip the header row
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;
                                continue;
                            }

                            // Map the Excel row to an Asset_list object
                            var asset = new Models.Asset_list
                            {
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

                            // Add the asset to the list
                            assetList.Add(asset);
                        }

                        // Save all assets to the database in one operation if any assets were read
                        if (assetList.Any())
                        {
                            await _context.AddRangeAsync(assetList);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                // Indicate success
                ViewBag.Message = "Success";
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during file upload or processing
                ViewBag.Message = $"An error occurred: {ex.Message}";
            }

            // Return the view with the result message
            return View();
        }

    }
}
