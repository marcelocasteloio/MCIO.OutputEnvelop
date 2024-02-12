namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers;

public abstract class ResponseBase
{
    public IEnumerable<Message> Messages { get; set; } = null!;
}
public abstract class ResponseBase<TData>
    : ResponseBase
{
    public TData Data { get; set; } = default!; 
}

public class Message
{
    public string Type { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
}