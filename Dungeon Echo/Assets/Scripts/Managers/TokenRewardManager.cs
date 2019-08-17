using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;

public class TokenRewardManager: ITokenRewardManager, ISubscriber
{
    private IPublisher _publisher;

    public TokenRewardManager(IPublisher publisher)
    {
        _publisher = publisher;
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
}
