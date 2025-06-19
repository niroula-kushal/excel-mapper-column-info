namespace excel_mapper_column_info.Models;

public class UploadExcelModel
{
    public string Identifier { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? Marks { get; set; }
}

public class UploadExcelModelColumnInfo
{
    public bool IdentifierPresent { get; set; }
    public bool NamePresent { get; set; }
    public bool DescriptionPresent { get; set; }
    public bool MarksPresent { get; set; }
}