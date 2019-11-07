using UnityEngine;

namespace InterfaceNamespace
{
    public interface IActivateCardManager
    {
        bool PossiblePlayPlayerCard(ICard card);
        bool IsManaPlayPlayerCard(ICard card);
        void SetDependecies(GameObject obj);
    }
}