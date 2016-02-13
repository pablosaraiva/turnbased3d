using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public int cooldown;
        public int currentCooldown;

        public float maxDistancePerTurn = 4;

        public void beginTurn()
        {
            isPlaying = true;
        }

        private bool isPlanning = true;
        private bool isTooFar;
        private Light uilight;
        public bool isPlaying;
        public string charname;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            uilight = GetComponentsInChildren<Light>()[0];


	        agent.updateRotation = false;
	        agent.updatePosition = true;

            agent.Stop();

            currentCooldown = cooldown;
        }


        private void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
                isPlanning = true;
                agent.Stop();
            }

            if (!isPlaying)
            {
                uilight.enabled = false;
                return;
            }

            if (isPlanning)
            {
                uilight.enabled = true;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    agent.SetDestination(hit.point);
                }

                if (agent.pathPending)
                {
                    return;
                }

                float remainingDistance = agent.remainingDistance;
                if (remainingDistance > maxDistancePerTurn)
                {
                    uilight.color = Color.red;
                    isTooFar = true;
                }
                else
                {
                    uilight.color = Color.green;
                    isTooFar = false;
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    if (!isTooFar)
                    {
                        isPlaying = false;
                        currentCooldown = cooldown;
                        isPlanning = false;
                        agent.Resume();
                    }
                }
            }
        }
    }

}
