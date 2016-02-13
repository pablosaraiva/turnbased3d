using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public float maxDistancePerTurn = 4;
        private bool isPlanning = true;
        private bool isTooFar;


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            agent.Stop();
        }


        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (isPlanning)
                {
                    agent.SetDestination(hit.point);
                    NavMeshPath path = agent.path;
                }
            }

            if (agent.pathPending)
            {
                return;
            }

            float remainingDistance = agent.remainingDistance;
            if (remainingDistance > maxDistancePerTurn)
            {
                isTooFar = true;
            }
            else
            {
                isTooFar = false;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (isTooFar)
                {
                    print("Too far Away: " + remainingDistance);
                }
                else
                {
                    print("Okay: " + remainingDistance);
                    isPlanning = false;
                    agent.Resume();
                }

            }

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
        }
    }

}
