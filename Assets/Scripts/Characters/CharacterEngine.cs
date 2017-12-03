using UnityEngine;
using EdkoCorpLD40.Weaponry;

namespace EdkoCorpLD40.Characters
{
    public class CharacterEngine : MonoBehaviour
    {
        public float maxSpeed = 5f;
        public float jumpForce = 8f;
        public float maxJetpackSpeed = 6f;
        public float jetpackForce = 100f;
        public float moveForce = 200f;
        public int maxHp = 1; //uniquement sur enemie & player ? pnj ?
        public float damageCooldown = 0f; 
        public int maxFuel = 0;
        public bool regenFuel = false;

        public GameObject weapon;

        protected GameObject currentWeapon;

        protected Vector2 lastPos;
        protected bool grounded = true;
        protected int currentHp;
        protected int currentFuel;
        protected bool vulnerable = true;
        protected Color bloodColor = new Color(1.0f, 0.0f, 0.0f, 0.3f);

        protected BoxCollider2D boxCollider;
        private Rigidbody2D rb2D;

        public bool IsAlive()
        {
            return currentHp > 0;
        }

        public virtual bool Damage(int damage)
        {
            bool damaged = false;
            if (vulnerable && damage > 0 && currentHp > 0) {
                currentHp -= damage;
                damaged = true;
                
                RenderDamage();
            }

            return damaged;
        }

        public bool Heal(int heal)
        {
            bool healed = false;
            if (heal > 0 && currentHp < maxHp) {
                if (currentHp + heal > maxHp) {
                    currentHp = maxHp;
                } else {
                    maxHp += heal;
                }

                healed = true;
            }

            return healed;
        }

        protected virtual void Equip(GameObject weapon)
        {
            if (currentWeapon != null) {
                currentWeapon.SetActive(false);
                Destroy(currentWeapon);
                currentWeapon = null;
            }

            currentWeapon = Instantiate(weapon, new Vector3(0, 0, 0), Quaternion.identity);
            WeaponEngine weaponEngine = currentWeapon.GetComponent<WeaponEngine>();
            weaponEngine.RegisterOwner(this.gameObject);
            currentWeapon.transform.SetParent(this.gameObject.transform.parent);
        }

        protected virtual void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            rb2D = GetComponent<Rigidbody2D>();
            if(rb2D != null) {
                rb2D.freezeRotation = true;
            }

            currentHp = maxHp;
            currentFuel = maxFuel;
        }

        protected virtual void FixedUpdate()
        {
            grounded = true;
            // grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            /*Vector2 currentPos = new Vector2(this.rb2D.transform.position.x, this.rb2D.transform.position.y);
            if (lastPos != null) {
                grounded = (lastPos.y == currentPos.y && rb2D.velocity.y == 0);
            }
            lastPos = currentPos;*/
            grounded = false;
            if (rb2D.velocity.y == 0) {
                /*Vector2 origin = rb2D.position;
                Debug.Log("Origin : " + origin);
                RaycastHit2D hit = Physics2D.Raycast(rb2D.position, Vector2.down, 1);
                Debug.DrawRay(this.transform.position, Vector2.down, Color.green);
                if (hit.collider != null) {
                    Debug.Log("Collided tag : " + hit.collider.gameObject.tag);
                    grounded = true;
                }*/
                grounded = true;
            }

            if (grounded && regenFuel && currentFuel < maxFuel) {
                currentFuel++;
            }

            if (currentWeapon == null && weapon != null) {
                Equip(weapon);
            }
            // Debug.Log("Velocity " + this.rb2D.velocity);
            // Debug.Log("Grounded " + grounded);
        }

        protected virtual void Update()
        {
            if(!IsAlive()) {
                OnDie();
            }
        }

        protected virtual void OnDie()
        {
            if (currentWeapon != null) {
                currentWeapon.SetActive(false);
                Destroy(currentWeapon.gameObject);
            }
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

        protected virtual void LookAt(float xPos, float yPos)
        {

            // 2d from top look at management
            /*Debug.Log("x: " + xPos);
            Debug.Log("y: " + yPos);

            Debug.Log("pos x: " + transform.position.x);
            Debug.Log("pos y: " + transform.position.y);*/

            /*float angleRad = Mathf.Atan2(yPos - transform.position.y, xPos - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;

            angleDeg += -90;

            this.transform.rotation = Quaternion.Euler(0, 0, angleDeg);*/

            // 2d scroller look at management
            if (xPos > transform.position.x) {
                this.transform.localScale = new Vector3(
                    Mathf.Abs(transform.localScale.x),
                    Mathf.Abs(transform.localScale.y),
                    Mathf.Abs(transform.localScale.z)
                );
            } else {
                this.transform.localScale = new Vector3(
                    -Mathf.Abs(transform.localScale.x),
                    Mathf.Abs(transform.localScale.y),
                    Mathf.Abs(transform.localScale.z)
                );
            }
        }

        protected void Move(float xDir, float yDir)
        {
            // rb2D.transform.position += new Vector3(xDir * speed, yDir * speed);
            // rb2D.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
            if(rb2D != null) {
                // Vector3 direction = new Vector3(xDir, 0);
                // if (direction.magnitude > 1) {
                //     direction = direction.normalized; // prevent sliding and normalize max speed
                // }
                // rb2D.velocity = direction * speed * 60 * Time.deltaTime;
                float force = grounded ? moveForce : (moveForce / 2);
                if (xDir * rb2D.velocity.x < maxSpeed) {
                    rb2D.AddForce(Vector2.right * xDir * force);
                }

                if (Mathf.Abs (rb2D.velocity.x) > maxSpeed) {
                    rb2D.velocity = new Vector2(Mathf.Sign (rb2D.velocity.x) * maxSpeed, rb2D.velocity.y);
                }
               
                if (yDir > 0) {
                    if (grounded) {
                        rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                        grounded = false;
                    } else if (currentFuel > 0) {
                        if (rb2D.velocity.y < maxJetpackSpeed) {
                            rb2D.AddForce(new Vector2(0f, jumpForce));
                        }
                        grounded = false;
                        currentFuel--;
                    }
                    
                    // Debug.Log("Current fuel : " + currentFuel);
                }
            }
        }

        protected void RenderDamage()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = bloodColor;
            float time = (damageCooldown != 0.0f) ? damageCooldown : 0.1f;

            Invoke("ResetMaterialColor", time);
        }

        protected void ResetMaterialColor()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}