using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EdkoCorpLD40.Managers;

namespace EdkoCorpLD40.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
         public GameObject[] enemies;
        public GameObject[] bosses;

        public float minDelay = 5f;
        public float maxDelay = 10f;
        public int nbSpawn = 1;
        
        private int nbMobs;
        private List<GameObject> mobs;

        private Animator animator;

        // Use this for initialization
        protected void Start ()
        {
            animator = GetComponent<Animator>();
            nbMobs = nbSpawn;
            mobs = new List<GameObject>();
            InvokeNextSpawn();
        }

        protected void Update()
        {
            if(mobs.Count > 0 && nbMobs > 0)
            {
                for(var i =  0; i < mobs.Count; i++)
                {
                    if (mobs[i] == null)
                    {
                        Debug.Log("Killed lol");
                        nbMobs--;
                        mobs.RemoveAt(i);
                        i--;
                    }
                }
            }

            if(nbMobs == 0)
            {
                //Remplacer par un stockage de l'objet dans le boardManager et verif d'existence depuis la bas (cf check mobs ici) pour decouplage de cet objet
                GameManager.instance.levelManager.OnSpawnerDestroyed();
                Destroy(this.gameObject);
            }
        }
        
        protected void InvokeNextSpawn()
        {
            float delay = Random.Range(minDelay, maxDelay);
            Invoke("Spawn", delay);
        }

        protected void Spawn()
        {
            if(nbSpawn > 0)
            {
                nbSpawn--;
                Debug.Log("Spawn remaining:"+nbSpawn);
                GameObject toInstantiate = (nbSpawn > 0) ? RandomEnemy() : RandomBoss();
                GameObject instance = Instantiate(toInstantiate, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity) as GameObject;

                mobs.Add(instance);

                instance.transform.SetParent(GameManager.instance.levelManager.boardHolder);
                animator.SetTrigger("Pop");
                InvokeNextSpawn();
            }
        }

        protected GameObject RandomEnemy()
        {
            Debug.Log("RandomEnemy");
            return enemies[Random.Range(0, enemies.Length)];
        }

        protected GameObject RandomBoss()
        {
            Debug.Log("RandomBoss");
            GameObject boss;

            if (bosses != null && bosses.Length != 0)
                boss = bosses[Random.Range(0, bosses.Length)];
            else
                boss = RandomEnemy();

            return boss;
        }
    } 
}