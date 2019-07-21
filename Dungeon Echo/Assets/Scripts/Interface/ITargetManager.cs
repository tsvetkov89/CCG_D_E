using UnityEngine;

namespace InterfaceNamespace
{
    public interface ITargetManager
    {
        void SetDependecies(GameObject targetPointer);
        bool PointerActive();
        void PointerInit(DraggableCard draggableCard);
    }
}