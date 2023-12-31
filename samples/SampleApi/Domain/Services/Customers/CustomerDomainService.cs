using MCIO.OutputEnvelop.Samples.SampleApi.Domain.DomainEvents;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Repositoes;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Inputs;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers.Outputs;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers;

public class CustomerDomainService
    : ICustomerDomainService
{
    // Fields
    private readonly ICustomerRepository _customerRepository;

    // Constructors
    public CustomerDomainService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // Public Methods
    public async Task<OutputEnvelop<RegisterNewOutput>> RegisterNewAsync(
        RegisterNewInput input,
        CancellationToken cancellationToken
    )
    {
        // Check if email already registered
        var checkByEmailOutputEnvelop = await _customerRepository.CheckByEmailAsync(input.Email, cancellationToken);
        if (!checkByEmailOutputEnvelop.IsSuccess)
            return OutputEnvelop <RegisterNewOutput>.CreateError(
                output: CreateFailedRegisterNewOutput(ref input),
                checkByEmailOutputEnvelop.AsOutputEnvelop()
            );

        if (checkByEmailOutputEnvelop.Output)
            return OutputEnvelop<RegisterNewOutput>.CreateError(
                output: CreateFailedRegisterNewOutput(ref input),
                outputMessageCode: ICustomerDomainService.CustomerEmailAlreadyRegisteredMessageCode,
                outputMessageDescription: ICustomerDomainService.CustomerEmailAlreadyRegisteredMessageDescription
            );

        // Register new
        var registerNewOutputEnvelop = Customer.RegisterNew(input.Name, input.Email, input.BirthDate);
        if (!registerNewOutputEnvelop.IsSuccess)
            // create a new output envelop to preserve all returned messages
            return OutputEnvelop<RegisterNewOutput>.CreateError(
                output: CreateFailedRegisterNewOutput(ref input),
                checkByEmailOutputEnvelop.AsOutputEnvelop(),
                registerNewOutputEnvelop.AsOutputEnvelop()
            );

        var customer = registerNewOutputEnvelop.Output;

        // Persist
        var persistOutputEnvelop = await _customerRepository.RegisterNewAsync(
            customer!,
            cancellationToken
        );
        if (!persistOutputEnvelop.IsSuccess)
            // create a new output envelop to preserve all returned messages
            return OutputEnvelop<RegisterNewOutput>.CreateError(
                output: CreateFailedRegisterNewOutput(ref input),
                checkByEmailOutputEnvelop.AsOutputEnvelop(),
                registerNewOutputEnvelop.AsOutputEnvelop(),
                persistOutputEnvelop
            );

        // create a new output envelop based on anothers output envelops 
        // to preserve all returned messages
        return OutputEnvelop<RegisterNewOutput>.CreateSuccess(
            output: CreateSuccessRegisterNewOutput(ref input, ref customer!),
            checkByEmailOutputEnvelop.AsOutputEnvelop(),
            registerNewOutputEnvelop.AsOutputEnvelop(),
            persistOutputEnvelop
        );
    }

    // Private Methods
    private static RegisterNewOutput CreateFailedRegisterNewOutput(
        ref RegisterNewInput input
    )
    {
        return new RegisterNewOutput(
            CustomerWasRegisteredDomainEvent: null,
            FailOnRegistrateNewCustomerDomainEvent: new FailOnRegistrateNewCustomerDomainEvent(input)
        );
    }
    private static RegisterNewOutput CreateSuccessRegisterNewOutput(
        ref RegisterNewInput input,
        ref Customer customer
    )
    {
        return new RegisterNewOutput(
            CustomerWasRegisteredDomainEvent: new CustomerWasRegisteredDomainEvent(input, customer!),
            FailOnRegistrateNewCustomerDomainEvent: null
        );
    }
}
