using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EdkoCorpLD40.Managers;

namespace EdkoCorpLD40
{
    public class Loader : MonoBehaviour 
    {
        public GameObject gameManager;

        protected void Awake()
        {
            if (GameManager.instance == null) {
                Instantiate(gameManager);
            }
        }
    }
}
