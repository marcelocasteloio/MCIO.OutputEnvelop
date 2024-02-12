namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers.Customers.Models;

public class CustomerApiModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly? BirthDate { get; set; }
}
