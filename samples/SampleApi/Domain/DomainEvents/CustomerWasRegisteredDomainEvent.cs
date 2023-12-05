using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Inputs;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.DomainEvents;

public readonly record struct CustomerWasRegisteredDomainEvent
(
    RegisterNewInput Input,
    Customer RegisteredCustomer
);
