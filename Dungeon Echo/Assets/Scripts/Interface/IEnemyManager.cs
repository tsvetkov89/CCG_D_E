using UnityEngine;

namespace InterfaceNamespace
{
    public interface IEnemyManager
    {
        GameObject GetRandomEnemy();
        void SetDependecies(GameObject panel);
        bool EnemyIsLive();
    }
}