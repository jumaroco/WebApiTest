using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Tareas.TareaCreate;
public class TareaCreateCommand
{
    public record TareaCreateCommandRequest(TareaCreateRequest tareaCreateRequest) : IRequest<Result<int>>;

    internal class TareaCreateCommandHandler : IRequestHandler<TareaCreateCommandRequest, Result<int>>
    {
        private readonly TestDbContext _context;

        public TareaCreateCommandHandler(TestDbContext context)
        {

            _context = context;
        }
        public async Task<Result<int>> Handle(TareaCreateCommandRequest request, CancellationToken cancellationToken)
        {
            var tarea = new Tarea {
                Fecha = request.tareaCreateRequest.Fecha,
                Descripcion = request.tareaCreateRequest.Descripcion,
                Estado = request.tareaCreateRequest.Estado,
            };

            _context.Add(tarea);

           var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;

            return resultado ? Result<int>.Success(tarea.Id) : Result<int>.Failure("No se pudo guardar la tarea.");
        }
    }

    public class TareaCreateCommandRequestValidator : AbstractValidator<TareaCreateCommandRequest>
    {
        public TareaCreateCommandRequestValidator()
        {
            RuleFor(x => x.tareaCreateRequest).SetValidator(new TareaCreateValidator());
        }
    }
}
