using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiButtonsPopups : MonoBehaviour, ISubscriber
{
    private IPublisher _publisher;
    private Membership _membership;
    /*public void ToSayNoClass()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoClosePopupGameCLass));
    }
    public void ToSayYesClass()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoStageSelectGameClass));
    }*/
    private void Awake()
    {
        _membership = Membership.Undefined;
    }

    public void ToSayNoEquip()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoClosePlaceInSlot));
    }
    public void ToSayYesEquip()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoSelectCardInInventory));
    }
    public void ToSayCloseInventory()
    {
        _publisher.Publish(this, new CustomEventArgs(GameEventName.GoCloseInventory));
    }
    public void SetDependecies( IPublisher publisher)
    {
        _publisher = publisher;
    }
    public void ToSayEndTurn()
    {
        if (_membership == Membership.Player)
        {
            _publisher.Publish(this,new CustomEventArgs(GameEventName.GoNextTurn));
            _publisher.Publish(this,new CustomEventArgs(GameEventName.GoEndTurnPlayer));
        }
    }
    public void ToSayOpenMenu()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoOpenGameMenu));
    }
    public void ToSayCloseMenu()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoCloseGameMenu));
    }
    public void ToSayClosePopupReward()
    {
        _publisher.Publish(this,new CustomEventArgs(GameEventName.GoClosePopupReward));
    }
    public void ToSayStartNewGame()
    {
        SceneManager.LoadScene("Bootstrapper");
    }
    public void ToSayQuitApplication()
    {
        Application.Quit();
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoNextTurn:
            {
                if (_membership == Membership.Undefined)
                {
                    _membership = Membership.Player;
                }
                else
                {
                    _membership = _membership == Membership.Player ? Membership.Enemy : Membership.Player;
                }
                return;
            }
            case GameEventName.GoFinishBattle:
                _membership = Membership.Undefined;
                break;
        }
    }
}
