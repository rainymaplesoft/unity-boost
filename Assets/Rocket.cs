using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
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
