using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "ProjectilePrefabs", menuName = "ScriptableObjects/ProjectilePrefabs", order = 1)]
    public class ProjectilePrefabs : ScriptableObject
    {
        [SerializeField] private AbstractProjectile[] projectiles;
        public AbstractProjectile Get(int id)
        {
            return projectiles[id];
        }
    }
}