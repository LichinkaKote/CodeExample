using Assets.Scripts.Core.Data;
using System;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private Transform body;
        private EnemyMovement movement;
        private void Awake()
        {
            body = transform.Find("Body");
            enabled = false;
        }
        public void Init(EnemyMovement enemyMovement)
        {
            movement = enemyMovement;
            enabled = true;
        }

        private void Start()
        {
            movement.IsLookLeft.Subscribe(LookDirChanged).AddTo(this);
            SetRandomStartinFrame();
        }

        private void LookDirChanged(bool value)
        {
            var x = value ? 1f : -1f;
            body.localScale = new Vector3(x * Math.Abs(body.localScale.x), body.localScale.y, body.localScale.z);
        }
        private void SetRandomStartinFrame()
        {
            var renderer = body.GetComponent<Renderer>();
            if (renderer != null)
            {
                var matProp = new MaterialPropertyBlock();
                var mat = renderer.material;
                var frames = mat.GetFloat("_FramesCount");
                matProp.SetFloat("_FrameOffset", frames * UnityEngine.Random.Range(0f, 1f));
                renderer.SetPropertyBlock(matProp);
            }
        }

    }
}