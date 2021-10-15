using System.Collections.Generic;
using UnityEngine;

namespace Neuron
{
    [System.Serializable]
    public class AIDescription
    {
        [SerializeReference] [SerializeReferenceButton] List<IAIInputTask> inputTasks;
        [SerializeReference] [SerializeReferenceButton] List<IAIOutputTask> outputTasks;
        internal List<IAIInputTask> InputTasks { get { return inputTasks; } }
        internal List<IAIOutputTask> OutputTasks { get { return outputTasks; } }
    }
}