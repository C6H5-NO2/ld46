using UnityEngine;
using System.Collections;

// todo
public class EnemyHealth : MonoBehaviour, IHasHealth {
    private int health;
    public int Health {
        get => health;

        private set {
            health = value > 0 ? value : 0;
            if(health == 0)
                Destroy(gameObject);
        }
    }

    public void TakeDmage(int dmg) { Health-=dmg; }

    private void Start() {
        Health = 10;
    }

    private void Update() {

    }
}
