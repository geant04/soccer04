using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonyScript : MonoBehaviour
{
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, 60.0f * Time.deltaTime, 0);
    }
}
