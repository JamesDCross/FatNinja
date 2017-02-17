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
            int incomingheal = ((healAmount * 100) + Random.Range(0, 100));
            healText.GetComponent<damageTextScr>().setDamage(incomingheal);

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
