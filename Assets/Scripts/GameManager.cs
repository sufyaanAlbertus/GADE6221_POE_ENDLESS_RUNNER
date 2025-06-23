using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
        public static GameManager Instance;

        public GameState CurrentState { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                CurrentState = GameState.Menu;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            CurrentState = GameState.Playing;
            Debug.Log("Game Started");
        }
        public void ResumeGame()
        {
            CurrentState = GameState.Playing;
            Debug.Log("Game Resumed");
        }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
    }

    public void EndGame()
    {
        CurrentState = GameState.GameOver;
    }

}
