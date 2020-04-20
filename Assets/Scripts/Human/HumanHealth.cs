using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HumanHealth : MonoBehaviour, IHasHealth {
    private Text humanHealthText;

    private int health;
    public int Health {
        get => health;
        private set {
            health = value > 0 ? value : 0;
            humanHealthText.text = "Human Health: " + health;

            if(health == 0)
                GameState.Instance.GameOver();
        }
    }

    public void TakeDmage(int dmg) { Health -= dmg; }

    private void Start() {
        humanHealthText = GameObject.Find("HumanHealthText").GetComponent<Text>();
        Health = 10;
    }
}
