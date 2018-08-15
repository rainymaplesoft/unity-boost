using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    new Rigidbody rigidbody;
    AudioSource rocketSound;

    /*
        Modifier            Change in Inspector?    Change From Other Scripts?
        [SerializeField]    Yes                     No
        public              Yes                     Yes
    */

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deadSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem deadParticles;

    bool isRocketSoundOn = false;

    enum State
    {
        Alive, Dead, Transcending
    }

    State state = State.Alive;
    int level = 0;
    int levelMax = 1;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "friendly":
                break;
            case "fuel":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeadSequence();
                break;
        }
    }

    private void StartDeadSequence()
    {
        state = State.Dead;
        rocketSound.Stop();
        rocketSound.PlayOneShot(deadSound);
        deadParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        rocketSound.Stop();
        rocketSound.PlayOneShot(winSound);
        winParticles.Play();
        level++;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        if (level <= levelMax)
        {
            SceneManager.LoadScene(level);
        }
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ApplyThrust();
        }
        else
        {
            rocketSound.Stop();
            mainEngineParticles.Stop();
        }

    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust); //* Time.deltaTime
        if (!rocketSound.isPlaying)
        {
            //rocketSound.Play();
            rocketSound.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
        // print("Thrusting");
    }

    private void RespondToRotateInput()
    {
        rigidbody.freezeRotation = true;    // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidbody.freezeRotation = false;    // resume physics control of rotation
    }
}
