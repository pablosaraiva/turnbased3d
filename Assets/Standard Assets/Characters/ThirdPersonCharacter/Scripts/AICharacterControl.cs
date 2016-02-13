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
        public float maxDistancePerTurn = 4;
        private bool isPlanning = true;
        private bool isTooFar;
        public Text uiText;


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
            if (isPlanning)
            {
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
                    uiText.text = "Too far";
                    isTooFar = true;
                }
                else
                {
                    uiText.text = "Can go";
                    isTooFar = false;
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    if (!isTooFar)
                    {
                        uiText.text = "Moving";
                        isPlanning = false;
                        agent.Resume();
                    }
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
