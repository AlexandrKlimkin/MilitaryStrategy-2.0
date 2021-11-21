using UnityEngine;

namespace Assets.Scripts.Gameplay.Camera {
    class RenderersStorage : MonoBehaviour {
        public Renderer[] Renderers { get; set; }

        private void Awake()
        {
            Renderers = GetComponentsInChildren<Renderer>();
        }
    }
}
