using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputController inputController;
    // Start is called before the first frame update
    void Start()
    {
        inputController = GameManagerSingleton.Instance.InputController;

    }

    // Update is called once per frame
    void Update()
    {
        print(inputController.horizontal + "horizontal");
        print(inputController.vertical + "vertical");
    }
}
