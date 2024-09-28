using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
    private bool isWin = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isWin) return;

        if (other.tag.Equals("ShotBall"))
        {
            isWin = true;
            Debug.Log("Win!!!");
        }
    }
}
