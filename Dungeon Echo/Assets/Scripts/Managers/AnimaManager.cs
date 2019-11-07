using UnityEngine;
using InterfaceNamespace;

public class AnimaManager : IAnimaManager //Наследуем класс и реализуем интерфейс.
{
    public void SetStateAnimation(GameObject obj, string name, bool flag)
    {
        var animator = obj.GetComponent<Animator>();
        if (animator != null)
            obj.GetComponent<Animator>().SetBool(name, flag);
    }

    public void SetStateAnimation(GameObject obj, string name, int i)
    {
        obj.GetComponent<Animator>().SetInteger(name, i);
    }
}
