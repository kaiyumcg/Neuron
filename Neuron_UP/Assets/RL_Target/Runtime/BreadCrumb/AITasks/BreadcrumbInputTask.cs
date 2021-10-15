using System.Collections.Generic;
using UnityEngine;
using System;

namespace Neuron
{
    [System.Serializable]
    public class BreadcrumbInputTask : IAIInputTask
    {
        [SerializeField] float pathWidth = 1f, pointLookupDistance = 5f;
        [SerializeField] LayerMask interestPointMask;
        [SerializeField] Color pathWidthColor = Color.white, distanceColor = Color.red;
        [SerializeField] float pathWidthDebugAheadOffset = 0.2f;
        [SerializeField] Vector3 raycastDistanceDebugSize = Vector3.one;

        RaycastHit[] hits, aheadHit;
        Transform AI_transform;
        BreadCrumbAI crumbAI;
        public Type ValidActorAIType => typeof(BreadCrumbAI);

        void IAIInputTask.GetAIInput()
        {
            int resultHeading = -1;
            resultHeading = Physics.RaycastNonAlloc(AI_transform.position, AI_transform.forward,
                aheadHit, pointLookupDistance, interestPointMask);
            var input = crumbAI.BreadCrumbAIInput;
            input.AheadCrumb = null;
            input.NearCrumbs = null;

            if (resultHeading > 0)
            {
                var comp = aheadHit[0].transform.GetComponent<Crumb>();
                if (comp != null)
                {
                    input.AheadCrumb = comp;
                }
            }
            else
            {
                int result = -1;
                result = Physics.SphereCastNonAlloc(AI_transform.position, pathWidth,
                    AI_transform.forward, hits, pointLookupDistance, interestPointMask);
                if (result > 0)
                {
                    input.AllCrumbs = new List<Crumb>();
                    for (int i = 0; i < result; i++)
                    {
                        var comp = hits[i].transform.GetComponent<Crumb>();
                        if (comp != null)
                        {
                            input.AllCrumbs.Add(comp);
                        }
                    }
                }
            }
        }

        void IAIInputTask.OnCleanupAI(ActorAI control)
        {
            
        }

        void IAIInputTask.OnInitAI(ActorAI control)
        {
            var crumbs = BreadCrumbAI.FindObjectsOfType<Crumb>();
            hits = new RaycastHit[crumbs.Length];
            aheadHit = new RaycastHit[1];
            AI_transform = control.transform;
            crumbAI = control.transform.GetComponent<BreadCrumbAI>();
        }

        void IAIInputTask.DrawDebug(Transform transform)
        {
            Gizmos.color = pathWidthColor;
            Gizmos.DrawSphere(transform.position + new Vector3(0, 0, pathWidthDebugAheadOffset), pathWidth);
            Gizmos.color = distanceColor;
            raycastDistanceDebugSize.z = pointLookupDistance;
            Gizmos.DrawCube(transform.position, raycastDistanceDebugSize);
        }
    }
}