using Application.Features.Stamps.Commands.AddEdit;
using Application.Features.Stamps.Commands.Delete;
using Application.Features.Stamps.Commands.Import;
using Application.Features.Stamps.Queries.Export;
using Application.Features.Stamps.Queries.GetAll;
using Application.Features.Stamps.Queries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Stamps
{
    public class StampController : BaseApiController<StampController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stamps = await _mediator.Send(new GetAllStampsQuery());
            return Ok(stamps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stamp = await _mediator.Send(new GetStampByIdQuery() { Id = id });
            return Ok(stamp);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddEditStampCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteStampCommand { Id = id }));
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export(string? searchString = "")
        {
            return Ok(await _mediator.Send(new ExportStampQuery(searchString)));
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(ImportStampCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
