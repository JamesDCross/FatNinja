using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickups : MonoBehaviour {

    public AudioSource audio;
    public GameObject healText;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerHealth.PlayersHP < PlayerHealth.MaxHP)
        {
            int healAmount = (int)PlayerHealth.MaxHP - (int)PlayerHealth.PlayersHP;

            healText.SetActive(true);
            healText.GetComponent<damageTextScr>().setDamage(healAmount);

            GetComponentInChildren <SpriteRenderer>().enabled = false;

            PlayerHealth.heal(healAmount);

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
