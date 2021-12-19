using UnityEngine;
using System;

namespace Neuron
{
    [System.Serializable]
    public class BreadcrumbOutputTask : IAIOutputTask
    {
        ActorAI actorAI;
        BreadCrumbAI crumbAI;
        public Type ValidActorAIType => typeof(BreadCrumbAI);
        Crumb lastCrumb;
        void IAIOutputTask.CalculateAIOutput()
        {            
            var input = crumbAI.BreadCrumbAIInput;
            if (input.AheadCrumb == null)
            {
                var breadCrumbs = input.AllCrumbs;
                if (breadCrumbs != null && breadCrumbs.Count > 0)
                {
                    foreach (var crumb in breadCrumbs)
                    {
                        if (lastCrumb == crumb || !IsCrumbValidForCamera(crumb)) { continue; }
                        var shouldChoose = crumb.ChooseCrumb();

                        if (shouldChoose)
                        {
                            actorAI.UnlockCondition = ()=> 
                            {
                                var AIPos = actorAI.transform.position;
                                var crumbPos = crumb.transform.position;
                                var dist = Vector3.Distance(AIPos, crumbPos);
                                var isCrumbValid = IsCrumbValidForCamera(crumb);
                                var shouldUnlock = dist < 5f || !isCrumbValid;
                                return shouldUnlock;
                            };
                            lastCrumb = crumb;
                            actorAI.AIOutput.Target = crumb.transform;
                            break;
                        }
                    }
                }
            }
            else
            {
                actorAI.AIOutput.Target = input.AheadCrumb.transform;
                actorAI.UnlockCondition = () =>
                {
                    var AIPos = actorAI.transform.position;
                    var crumbPos = input.AheadCrumb.transform.position;
                    var dist = Vector3.Distance(AIPos, crumbPos);
                    return dist < 5f || !IsCrumbValidForCamera(input.AheadCrumb);
                };
            }
        }


        bool IsCrumbValidForCamera(Crumb crumb)
        {
            var camToCrumbDir = crumb.transform.position - camTr.position;
            var camToForward = camTr.forward;
            var dbAngle = Vector3.Angle(camToCrumbDir, camToForward);
            var dot = Vector3.Dot(camToCrumbDir, camToForward);

            var AIPos = actorAI.transform.position;
            var crumbPos = crumb.transform.position;
            var dist = Vector3.Distance(AIPos, crumbPos);
            return dot >= 15f && dist > 5f;
        }

        void IAIOutputTask.DrawDebug(Transform transform)
        {
            
        }

        void IAIOutputTask.OnCleanupAI(ActorAI control)
        {
            
        }

        Transform camTr, actorTr;
        void IAIOutputTask.OnInitAI(ActorAI control)
        {
            camTr = Camera.main.transform;
            this.actorAI = control;
            actorTr = control.transform;
            crumbAI = control.transform.GetComponent<BreadCrumbAI>();
        }
    }
}