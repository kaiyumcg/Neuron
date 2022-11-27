using UnityEngine;
using System.Collections;

namespace Neuron
{
    public class Crumb : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] float ai_probability;
        public virtual bool ChooseCrumb() 
        {
            var valFromUnity = Random.value;
            return valFromUnity <= ai_probability;
        }
    }
}