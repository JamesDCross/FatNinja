using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickups : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerHealth.PlayersHP < PlayerHealth.MaxHP)
        {
            Destroy(gameObject);
            PlayerHealth.heal(10);
        }
    }
}
