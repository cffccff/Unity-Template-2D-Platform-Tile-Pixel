using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CharacterHP : MonoBehaviour
{
   [SerializeField] Slider slider;
   public static float playerHP=2;
    private float currentHealth;
    private float animationTime =0.5f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("PlayerSlider").GetComponent<Slider>();
        slider.maxValue = playerHP;
        slider.minValue = 0;
        slider.value = playerHP;
        currentHealth = slider.value;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
       
        DOTween.To(() => slider.value, x => slider.value = x, currentHealth, animationTime);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("Character die");
        animator.SetInteger("State",4);
      
       // Invoke("DestroySelf", 1f);



    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
