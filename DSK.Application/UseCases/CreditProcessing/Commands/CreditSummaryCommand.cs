﻿using DSK.Application.Models.DTOs;
using DSK.Application.Models.Generic.GenericResult;
using MediatR;

namespace DSK.Application.UseCases.CreditProcessing.Commands;

public sealed record CreditSummaryCommand()
: IRequest<Result<CreditSummaryDto>>
{
}

