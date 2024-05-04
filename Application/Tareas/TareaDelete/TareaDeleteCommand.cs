using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tareas.TareaDelete;

public class TareaDeleteCommand
{
    public record TareaDeleteCommandRequest(int? TareaId) : IRequest<Result<Unit>>;

    internal class TareaDeleteCommandHandler : IRequestHandler<TareaDeleteCommandRequest, Result<Unit>>
    {
        private readonly TestDbContext _context;
        public TareaDeleteCommandHandler(TestDbContext context) => _context = context; 

        public async Task<Result<Unit>> Handle(TareaDeleteCommandRequest request, CancellationToken cancellationToken)
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(x => x.Id == request.TareaId);
            if (tarea == null) return Result<Unit>.Failure("La tarea no existe.");

            _context.Tareas!.Remove(tarea);
            var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;

            return resultado ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Error en la transacción");
        }

    }

    public class TareaDeleteCommandRequestValidator : AbstractValidator<TareaDeleteCommandRequest>
    {
        public TareaDeleteCommandRequestValidator() 
        {
            RuleFor(x => x.TareaId).NotNull().WithMessage("No tiene tarea id");
        }
    }
}
