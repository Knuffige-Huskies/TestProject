using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    NetworkAdapter m_NetworkAdapter;
    bool m_Connected;
    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_Connected = false;
        m_NetworkAdapter = new NetworkAdapter();
        Connect();
        Debug.Log("Client init");
    }

    private void Connect()
    {
        m_NetworkAdapter.Connect("192.168.8.27:9000");   
    }

    // Update is called once per frame
    void Update()
    {
        m_NetworkAdapter.ScheduleUpdate();

        //Recive Packages
        ReadData();

        //Send Data
        if (m_Connected)
        {
            byte[] data = System.BitConverter.GetBytes(i);
            i++;
            m_NetworkAdapter.SendData(data, true);
        }
    }

    private void ReadData()
    {
        NetworkEvent.Type cmd;
        while ((cmd = m_NetworkAdapter.ReciveData(out DataStreamReader stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {

            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                byte[] data = NetworkUtils.ReadPackage(stream);
                if (data.Length > 0)
                {
                    Debug.Log("Client connected");
                    m_Connected = true;
                }
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("You got disconnected from the server.");
                //m_Connection = default;
            }
        }
    }
}
