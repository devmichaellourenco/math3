using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OBS: fonte: https://pressstart.vip/tutorials/2018/06/28/41/keep-object-in-bounds.html
// obs: para utilizar, acoplar este script no objeto que quer limitar a movimentação e a camera que vai ser base desta limitação
public class PSBoundariesOrthographic : MonoBehaviour
{
    public Camera MainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Use this for initialization
    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width-100, Screen.height-20, MainCamera.transform.position.z));//aqui só alterei o width-100 e height -20 para ficar encaixado no tabuleiro. em outros jogos pode ser utilizado de forma diferente
       // objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
       // objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }
}
