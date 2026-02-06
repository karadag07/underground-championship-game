using UnityEngine;

public class PlayerBlockInput : MonoBehaviour
{
    PlayerStamina playerStamina;

    void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
    }

    void Update()
    {
        if (playerStamina == null)
            return;

        // K BASILIYKEN BLOCK
        playerStamina.SetBlocking(Input.GetKey(KeyCode.K));
    }
}