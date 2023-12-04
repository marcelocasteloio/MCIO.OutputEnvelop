using MCIO.OutputEnvelop.Samples.SampleApi.Domain.DomainEvents;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Outputs;

public readonly record struct  RegisterNewOutput
(
    CustomerWasRegisteredDomainEvent? CustomerWasRegisteredDomainEvent,
    FailOnRegistrateNewCustomerDomainEvent? FailOnRegistrateNewCustomerDomainEvent
);
