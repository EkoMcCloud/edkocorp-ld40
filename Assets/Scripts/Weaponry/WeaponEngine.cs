using UnityEngine;

namespace EdkoCorpLD40.Weaponry
{
    public class WeaponEngine : MonoBehaviour
    {
        public float cooldownDelay = 0.3f;
        protected bool cooldownRunning = false;
        public GameObject bullet;
        //opt
        //- ammoPerLoader:int = -1;
        //- reloadDelay:int = 0;
        //super opt
        //- magazineCurrent:int => sur perso ?
        //- magazineMax:int => sur perso ?
        private GameObject owner;

        public void RegisterOwner(GameObject gameObject) {
            owner = gameObject;
        }
        
        public virtual void Aim(Vector3 position) {
            Vector2 direction = position - this.transform.position;
            float angleRad = Mathf.Atan2(direction.y, direction.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            if(direction.x < 0) {
                // Flipping weapon if target aiming behind
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), -Mathf.Abs(transform.localScale.y));
            } else {
                // Reverting scale
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
            }

            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        }

        public virtual void Fire(GameObject shooter, Vector2 direction)
        {
            if(!cooldownRunning) {
                Vector2 startingPos = new Vector2(shooter.transform.position.x, shooter.transform.position.y);
                GameObject shot = Instantiate(bullet, startingPos, Quaternion.identity) as GameObject;
                
                // create GameObject bullet and throw lol
                BulletEngine bulletEngine = shot.GetComponent<BulletEngine>();
                bulletEngine.Init(shooter, direction); // pass startingPos pour affiner le depart ?
                shot.transform.SetParent(shooter.transform.parent);
                
                StartCooldown();
            }
        }

        protected void FixedUpdate()
        {
            if(owner != null) {
                transform.position = owner.transform.position;
            }
        }

        protected void StartCooldown()
        {
            cooldownRunning = true;
            Invoke("ResetCooldown", cooldownDelay);
        }

        protected void ResetCooldown()
        {
            cooldownRunning = false;
        }
    }
}
