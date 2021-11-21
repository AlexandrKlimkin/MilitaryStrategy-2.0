using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAttachedParticleEffect : ParticleEffect {

    public Transform Parent { get; set; }

    protected override void OnParticleEffectStarted() {
        transform.SetParent(Parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    protected override void OnParticleEffectComplete() {
        transform.SetParent(EffectsHost);
    }

    public void ResetLocalPosition() {
        transform.localPosition = Vector3.zero;
    }
}