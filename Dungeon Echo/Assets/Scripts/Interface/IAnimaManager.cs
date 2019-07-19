using UnityEngine;
namespace InterfaceNamespace
{
    public interface IAnimaManager
    {
        void SetStateAnimation(GameObject obj, string name, bool flag);
        void SetStateAnimation(GameObject obj, string name, int num);
    }
}