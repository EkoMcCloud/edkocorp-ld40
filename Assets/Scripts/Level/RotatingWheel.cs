using UnityEngine;
using EdkoCorpLD40.Managers;

namespace EdkoCorpLD40.Level
{
    public class RotatingWheel : MonoBehaviour
    {
        protected void FixedUpdate()
        {
            this.transform.Rotate(Vector3.forward * 60 * Time.deltaTime * - (GameManager.instance.levelManager.trainSpeed));
        }
    }
}