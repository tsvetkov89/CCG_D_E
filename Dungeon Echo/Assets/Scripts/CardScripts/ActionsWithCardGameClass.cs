
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionsWithCardGameClass : Card , ISubscriber, IPointerDownHandler{

	private IPublisher _publisher;
	public bool Selected { get;  private set; }
	public ICard CardGame { get; private set; }
	private bool _flagOpenInventory;
	public void СhangeCardType(ICard card)
	{
		CardGame = card;
		Selected = false;
	}
	public void SetSelected()
	{
		Selected = true;
	}

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		if (_flagOpenInventory)
			_publisher.Publish(this, new CustomEventArgs(GameEventName.GoSelectedIconPlayer, CardGame));
		else
		{
			_publisher.Publish(this, new CustomEventArgs(GameEventName.GoSelectedIconPlayer, CardGame));
			_publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenInventory));
		}
	}
	public void SetDependecies(IPublisher publisher)
	{
		_publisher = publisher;
		_flagOpenInventory = false;
	}

	public void OnEvent(CustomEventArgs messageData)
	{
		var message = messageData.Message;
		if (message == GameEventName.GoOpenInventory)
			_flagOpenInventory = true;
		else if (message == GameEventName.GoCloseInventory)
			_flagOpenInventory = false;
		else if (message == GameEventName.GoOpenPanelPlayers) _flagOpenInventory = true;
	}
}
