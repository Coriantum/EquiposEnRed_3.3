using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class GameManager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewEquipo1();
                SubmitNewEquipo2();
                SubmitNewSinEquipo();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }


        // TODO: Botones para cada equipo
        static void SubmitNewEquipo1()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Equipo1" : "Request Equipo1"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.MoveEquipo1();
            }
        }
        static void SubmitNewEquipo2()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Equipo2" : "Request Equipo2"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.MoveEquipo2();
            }
        }
        static void SubmitNewSinEquipo()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Sin Equipo" : "Request Sin Equipo"))
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<Player>();
                player.MoveSinEquipo();
            }
        }

    }
}