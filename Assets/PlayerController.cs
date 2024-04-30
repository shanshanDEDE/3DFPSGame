using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputController m_Input;

    void Awake()
    {
        m_Input = GameManagerSingleton.Instance.InputController;
    }

    void Start()
    {

    }

    void Update()
    {
        if (m_Input.GetMoveInput() != Vector3.zero)
        {
            print(m_Input.GetMoveInput());
        }
    }
}
