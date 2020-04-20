using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatHunger : MonoBehaviour, IHasHealth {
    public const int fishVal = 2;
    public int resetVal = 10;

    private int health;
    public int Health {
        get { return health; }
        private set {
            health = value > 0 ? value : 0;
            catHungerText.text = "Cat Hunger: " + health;
        }
    }

    private Text catHungerText;

    // reset when entering new level
    public void ResetHunger() { Health = resetVal; }

    public void ReciveFish() {
        Health += fishVal;
        // todo: play sound (?)
    }

    public void TakeDmage(int _) {
        --Health;
        // todo: play sound
    }

    public void DoMove() { --Health; }

    private void Start() {
        catHungerText = GameObject.Find("CatHungerText").GetComponent<Text>();
        ResetHunger();
    }
}
