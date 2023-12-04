using MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;
using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Payloads;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers;
[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    // Fields
    private IRegisterNewCustomerUseCase _registerNewCustomerUseCase;

    // Constructors
    public CustomersController(
        IRegisterNewCustomerUseCase registerNewCustomerUseCase
    )
    {
        _registerNewCustomerUseCase = registerNewCustomerUseCase;
    }

    [HttpPost]
    public async void PostAsync(
        PostCustomerPayload payload, 
        CancellationToken cancellationToken
    )
    {

    }
}
