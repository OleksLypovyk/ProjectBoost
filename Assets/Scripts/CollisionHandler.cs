using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 5f;
    [SerializeField] AudioClip audioSuccess;
    [SerializeField] AudioClip audioCrash;
    [SerializeField] ParticleSystem particleSuccess;
    [SerializeField] ParticleSystem particleCrash; 

    AudioSource audioSource;

    bool isTrabsitioning = false;
    bool isCollidable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();        
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            isCollidable = !isCollidable;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTrabsitioning || (isCollidable == false))
        {
            return;
        }
        switch(other.gameObject.tag)
        {
            case "Respawn": Debug.Log("It's your Respawn point!"); break;
            case "Finish": Debug.Log("You successfuly ended the mission!"); StartSuccessSequence(); break;
            default: Debug.Log("YOU DIED!"); StartCrashSequence(); break;
        }
    }
    void StartSuccessSequence()
    {
        GetComponent<Movement>().enabled = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        audioSource.PlayOneShot(audioSuccess);
        particleSuccess.Play();
        Invoke("LoadNextLevel", delayTime);
    }
    void StartCrashSequence()
    {
        GetComponent<Movement>().enabled = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        audioSource.PlayOneShot(audioCrash);
        particleCrash.Play();
        Invoke("ReloadLevel", delayTime);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if ((currentSceneIndex + 1) == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
