namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;

public interface IRegisterNewCustomerUseCase
{
    Task<OutputEnvelop> ExecuteAsync(
        RegisterNewCustomerUseCaseInput input,
        CancellationToken cancellationToken
    );
}
