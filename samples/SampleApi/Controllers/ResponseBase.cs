namespace MCIO.OutputEnvelop.Samples.SampleApi.Controllers;

public abstract class ResponseBase
{
    public IEnumerable<Message> Messages { get; set; }
}
public abstract class ResponseBase<TData>
    : ResponseBase
{
    public TData Data { get; set; }
}

public class Message
{
    public string Type { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}