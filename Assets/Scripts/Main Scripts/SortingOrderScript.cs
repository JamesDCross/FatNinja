using UnityEngine;
using System.Collections;


public class SortingOrderScript : MonoBehaviour {
    public string LAYER_NAME1 = "PlayerBehind";
    public string LAYER_NAME2 = "Player";

    public int sortingOrder = 0;
    public SpriteRenderer sprite;
    public bool ShiboBehind;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            ShiboBehind = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ShiboBehind = false;
    }




    void Update()
    {
        if (ShiboBehind)
        {
            sprite.sortingOrder = sortingOrder;
            sprite.sortingLayerName = LAYER_NAME1;

        } else if (!ShiboBehind)
        {
            sprite.sortingOrder = sortingOrder;
            sprite.sortingLayerName = LAYER_NAME2;
        }
    }
}