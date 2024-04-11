using UnityEngine;
using Ziumper.Shooter.Weapons;

namespace Ziumper.Shooter
{
    public class CameraRecoil : MonoBehaviour
    {
        [SerializeField] private float snapiness;
        [SerializeField] private float returnSpeed;
        [SerializeField] private float offsetClampPitch = -20f;

        private Vector3 targetRoation;
        private Vector3 currentRotation;
        

        private PlayerCameraLook cameraLook;

        private void Start()
        {
            cameraLook = GetComponentInParent<PlayerCameraLook>();
        }

        // Update is called once per frame
        void Update()
        {
            targetRoation = Vector3.Lerp(targetRoation, Vector3.zero, Time.deltaTime * returnSpeed);
            currentRotation = Vector3.Slerp(currentRotation, targetRoation, Time.fixedDeltaTime * snapiness);
            transform.localRotation = Quaternion.Euler(currentRotation);
        }

        public void RecoilShot(RecoilSettings settings)
        {
            var recoilVector =  new Vector3(settings.RecoilX, Random.Range(-settings.RecoilY, settings.RecoilY), Random.Range(-settings.RecoilZ, settings.RecoilZ));
            var recoilQuaterninon = Quaternion.Euler(recoilVector);


            Quaternion localRotation = transform.localRotation;
            float currentAngle = localRotation.x;
            currentAngle += recoilQuaterninon.x + cameraLook.transform.localRotation.x;
            float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(currentAngle);

            
          
            if (cameraLook.IsInClamp(pitch+offsetClampPitch))
            {
                targetRoation += recoilVector;
            }
        }
    }
}
