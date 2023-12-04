using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Repositoes;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Infra.Data.Repositories;

public class CustomerRepository
    : ICustomerRepository
{
    // Fields
    private static List<Customer> _customerCollection = [];

    // Public Methods
    public Task<OutputEnvelop> RegisterNewAsync(Customer customer, CancellationToken cancellationToken)
    {
        return OutputEnvelop.ExecuteAsync(
            cancellationToken =>
            {
                _customerCollection.Add(customer);

                return Task.FromResult(OutputEnvelop.CreateSuccess());
            },
            cancellationToken
        );
    }

    public Task<OutputEnvelop<bool>> CheckByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return OutputEnvelop<bool>.ExecuteAsync(
            cancellationToken =>
            {
                return Task.FromResult(
                    OutputEnvelop<bool>.CreateSuccess(
                        output: _customerCollection.Any(q => q.Email == email)
                    )
                );
            },
            cancellationToken
        );
    }

    public Task<OutputEnvelop<Customer?>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return OutputEnvelop<Customer?>.ExecuteAsync(
            cancellationToken =>
            {
                return Task.FromResult(
                    OutputEnvelop<Customer?>.CreateSuccess(
                        output: _customerCollection.FirstOrDefault(q => q.Email == email)
                    )
                );
            },
            cancellationToken
        );
    }

    public Task<OutputEnvelop> GetAllAsync<TState>(
        TState state,
        Func<TState, Customer, CancellationToken, Task> handler,
        CancellationToken cancellationToken)
    {
        return OutputEnvelop.ExecuteAsync(
            async cancellationToken =>
            {
                foreach (var customer in _customerCollection)
                    await handler(state, customer, cancellationToken);

                return OutputEnvelop.CreateSuccess();
            },
            cancellationToken
        );
    }
}
