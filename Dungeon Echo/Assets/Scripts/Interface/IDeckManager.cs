using EnumNamespace;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface IDeckManager
    {
        void SetUiComponents(GameObject obj,GameObject pointStopDrag, GameObject pointReturnCard);
        void PlaceObjects();
    }
}