using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionsWithSlotInventory :  Card , IPointerDownHandler{
    public bool Empty { get; private set; }
    public bool Equpped{ get; private set; }
    private IPublisher _publisher;
    private int _idSlot;
    public ICard CardGame { get; private set; }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (Empty) return;
        if (!Equpped)   
        {
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoSetIdSlotInventory, _idSlot));
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenPlaceInSlot, CardGame));
        }
        else
        {
            var args = new CustomEventArgs(GameEventName.GoSetIdSlotInventory, _idSlot);
            _publisher.Publish(this,args );
            var parent = gameObject;
            args = new CustomEventArgs(GameEventName.GoUnPlaceInSlot, parent);
            _publisher.Publish(this,args );
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenUnequipmentSlot, CardGame));
        }    
    }
    public void SetDependecies(IPublisher publisher)
    {
        _publisher = publisher;
    }
    public void SetVarsSlot(bool flagEmpty, bool flagEqupped)
    {
        Empty = flagEmpty;
        Equpped = flagEqupped;
    }
    public void SetVarsSlot(bool flagEmpty, bool flagEqupped, int index, ICard card)
    {
        Empty = flagEmpty;
        Equpped = flagEqupped;
        _idSlot = index;
        if (card == null) return;
        CardGame = card;
    }
}
