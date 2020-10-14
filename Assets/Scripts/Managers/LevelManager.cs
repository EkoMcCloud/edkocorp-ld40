using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EdkoCorpLD40.Characters;

namespace EdkoCorpLD40.Managers 
{
    public class LevelManager : MonoBehaviour 
    {
        public float trainSpeed = 0f;
        public int trainCoreMaxHp = 1000;
        public int coins = 0;
        public int bismuth = 0;
        public int scraps = 0;

        public List<GameObject> spawners;
        private int nbSpawners;

        public int trainCoreCurrentHp { get; private set; }

        public Transform boardHolder { get; private set; }

        private GameObject player;

        public void Init()
        {   
            player = GameObject.FindGameObjectWithTag("Player");
            boardHolder = player.transform.parent;
            CameraManager.instance.Follow(player);

            trainCoreCurrentHp = trainCoreMaxHp;
            NextLevel();
        }

        public void NextLevel()
        {
            LevelSetup();
        }

        public void Damage(int damage)
        {
            if(damage > 0 && trainCoreCurrentHp > 0) {
                trainCoreCurrentHp -= damage;
                if (trainCoreCurrentHp < 0) {
                    trainCoreCurrentHp = 0;
                }

                if (trainCoreCurrentHp == 0) {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if(player != null) {
                        PlayerEngine playerEngine = player.GetComponent<PlayerEngine>();
                        playerEngine.InstaKill();
                    }
                }
            }
        }

        public void OnSpawnerDestroyed()
        {
            nbSpawners--;
            if(nbSpawners == 0) {
                bismuth++;
                GameManager.instance.OnLevelClear();
            }
        }

        private void LevelSetup()
        {
            // TODO : stuff
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            if (spawnPoint != null) {
                nbSpawners = bismuth + 1;
                for (int i = 0; i < nbSpawners; i++) {
                    GameObject toInstanciate = RandomSpawner();
                    if (toInstanciate != null) {
                        GameObject spawner = Instantiate(toInstanciate);
                        spawner.transform.position = spawnPoint.transform.position;
                    }
                }
            }
        }


        private GameObject RandomSpawner()
        {
            if (spawners != null && spawners.Count > 0) {
                return spawners[Random.Range(0, spawners.Count - 1)];
            } else {
                return null;
            }
        }

        
    }
}