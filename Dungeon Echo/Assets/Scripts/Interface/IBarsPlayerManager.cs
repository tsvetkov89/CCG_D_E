using EnumNamespace;
using UnityEngine;

namespace InterfaceNamespace
{
    public interface IBarsPlayerManager
    {
        bool PlayerIsAlive(GameClass gameClass);
        bool PlayerIsMana(GameClass gameClass);     
    }
}
