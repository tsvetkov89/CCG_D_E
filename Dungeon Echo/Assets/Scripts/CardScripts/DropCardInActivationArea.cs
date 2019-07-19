using System;
using UnityEngine;
using UnityEngine.EventSystems;
using EnumNamespace;
using InterfaceNamespace;

public class DropCardInActivationArea : Card, IDropHandler,IPointerEnterHandler
{
    private IPublisher _publisher;
    private IActivateCardManager _activateCardManager;
    private IAnimaManager _animaManager;
    

    public void OnDrop(PointerEventData eventData)
    {
        var dropObject = eventData.pointerDrag;
        if (!dropObject.HasComponent<DraggableCard>()) return;
        var component = dropObject.GetComponent<ActionsWithCards>();
        var card = component.CardGame;
        if (!_activateCardManager.PossiblePlayPlayerCard(card)) return;
        if (!_activateCardManager.IsManaPlayPlayerCard( card)) return;
        dropObject.SetActive(false);
        dropObject.SetActive(true);
        var draggable = dropObject.GetComponent<DraggableCard>();
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoActivateCard, card));
        draggable.ParentToReturnTo = transform;
        draggable.enabled = false;
        component.Animator.enabled = true;

        //_animaManager.SetStateAnimation(dropObject,"go_hand",false);
        //var ancohors = dropObject.GetComponent<RectTransform>().SetAnachors();
        //dropObject.GetComponent<RectTransform>().SetRect(ancohors[0],ancohors[1],ancohors[2],ancohors[3]);
        _animaManager.SetStateAnimation(dropObject,"dissolve",true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
    public void SetDependecies(IPublisher publisher, IActivateCardManager activateCardManager, IAnimaManager animaManager)
    {
        _publisher = publisher;
        _activateCardManager = activateCardManager;
        _animaManager = animaManager;
    }
}
