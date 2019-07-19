using  UnityEngine;

namespace InterfaceNamespace
{
    public interface ICard
    {
        void DisplayCardInGame<T>(T obj) where T : Object;
        DataCard GetDataCard();
    }
}