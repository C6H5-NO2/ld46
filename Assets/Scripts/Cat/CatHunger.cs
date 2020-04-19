using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatHunger : MonoBehaviour, IHasHealth {
    public Text catHungerText;

    public const int fishVal = 2;

    private int health;
    // This is acturally hunger value.
    public int Health {
        get { return health; }
        private set {
            health = value;
            catHungerText.text = "Cat Hunger: " + health;
        }
    }

    // reset when entering new level
    public void ResetHunger(int h) { Health = h; }

    public void ReciveFish() {
        Health += fishVal;
        // todo: play sound (?)
    }

    public void TakeDmage(int _) {
        --Health;
        // todo: play sound
    }

    public void DoMove() { --Health; }

    void Start() {
        ResetHunger(10);
    }
}
