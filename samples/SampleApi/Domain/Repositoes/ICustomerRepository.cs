using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.Repositoes;

public interface ICustomerRepository
{
    Task<OutputEnvelop> RegisterNewAsync(Customer customer, CancellationToken cancellationToken);
    Task<OutputEnvelop<bool>> CheckByEmailAsync(string email, CancellationToken cancellationToken);
    Task<OutputEnvelop<Customer?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<OutputEnvelop> GetAllAsync<TState>(
        TState state,
        Func<TState, Customer, CancellationToken, Task> handler,
        CancellationToken cancellationToken
    );
}
