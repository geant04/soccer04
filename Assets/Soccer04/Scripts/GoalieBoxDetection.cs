using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalieBoxDetection : MonoBehaviour
{
    public Goalie goalie;

    private void OnTriggerEnter(Collider other)
    {
        if (goalie == null) return;

        if (other.tag.Equals("ShotBall"))
        {
            Debug.Log("save that mf");
            goalie.EnterSaveState(other);
        }
    }
}
