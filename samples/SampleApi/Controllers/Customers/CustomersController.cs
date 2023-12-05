using MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetAllCustomers;
using MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetCustomerByEmail;
using MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;
using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Models;
using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Payloads;
using MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers;
[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    // Fields
    private readonly IRegisterNewCustomerUseCase _registerNewCustomerUseCase;
    private readonly IGetAllCustomersQuery _getAllCustomersQuery;
    private readonly IGetCustomerByEmailQuery _getCustomerByEmailQuery;

    // Constructors
    public CustomersController(
        IRegisterNewCustomerUseCase registerNewCustomerUseCase,
        IGetAllCustomersQuery getAllCustomersQuery,
        IGetCustomerByEmailQuery getCustomerByEmailQuery
    )
    {
        _registerNewCustomerUseCase = registerNewCustomerUseCase;
        _getAllCustomersQuery = getAllCustomersQuery;
        _getCustomerByEmailQuery = getCustomerByEmailQuery;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostCustomerResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(PostCustomerResponse))]
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCustomersResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetCustomersResponse))]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var customerCollection = new List<CustomerApiModel>();

        var getAllCustomerOutputEnvelop = await _getAllCustomersQuery.GetAsync(
            state: customerCollection,
            handler: (state, customer, cancellationToken) =>
            {
                state.Add(new CustomerApiModel
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    BirthDate = customer.BirthDate
                });

                return Task.CompletedTask;
            },
            cancellationToken
        );

        var messages = getAllCustomerOutputEnvelop.OutputMessageCollection.Select(q =>
        {
            return new Message
            {
                Type = q.Type.ToString(),
                Code = q.Code,
                Description = q.Description,
            };
        });

        return customerCollection.Count == 0 
            ? NotFound(
                new GetCustomersResponse
                {
                    Data = null,
                    Messages = messages
                }
            ) 
            : Ok(
                new GetCustomersResponse
                {
                    Data = customerCollection,
                    Messages = messages
                }
            );
    }

    [HttpGet("{email}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCustomerByEmailResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetCustomerByEmailResponse))]
    public async Task<IActionResult> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var getCustomerByEmailOutputEnvelop = await _getCustomerByEmailQuery.GetAsync(email, cancellationToken);

        var messages = getCustomerByEmailOutputEnvelop.OutputMessageCollection.Select(q =>
        {
            return new Message
            {
                Type = q.Type.ToString(),
                Code = q.Code,
                Description = q.Description,
            };
        });

        return getCustomerByEmailOutputEnvelop.Output is null
            ? NotFound(
                new GetCustomerByEmailResponse
                {
                    Data = null,
                    Messages = messages
                }
            )
            : Ok(
                new GetCustomerByEmailResponse
                {
                    Data = new CustomerApiModel
                    {
                        Name = getCustomerByEmailOutputEnvelop.Output.Name,
                        Email = getCustomerByEmailOutputEnvelop.Output.Email,
                        BirthDate = getCustomerByEmailOutputEnvelop.Output.BirthDate
                    },
                    Messages = messages
                }
            );
    }
}
