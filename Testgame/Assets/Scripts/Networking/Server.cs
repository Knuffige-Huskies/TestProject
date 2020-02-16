using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class Server : MonoBehaviour
{
    private NetworkAdapter m_DefaultNetworkAdapter;
    private Dictionary<int, NetworkAdapter> m_Clients;
    private int uniqueID;

    //Handle somewhere else? Just Put NetworkingStuff here!
    //private Dictionary<int, GameObject> m_GameObjects;

    private void Initialize()
    {
        m_Clients = new Dictionary<int, NetworkAdapter>();
        uniqueID = -1;
        m_DefaultNetworkAdapter = new NetworkAdapter();
        m_DefaultNetworkAdapter.BindPortAndListen();
        Debug.Log("Init Server");
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_DefaultNetworkAdapter.ScheduleUpdate();

        //Check new Connections
        AcceptConnections();

        //Read incoming Packages
        ReadData();

        //Send new Data    
    }

    private void AcceptConnections()
    {
        NetworkConnection connection;
        while ((connection = m_DefaultNetworkAdapter.Accept()) != default(NetworkConnection))
        {
            Debug.Log("new Connection");
            m_DefaultNetworkAdapter.GetDriverAndPipeline(out UdpNetworkDriver driver, out NetworkPipeline rp, out NetworkPipeline up);
            NetworkAdapter clientAdapter = new NetworkAdapter(connection, driver, rp, up);
            m_Clients.Add(GetUniqueId(), clientAdapter);
            clientAdapter.SendData(GetIntialData(), true);
        }
    }

    private void ReadData()
    {
        foreach (var key in m_Clients.Keys)
        {
            NetworkEvent.Type cmd;
            while ((cmd = m_Clients[key].ReciveData(out DataStreamReader stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {

                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    byte[] data = NetworkUtils.ReadPackage(stream);
                    if (data.Length > 0)
                    {
                        Debug.Log(string.Format("recived: {0}", System.BitConverter.ToInt32(data, 0)));
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

    private byte[] GetIntialData()
    {
        return System.BitConverter.GetBytes(0);
    }

    private int GetUniqueId()
    {
        uniqueID++;
        return uniqueID;
    }
}
