using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.Animators
{
    public class SpineAnimator : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation _animation;
        [SerializeField] private Transform body;
        public void Play(string animation, bool flip = false, bool loop = true, float animSpeed = 1f)
        {
            _animation.AnimationName = animation;
            _animation.timeScale = animSpeed;
            _animation.loop = loop;

            int mult = flip ? -1 : 1;
            var x = Mathf.Abs(body.localScale.x) * mult;
            body.localScale = new Vector3(x, body.localScale.y, body.localScale.z);
        }
        public void PlayParralel(int index, string animation, bool loop = true)
        {
            _animation.state.ClearTrack(index);
            _animation.state.SetAnimation(index, animation, loop);
        }
    }
}