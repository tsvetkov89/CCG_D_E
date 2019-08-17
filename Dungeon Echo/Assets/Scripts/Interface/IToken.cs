using  UnityEngine;
namespace InterfaceNamespace
{
    public interface IToken
    {
        void DisplayToken<T>(T obj) where T : Object;
        DataToken GetDataToken();
    }
}