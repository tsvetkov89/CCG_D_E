using EnumNamespace;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface ITokenRewardManager
    {
        void CreateIconTokenByGameClass(GameClass gameClass);
        void SetDependecies(GameObject panelPlayes);
    }
}