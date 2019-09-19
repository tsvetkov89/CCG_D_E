using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;

public class TokenRewardManager: ITokenRewardManager, ISubscriber
{
    private IPublisher _publisher;
    private readonly IObjectStorage _objectStorage;
    private readonly IConfigurateManager _configurateManager;
    private IDictionary<GameClass, List<GameObject>> _dictionaryIconToken;
    private List<GameObject> _poolIconToken;
    private GameObject _panelPlayers;
    public TokenRewardManager(IPublisher publisher, IObjectStorage objectStorage, IConfigurateManager configurateManager)
    {
        _publisher = publisher;
        _objectStorage = objectStorage;
        _configurateManager = configurateManager;
        _dictionaryIconToken = new Dictionary<GameClass, List<GameObject>>();
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoUseAutoReward:
                var tokensReward = messageData.Value as List<TokenRewardEnum>;
                ProcessAutoUseTokens(tokensReward);
                break;
        }
    }

    private void ProcessAutoUseTokens(IEnumerable<TokenRewardEnum> tokensReward)
    {
        foreach (var token in tokensReward)
        {
            switch (token)
            {
                case TokenRewardEnum.Pass:
                    Debug.Log("token: "+ token);
                    break;
                case TokenRewardEnum.RestoreHealth:
                    Debug.Log("token: "+ token);
                    break;
            }
        }
    }

    public void CreateIconTokenByGameClass(GameClass gameClass)
    {
        _dictionaryIconToken[gameClass] = new List<GameObject>();
        var cout = 0;
        foreach (var icon in _poolIconToken)
        {
            if(icon.activeSelf) continue;
            if(cout == 3) return;
            _dictionaryIconToken[gameClass].Add(icon);
            icon.SetActive(true);
            ++cout;
        }
    }

    public void SetDependecies(GameObject panelPlayes)
    {
        _panelPlayers = panelPlayes;
        //----------------------------
        _poolIconToken = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabIconToken, 12);
        var art = _poolIconToken[0].GetComponent<Transform>(); 
        var refRect = art.GetComponent<RectTransform>();
        var count = 0;
        foreach (var icon in _poolIconToken)
        {
            _configurateManager.ConfigurateByParent(icon, _panelPlayers, false);
            var rectIcon = icon.GetComponent<RectTransform>();
            rectIcon.SetOffset(0,0,0,0);
            rectIcon.OffsetAnchorY(-0.0825f*count);
            var artSprite = icon.GetComponent<SpriteRenderer>(); 
            _configurateManager.ConfigurateSprite(artSprite.gameObject,refRect);
            ++count;
        }
    }
}
