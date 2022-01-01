using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationInstancing.AnimationInstancing))]
public class AnimationController : MonoBehaviour
{
    public int AnimationIndex;
    public bool Loop;
    
    [Button]
    public bool _PlayAnimation;

    public Actor Actor { get; private set; }
    public AnimationInstancing.AnimationInstancing AnimationInstancing { get; private set; }

    private void Awake()
    {
        Actor = GetComponentInParent<Actor>();
        AnimationInstancing = GetComponent<AnimationInstancing.AnimationInstancing>();
    }

    private void OnPlayAnimation()
    {
        if(Loop)
            PlayAnimationLoop(AnimationIndex);
        else
        {
            PlayAnimation(AnimationIndex);
        }
    }

    private void PlayAnimation(int index)
    {
        if (AnimationIndex < AnimationInstancing.GetAnimationCount())
        {
            AnimationInstancing.Stop();
            AnimationInstancing.CrossFade(AnimationIndex, 0.2f);
        }
    }

    private Coroutine _loopAnimationCoroutine;
    private void PlayAnimationLoop(int index)
    {
        AnimationIndex = index;
        StopAllCoroutines();
        _loopAnimationCoroutine = StartCoroutine(LoopAnimationRoutine());
    }

    private IEnumerator LoopAnimationRoutine()
    {
        while (true)
        {
            var animationInfo = AnimationInstancing.GetCurrentAnimationInfo();
            var fps = animationInfo.fps;
            var frames = animationInfo.totalFrame;
            var animationTime = frames / fps;
            PlayAnimation(AnimationIndex);
            yield return new WaitForSeconds(animationTime);
        }
    }
}
