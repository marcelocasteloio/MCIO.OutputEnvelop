using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers;
using MCIO.OutputEnvelop.Samples.SampleApi.Infra.CrossCutting;
using MCIO.OutputEnvelop.Samples.SampleApi.Infra.Data.UnitOfWork;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;

public class RegisterNewCustomerUseCase
    : IRegisterNewCustomerUseCase
{
    // Fields
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    private readonly ICustomerDomainService _customerDomainService;

    // Constructors
    public RegisterNewCustomerUseCase(
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher,
        ICustomerDomainService customerDomainService
    )
    {
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _customerDomainService = customerDomainService;
    }

    // Public Methods
    public Task<OutputEnvelop> ExecuteAsync(
        RegisterNewCustomerUseCaseInput input, 
        CancellationToken cancellationToken
    )
    {
        return _unitOfWork.ExecuteWithTransactionAsync(
            input,
            handler: async (input, cancellationToken) =>
            {
                // Process
                var registerNewCustomerOutputEnvelop = await _customerDomainService.RegisterNewAsync(
                    new Domain.Services.Customers.Inputs.RegisterNewInput(
                        input.Name,
                        input.Email,
                        input.BirthDate
                    ),
                    cancellationToken
                );

                // Send event
                var publishEventOutputEvelop = registerNewCustomerOutputEnvelop.IsSuccess
                    ? await _eventPublisher.PublishEventAsync(
                        registerNewCustomerOutputEnvelop.Output.CustomerWasRegisteredDomainEvent,
                        cancellationToken
                    )
                    : await _eventPublisher.PublishEventAsync(
                        registerNewCustomerOutputEnvelop.Output.FailOnRegistrateNewCustomerDomainEvent,
                        cancellationToken
                    );

                return OutputEnvelop.Create(
                    registerNewCustomerOutputEnvelop,
                    publishEventOutputEvelop
                );
            },
            cancellationToken
        );
    }
}
