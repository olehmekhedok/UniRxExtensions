using UniRx;

public interface IProviderController<TType, TProduct>
{
    IReadOnlyReactiveDictionary<TType, TProduct> Products { get; }
    void Register(TType type, TProduct product);
    bool TryGet(TType type, out TProduct product);
}

public class ProviderController<TType, TProduct> : IProviderController<TType, TProduct>
{
    private readonly ReactiveDictionary<TType, TProduct> _products = new ReactiveDictionary<TType, TProduct>();

    public IReadOnlyReactiveDictionary<TType, TProduct> Products => _products;

    void IProviderController<TType, TProduct>.Register(TType type, TProduct product)
    {
        _products.Add(type, product);
    }

    public bool TryGet(TType type, out TProduct product)
    {
        return _products.TryGetValue(type, out product);
    }
}