using UnityEngine;
using Zenject;

public abstract class AbstractProvider<TType, TProduct> : MonoBehaviour
{
    [Inject] private IProviderController<TType, TProduct> _provider = default;

    public abstract TType Type { get; }

    private void Awake()
    {
        _provider.Register(Type, GetComponent<TProduct>());
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }
}