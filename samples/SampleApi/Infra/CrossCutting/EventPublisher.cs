
namespace MCIO.OutputEnvelop.Samples.SampleApi.Infra.CrossCutting;

public class EventPublisher
    : IEventPublisher
{
    public Task<OutputEnvelop> PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(OutputEnvelop.CreateSuccess());
    }
}
