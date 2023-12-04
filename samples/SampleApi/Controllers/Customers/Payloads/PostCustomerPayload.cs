using System.ComponentModel.DataAnnotations;

namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Payloads;

public class PostCustomerPayload
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }
}
