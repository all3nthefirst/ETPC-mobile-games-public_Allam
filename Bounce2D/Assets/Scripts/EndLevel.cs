using UnityEngine;

public class EndLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController ctr = collision.gameObject.GetComponent<PlayerController>();
            
            if(ctr.GetCheckpointObtained() != 0)
            {
                Debug.Log("No has cogido todos los checkpoints");
            }
            else
            {
                Debug.Log("El nivel se ha terminado");

                // Load the next level
                GameStateManager.instance.ChangeGameState(GameStateManager.GameState.WIN);
            }
        }
    }

}
