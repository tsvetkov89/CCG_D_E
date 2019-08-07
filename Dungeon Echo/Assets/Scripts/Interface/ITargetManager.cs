using EnumNamespace;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface ITargetManager
    {
        void SetDependecies(GameObject targetPointer,GameObject target, GameObject hud);
        bool PointerActive();
        void PointerInit(DraggableCard draggableCard);
    }
}