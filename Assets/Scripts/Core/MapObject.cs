using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class MapObject : MonoBehaviour
    {
        [SerializeField] private Transform body;
        [SerializeField] private SpriteRenderer render;

        public void Set(Sprite sprite)
        {
            render.sprite = sprite;
        }
        public void SetScale(float scale)
        {
            body.localScale = new Vector3(scale, scale, scale);
        }
    }
}