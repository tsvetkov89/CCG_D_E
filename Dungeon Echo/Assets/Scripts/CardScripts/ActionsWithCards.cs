using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionsWithCards: Card, IPointerClickHandler
{
    public bool Selected { get; private set; }
    public int IdCard{ get; private set; }
    public ICard CardGame { get; private set; }
    
    private IPublisher _publisher;
    private IAnimaManager _animaManager;
    private RectTransform _rectTransform;
    public Animator Animator{ get; private set; }
    public void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        Animator = gameObject.GetComponent<Animator>();
    }
    public void СhangeCardType(ICard card)
    {
        CardGame = card;
        Selected = false;
    }
    public void SetIdCard(int id)
    {
        IdCard = id;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Selected = true;
        _animaManager.SetStateAnimation(gameObject, "select_num", IdCard);
        var type = CardGame.GetDataCard().TypeSubCard;
        if (type == SubTypeCard.LightEnemy)
        {
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenPopupNoDescription, CardGame));
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoClosePopupDescriptionCard, CardGame));
            return;
        }
        if (type != SubTypeCard.GameArea && type != SubTypeCard.GameEvent)
        {
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoAddCardInInventory, CardGame.GetDataCard().NameCard));
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenPopupWithDescriptionCard, CardGame));
        }
        else
        {
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenPopupNoDescription, CardGame));
        }
    }
    public void SetDependecies(IPublisher publisher, IAnimaManager animaManager)
    {
        _publisher = publisher;
        _animaManager = animaManager;
    }
    public void SetActive()
    {
        _rectTransform.SetRect(0.805f,0.56f,0.965f,0.96f);
        _rectTransform.SetOffset(0,0,0,0);
        gameObject.transform.eulerAngles = new Vector3(0f,0f,0f);
        gameObject.SetActive(false);
        
    }
    public void SetConfigurateByBattle()
    {
        Animator.enabled = false; 
        //_animator.enabled = true; 
    }
}
