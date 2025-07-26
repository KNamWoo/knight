using System;
using UnityEngine;

namespace UnityNote {
    public class LoadingRotate : MonoBehaviour {
        [SerializeField] private float maxSpeed = 360f;
        [SerializeField] private float minSpeed = 60f;
        private void Update() {
            if (SceneLoader.instance == null) {
                return;
            }

            float progress = SceneLoader.instance.CurrentProgress;

            float rotateSpeed = Mathf.Lerp(minSpeed, maxSpeed, progress);
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }
}
