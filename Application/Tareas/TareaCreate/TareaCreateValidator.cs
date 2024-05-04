using FluentValidation;

namespace Application.Tareas.TareaCreate;

public class TareaCreateValidator : AbstractValidator<TareaCreateRequest>
{
    public TareaCreateValidator()
    {
        RuleFor(x => x.Fecha).NotEmpty().WithMessage("Debe ingresar Fecha.");
        RuleFor(x => x.Descripcion).NotEmpty().WithMessage("Debe ingresar una Descripción.");
        RuleFor(x => x.Estado).NotNull().WithMessage("Debe ingresar el Estado.");
    }
}
