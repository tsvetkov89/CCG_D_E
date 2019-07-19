using UnityEngine;

namespace InterfaceNamespace
{
    public interface IEnemyManager
    {
        GameObject GetRandomEnemy();
        bool EnemyIsLive();
    }
}