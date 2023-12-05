using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Repositoes;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetAllCustomers;

public class GetAllCustomersQuery
    : IGetAllCustomersQuery
{
    // Fields
    private readonly ICustomerRepository _customerRepository;

    // Constructors
    public GetAllCustomersQuery(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // Public Methods
    public Task<OutputEnvelop> GetAsync<TState>(
        TState state,
        Func<TState, Customer, CancellationToken, Task> handler, 
        CancellationToken cancellationToken
    )
    {
        return _customerRepository.GetAllAsync(
            state: (State: state, Handler: handler),
            handler: async (state, customer, cancellationToken) => await state.Handler(state.State, customer, cancellationToken),
            cancellationToken
        );
    }
}
