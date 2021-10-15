using UnityEngine;

namespace Neuron
{
    [System.Serializable]
    public class AIOutput
    {
        [SerializeField] [ViewOnly] Transform target;
        [SerializeField] [ViewOnly] Vector3 targetPoint;
        [SerializeField] [ViewOnly] Vector3 targetDirection;

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
}