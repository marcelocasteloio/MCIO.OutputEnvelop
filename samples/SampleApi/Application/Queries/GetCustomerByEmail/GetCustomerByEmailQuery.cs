using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Repositoes;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetCustomerByEmail;

public class GetCustomerByEmailQuery
    : IGetCustomerByEmailQuery
{
    // Fields
    private readonly ICustomerRepository _customerRepository;

    // Constructors
    public GetCustomerByEmailQuery(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // Public Methods
    public Task<OutputEnvelop<Customer?>> GetAsync(
        string email, 
        CancellationToken cancellationToken
    )
    {
        return _customerRepository.GetByEmailAsync(email, cancellationToken);
    }
}
