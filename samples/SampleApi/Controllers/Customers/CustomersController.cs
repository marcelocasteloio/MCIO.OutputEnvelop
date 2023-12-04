using MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;
using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Payloads;
using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Responses;
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
    public async Task<IActionResult> PostAsync(
        PostCustomerPayload payload, 
        CancellationToken cancellationToken
    )
    {
        var registerNewCustomerOutputEnvelop = await _registerNewCustomerUseCase.ExecuteAsync(
            input: new RegisterNewCustomerUseCaseInput(
                payload.Name,
                payload.Email,
                payload.BirthDate
            ),
            cancellationToken
        );

        var response = new PostCustomerResponse
        {
            Messages = registerNewCustomerOutputEnvelop.OutputMessageCollection.Select(q =>
            {
                return new Message
                {
                    Type = q.Type.ToString(),
                    Code = q.Code,
                    Description = q.Description,
                };
            })
        };

        return registerNewCustomerOutputEnvelop.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, response)
            : (IActionResult)StatusCode(StatusCodes.Status422UnprocessableEntity, response);
    }
}
