using UnityEngine;
using System;

namespace Neuron
{
    public interface IAIOutputTask
    {
        public Type ValidActorAIType { get; } 
        public void OnInitAI(ActorAI actorAI);
        public void CalculateAIOutput();
        public void OnCleanupAI(ActorAI actorAI);
        public void DrawDebug(Transform transform);
    }
}