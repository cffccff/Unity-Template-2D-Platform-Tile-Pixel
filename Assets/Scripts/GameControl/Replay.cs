using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Replay : MonoBehaviour
{
   [SerializeField] Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ReplayGame);
    }
    private void ReplayGame()
    {
        SceneManager.UnloadSceneAsync("SampleScene 1");
        SceneManager.LoadScene("SampleScene 1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
