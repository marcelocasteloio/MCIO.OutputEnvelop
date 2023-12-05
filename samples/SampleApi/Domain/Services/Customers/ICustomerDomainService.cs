using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Inputs;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Outputs;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers;

public interface ICustomerDomainService
{
    // Fields
    public static readonly string CustomerEmailAlreadyRegisteredMessageCode = $"{nameof(ICustomerDomainService)}.Email.AlreadyRegistered";
    public static readonly string CustomerEmailAlreadyRegisteredMessageDescription = $"{nameof(ICustomerDomainService)} E-mail already registered";

    // Methods
    Task<OutputEnvelop<RegisterNewOutput>> RegisterNewAsync(
        RegisterNewInput input,
        CancellationToken cancellationToken
    );
}
