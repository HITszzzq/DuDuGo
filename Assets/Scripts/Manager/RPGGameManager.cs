using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager sharedInstance = null;

    public GameObject winPanel;
    public GameObject losePanel;
    private AudioSource aSource;
    public AudioClip backgroundMusic;
    public AudioClip winAudio;
    public AudioClip loseAudio;

    private void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
        if(backgroundMusic != null)
        {
            aSource.clip = backgroundMusic;
            aSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(bool playWin)
    {
        if(playWin)
        {
            if (winAudio != null)
            {
                aSource.clip = winAudio;
                aSource.Play();
            }
            winPanel.SetActive(true);
        }
        else
        {
            if (loseAudio != null)
            {
                aSource.clip = loseAudio;
                aSource.Play();
            }
            losePanel.SetActive(true);
        }
        Time.timeScale = 0;
    }

    public void LoadNextLevel(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
