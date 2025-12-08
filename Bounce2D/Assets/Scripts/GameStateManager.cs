using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public enum GameState
    {
        WIN,
        OVER,
        OVERMAIN,
        MAIN,
        PAUSE,
        GAMEPLAY
    }

    public GameState currentState;

    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.PAUSE)
            {
                ChangeGameState(GameState.GAMEPLAY);
            }
            else
            {
                ChangeGameState(GameState.PAUSE);
            }
        }
    }

    public void ChangeGameState(GameState state)
    {
        Debug.Log("CHANGE STATE " + state);

        switch (state)
        {
            case GameState.MAIN:
                {
                    SceneManager.LoadScene("spmap_mainmenu");
                    Time.timeScale = 1f;
                    currentState = GameState.MAIN;
                }
                break;

            case GameState.OVER:
                {
                    StartCoroutine(Respawn());
                    Time.timeScale = 0f;
                    currentState = GameState.OVER;
                }
                break;

            case GameState.WIN:
                {
                    Time.timeScale = 0f;
                    StartCoroutine(RestartGame());
                    currentState = GameState.WIN;
                }
                break;

            case GameState.PAUSE:
                {
                    Time.timeScale = 0f;
                    currentState = GameState.PAUSE;
                }
                break;

            case GameState.GAMEPLAY:
                {
                    Time.timeScale = 1f;
                    currentState = GameState.GAMEPLAY;
                }
                break;

            case GameState.OVERMAIN:
                {
                    Time.timeScale = 0f;
                    StartCoroutine(RestartGame());
                    currentState = GameState.OVERMAIN;
                }
                break;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(2f);

        Debug.Log("RESPAWNING");

        PlayerController pctr = FindAnyObjectByType<PlayerController>();
        if (pctr != null)
        {
            pctr.Respawn();
        }

        ChangeGameState(GameState.GAMEPLAY);
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(2f);

        ChangeGameState(GameState.MAIN);
    }
}
