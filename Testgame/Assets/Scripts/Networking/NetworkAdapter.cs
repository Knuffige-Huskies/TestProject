using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;
using UnityEngine;

public class NetworkAdapter
{
    NetworkEndPoint m_Endpoint;
    NetworkConnection m_Connection;
    UdpNetworkDriver m_Driver;
    NetworkPipeline m_ReliablePipeline;
    NetworkPipeline m_UnreliablePipeline;

    public NetworkAdapter()
    {
        this.m_Connection = default;
        m_Driver = new UdpNetworkDriver(new SimulatorUtility.Parameters { MaxPacketSize = 256, MaxPacketCount = 30, PacketDelayMs = 10, PacketDropPercentage = 20});
        m_ReliablePipeline = m_Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage), typeof(SimulatorPipelineStage));
        m_UnreliablePipeline = m_Driver.CreatePipeline(typeof(UnreliableSequencedPipelineStage), typeof(SimulatorPipelineStage));
    }

    public NetworkAdapter(NetworkConnection connection, UdpNetworkDriver driver, NetworkPipeline reliablePipeline, NetworkPipeline unreliablePipeline)
    {
        this.m_Connection = connection;
        m_Driver = driver;
        m_ReliablePipeline = reliablePipeline;
        m_UnreliablePipeline = unreliablePipeline;
    }

    public void Connect(string ip_port)
    {
        NetworkUtils.ParseConnection(ip_port, out string ipAddress, out ushort addressPort);
        m_Endpoint = NetworkEndPoint.Parse(ipAddress, addressPort);
        Debug.Log(string.Format("connect to ip: {0} and port: {1}", ipAddress, addressPort));
        Connect();
    }

    public void Connect(NetworkEndPoint endpoint)
    {
        m_Endpoint = endpoint;
        Connect();
    }

    private void Connect()
    {
        m_Connection = m_Driver.Connect(m_Endpoint);
    }

    public NetworkConnection Accept()
    {
        return m_Driver.Accept();
    }

    /// <summary>
    /// Set the Mode of the Adapter to Listen, so incoming connections will not rejected by default
    /// </summary>
    public void BindPortAndListen()
    {
        m_Endpoint = new NetworkEndPoint();
        m_Endpoint = NetworkEndPoint.Parse("0.0.0.0", Settings.DEFAULT_PORT);

        if (m_Driver.Bind(m_Endpoint) != 0)
        {
            Debug.Log(string.Format("Failed to bind to port {0}", Settings.DEFAULT_PORT));
        }
        else
        {
            m_Driver.Listen();
        }
    }

    public void ScheduleUpdate()
    {
        m_Driver.ScheduleUpdate().Complete();
    }

    public void SendData(byte[] data, bool reliable)
    {
        using (var writer = new DataStreamWriter(data.Length, Allocator.Temp))
        {
            writer.Write(data, data.Length);
            if (reliable)
            {
                m_Connection.Send(m_Driver, m_ReliablePipeline, writer);
            }
            else
            {
                m_Connection.Send(m_Driver, m_UnreliablePipeline, writer);
            }
        }
    }

    public NetworkEvent.Type ReciveData(out DataStreamReader stream)
    {
        /*
        byte[] data = new byte[0];
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out DataStreamReader stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
 
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                data = NetworkUtils.ReadPackage(stream);
                return data;
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("You got disconnected from the server.");
                //m_Connection = default;
            }
        }
        return data;
        */
       return m_Connection.PopEvent(m_Driver, out stream);
    }

    public void GetDriverAndPipeline(out UdpNetworkDriver driver, out NetworkPipeline reliablePipeline, out NetworkPipeline unraliablePipeline)
    {
        driver = m_Driver;
        reliablePipeline = m_ReliablePipeline;
        unraliablePipeline = m_UnreliablePipeline;
    }

    public void OnDestroy()
    {
        m_Driver.Dispose();
    }
}
