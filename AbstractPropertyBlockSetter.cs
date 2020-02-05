using UnityEngine;

public abstract class AbstractPropertyBlockSetter<T>
{
    protected readonly MaterialPropertyBlock PropertyBlock;
    protected readonly int PropertyId;
    private readonly Renderer _renderer;

    protected AbstractPropertyBlockSetter(Renderer renderer, string propertyName)
    {
        PropertyBlock = new MaterialPropertyBlock();
        _renderer = renderer;
        PropertyId = Shader.PropertyToID(propertyName);
    }

    public void SetValue(T value)
    {
        SetValueToBlock(value);
        _renderer.SetPropertyBlock(PropertyBlock);
    }

    protected abstract void SetValueToBlock(T value);
}

public class FloatPropertyBlockSetter : AbstractPropertyBlockSetter<float>
{
    public FloatPropertyBlockSetter(Renderer renderer, string propertyName) : base(renderer, propertyName)
    {
    }

    protected override void SetValueToBlock(float value)
    {
        PropertyBlock.SetFloat(PropertyId, value);
    }
}