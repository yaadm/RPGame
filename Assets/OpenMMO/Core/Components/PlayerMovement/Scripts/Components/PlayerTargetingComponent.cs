//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
//using OpenMMO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace OpenMMO
{

    // ===================================================================================
    // Targeting System
    // ===================================================================================
    public partial class PlayerControllerComponent
    {
        [Header("Target Settings")]
        [HideInInspector]
        [SyncVar]
        public Transform currentTarget;

        private List<Transform> nearbyPlayers = new List<Transform>();

        private bool switchTarget = false;

        private Vector3 mouseDownPos;

        public float minClickDistance;


        public virtual void setTarget(Transform target)
        {
            currentTarget = target;
        }

        public virtual Transform getTarget()
        {
            // TODO: check if target is dead ? (or it wil be handled in another place ?)
            return currentTarget;
        }

        public virtual void onTargetButtonClicked()
        {
            // If no nearby players.
            if (nearbyPlayers.Count <= 0)
            {

                currentTarget = null;
                return;
            }

            if (currentTarget)
            {
                int index = nearbyPlayers.IndexOf(currentTarget);

                // current target doest not exists in nearbyPlayers
                if (index > nearbyPlayers.Count - 1 || index < 0)
                {
                    // current target is not available \ out of range

                    // get closest enemy
                    currentTarget = nearbyPlayers[0];
                }
                else
                {

                    // if next index is out of bounds, start again from index 0;
                    index++;
                    if (index > nearbyPlayers.Count - 1)
                    {
                        index = 0;
                    }

                    currentTarget = nearbyPlayers[index];
                }
            }
            else
            {
                currentTarget = nearbyPlayers[0];
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                //TODO: check if enemy

                if (!nearbyPlayers.Contains(other.transform))
                {
                    // add enemy to nearby list.
                    nearbyPlayers.Add(other.transform);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                //TODO: check if enemy ? i dont think we need to check that... maybe...

                if (nearbyPlayers.Contains(other.transform))
                {
                    // remove enemy from nearby list.
                    nearbyPlayers.Remove(other.transform);
                }
            }
        }

        void Sort()
        {
            //TODO: should only occure once you enter combat !
            nearbyPlayers.Sort(delegate (Transform a, Transform b)
            {
                return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position));
            });
        }

        public Transform haveTarget()
        {
            return currentTarget;
        }

    }


}