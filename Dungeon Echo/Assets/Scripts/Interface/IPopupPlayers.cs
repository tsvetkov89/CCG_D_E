using System.Collections.Generic;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface IPopupPlayers
    {
        void SetDependecies(GameObject popupPlayers);
        List<GameObject> GetArrayPlayers();
    }
}