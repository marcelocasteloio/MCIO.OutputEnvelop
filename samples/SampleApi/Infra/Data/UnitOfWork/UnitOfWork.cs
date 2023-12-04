
namespace MCIO.OutputEnvelop.Samples.SampleApi.Infra.Data.UnitOfWork;

public class UnitOfWork
    : IUnitOfWork
{
    public Task<OutputEnvelop> ExecuteWithTransactionAsync<TInput>(
        TInput input, 
        Func<TInput, CancellationToken, Task<OutputEnvelop>> handler, 
        CancellationToken cancellationToken
    )
    {
        return handler(input, cancellationToken);
    }
}
