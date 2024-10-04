using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (rb == null) return;
        if (rb.velocity != Vector3.zero)
        {
            // Get the direction of Magnus force: spin cross velocity
            Vector3 magnusForce = Vector3.Cross(rb.angularVelocity, rb.velocity);

            // Apply Magnus force scaled by some coefficient to control curve intensity
            //rb.AddForce(magnusForce * 0.1f);
        }
    }
}
