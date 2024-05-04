
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tareas.TareaUpdate;

public class TareaUpdateCommand
{
    public record TareaUpdateCommandRequest(TareaUpdateRequest TareaUpdateRequest, int? TareaId) : IRequest<Result<int>>;

    internal class TareaUpdateCommandHandler : IRequestHandler<TareaUpdateCommandRequest,Result<int>>
    {
        private readonly TestDbContext _context;

        public TareaUpdateCommandHandler( TestDbContext context)
        {
            _context = context;
            
        }

        public async Task<Result<int>> Handle(TareaUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var tarea = await _context.Tareas!.FirstOrDefaultAsync(x => x.Id == request.TareaId);

            if (tarea is null) return Result<int>.Failure("La tarea no existe");

            tarea.Fecha = request.TareaUpdateRequest.Fecha;
            tarea.Descripcion = request.TareaUpdateRequest?.Descripcion;
            tarea.Estado = request.TareaUpdateRequest!.Estado;

            _context.Entry(tarea).State = EntityState.Modified;
            var resultado = await _context.SaveChangesAsync() > 0;

            return resultado ? Result<int>.Success(tarea.Id) : Result<int>.Failure("Error la actualizar la tarea");

        }
    }

    public class TareaUpdateCommandRequestValidator : AbstractValidator<TareaUpdateCommandRequest> 
    {
        public TareaUpdateCommandRequestValidator()
        {
            RuleFor(x => x.TareaUpdateRequest).SetValidator(new TareaUpdateValidator());
        }
    }
}
