namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Inputs;

public readonly record struct RegisterNewInput
(
    string Name,
    string Email,
    DateOnly? BirthDate
);
