using UnityEngine;

public class Provider<TType, TProduct> : AbstractProvider<TType, TProduct>
{
    [SerializeField] private TType _type = default;

    public override TType Type => _type;
}