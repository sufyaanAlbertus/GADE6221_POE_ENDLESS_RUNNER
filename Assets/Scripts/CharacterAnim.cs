using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    private Animator anim;
    private GameState lastGameState;

    void Start()
    {
        anim = GetComponent<Animator>();
        lastGameState = GameManager.Instance.CurrentState;
        ApplyAnimationState(lastGameState);
    }

    void Update()
    {
        GameState current = GameManager.Instance.CurrentState;

        if (current != lastGameState)
        {
            ApplyAnimationState(current);
            lastGameState = current;
        }
    }

    private void ApplyAnimationState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                anim.SetBool("isRunning", false);
                break;
            case GameState.Playing:
                anim.SetBool("isRunning", true);
                break;

        }
    }
}
