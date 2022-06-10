using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemJump : MonoBehaviour
{
    TextMeshProUGUI jumpTxt;
    // Start is called before the first frame update
    void Start()
    {
        jumpTxt = GameObject.Find("JumpPwrTxt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterMovement.jumpPower+=4;
            jumpTxt.text = "2";



            Destroy(gameObject);
        }


    }
}
