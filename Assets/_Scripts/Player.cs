using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        
        Renderer rend;
        
        //Lista de players
        public List<GameObject> players = new List<GameObject>();




        //Variables
        public NetworkVariable<Vector3> CentralPos = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> Equipo1Pos = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> Equipo2Pos = new NetworkVariable<Vector3>();

        // Network variable para el color
        public NetworkVariable<Color> ColorVariable = new NetworkVariable<Color>();


        private void Start() {

            CentralPos.OnValueChanged += OnCentralPosChange;
            Equipo1Pos.OnValueChanged += OnEquipo1PosChange;
            Equipo2Pos.OnValueChanged += OnEquipo2PosChange;

            // Al inicio el jugador reaparecerá en la zona central
            GetRandomPositionSinEquipo();


        }


        public void OnCentralPosChange(Vector3 previousValue, Vector3 newValue){
            transform.position = CentralPos.Value;
        }
        public void OnEquipo1PosChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Equipo1Pos.Value;
        }
        public void OnEquipo2PosChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Equipo2Pos.Value;
        }



        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                
               
            }
        }


        // Metodo encargado de mover al equipo 1 
        public void MoveEquipo1(){
            // Mover aleatoriamente en la zona especificada
            SubmitEquipo1RequestServerRpc();
            Debug.Log("Paso por MoveEquipo1");

        }

        
        // Metodo encargado de mover al equipo 2
        public void MoveEquipo2(){
            // Mover aleatoriamente en la zona derecha
            SubmitEquipo2RequestServerRpc();

        }

        // Metodo encargado de mover al sin equipo
        public void MoveSinEquipo(){
            // Mover a la parte central
            SubmitCentroRequestServerRpc();
            
        }

        // Si me da tiempo: Encontrar una forma de dividir en 3 partes la zona y que se mueva en una de ellas
        static Vector3 GetRandomPositionEquipo1(){
            Debug.Log("Y obtengo nueva posicion");
            return new Vector3(Random.Range(-5f, -1.77f), 1f, Random.Range(-3f, 3f));
        }

        static Vector3 GetRandomPositionEquipo2(){
            return new Vector3(Random.Range(1.77f, 5f), 1f, Random.Range(-3f, 3f));
        }

        
        static Vector3 GetRandomPositionSinEquipo(){
            return new Vector3(Random.Range(-1.77f, 1.77f), 1f, Random.Range(-3f, 3f));
        }


        

        [ServerRpc]
        void SubmitCentroRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            CentralPos.Value = GetRandomPositionSinEquipo();

            // Sin color o color blanco
            gameObject.GetComponent<Renderer>().material.color = Color.white;

            

        }

        [ServerRpc]
        void SubmitEquipo1RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Equipo1Pos.Value = GetRandomPositionEquipo1();

            // Dar el color azul
            gameObject.GetComponent<Renderer>().material.color = Color.blue;

            // Si la lista de player contiene mas de 2 jugadores, no se podrán poner más
            if(players.Count > 2){
                

                // Testeo 
                Debug.Log("No puedes poner más!!");
            } else {
                Debug.Log("Aun puedes poner mas jugadores");
            }
        }

        [ServerRpc]
        void SubmitEquipo2RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Equipo2Pos.Value = GetRandomPositionEquipo2();
            // Dar el color rojo
            gameObject.GetComponent<Renderer>().material.color = Color.red;

            // Si la lista de player contiene mas de 2 jugadores, no se podrán poner más
            if(players.Count > 2){

            } else {
                Debug.Log("Máximo 2 jugadores");
            }
        }


        void Update()
        {
           // transform.position = CentralPos.Value;
           //gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}