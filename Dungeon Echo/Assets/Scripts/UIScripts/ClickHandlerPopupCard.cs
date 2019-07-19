using EnumNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using InterfaceNamespace;
public class ClickHandlerPopupCard : MonoBehaviour, IPointerDownHandler
{
    private IPublisher _publisher;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _publisher.Publish(this, new CustomEventArgs(GameEventName.GoSetNextStage));
        _publisher.Publish(this, new CustomEventArgs(GameEventName.GoClosePopupDescriptionCard));
    }
    public void SetDependecies(IPublisher publisher)
    {
        _publisher = publisher;
    }
}
