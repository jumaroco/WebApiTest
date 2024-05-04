
using Application.Core;
using Application.Tareas.GetTareas;
using Application.Tareas.TareaCreate;
using Application.Tareas.TareaUpdate;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Tareas.GetTarea.GetTareaQuery;
using static Application.Tareas.GetTareas.GetTareasQuery;
using static Application.Tareas.TareaCreate.TareaCreateCommand;
using static Application.Tareas.TareaDelete.TareaDeleteCommand;
using static Application.Tareas.TareaReporteExcel.TareaReporteExcelQuery;
using static Application.Tareas.TareaUpdate.TareaUpdateCommand;

namespace Web.Controllers;

[ApiController]
[Route("api/tareas")]
public class TareasController : ControllerBase
{
    private readonly ISender _sender;

    public TareasController(ISender sender) { _sender = sender; }

    // [Authorize(Policy = PolicyMaster.TAREA_READ)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> Cursos([FromQuery] GetTareasRequest request, CancellationToken cancellationToken)
    {
        var query = new GetTareasQueryRequest { TareasRequest = request };
        var resultados = await _sender.Send(query, cancellationToken);

        return resultados.IsSuccess ? Ok(resultados.Value) : NotFound();
    }

    [Authorize(Policy = PolicyMaster.TAREA_WRITE)]
    [HttpPost]
    public async Task<ActionResult<Result<int>>> TareaCreate(
        [FromForm] TareaCreateRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new TareaCreateCommandRequest(request);
        return await _sender.Send(command, cancellationToken);
    }

    [Authorize(Policy = PolicyMaster.TAREA_UPDATE)]
    [HttpPut]
    public async Task<ActionResult<Result<int>>> TareaUpdate(
        [FromBody] TareaUpdateRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        var command = new TareaUpdateCommandRequest(request, id);
        var resultado = await _sender.Send(command, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado) : BadRequest();
    }

    [Authorize(Policy = PolicyMaster.TAREA_DELETE)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Unit>> TareaDelete(int id, CancellationToken cancellationToken)
    {
        var command = new TareaDeleteCommandRequest(id);
        var resultado = await _sender.Send(command, cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(Policy = PolicyMaster.TAREA_READ)]
    [HttpGet("{id}")]
    public async Task<ActionResult> TareaGet(int id, CancellationToken cancellationToken) 
    {
        var query = new GetTareaQueryRequest { Id = id };
        var resultado = await _sender.Send(query,cancellationToken);

        return resultado.IsSuccess ? Ok(resultado.Value) : BadRequest();
    }

    [Authorize(Policy = PolicyMaster.TAREA_READ)]
    [HttpGet("report")]
    public async Task<IActionResult> ReporteExcel(CancellationToken cancellationToken)
    {
        var query = new TareaReporteExcelQueryRequest();
        var resultado = await _sender.Send(query,cancellationToken);
        byte[] excelBytes = resultado.ToArray();
        return File(excelBytes,"text/csv","tareas.csv");
        
    }
    
}
