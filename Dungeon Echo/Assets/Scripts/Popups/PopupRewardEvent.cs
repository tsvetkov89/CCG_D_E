
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
        }
    }

    public void SetDependecies(GameObject popupRewardEvent, GameObject btn)
    {
        _popupReward = popupRewardEvent;
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
        _tokensReward.Clear();
        var count = 0;
        yield return new WaitForSeconds(pause);
        var flag = true;
        foreach (var item in tokens)
        {
            var token = _objectStorage.GetTokenByName(item);
            var data = token.GetDataToken();
            
            if (data.Use == TokenUse.Auto)
                _tokensReward.Add(data.Token);
            else
                flag = false;
            foreach (var objToken in _poolTokensReward)
            {
                if(objToken.activeSelf) continue;
                token.DisplayToken(objToken);
                var rect = objToken.GetComponent<RectTransform>();
                rect.OffsetAnchorY(-0.18f*count);
                objToken.SetActive(true);
                ++count;
                break;
            }
        }

        if (flag)
            _buttonContinue.interactable = true;

    }
}
