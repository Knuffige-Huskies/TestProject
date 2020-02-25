using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ServerBehaviour m_ServerBehaviour;


    private void Initialized()
    {
        m_ServerBehaviour = new ServerBehaviour();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 
    private void FixedUpdate()
    {
        
    }
}
