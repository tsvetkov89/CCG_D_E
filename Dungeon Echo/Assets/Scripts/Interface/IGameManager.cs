using System.Collections;
using UnityEngine;
using EnumNamespace;
namespace InterfaceNamespace
{
    public interface IGameManager
    {
        void SetUiComponents(GameObject obj, GameObject deckInGame);
        void PlaceObjects();
        //IEnumerator SpawnCardsByType(CardType type, DropChance typeDropChance, DropChance rarityDropChance, int count);
    }
}

