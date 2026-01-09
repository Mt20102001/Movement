using System.Collections;
using PlayerControllers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IController[] controls;
    private PlayerMovement playerMovement;

    public bool IsInitialized { get; private set; }
    public IMovement Movement { get; private set; }

    IEnumerator Start()
    {
        controls = GetComponents<IController>();
        if (controls != null && controls.Length > 0)
        {
            foreach (IController control in controls)
            {
                if (control is IMovement)
                {
                    Movement = (IMovement)control;
                    playerMovement = (PlayerMovement)control;
                    Debug.Log("Cache Movement System");
                }

                control.Initialize();
            }
        }

        yield return new WaitUntil(() => InputManager.Instance != null);
        IsInitialized = true;
    }


    void Update()
    {
        if (!IsInitialized) return;

        var input = InputManager.Instance;

        playerMovement.Move(input.MoveInput, input.RunInput);

        if (controls != null && controls.Length > 0)
        {
            foreach (IController control in controls)
            {
                control.CustomUpdate(Time.deltaTime);
            }
        }
    }
}
