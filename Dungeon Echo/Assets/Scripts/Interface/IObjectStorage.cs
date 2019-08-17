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
        void CreateAllPools();
        List<Adventure> GetAdventure();
        AudioClip GetAudioByName(string name);
        IToken GetTokenByName(TokenRewardEnum token);

    }
}