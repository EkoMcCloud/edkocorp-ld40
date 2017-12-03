using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdkoCorpLD40.Managers
{
    public class GameManager : MonoBehaviour 
    {
        public LevelManager levelManager;
        public static GameManager instance = null;

        // TODO changer la gestion du loading pour etre sur tout s'initialise bien (tester boardHolder ou autre avant chargement niveau)

        public void OnLevelClear() {
            // TODO : Do something
            // SceneManager.LoadScene(0);
            Debug.Log("YOU WINZ");
        }

        // Use this for initialization
        protected void Awake()
        {
            Debug.Log("GameManager - Awake");

            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            this.levelManager = GetComponent<LevelManager>();
        }

        // This is called each time a scene is loaded.
        protected void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("GameManager - OnLevelFinishedLoading");
            // Setup a new board
            levelManager.Init();
        }

        protected void OnEnable()
        {
            // Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        protected void OnDisable()
        {
            // Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled. 
            // Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }

}

