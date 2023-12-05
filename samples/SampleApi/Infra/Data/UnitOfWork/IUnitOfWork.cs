namespace MCIO.OutputEnvelop.Samples.SampleApi.Infra.Data.UnitOfWork;

public interface IUnitOfWork
{
    Task<OutputEnvelop> ExecuteWithTransactionAsync<TInput>(
        TInput input,
        Func<TInput, CancellationToken, Task<OutputEnvelop>> handler,
        CancellationToken cancellationToken
    );
}
