using Domain;

namespace Application.Tareas.TareaCreate;

public class TareaCreateRequest
{
    public DateTime? Fecha {  get; set; }
    public string? Descripcion { get; set; }
    public Estado Estado { get; set; }
}
