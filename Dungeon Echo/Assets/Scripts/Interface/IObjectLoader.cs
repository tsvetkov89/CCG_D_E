using UnityEngine;

namespace InterfaceNamespace
{
 /// <summary>
 /// Менеджер загрузки ресурсов
 /// </summary>
    public interface IObjectLoader
    {
        T Load<T>(string path) where T : Object;
        T [] LoadAll<T>(string path) where T : Object;
        T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object;
        T Instantiate<T>(T original, Transform parent) where T : Object;
    }
}