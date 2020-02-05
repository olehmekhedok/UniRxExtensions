using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
public interface IConfigController
{
    bool TryGetConfig<T>(out T value) where T : class;
}

public class ConfigController : IConfigController
{
    private readonly Dictionary<Type, object> _configs = new Dictionary<Type, object>();

    public ConfigController()
    {
        foreach (var textAsset in Resources.LoadAll<TextAsset>(Consts.ConfigsPath))
        {
            Type type = Type.GetType(textAsset.name);
    
            if (type == null)
            {
                Debug.LogError("There are no Config IngredientType: " + textAsset.name);
                continue;
            }

            var obj = JsonConvert.DeserializeObject(textAsset.text, type);
            if (obj == null)
                continue;

            _configs.Add(type, obj);
        }
    }

    bool IConfigController.TryGetConfig<T>(out T value)
    {
        value = default;

        if (_configs.TryGetValue(typeof(T), out var config))
        {
            value = config as T;
        }

        return value != null;
    }
}
