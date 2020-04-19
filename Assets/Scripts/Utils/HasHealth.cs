using UnityEngine;
using System.Collections;

public interface IHasHealth {
    int Health { get; }
    void TakeDmage(int dmg);
}
