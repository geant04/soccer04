using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Goalie goalie;
    private Player player;
    private CameraScript cameraController;
    private bool WeWin = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }   

    public void Win()
    {
        // Call if win
        WeWin = true;
        cameraController.ActivateTracking(player.GetTransform());
    }

    private void Update()
    {
        if (WeWin) cameraController.ActivateTracking(player.GetTransform());
    }

    public void SetGoalie(Goalie goalie) { this.goalie = goalie; }
    public void SetPlayer(Player player) { this.player = player; }
    public void SetCameraController(CameraScript camera) { this.cameraController = camera; }
    public Goalie GetGoalie() { return goalie; }
    public Player GetPlayer() { return player; }

}
