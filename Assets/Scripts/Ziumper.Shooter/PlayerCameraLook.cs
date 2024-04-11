using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Ziumper.Shooter
{
    public class PlayerCameraLook : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]

        [Tooltip("Sensitivity when looking around.")]
        [SerializeField]
        private Vector2 sensitivity = new Vector2(1, 1);

        [Tooltip("Minimum and maximum up/down rotation angle the camera can have.")]
        [SerializeField]
        private Vector2 yClamp = new Vector2(-60, 60);

        [Tooltip("Should the look rotation be interpolated?")]
        [SerializeField]
        private bool smooth;

        [Tooltip("The speed at which the look rotation is interpolated.")]
        [SerializeField]
        private float interpolationSpeed = 25.0f;

        #endregion

        #region FIELDS

        /// <summary>
        /// Player Character.
        /// </summary>
        private CharacterBehaviour player;

        /// <summary>
        /// The player character's rotation.
        /// </summary>
        private Quaternion rotationCharacter;
        /// <summary>
        /// The camera's rotation.
        /// </summary>
        private Quaternion rotationCamera;

        #endregion

        #region UNITY

        private void Awake()
        {
            //Get Player Character.
            player = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        }

        private void Start()
        {
            //Cache the character's initial rotation.
            rotationCharacter = player.transform.localRotation;
            //Cache the camera's initial rotation.
            rotationCamera = transform.localRotation;
        }
        private void LateUpdate()
        {
            //Frame Input. The Input to add this frame!
            Vector2 frameInput = player.IsCursorLocked() ? player.GetInputLook() : default;
            //Sensitivity.
            frameInput *= sensitivity;

            //Yaw.
            Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
            //Pitch.
            Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

            //Save rotation. We use this for smooth rotation.
            rotationCamera *= rotationPitch;
            rotationCharacter *= rotationYaw;

            //Local Rotation.
            Quaternion localRotation = transform.localRotation;

            //Smooth.
            if (smooth)
            {
                //Interpolate local rotation.
                localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
                
                localRotation = Clamp(localRotation);

                //Interpolate character rotation.
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed);
            }
            else
            {
                //Rotate local.
                localRotation *= rotationPitch;
                //Clamp.
                localRotation = Clamp(localRotation);

                //Rotate character.
                player.transform.rotation = player.transform.rotation * rotationYaw;
            }

            //Set.
            transform.localRotation = localRotation;
        }

        #endregion

        #region FUNCTIONS

        /// <summary>
        /// Clamps the pitch of a quaternion according to our clamps.
        /// </summary>
        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            //Pitch.
            float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

            //Clamp.
            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
            rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

            //Return.
            return rotation;
        }

        public bool IsInClamp(float angle)
        {
            return angle > yClamp.x && angle < yClamp.y;        }



        #endregion
    }
}