using UnityEngine;

[CreateAssetMenu(fileName = "GameStateGameplay", menuName = "GameStates/GSGameplay", order = 1)]
public class GSGameplay: GameState
{
    public override void OnEnter()
    {
        if (SlingshotController.instance != null)
        {
            SlingshotController.instance.isActive = true;
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.ChangeGameState(StateType.PAUSE);
        }
    }

    public override void OnExit()
    {
        SlingshotController.instance.isActive = false;
    }
}
