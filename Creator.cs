using UnityEngine;
using Zenject;

public interface ICreator
{
    T Instantiate<T>(T original, Transform parent) where T : Object;
    GameObject Instantiate(GameObject original, Transform parent);
    T New<T>(params object[] extraArgs);
}

public class Creator : ICreator
{
    [Inject] private DiContainer _container = default;

    public T Instantiate<T>(T original, Transform parent) where T : Object
    {
        return _container.InstantiatePrefabForComponent<T>(original, parent);
    }

    public GameObject Instantiate(GameObject original, Transform parent)
    {
        return _container.InstantiatePrefab(original, parent);
    }

    public T New<T>(params object[] extraArgs)
    {
        if (extraArgs == null)
            extraArgs = new object[0];

        var instance = _container.Instantiate<T>(extraArgs);

        var initializable = instance as IInitializable;
        initializable?.Initialize();

        return instance;
    }
}
