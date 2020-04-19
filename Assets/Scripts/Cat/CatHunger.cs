using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatHunger : MonoBehaviour {
    public Text catHungerText;

    public const int fishVal = 2;

    private int hunger;
    public int HungerVal {
        get { return hunger; }
        private set {
            hunger = value;
            catHungerText.text = "Cat Hunger: " + hunger;
        }
    }

    // reset when entering new level
    public void ResetHunger(int h) { HungerVal = h; }

    public void ReciveFish() {
        HungerVal += fishVal;
        // todo: play sound (?)
    }

    public void TakeDmg() {
        --HungerVal;
        // todo: play sound
    }

    public void DoMove() { --HungerVal; }

    void Start() {
        ResetHunger(10);
    }

    void Update() {

    }
}
