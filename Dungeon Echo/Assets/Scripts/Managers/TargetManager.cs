using InterfaceNamespace;
using UnityEngine;

public class TargetManager : ITargetManager
{
    private DraggableCard _draggableCard;
    private GameObject _targetPointer;
    private DraggeblePointer _draggeblePointer;

    public void SetDependecies(GameObject targetPointer)
    {
        _targetPointer = targetPointer;
        _draggeblePointer = _targetPointer.GetComponent<DraggeblePointer>();
    }

    public bool PointerActive()
    {
        return _targetPointer.activeSelf;
    }
    public void PointerInit(DraggableCard draggableCard)
    {
        _draggableCard = draggableCard;
        _draggeblePointer.Init(_draggableCard);
    }
}
