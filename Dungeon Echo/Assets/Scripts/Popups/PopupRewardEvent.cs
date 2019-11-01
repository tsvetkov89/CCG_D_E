
using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.UI;

public class PopupRewardEvent : IPopupRewardEvent, ISubscriber
{
    private readonly IPublisher _publisher;
    private readonly IObjectStorage _objectStorage;
    private readonly IConfigurateManager _configurateManager;
    private ICoroutiner _coroutiner;
    
    private GameObject _popupReward;
    private GameObject _closePanel;
    private GameObject _btnContinue;
    private Button _buttonContinue;
    private List<GameObject> _poolTokensReward;
   // private List<TokenRewardEnum> _emptyTokens;
    private RectTransform _rectToken;
    private List<TokenRewardEnum> _tokensReward;
    public PopupRewardEvent(IPublisher publisher, IObjectStorage objectStorage, IConfigurateManager configurateManager, ICoroutiner coroutiner)
    {
        _publisher = publisher;
        _objectStorage = objectStorage;
        _configurateManager = configurateManager;
        _coroutiner = coroutiner;
        //_emptyTokens = new List<TokenRewardEnum>(){TokenRewardEnum.Pass, TokenRewardEnum.RestoreHealth};
        _tokensReward = new List<TokenRewardEnum>();
        //_rectTokensReward = new List<float>(){0.3435f,0.4242f,0.7052f,0.574f};
    }

    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoRewardEvent:
                _buttonContinue.interactable = false;
                _popupReward.transform.SetAsLastSibling();
                _popupReward.SetActive(true);
                var reward = messageData.Value as List<TokenRewardEnum>;
                _coroutiner.StartCoroutine(DisplayTokensReward(1f,reward));
                break;
            case GameEventName.GoClosePopupReward:
                _popupReward.SetActive(false);
                _publisher.Publish(null,new CustomEventArgs(GameEventName.GoUseAutoReward,_tokensReward));
                _publisher.Publish(null,new CustomEventArgs(GameEventName.GoFinishStageEvent));
                break;
            case GameEventName.GoSelectTokenReward:
            {
                _popupReward.GetComponent<Image>().raycastTarget = false;
                _closePanel.SetActive(false);
                break;
            }
            case GameEventName.GoDeActivateTargetsPlayer:
            {
                _popupReward.GetComponent<Image>().raycastTarget = true;
                _closePanel.SetActive(true);
                break;
            }
            case GameEventName.GoEndSelectTokenReward:
                var token = messageData.Value as IToken;
                var index = _tokensReward.IndexOf(token.GetDataToken().Token);
                _tokensReward.RemoveAt(index);
                var obj = _poolTokensReward[index].GetComponent<ClickHandlerTokenReward>();
                obj.DestoyIt();
                _closePanel.SetActive(true);
                checkСanСontinue();
                break;
        }
    }

    public void SetDependecies(GameObject popupRewardEvent, GameObject btn)
    {
        _popupReward = popupRewardEvent;
        _closePanel =  _popupReward.GetComponentsInChildren<Transform>().SearchChild("ClosePanelPlayer").gameObject; 
        _btnContinue = btn;
        _buttonContinue = _btnContinue.GetComponent<Button>();
        _poolTokensReward = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabToken, 3);
        var art = _poolTokensReward[0].GetComponentsInChildren<Transform>().SearchChild("SpriteArt"); 
        var rectToken = art.GetComponent<RectTransform>();
        foreach (var objToken in _poolTokensReward)
        {
            _configurateManager.ConfigurateByParent(objToken, _popupReward, false);
            objToken.GetComponent<RectTransform>().SetOffset(0,0,0,0);
            var artSprite = objToken.GetComponentsInChildren<Transform>().SearchChild("SpriteArt"); 
            _configurateManager.ConfigurateSprite(artSprite.gameObject,rectToken);
        }
          
    }
    private IEnumerator DisplayTokensReward(float pause, List<TokenRewardEnum> tokens)
    {
        yield return new WaitForSeconds(pause);
        _tokensReward = tokens;
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = _objectStorage.GetTokenByName(tokens[i]);
            var data = token.GetDataToken();
            
            foreach (var objToken in _poolTokensReward)
            {
                if(objToken.activeSelf) continue;
                token.DisplayToken(objToken);
                var rect = objToken.GetComponent<RectTransform>();
                rect.OffsetAnchorY(-0.18f*i);
                if (data.Use != TokenUse.Auto)
                {
                    objToken.AddComponent<ClickHandlerTokenReward>();
                    var component = objToken.GetComponent<ClickHandlerTokenReward>();
                    component.SetDependecies(_publisher, token);    
                }
                objToken.SetActive(true);
                break;
            }
        }
        checkСanСontinue();
    }

    private void checkСanСontinue()
    {
        foreach (var t in _tokensReward)
        {
            var token = _objectStorage.GetTokenByName(t);
            var data = token.GetDataToken();
            if (data.Use == TokenUse.Auto) continue;
            _buttonContinue.interactable = false;
            return;
        }
        _buttonContinue.interactable = true;
        _publisher.Publish(null,new CustomEventArgs(GameEventName.GoEndGiveTokensReward));
    }
}
