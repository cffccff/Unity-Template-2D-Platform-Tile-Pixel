using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ItemCollection : MonoBehaviour
{
    [SerializeField] int speedRotate = 2;
    // Start is called before the first frame update
    void Start()
    {
        transform.position=new Vector2(transform.position.x, transform.position.y + 2);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector2(0f, 90f * Time.deltaTime * speedRotate));
    }
}
