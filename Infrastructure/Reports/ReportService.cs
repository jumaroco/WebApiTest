using Application.Interfaces;
using CsvHelper;
using Domain;
using System.Globalization;

namespace Infrastructure.Reports;

public class ReportService<T> : IReportService<T> where T : class
{
    public Task<MemoryStream> GetExcelReport(IEnumerable<T> records)
    {
        using var memoryStream = new MemoryStream();
        using var textWritter = new StreamWriter(memoryStream);
        using var csvWritter = new CsvWriter(textWritter, CultureInfo.InvariantCulture);

        csvWritter.WriteRecords(records);
        textWritter.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        return Task.FromResult(memoryStream);
    }
}
