using UnityEngine;
using UnityEngine.EventSystems;


namespace Camera
{
    public class CameraZoom : MonoBehaviour
    {
        public float minSize = 1;
        public float maxSize = 8;
        public float sensitivity = 150;
        [Min(1)]
        public float damping = 1.4f;
        private float speed;
        private new UnityEngine.Camera camera;


        private void Awake()
        {
            camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                float scrollInput = Input.GetAxis("Mouse ScrollWheel");
    
                float scroll = scrollInput * Time.deltaTime * sensitivity * -1;
                speed += scroll;
    
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + speed, minSize, maxSize);
                speed /= damping;
            }
        }
    }
}