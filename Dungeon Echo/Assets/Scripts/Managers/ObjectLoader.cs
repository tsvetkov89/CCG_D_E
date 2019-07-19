 using System.Collections.Generic;
 using InterfaceNamespace;
 using UnityEngine;
/// <summary>
/// Выполняет роль загрузчика объектов
/// </summary>
 public class ObjectLoader: IObjectLoader
    {
        public T Load<T>(string path) where T : Object
        {
            return (T) Resources.Load(path, typeof(T));
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
           return ConvertObjects<T>(Resources.LoadAll(path, typeof (T)));
        }
        private T[] ConvertObjects<T>(IList<Object> rawObjects) where T : Object
        {
            if (rawObjects == null)
                return (T[]) null;
            var objArray = new T[rawObjects.Count];
            for (var index = 0; index < objArray.Length; ++index)
                objArray[index] = (T) rawObjects[index];
            return objArray;
        }
        public T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
        {
            return (T) Object.Instantiate((Object) original, parent, worldPositionStays);
        }
        public T Instantiate<T>(T  original, Transform parent) where T : Object
        {
            return (T) Object.Instantiate((Object) original, parent, false);
        }
    }
