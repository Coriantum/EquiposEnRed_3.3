using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        
        Renderer rend;
        
        
        public static List<int> PosEquipo = new List<int>(); // Lista entero que indica el equipo y su cantidad
        private int numMax = 2;

        //Variables
        public NetworkVariable<Vector3> CentralPos = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> Equipo1Pos = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> Equipo2Pos = new NetworkVariable<Vector3>();

        // Network variable para el color
        public NetworkVariable<Color> ColorVariable = new NetworkVariable<Color>();

        // NetworkVariable para el numero de equipo
        public NetworkVariable<int> Team = new NetworkVariable<int>();

        private void Awake() {
            rend = GetComponent<Renderer>();
            CentralPos.OnValueChanged += OnCentralPosChange;
            Equipo1Pos.OnValueChanged += OnEquipo1PosChange;
            Equipo2Pos.OnValueChanged += OnEquipo2PosChange;
            ColorVariable.OnValueChanged += OnColorChange;
            
        }

        private void Start() {
            

            
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
        public void OnColorChange(Color previousColor, Color newColor){
            rend.material.color = ColorVariable.Value;
        }



        public override void OnNetworkSpawn()
        {
            if(IsServer && IsOwner){
                PosEquipo.Add(0);
                PosEquipo.Add(0);
                PosEquipo.Add(0);

            }

             if (IsOwner)
            {
                SubmitEquipoRequestServerRpc(-1);
            }
        }



        // Metodo encargado de mover al sin equipo
        public void Move(int equipo){
            // Mover a la parte central
            SubmitEquipoRequestServerRpc(equipo);
            
        }

        // Si me da tiempo: Encontrar una forma de dividir en 3 partes la zona y que se mueva en una de ellas
        static Vector3 GetRandomPositionEquipo1(){
            return new Vector3(Random.Range(-5f, -1.77f), 1f, Random.Range(-3f, 3f));
        }

        static Vector3 GetRandomPositionEquipo2(){
            return new Vector3(Random.Range(1.77f, 5f), 1f, Random.Range(-3f, 3f));
        }

        
        static Vector3 GetRandomPositionSinEquipo(){
            return new Vector3(Random.Range(-1.77f, 1.77f), 1f, Random.Range(-3f, 3f));
        }


        [ServerRpc]
        public void SubmitEquipoRequestServerRpc(int numEquipo, ServerRpcParams rpcParams = default)
        {
            // Si el equipo es -1 o 1
            if(numEquipo == -1){
                // Cambio Posicion
                CentralPos.Value = GetRandomPositionSinEquipo();
                // Cambio Color
                Color newColor = Color.white;
                ColorVariable.Value = newColor;
                Debug.Log("INICIO");

                // Añado +1 al equipo 0
                PosEquipo[0]++;
                Team.Value = 0;

            }else if(numEquipo == 0){
                // Cambio Posicion
                CentralPos.Value = GetRandomPositionSinEquipo();
                // Cambio Color
                Color newColor = Color.white;
                ColorVariable.Value = newColor;

                // Añado +1 al equipo 0
                PosEquipo[0]++;
                PosEquipo[Team.Value]--;
                Team.Value = 0;

            }

            // Si el numero de jugadores del equipo 1 es menor que el indicado(2) y numequipo =1.
            else if(numEquipo == 1 && PosEquipo[1] < numMax){
                Equipo1Pos.Value = GetRandomPositionEquipo1();
                // Dar el color azul           
                Color newColor = Color.blue;
                ColorVariable.Value = newColor;

                // Se añade un jugador y se quita del anterior equipo
                PosEquipo[1]++;
                PosEquipo[Team.Value]--;
                Team.Value= 1;
                Debug.Log("Equipo 1 "+PosEquipo);
            }
            
            // Si el número de jugadores del equipo 2 es menor que el indicado y el numEquipo = 2
            else if(numEquipo == 2 && PosEquipo[2] < numMax){
                Equipo2Pos.Value = GetRandomPositionEquipo2();
                // Dar el color azul           
                Color newColor = Color.red;
                ColorVariable.Value = newColor;
                
                // Se añade un jugador y se quita del anterior equipo
                PosEquipo[2]++;
                PosEquipo[Team.Value]--;
                Team.Value = 2;
                Debug.Log("Equipo 2 "+PosEquipo);
            }else{

                Debug.Log("Está lleno el equipo " + numEquipo);
            }
        }



        void Update()
        {
           
        }
    }
}