using UnityEngine;
using System.Collections.Generic;

namespace Neuron
{
    public class BreadCrumbAI : ActorAI
    {
        public BreadCrumbAIInput BreadCrumbAIInput { get; private set; }
        public override AIInput AIInput => BreadCrumbAIInput;

        public override void OnInitAI()
        {
            BreadCrumbAIInput = new BreadCrumbAIInput
            {
                AheadCrumb = null,
                AllCrumbs = null,
                NearCrumbs = null
            };
            base.OnInitAI();
        }
    }
}