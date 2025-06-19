# Excel Mapper Column Info

This project is an ASP.NET Core web application designed to extract and analyze column headers from uploaded Excel files. It provides a simple web interface for users to upload Excel files and processes them to fetch header information, which can be used for further data mapping or validation.

## Key Component: ExcelHeaderFetcher

The core logic for extracting Excel headers resides in the `ExcelHeaderFetcher` class, located at:


### What Does ExcelHeaderFetcher Do?

- **Reads Excel Files:** Accepts an Excel file stream and uses the [Ganss.Excel](https://github.com/mganss/ExcelMapper) library to parse the file.
- **Fetches Headers:** Extracts the column headers from the first row of the Excel sheet.
- **Configurable Mapping:** Optionally accepts a dictionary of header names mapped to actions, allowing you to perform custom logic if specific headers are present.
- **Returns Results:** Provides both the list of headers and the configured `ExcelMapper` instance for further processing.

### Example Usage

When a user uploads an Excel file via the web interface, the controller uses `ExcelHeaderFetcher.GetHeadersAndMapper` to:

1. Extract the headers.
2. Check for the presence of specific columns (like `Identifier`, `Name`, `Description`, `Marks`).
3. Optionally trigger custom logic based on which headers are found.

### Method Signature

```csharp
public static (List<string> Headers, ExcelMapper Mapper) GetHeadersAndMapper(
    Stream memoryStream,
    Dictionary<string, Action>? configMapping = null
)
```


### Example Usage in HomeController

When a user uploads an Excel file, the `UploadExcel` method in `HomeController` processes the file and uses `ExcelHeaderFetcher` to extract headers and check for specific columns:

```csharp
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
        var columnInfo = new UploadExcelModelColumnInfo();
        var (headers, mapper) = ExcelHeaderFetcher.GetHeadersAndMapper(memoryStream, new Dictionary<string, Action>()
        {
            {"Identifier", () => columnInfo.IdentifierPresent = true },
            {"Description", () => columnInfo.DescriptionPresent = true },
            {"Name", () => columnInfo.NamePresent = true },
            {"Marks", () => columnInfo.MarksPresent = true },
        });

        // Further processing...
    }
    catch (Exception ex)
    {
        // Error handling...
    }
}
