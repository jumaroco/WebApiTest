using Domain;

namespace Application.Tareas.TareaUpdate;

public class TareaUpdateRequest
{
    public DateTime? Fecha {  get; set; }
    public string? Descripcion { get; set; }
    public Estado Estado { get; set; }
}
