namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IModDaoService
{
    Task<int> GetRandomPkValueAsync(string tableName, string pkField, CancellationToken cancellationToken = default);

    bool HasField(string tableName, string fieldName);

    bool HasProperty(object target, string propertyName);
}
