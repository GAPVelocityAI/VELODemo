namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IModOrdersService
{
    int GetRandom(int lowerBound, int upperBound);

    Task<string> CreateRandomOrdersAsync(short orderCount, CancellationToken cancellationToken = default);

    Task SetDatesToCurrentAsync(CancellationToken cancellationToken = default);
}
