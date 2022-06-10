using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPower : MonoBehaviour
{
     TextMeshProUGUI attTxt;
    // Start is called before the first frame update
    void Start()
    {
        attTxt = GameObject.Find("AtkPwrTxt").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterMovement.playerDamage++;
            attTxt.text = "2";




            Destroy(gameObject);
        }


    }
}
