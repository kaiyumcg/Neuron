using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeExt;

namespace Neuron
{
    public delegate void OnDoAIAnything();
    public delegate bool AIPredicateDelegate();
    public delegate void OnAIStateChangeFunc(AIOutput output);

    public interface IAIInputTask
    {
        public Type ValidActorAIType { get; }
        public void OnInitAI(ActorAI actorAI);
        public void GetAIInput();
        public void OnCleanupAI(ActorAI actorAI);
        public void DrawDebug(Transform transform);
    }

    public interface IAIOutputTask
    {
        public Type ValidActorAIType { get; }
        public void OnInitAI(ActorAI actorAI);
        public void CalculateAIOutput();
        public void OnCleanupAI(ActorAI actorAI);
        public void DrawDebug(Transform transform);
    }

    public class AIInput
    {

    }

    [System.Serializable]
    public class AIOutput
    {
        [SerializeField] [CanNotEdit] Transform target;
        [SerializeField] [CanNotEdit] Vector3 targetPoint;
        [SerializeField] [CanNotEdit] Vector3 targetDirection;

        public Transform Target { get { return target; } set { target = value; } }
        public Vector3 TargetPoint { get { return targetPoint; } set { targetPoint = value; } }
        public Vector3 TargetDirection { get { return targetDirection; } set { targetDirection = value; } }

        internal AIOutput()
        {
            Target = null;
            TargetPoint = Vector3.zero;
            TargetDirection = Vector3.zero;
        }
    }

    [System.Serializable]
    public class AIDescription
    {
        [SerializeReference] [SerializeReferenceButton] IAIInputTask inputTask;
        [SerializeReference] [SerializeReferenceButton] IAIOutputTask outputTask;
        internal IAIInputTask InputTask { get { return inputTask; } }
        internal IAIOutputTask OutputTask { get { return outputTask; } }
    }

}