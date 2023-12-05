using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetAllCustomers;

public interface IGetAllCustomersQuery
{
    Task<OutputEnvelop> GetAsync<TState>(
        TState state,
        Func<TState, Customer, CancellationToken, Task> handler,
        CancellationToken cancellationToken
    );
}
