using System;
using UnityEngine;

namespace Gameplay.Camera
{
    public class CameraTarget : MonoBehaviour, ICameraTarget
    {
        [SerializeField]
        private float _MoveSpeed;
        [SerializeField] 
        private float _RotateSpeed;
        public Vector3 LookPosition => transform.position;

        public Quaternion LookRotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        public bool UseRotation => true;

        private void Update()
        {
            var horizontal = Input.GetAxis("CameraHorizontal");
            var vertical = Input.GetAxis("CameraVertical");
            // var upDown = Input.GetAxis("CameraUpDown");
            var rotateY = Input.GetAxis("CameraRotateByY");
            
            var verticalProject = Vector3.ProjectOnPlane(CameraBehaviour.Instance.transform.forward, Vector3.up);
            
            transform.position += verticalProject * vertical * _MoveSpeed * Time.deltaTime;
            transform.position += CameraBehaviour.Instance.transform.right * horizontal * _MoveSpeed * Time.deltaTime;
            // transform.position += transform.up * upDown * _MoveSpeed * Time.deltaTime;
            transform.RotateAround(Vector3.up, rotateY * _RotateSpeed * Time.deltaTime);
            
        }
    }
}