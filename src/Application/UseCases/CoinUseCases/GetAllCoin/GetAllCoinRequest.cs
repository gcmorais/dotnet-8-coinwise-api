using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.UserUseCases.Common;
using MediatR;

namespace Application.UseCases.CoinUseCases.GetAllCoin
{
    public sealed record GetAllCoinRequest : IRequest<List<CoinResponse>>;
}
