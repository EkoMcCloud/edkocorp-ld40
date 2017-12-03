using UnityEngine;

namespace EdkoCorpLD40.Characters
{
    public class NpcEngine : CharacterEngine
    {
        protected override void FixedUpdate() 
        {
            base.FixedUpdate();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null) {
                LookAt(player.transform.position.x, player.transform.position.y);
            }
        }
    }
}