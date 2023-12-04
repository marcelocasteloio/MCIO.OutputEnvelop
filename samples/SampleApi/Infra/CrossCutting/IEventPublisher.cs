namespace MCIO.OutputEnvelop.Samples.SampleApi.Infra.CrossCutting;

public interface IEventPublisher
{
    Task<OutputEnvelop> PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
}
