using Application.Tareas.GetTarea;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tarea, TareaResponse>();
    }
}
