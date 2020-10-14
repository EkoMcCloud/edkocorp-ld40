using UnityEngine;
using EdkoCorpLD40.Managers;
using EdkoCorpLD40.Weaponry;

namespace EdkoCorpLD40.Characters
{
    public class PlayerEngine : CharacterEngine
    {
        

        override protected void Start()
        {
            base.Start();
        }

        override protected void FixedUpdate() 
        {
            base.FixedUpdate();

            if (IsAlive()) {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                Move(horizontal, vertical);
                LookAt(mousePos.x, mousePos.y);

                if (currentWeapon != null) {
                    // TODO : handle joypad button
                    WeaponEngine weaponEngine = currentWeapon.GetComponent<WeaponEngine>();
                    // TODO ajouter controls manager pour switch clavier/sourie (lastInput = clavier OU stick)
                    Vector2 position = new Vector2(transform.position.x, transform.position.y);
                    Vector2 direction = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
                    if(direction.magnitude > 0) {
                        weaponEngine.Aim(position + direction);
                        weaponEngine.Fire(gameObject, direction);                            
                    } else {
                        weaponEngine.Aim(mousePos);
                        if (Input.GetMouseButton(0)) { // PRESSED
                            //Debug.Log("Pressed left.");
                            direction = mousePos - position;
                            weaponEngine.Fire(gameObject, direction);
                        }
                    }

                    if (Input.GetMouseButtonDown(1)) { //CLICK
                        Debug.Log("Clicked right. ROULAAAAAADE");
                    }

                    if (Input.GetMouseButtonDown(2)) {
                        Debug.Log("Clicked middle.");
                    }
                }
            }
        }

        override protected void OnDead()
        {
            GameManager.instance.OnGameOver();
        }
    }
}