using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
public class UiButtonsMenu : MonoBehaviour {
    
    private ISaveManager   _saveManager;
    private IAnimaManager  _animaManager;
    private IPublisher     _publisher;
	public void ToNewGame () {

        if (_saveManager.CheckLoad())
        {
            var parent = transform.parent;
            _animaManager.SetStateAnimation(parent.gameObject, "new_if_save", true);
        }
        else
        {
            var parent = transform.parent;
            _animaManager.SetStateAnimation(parent.gameObject, "fading", true);
            _publisher.Publish(this, new CustomEventArgs(GameEventName.GoStageStartNewGame,null) );
        }
    }
    public void ToContinueGame()
    {
        var parent = transform.parent;
        _animaManager.SetStateAnimation(parent.gameObject, "fading", true);
    }
    public void ToSayNo()
    {
        var parent = transform.parent;
        _animaManager.SetStateAnimation(parent.gameObject, "open", false);
        _animaManager.SetStateAnimation(parent.gameObject, "new_if_save", false);
    }
    public void ToSayYes()
    {
        var parent = transform.parent;
        _animaManager.SetStateAnimation(parent.gameObject, "fading", true);
        _saveManager.DeleteSave();
        _publisher.Publish(this, new CustomEventArgs(GameEventName.GoStageStartNewGame,null) );
    }
    public void SetDependecies(ISaveManager saveManager, IAnimaManager animaManager, IPublisher publisher)
    {
        _saveManager = saveManager;
        _animaManager = animaManager;
        _publisher =  publisher;
    }
}
