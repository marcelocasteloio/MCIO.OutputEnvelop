﻿using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Models;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Responses;

public class GetCustomersResponse
    : ResponseBase<IEnumerable<CustomerApiModel>?>
{
}
