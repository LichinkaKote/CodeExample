using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Core.Data
{
    public class EnemyData
    {
        public uint id;
        public string name;
        public string desc;
        public int textureID;
        public int bloodID;
        public float size;
        public float health;
        public float moveSpeed;
        public float moveSpeedRange;
        public float? mass;
        public float? ragdollResistance;
        public DamageData attackDamage;
        public float attackCD;
        public float attackDistance;
        public bool isRanged;
        public uint[] debuffs;
        public EnemyDamageResistanceData damageResistance;
        public int? exp;

        [JsonIgnore]
        public EnemyDamageResistanceData DamageResistance => damageResistance == null ? new EnemyDamageResistanceData() : damageResistance;
        [JsonIgnore]
        public string Texture => Strings.EnemyTexture + textureID.ToString();
        [JsonIgnore]
        public string DeadTexture => Strings.EnemyTextureDead + textureID.ToString();
        [JsonIgnore]
        public string BloodTexture => Strings.EnemyTextureBlood + bloodID.ToString();
        [JsonIgnore]
        public Sprite Icon => GetIcon();

        private Sprite GetIcon()
        {
            if (textureID < Game.Library.SpriteLib.Enemies.Length)
                return Game.Library.SpriteLib.Enemies[textureID];
            return null;
        }
    }
}