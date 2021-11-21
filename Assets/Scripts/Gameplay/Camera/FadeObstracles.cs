using Assets.Scripts.Gameplay.Camera;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

namespace Game.Rendering {
    public class FadeObstracles : MonoBehaviour {
        public Transform Target = null;

        public float FadeToAlpha = 0.25f;
        public float FadeTime = 0.4f;
        public float UnfadeTime = 0.25f;
        
        private List<RenderersStorage> _FadedStorages = new List<RenderersStorage>();
        private List<RenderersStorage> _FrameStorages = new List<RenderersStorage>();

        private void Update() {
            _FrameStorages.Clear();
            var delta = Target.position - transform.position;
            var hits = Physics.RaycastAll(transform.position, delta, delta.magnitude, Constants.Layers.Masks.DamageObstracle);
            foreach (var hit in hits) {
                var renderersStorage = hit.collider.gameObject.GetComponent<RenderersStorage>();
                if (renderersStorage != null) {
                    _FrameStorages.Add(renderersStorage);
                    if (!_FadedStorages.Contains(renderersStorage)) {
                        FadeRenderersStorage(renderersStorage);
                    }
                }
            }

            Debug.DrawLine(transform.position, transform.position + delta);

            var unfaded = _FadedStorages.Where(_ => !_FrameStorages.Contains(_)).ToList();
            unfaded.ForEach(_ => UnFadeRenderersStorage(_));
            //_FadedStorages.RemoveAll(_ => unfaded.Contains(_));
        }

        private void FadeRenderersStorage(RenderersStorage storage) {
            _FadedStorages.Add(storage);
            foreach (var renderer in storage.Renderers) {
                SetMaterialTransparent(renderer);
            }
            iTween.FadeTo(storage.gameObject, FadeToAlpha, FadeTime);
        }

        private void SetMaterialTransparent(Renderer renderer) {
            foreach (Material m in renderer.materials) {
                m.SetFloat("_Mode", 2);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 1);
                m.DisableKeyword("_ALPHATEST_ON");
                m.EnableKeyword("_ALPHABLEND_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.renderQueue = 3000;
            }
        }


        private void SetMaterialOpaque(Renderer renderer) {
            foreach (Material m in renderer.materials) {
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                m.SetInt("_ZWrite", 1);
                m.DisableKeyword("_ALPHATEST_ON");
                m.DisableKeyword("_ALPHABLEND_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.renderQueue = -1;
            }
        }

        private void UnFadeRenderersStorage(RenderersStorage storage) {
            StartCoroutine(UnFadeRenderersStorageRoutine(storage));
        }

        private IEnumerator UnFadeRenderersStorageRoutine(RenderersStorage storage) {
            iTween.FadeTo(storage.gameObject, 1, UnfadeTime);
                yield return new WaitForSeconds(0.25f);
            foreach (var renderer in storage.Renderers) {
                SetMaterialOpaque(renderer);
            }
            _FadedStorages.Remove(storage);
        }
    }
}