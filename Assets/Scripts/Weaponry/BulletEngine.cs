using UnityEngine;
using EdkoCorpLD40.Characters;

namespace EdkoCorpLD40.Weaponry
{
    public class BulletEngine : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 1;

        public float ttl = 10f; // Bullet lifespan (in seconds)

        protected GameObject shooter;
        protected Vector2 direction;

        protected static int BUFF = 1;

        //rajouter time to live pour auto destruction ?
        
        // Use this for initialization
        public void Init (GameObject shooter, Vector2 direction)
        {
            this.shooter = shooter;
            this.direction = direction;

            transform.localScale = new Vector2(transform.localScale.x * BUFF, transform.localScale.y * BUFF);
            float angleRad = Mathf.Atan2(direction.y, direction.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;

            this.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

            Invoke("SelfDestruct", ttl); // Autodestroy to prevent ghost gameObject accumulation 
        }
        
        // Update is called once per frame
        protected void Update ()
        {
            // move toward direction
            // on normalize le vecteur de direction
            Vector2 move = direction.normalized * speed * Time.deltaTime;
            transform.position += new Vector3(move.x, move.y, 0.0f);

            // Rotate transform pour direction de balle
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Level") {
                SelfDestruct();
            } else if (collision.tag == "Enemy" || collision.tag == "Player") { // pour friendly fire
                // immune owner
                if(shooter == null || collision.gameObject.GetInstanceID() != shooter.GetInstanceID()) {
                    CharacterEngine character = collision.gameObject.GetComponent<CharacterEngine>();
                    // hpLoss + self destroy
                    if (character != null && character.Damage(damage * BUFF)) {
                        SelfDestruct();
                    }
                }
            }
        }

        protected void SelfDestruct()
        {
            // gameObject.SetActive(false); //+self destroy
            // Debug.Log("SELF DEEESTRUCT");
            Destroy(this.gameObject); 
        }
    }
}