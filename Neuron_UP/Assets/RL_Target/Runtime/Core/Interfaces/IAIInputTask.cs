using System;
using UnityEngine;

namespace Neuron
{
    public interface IAIInputTask
    {
        public Type ValidActorAIType { get; }
        public void OnInitAI(ActorAI actorAI);
        public void GetAIInput();
        public void OnCleanupAI(ActorAI actorAI);
        public void DrawDebug(Transform transform);
    }
}