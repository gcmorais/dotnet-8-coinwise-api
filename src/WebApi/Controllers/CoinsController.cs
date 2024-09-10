using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.CoinUseCases.CreateCoin;
using Application.UseCases.CoinUseCases.DeleteCoin;
using Application.UseCases.CoinUseCases.GetAllCoin;
using Application.UseCases.CoinUseCases.UpdateCoin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoinsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CoinResponse>> Create(CreateCoinRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<CoinResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetAllCoinRequest(), cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id is null) return BadRequest();

            var deleteCoinRequest = new DeleteCoinRequest(id.Value);

            var response = await _mediator.Send(deleteCoinRequest, cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CoinResponse>> Update(Guid id, UpdateCoinRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id) return BadRequest();

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
