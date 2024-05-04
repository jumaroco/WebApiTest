using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tareas.TareaReporteExcel;

public class TareaReporteExcelQuery
{
    public record TareaReporteExcelQueryRequest : IRequest<MemoryStream>;

    internal class TareaReporteExcelQueryHandler : IRequestHandler<TareaReporteExcelQueryRequest, MemoryStream>
    {
        private readonly TestDbContext _context;
        private readonly IReportService<Tarea> _reportService;

        public TareaReporteExcelQueryHandler(TestDbContext context, IReportService<Tarea> reportService)
        {
            _context = context;
            _reportService = reportService;
        }
        public async Task<MemoryStream> Handle(
            TareaReporteExcelQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var tareas = await _context.Tareas!.Take(10).Skip(0).ToListAsync();
            return await _reportService.GetExcelReport(tareas);
        }
    }
}
