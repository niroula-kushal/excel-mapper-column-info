using System.Dynamic;
using Ganss.Excel;

namespace excel_mapper_column_info.Controllers
{
    public static class ExcelHeaderFetcher
    {
        public static (List<string> Headers, ExcelMapper Mapper) GetHeadersAndMapper(Stream memoryStream, Dictionary<string, Action>? configMapping = null)
        {
            var mapper = new ExcelMapper(memoryStream)
            {
                MaxRowNumber = 2
            };

            var fetchedData = mapper.Fetch().ToList();
            var headers = new List<string>();

            if (fetchedData.Any())
            {
                var firstRow = fetchedData.First() as ExpandoObject;
                var headerDictionary = (IDictionary<string, object>) firstRow!;
                headers = headerDictionary.Keys.ToList();
            }
            mapper.MaxRowNumber = Int32.MaxValue;

            if (configMapping is not null)
            {
                foreach (var (key, func) in configMapping)
                {
                    if (headers.Contains(key))
                    {
                        func.Invoke();
                    }
                }
            }

            return (headers, mapper);
        }
    }
}