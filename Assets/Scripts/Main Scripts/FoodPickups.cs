using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickups : MonoBehaviour {

    public AudioSource audio;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerHealth.PlayersHP < PlayerHealth.MaxHP)
        {



            GetComponentInChildren <SpriteRenderer>().enabled = false;

            PlayerHealth.heal(10);

            StartCoroutine(Ding());
           // Destroy(gameObject);
           
        }
    }

    IEnumerator Ding()

    {

        audio.Play();

        yield return new WaitForSeconds(audio.clip.length);

        Destroy(gameObject);

    }


}
