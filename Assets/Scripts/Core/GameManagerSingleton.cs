using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton
{
    private GameObject gameObject;

    private static GameManagerSingleton m_Instance;

    public static GameManagerSingleton Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManagerSingleton();
                m_Instance.gameObject = new GameObject("GameManager");
                m_Instance.gameObject.AddComponent<InputController>();
            }
            return m_Instance;
        }
    }
    private InputController m_InputController;

    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
            {
                m_InputController = gameObject.GetComponent<InputController>();
            }
            return m_InputController;
        }
    }
}
