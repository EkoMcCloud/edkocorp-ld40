using UnityEngine;

namespace EdkoCorpLD40.Managers
{
    public class CameraManager : MonoBehaviour 
    {
        public int xMin = int.MinValue;
        public int xMax = int.MaxValue;
        public int yMin = int.MinValue;
        public int yMax = int.MaxValue;

        private GameObject target;
        private bool followTarget = true;

        private float dampTime = 0.3f; //offset from the viewport center to fix damping
        private Vector3 velocity = Vector3.zero;

        public static CameraManager instance;

        public void Follow(GameObject gameObject)
        {
            target = gameObject;
            followTarget = true;
        }

        protected void Awake()
        {
            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        protected void LateUpdate()
        {
            if (target != null && followTarget) {
                // Rajouter move "smooth" pour deplacement lissé
                Vector3 point  = Camera.main.WorldToViewportPoint(target.transform.position);
                Vector3 delta  = target.transform.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination  = transform.position + delta;
                if(destination.x < xMin) {
                    destination.x = xMin;
                }
                if(destination.x > xMax) {
                    destination.x = xMax;
                }
                if(destination.y < yMin) {
                    destination.y = yMin;
                }
                if(destination.y > yMax) {
                    destination.y = yMax;
                }
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }

    }
}