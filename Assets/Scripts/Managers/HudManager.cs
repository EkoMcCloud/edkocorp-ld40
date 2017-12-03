using UnityEngine;
using UnityEngine.UI;
using EdkoCorpLD40.Characters;

namespace EdkoCorpLD40.Managers
{
    public class HudManager : MonoBehaviour
    {
        public Text txHealth;
        public Text txCoins;
        public Text txTrainStatus;
        public Text txBismuth;
        public Text txScraps;

        protected void Awake()
        {
            this.UpdateAll();
        }

        protected void FixedUpdate()
        {
            this.UpdateAll();
        }

        protected void UpdateAll()
        {
            this.UpdateHealth();
            this.UpdateCoins();
            this.UpdateTrainStatus();
            this.UpdateBismuth();
            this.UpdateScraps();
        }

        protected void UpdateHealth()
        {
            if (txHealth != null && GameManager.instance != null && GameManager.instance.levelManager != null) {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                int value = 0; 
                if (player != null) {
                    PlayerEngine playerEngine = player.GetComponent<PlayerEngine>();
                    value = playerEngine.currentHp;
                }
                 txHealth.text = value.ToString();
            }
        }

        protected void UpdateCoins()
        {
            if (txCoins != null && GameManager.instance != null && GameManager.instance.levelManager != null) {
                txCoins.text = GameManager.instance.levelManager.coins.ToString();
            }
        }

        protected void UpdateTrainStatus()
        {
            if (txTrainStatus != null && GameManager.instance != null && GameManager.instance.levelManager != null) {
                int percent = 0;
                if (GameManager.instance.levelManager.trainCoreMaxHp > 0) {
                    percent = Mathf.RoundToInt((float)GameManager.instance.levelManager.trainCoreCurrentHp / (float)GameManager.instance.levelManager.trainCoreMaxHp * 100f);
                }
                txTrainStatus.text = percent.ToString() + "%";
            }
        }

        protected void UpdateBismuth()
        {
            if (txBismuth != null && GameManager.instance != null && GameManager.instance.levelManager != null) {
                txBismuth.text = GameManager.instance.levelManager.bismuth.ToString();
            }
        }

        protected void UpdateScraps()
        {
            if (txScraps != null && GameManager.instance != null && GameManager.instance.levelManager != null) {
                txScraps.text = GameManager.instance.levelManager.scraps.ToString();
            }
        }
    }
}