using Application.Core;
using Application.Tareas.GetTarea;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tareas.GetTareas;

public class GetTareasQuery
{
    public record GetTareasQueryRequest: IRequest<Result<List<TareaResponse>>>
    {
        public GetTareasRequest? TareasRequest { get; set; }
    }

    internal class GetTareasQueryHandler : IRequestHandler<GetTareasQueryRequest, Result<List<TareaResponse>>>
    {
        private readonly TestDbContext _context;
        private readonly IMapper _mapper;

        public GetTareasQueryHandler(TestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<TareaResponse>>> Handle(GetTareasQueryRequest request, CancellationToken cancellationToken)
        {

            var tareas = await _context.Tareas!.ProjectTo<TareaResponse>(_mapper.ConfigurationProvider).ToListAsync();

            return Result<List<TareaResponse>>.Success(tareas!);
        }
    }
}
