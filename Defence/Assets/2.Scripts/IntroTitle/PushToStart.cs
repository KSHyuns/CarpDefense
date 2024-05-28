using UnityEngine;
using UnityEngine.InputSystem;
public class PushToStart : MonoBehaviour
{
    public GameInput gameInput;

    private void Awake()
    {
        GameDataBase.Instance.saveData.daia += 15000;
        SoundMGR.Instance.SoundPlay(audioName.Intro);
        gameInput = new GameInput();
        gameInput.Enable();

        gameInput.Input.Press.performed += PressInput;
    }

    private void OnDisable()
    {
        gameInput.Input.Press.performed -= PressInput;
        gameInput.Disable();
    }

    private void PressInput(InputAction.CallbackContext obj)
    {
        var Press = obj.ReadValue<float>();

        if (Press == 1f)
        {
            SceneChanger.Instance.MainScene();
        }
    }
}
