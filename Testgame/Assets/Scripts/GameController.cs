using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Server m_Server;
    private Client m_Client;
    private static GameController m_Instance;
    private bool m_IsServer;

    private void Start()
    {
        m_IsServer = false;
        GetInstance();
    }

    void Update()
    {
        
    }

    public static GameController GetInstance()
    {
        if(m_Instance == null)
        {
            m_Instance = new GameController();
        }

        return m_Instance;
    }

    private GameController()
    {

    }

    public void SwitchBecomingServer()
    {
        m_IsServer = !m_IsServer;
    }

    private void BecomeServer()
    {

    }
     
    private void BecomeClient()
    {

    }

    public void StartGame()
    {
        if (m_IsServer)
        {
            BecomeServer();
        }

        BecomeClient();
    }

}
