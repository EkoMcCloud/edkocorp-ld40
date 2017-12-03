using UnityEngine;

namespace EdkoCorpLD40.Characters
{
    public class EnemyEngine : CharacterEngine 
    {
        public int damage = 1;

        // Use this for initialization
        protected override void Start ()
        {
            // bloodColor = new Color(0.0f, 0.0f, 0.0f, 0.3f);
            base.Start();
        }
        
        // Update is called once per frame
        protected override void FixedUpdate ()
        {
            base.FixedUpdate();

            if (IsAlive()) {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject core = GameObject.FindGameObjectWithTag("Core");
                Vector3 move = new Vector3();

                // TODO : shoot & jump occasionnaly 
                if (player != null) {
                    move = player.transform.position - transform.position;
                    LookAt(player.transform.position.x, player.transform.position.y);
                }

                if (core != null) {
                    move = player.transform.position - transform.position;
                }
                Move(move.x, move.y);
            } else {
                // Move(0, 0); // empecher slide (a faire autrement)
            }
        }

        protected override void OnDie()
        {
            // TODO utiliser getter pour recup temps d'anim
            float time = (damageCooldown != 0.0f) ? damageCooldown : 0.1f;

            Invoke("ProcessDie", time);
            // prevent + StartCoroutine("FadeOut") then Destroy;
        }

        protected void ProcessDie()
        {
            base.OnDie();
        }

        /*protected IEnumerator FadeOut()
        {
            //timer
            float progress = 0;
            float increment = smoothness / duration;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            while (progress < 1)
            {
                //lerp colour
                renderer.color = Color.Lerp(A, B, progress);
                //move time forward
                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }

            yield return true;
        }*/

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("COLISION");
            if (collision.tag == "Player") { // TODO ADD DAMAGE ON TRIGGER STAY sinon immune
                PlayerEngine player = collision.gameObject.GetComponent<PlayerEngine>();
                // hpLoss
                if (player != null) {
                    player.Damage(damage);
                }
            }
        }
    }
}