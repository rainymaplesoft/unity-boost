using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // cannot apply to object more than once
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    // [Range(0, 1)] [SerializeField] 
    float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / period;  // grows continually from 0

        const float tau = Mathf.PI * 2;
        float rawSinwave = Mathf.Sin(tau * cycles); // goes from -1 to 1

        movementFactor = rawSinwave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
