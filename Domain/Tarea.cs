namespace Domain;

public class Tarea : BaseEntity
{
    public DateTime? Fecha { get; set; }
    public string? Descripcion { get; set; }
    public Estado Estado { get; set; }

}

public enum Estado
{
    Pendiente = 0,
    Completado
}
