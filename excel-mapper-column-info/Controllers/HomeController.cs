using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using excel_mapper_column_info.Models;
using Ganss.Excel;

namespace excel_mapper_column_info.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was selected for upload or the file is empty.");
        }

        try
        {
            using var memoryStream = file.OpenReadStream();
            var mapper = new ExcelMapper(memoryStream);
            mapper.MaxRowNumber = 2;
            var headers = mapper.Fetch();
            var columnInfo = new UploadExcelModelColumnInfo();
            mapper.MaxRowNumber = Int32.MaxValue;
            if (headers.Any())
            {
                var header = headers.First() as ExpandoObject;
                // check if header object contains "Identifier" key
                
                var headerDictionary = (IDictionary<string, object>) header;
                if (headerDictionary.ContainsKey("Identifier"))
                {
                    columnInfo.IdentifierPresent = true;
                }
                
                if (headerDictionary.ContainsKey("Description"))
                {
                    columnInfo.DescriptionPresent = true;
                }

                if (headerDictionary.ContainsKey("Name"))
                {
                    columnInfo.NamePresent = true;
                }

                if (headerDictionary.ContainsKey("Marks"))
                {
                    columnInfo.MarksPresent = true;
                }
            }
            
            var allItems = mapper.Fetch<UploadExcelModel>();

            // After processing, redirect or load a view as needed
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An error occurred while uploading the Excel file.");

            return StatusCode(StatusCodes.Status500InternalServerError, 
                "An error occurred while processing the file.");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}