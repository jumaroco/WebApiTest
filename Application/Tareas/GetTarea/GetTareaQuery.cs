using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tareas.GetTarea;

public class GetTareaQuery
{
    public record GetTareaQueryRequest : IRequest<Result<TareaResponse>> 
    {
        public int Id { get; set; }
    };

    internal class GetTareaQueryHandler : IRequestHandler<GetTareaQueryRequest, Result<TareaResponse>>
    {
        private readonly TestDbContext _context;
        private readonly IMapper _mapper;

        public GetTareaQueryHandler(TestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<TareaResponse>> Handle(
            GetTareaQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            var tarea = await _context.Tareas!.Where(x => x.Id == request.Id)
                                        .ProjectTo<TareaResponse>(_mapper.ConfigurationProvider)
                                        .FirstOrDefaultAsync();

            return Result<TareaResponse>.Success(tarea!);
            
        }
    }
}

public record TareaResponse(
    int? Id,
    DateTime? Fecha,
    string? Descripcion,
    Estado? Estado
);
