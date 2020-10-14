using System.Collections;
using UnityEngine;

namespace EdkoCorpLD40.Weaponry
{
    public class ShotgunEngine : WeaponEngine
    {
        public int nbProjectiles = 5;
        public float precision = 0.8f;

        override public void Fire(GameObject shooter, Vector2 direction)
        {
            if(!cooldownRunning) {
                Vector2 startingPos = new Vector2(shooter.transform.position.x, shooter.transform.position.y);

                for(int i = 0; i < nbProjectiles; i++) {
                    GameObject shot = Instantiate(bullet, startingPos, Quaternion.identity) as GameObject;
                    direction = direction.normalized; // Prevent the shotgun to be more precise if a far away point is aimed
                    
                    // create GameObject bullet and throw lol
                    BulletEngine bulletEngine = shot.GetComponent<BulletEngine>();
                    Vector2 bulletDirection = new Vector2(direction.x, direction.y);
                    float xRange = precision < 1 ? bulletDirection.x - bulletDirection.x * precision : bulletDirection.x;
                    float yRange = precision < 1 ? bulletDirection.y - bulletDirection.y * precision : bulletDirection.y;
                    bulletDirection.x = Random.Range(bulletDirection.x - xRange, bulletDirection.x + xRange); 
                    bulletDirection.y = Random.Range(bulletDirection.y - yRange, bulletDirection.y + yRange); 
                    bulletEngine.Init(shooter, bulletDirection); // pass startingPos pour affiner le depart ?
                    shot.transform.SetParent(shooter.transform.parent);
                }
                
                StartCooldown();
            }
        }
    }
}