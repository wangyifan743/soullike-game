using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [Header("Network Join")]
    [SerializeField]bool startASClient = false;
    [HideInInspector]public PlayerUIHUDManager playerUIHUDManager;
    [HideInInspector]public PlayerUIPopUpManager playerUIPopUpManager;
    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
    private void Update() {
        if(startASClient){
            startASClient = false;
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}
