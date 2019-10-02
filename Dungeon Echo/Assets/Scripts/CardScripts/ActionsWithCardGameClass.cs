
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionsWithCardGameClass : Card , ISubscriber, IPointerDownHandler{

	private IPublisher _publisher;
	private GameEventName _event;
	public bool Selected { get;  private set; }
	public ICard CardGame { get; private set; }
	private bool _flagOpenInventory;
	public void СhangeCardType(ICard card)
	{
		CardGame = card;
		Selected = false;
		_event = GameEventName.Undefined;
	}
	public void SetSelected()
	{
		Selected = true;
	}
	public void SetEvent(GameEventName name)
	{
		_event = name;
	}

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		switch (_event)
		{
				
			case GameEventName.GoSelectTokenReward:
				Debug.Log("Add token: "+CardGame.GetDataCard().GameClass);
				_publisher.Publish(this, new CustomEventArgs(GameEventName.GoAddTokenReward, CardGame.GetDataCard().GameClass));
				break;
			default:
				if (_flagOpenInventory)
					_publisher.Publish(this, new CustomEventArgs(GameEventName.GoSelectedIconPlayer, CardGame));
				else
				{
					_publisher.Publish(this, new CustomEventArgs(GameEventName.GoSelectedIconPlayer, CardGame));
					_publisher.Publish(this, new CustomEventArgs(GameEventName.GoOpenInventory));
				}
				break;
		}
	}
	public void SetDependecies(IPublisher publisher)
	{
		_publisher = publisher;
		_flagOpenInventory = false;
	}

	public void OnEvent(CustomEventArgs messageData)
	{
		var message = messageData.Value;
		switch (message)
		{
			case GameEventName.GoOpenInventory:
				_flagOpenInventory = true;
				break;
			case GameEventName.GoCloseInventory:
				_flagOpenInventory = false;
				break;
			case GameEventName.GoOpenPanelPlayers:
				_flagOpenInventory = true;
				break;

		}
	}
}
