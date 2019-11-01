
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;

public class PopupPlaceInSlot: IPopupPlaceInSlot, ISubscriber
{
    private IPublisher _publisher;
    private GameObject _popupPlaceInSlot;    //вспылывающее окно, просмотр карт в инвентаре
    private GameObject _cardView;
    private GameObject _shirtCard;
    private ICard _cardGame;
    private bool _flagPopup;
    private bool _flagUnEqupment;
    private TMP_Text _fullDescription;
    public PopupPlaceInSlot(IPublisher publisher)
    {
        _publisher = publisher;
    }
    //--------обрабатываем события
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoOpenPlaceInSlot when _flagPopup:
                return;
            case GameEventName.GoOpenPlaceInSlot:
            {
                _popupPlaceInSlot.transform.SetAsLastSibling();
                _popupPlaceInSlot.SetActive(true);
                var cardDisplay = _cardView.GetComponent<ActionsWithCards>();
                _cardGame = messageData.Value as ICard;
                cardDisplay.СhangeCardType(_cardGame);
                cardDisplay.CardGame.DisplayCardInGame(_cardView);
                _fullDescription.text = _cardGame.GetDataCard().FullDescription;
                _flagPopup = true;
                break;
            }
            case GameEventName.GoClosePlaceInSlot when !_flagPopup:
                return;
            case GameEventName.GoClosePlaceInSlot:
                _popupPlaceInSlot.SetActive(false);
                _flagPopup = false;
                _flagUnEqupment = false;
                break;
            case GameEventName.GoSelectCardInInventory when !_flagPopup:
                return;
            case GameEventName.GoSelectCardInInventory when !_flagUnEqupment:
                _publisher.Publish(_popupPlaceInSlot, new CustomEventArgs(GameEventName.GoEquipmentCard, _cardGame));
                _popupPlaceInSlot.SetActive(false);
                _flagPopup = false;
                break;
            case GameEventName.GoSelectCardInInventory:
                _publisher.Publish(_popupPlaceInSlot, new CustomEventArgs(GameEventName.GoUnequipmentCard, _cardGame));
                _popupPlaceInSlot.SetActive(false);
                _flagPopup = false;
                _flagUnEqupment = false;
                break;
            case GameEventName.GoOpenUnequipmentSlot when _flagPopup:
                return;
            case GameEventName.GoOpenUnequipmentSlot:
            {
                _popupPlaceInSlot.transform.SetAsLastSibling();
                _popupPlaceInSlot.SetActive(true);
                var cardDisplay = _cardView.GetComponent<ActionsWithCards>();
                _cardGame = messageData.Value as ICard;
                cardDisplay.СhangeCardType(_cardGame);
                cardDisplay.CardGame.DisplayCardInGame(_cardView);
                _fullDescription.text = _cardGame.GetDataCard().FullDescription;
                _flagPopup = true;
                _flagUnEqupment = true;
                break;
            }
        }
    }
    public void SetDependecies( GameObject popupPlace, GameObject card)
    {
        _popupPlaceInSlot = popupPlace;
        _cardView = card;
        _flagPopup = false;
        _flagUnEqupment = false;
        var animator = _cardView.GetComponent<Animator>();
        animator.enabled = false;
        var component = _cardView.GetComponentsInChildren<Transform>().SearchChild("ShirtCard");
        component.gameObject.SetActive(false);
        var description = _popupPlaceInSlot.GetComponentsInChildren<Transform>().SearchChild("FullDescription");
        _fullDescription = description.GetComponent<TMP_Text>();   
    }
}