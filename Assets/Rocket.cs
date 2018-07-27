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

    [SerializeField]
    float rcsThrust = 250f;
    [SerializeField]
    float mainThrust = 30f;
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
            Thrust();
            Rotate();
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
                state = State.Transcending;
                level++;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                state = State.Dead;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
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
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!rocketSound.isPlaying)
            {
                rocketSound.Play();
            }
            // print("Thrusting");
        }
        else
        {
            rocketSound.Stop();
        }

    }

    private void Rotate()
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
