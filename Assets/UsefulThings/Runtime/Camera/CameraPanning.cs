using UnityEngine;


namespace UsefulThings.Camera
{
    public class CameraPanning : MonoBehaviour
    {
        public float edgeSize = 10;
        public float panSpeed = 3;
        public bool isMouseControlled = true;
        public bool isKeyboardControlled = true;
        [Min(1)]
        public float damping = 1.15f;
        public Vector2 bounds = Vector2.positiveInfinity;

        private Vector3 velocity = Vector3.zero;

        private void Update()
        {
            // Pan right.
            if ((isMouseControlled && Input.mousePosition.x > Screen.width - edgeSize) || (isKeyboardControlled && Input.GetKey(KeyCode.D)))
            {
                velocity.x += panSpeed * Time.deltaTime;
            }
    
            // Pan left.
            if ((isMouseControlled && Input.mousePosition.x < edgeSize) || (isKeyboardControlled && Input.GetKey(KeyCode.A)))
            {
                velocity.x -= panSpeed * Time.deltaTime;
            }
    
            // Pan up.
            if ((isMouseControlled && Input.mousePosition.y > Screen.height - edgeSize) || (isKeyboardControlled && Input.GetKey(KeyCode.W)))
            {
                velocity.y += panSpeed * Time.deltaTime;
            }
    
            // Pan down.
            if ((isMouseControlled && Input.mousePosition.y < edgeSize) || (isKeyboardControlled && Input.GetKey(KeyCode.S)))
            {
                velocity.y -= panSpeed * Time.deltaTime;
            }
    
            Vector3 pos = transform.position;

            // Panning
            pos += velocity;
            velocity /= damping;
    
            // Clamping
            float clampedX = Mathf.Clamp(pos.x, -bounds.x, bounds.x);
            float clampedY = Mathf.Clamp(pos.y, -bounds.y, bounds.y);
            pos = new Vector3(clampedX, clampedY, pos.z);

            transform.position = pos;
        }
    }
}