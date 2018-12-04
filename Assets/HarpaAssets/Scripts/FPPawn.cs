using UnityEngine;

namespace Harpa
{
    public class FPPawn : MonoBehaviour
    {
        CharacterController characterController;
        Transform cameraTransform;

        [Header("Settings")]
        public float moveSpeed;
        public float runSpeed;
        public float flySpeed;
        public float gravity;
        public float sensYaw;
        public float sensPitch;
        public float minCamPitch;
        public float maxCamPitch;

        [Header("Inputs")]
        public Vector3 moveInput;
        public float lookYaw;
        public float lookPitch;
        public bool run;
        public bool flying;
        public float fly;
        public float pitch;
        float yVelocity;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            Camera cam = GetComponentInChildren<Camera>();
            cameraTransform = cam.transform;
        }

        void Update()
        {
            transform.Rotate(0, lookYaw * sensYaw, 0);
            lookYaw = 0;
            pitch = Mathf.Clamp(pitch + lookPitch * sensPitch, minCamPitch, maxCamPitch);
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
            lookPitch = 0;

            moveInput = Vector3.ClampMagnitude(moveInput, 1);
            Vector3 deltaPosition = Vector3.zero;
            deltaPosition += transform.forward * moveInput.z;
            deltaPosition += transform.right * moveInput.x;
            deltaPosition *= run ? runSpeed : moveSpeed;
            deltaPosition *= Time.deltaTime;

            if (flying)
            {
                deltaPosition += Vector3.up * fly * flySpeed * Time.deltaTime;
                yVelocity = 0;
            }
            else if (!characterController.isGrounded)
            {
                yVelocity -= gravity * Time.deltaTime;
                deltaPosition += Vector3.up * yVelocity * Time.deltaTime;
            }
            else
            {
                yVelocity = 0;
            }

            characterController.Move(deltaPosition);
            moveInput = Vector3.zero;
            fly = 0;
        }
    }
}
