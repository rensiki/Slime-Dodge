using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject forgeNPC;
    public GameObject UIScreen;
    public void SpawnNPC(){
        forgeNPC.SetActive(true);
    }
}
