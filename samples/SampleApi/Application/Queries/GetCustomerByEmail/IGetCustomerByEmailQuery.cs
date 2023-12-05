using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetCustomerByEmail;

public interface IGetCustomerByEmailQuery
{
    Task<OutputEnvelop<Customer?>> GetAsync(
        string email,
        CancellationToken cancellationToken
    );
}
