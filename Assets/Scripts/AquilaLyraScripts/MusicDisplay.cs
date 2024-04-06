using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDisplay : MonoBehaviour
{
    [SerializeField] GameObject display;
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            display.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag =="Player") {
            display.SetActive(false);
        }
    }

}
