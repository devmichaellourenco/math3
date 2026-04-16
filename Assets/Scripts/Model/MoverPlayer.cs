using UnityEngine;
using System.Collections;

public class MoverPlayer : MonoBehaviour
{
    public JogadorDisplay jogadorDisplay;
    public int idPlayer;
    public string tagPlayer;
    public string inputFire1;
    public string inputFire2;
    public string inputFire3;
    public string inputHorizontal;
    public string inputvertical;
    CharacterController characterController;

    public float speed = 200.0f;
    public float gravity =0f;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputFire1 = "Fire1" + tagPlayer;
        inputFire2 = "Fire2" + tagPlayer;
        inputFire3 = "Fire3" + tagPlayer;
        inputHorizontal = "Horizontal" + tagPlayer;
        inputvertical = "Vertical" + tagPlayer;
    }

    void Update()
    {
        if (GameController.Instance.gameHasEnded == false && GameController.Instance.gameOn == true)
        {
            moveDirection = new Vector3(Input.GetAxis(inputHorizontal), Input.GetAxis(inputvertical), 0.0f);
            moveDirection *= speed;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
