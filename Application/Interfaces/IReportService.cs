using Domain;

namespace Application.Interfaces;

public interface IReportService<T> where T : class
{
    Task<MemoryStream> GetExcelReport(IEnumerable<T> records);
}
