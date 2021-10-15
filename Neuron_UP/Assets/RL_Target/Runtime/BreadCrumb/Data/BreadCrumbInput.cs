using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neuron
{
    public class BreadCrumbAIInput : AIInput
    {
        public Crumb AheadCrumb { get; internal set; }
        public List<Crumb> NearCrumbs { get; internal set; }
        public List<Crumb> AllCrumbs { get; internal set; }

        internal BreadCrumbAIInput()
        {
            AheadCrumb = null;
            NearCrumbs = null;
            AllCrumbs = null;
        }
    }
}