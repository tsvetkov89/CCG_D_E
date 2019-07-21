using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface IObjectStorage
    {
        T GetObjectInStorage<T>(ObjectTypeEnum typeEnum) where T : Object;
        T[] GetPoolObjectsInStorage<T>(ObjectTypeEnum typeEnum) where T : Object;
        List<GameObject> GetPollObjects(ObjectTypeEnum typeEnum, int size);
        IEnumerator ReturnInPool(ObjectTypeEnum typeEnum, GameObject obj, float t);
        IDictionary<string, ICard> GetDictionaryCards(SubTypeCard subTypeCard);
        ICard GetRandomCardByType(SubTypeCard type, CardRarity rarity);
        ICard GetCardByName(string name);
        void CreateAllPools(GameObject obj);
        void ConfigurateByParent(GameObject child, GameObject parent, float xmin, float ymin, float xmax, float ymax);
        void ConfigurateByParent(GameObject child, GameObject parent, bool active);
        void Configurate(GameObject child, GameObject parent, bool active, float xmin, float xmax, float ymin, float ymax);
        List<Adventure> GetAdventure();
    }
}