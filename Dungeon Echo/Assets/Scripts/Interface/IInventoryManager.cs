using System.Collections.Generic;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface IInventoryManager
    {
        //void AddItemInventory(string name, int index);
        //void AddItemInventory(string name);
        //void DeleteItemInventory(int index);
        //void DeleteEqupmentItem(int index);
        //void AddEqupmentItem(int index, string name);
        void SetHolders(GameObject holderCards, GameObject holderInventory,List<GameObject> gameObjects);
        void CreateSlotsInHolders();
        List<string> GetCardInDeck();
    }
}