using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCutscene : MonoBehaviour
{
    public Animator CurtainAnim;

    [SerializeField] private Button ClickEvent;
    // Start is called before the first frame update
    void Start()
    {
        ClickEvent = GameObject.Find("Newspaper").GetComponent<Button>();
        ClickEvent.onClick.AddListener(IsClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IsClicked()
    {
        // Click the newspaper and then set the fade_out parameter to true
        Debug.Log("Click happens");

        CurtainAnim.SetTrigger("Black");

        SceneManager.LoadScene("MainMenu");
    }
}
