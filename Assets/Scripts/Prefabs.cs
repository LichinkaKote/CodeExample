using Assets.Scripts.Core;
using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.MapGen;
using Assets.Scripts.Core.PlayerSystems;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Base;
using Assets.Scripts.UI.Windows;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Prefabs", menuName = "ScriptableObjects/Prefabs", order = 1)]
    public class Prefabs : ScriptableObject
    {
        [SerializeField] private Player player;
        [SerializeField] private Enemy enemy;
        [SerializeField] private AbstractProjectile projectile;
        [SerializeField] private ProjectilePrefabs enemyProjectiles;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private DamagePopup damagePopup;
        [SerializeField] private WindowController windowController;
        [SerializeField] private GameWorld gameWorld;
        [SerializeField] private MapObject mapObject;

        public Player Player => player;
        public Enemy Enemy => enemy;
        public AbstractProjectile Projectile => projectile;
        public DamagePopup DamagePopup => damagePopup;
        public GameUI GameUI => gameUI;
        public WindowController WindowController => windowController;
        public GameWorld GameWorld => gameWorld;
        public MapObject MapObject => mapObject;

        public AbstractProjectile GetEnemyProjectile(int projID)
        {
            return enemyProjectiles.Get(projID);
        }
    }
}