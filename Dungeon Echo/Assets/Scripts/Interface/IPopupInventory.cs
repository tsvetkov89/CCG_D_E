using System.Collections.Generic;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface IPopupInventory
    {
        void SetDependecies(GameObject popupInventory);
        void SearchAndSetHolders(List<GameObject> gameObjects);
    }
}