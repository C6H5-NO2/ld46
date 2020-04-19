using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatHunger : MonoBehaviour {
    public Text catHungerText;

    public const int fishVal = 2;

    private int hunger;
    public int Hunger {
        get { return hunger; }
        set {
            hunger = value;
            catHungerText.text = "Cat Hunger: " + hunger;
        }
    }

    // reset when entering new level
    public void ResetHunger(int h) { Hunger = h; }

    public void ReciveFish() { Hunger += fishVal; }

    public void TakeDmg() { --Hunger; }

    void Start() {
        ResetHunger(10);
    }

    void Update() {

    }
}
