namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;

public readonly record struct RegisterNewCustomerUseCaseInput
(
    string Name,
    string Email,
    DateOnly? BirthDate
);
