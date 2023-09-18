using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

//This Class Allows The Player And Interactable Objects To Change Universe
public class ChangeUniverse : MonoBehaviour
{
    [Header("UNIVERSE NUMBERS")]
    //Stores The Number Of The Visible Universe
    private int visibleUniverse;

    [HideInInspector]
    //Stores The Number Of The Universe Shown Through The Portal
    public int hiddenUniverse;

    [Header("PLAYERUNIVERSETRACKER")]
    //References The PlayerUniverseTracker Class
    private PlayerUniverseTracker playerUniverse;

    [Header("CHECK INTERACTABLE OBJECT")]
    //Stores An Interactable Object That Is Colliding With The Portal
    private Collider checkObject;

    // Start is called before the first frame update
    void Start()
    {
        //Shows That None Of The Interactable Object Needs To Be Checked
        checkObject = null;
    }

    //This Function Allows The Portal To Show The Correct Universe
    public void setUp(int hidden)
    {
        //Sets The Visible Universe To The Player's Current Universe, And Sets The Hidden Universe To Be Equal To hidden
        playerUniverse = GameObject.FindWithTag("MainPlayerBody").GetComponent<PlayerUniverseTracker>();

        visibleUniverse = playerUniverse.currentUniverse;
        hiddenUniverse = hidden;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks If The Player's Camera Is Colliding With The Portal
        if (other.name == "CenterEyeAnchor")
        {
            //Sets The Player To Exist In the Hidden Universe
            playerUniverse.currentUniverse = hiddenUniverse;

            //If The Player Was Holding An Interactable Object When Colliding With the Portal,
            //the Object Is Set To Exist In the Hidden Universe. It Is Also Shown That None Of The Interactable Object Needs To Be Checked
            GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("InteractableObject");
            foreach (GameObject intObject in interactableObjects)
            {
                Grabbable grabbing = intObject.GetComponent<Grabbable>();

                if(grabbing.isBeingGrabbed == true)
                {
                    ChangeObjectLayer change = intObject.GetComponent<ChangeObjectLayer>();
                    change.currentUniverse = hiddenUniverse;
                    checkObject = null;
                }
            }

            //Removes All Existing Portals
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            foreach (GameObject portal in portals)
            {
                Destroy(portal);
            }
        }

        //If An Interactable Object Is Colliding With the Portal, Then It Is Shown That The Object Is Being Checked
        if(other.tag == "InteractableObject")
        {
            checkObject = other;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Checks If The Object Leaving The Portal Is The Interactable Object Being Checked
        if (other.tag == "InteractableObject" && other == checkObject)
        {
            //References the ChangeObjectLayer Class
            ChangeObjectLayer change = other.GetComponent<ChangeObjectLayer>();

            //Player's Camera
            GameObject player = GameObject.FindGameObjectWithTag("MainCamera");

            /*//Sets The Object To Exist In Same Universe As The Player:
            //If The Distance Between the Player And the Object Is Less Or Equal To The Distance Between the Player And The Portal
            if ((Vector3.Distance(player.transform.position, other.transform.position) <=
                 Vector3.Distance(player.transform.position, transform.position)))
            {
                
                change.currentUniverse = playerUniverse.currentUniverse;

            
            }else{
                
            }*/

            if (change.currentUniverse == visibleUniverse)
            {
                //Otherwise The Object Is Set To Exist In The Hidden Universe
                change.currentUniverse = hiddenUniverse;
            }
            else
            {
                //Otherwise The Object Is Set To Exist In The Hidden Universe
                change.currentUniverse = visibleUniverse;
            }

            //Shows That None Of The Interactable Object Needs To Be Checked
            checkObject = null;
        }
    }
}
