using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Neuron
{
    public class ActorAI : MonoBehaviour
    {
        [SerializeField] float tickInterval = 0.2f;
        [SerializeField] bool realtime = false;
        [SerializeField] AIDescription description;
        [SerializeField] bool willDebug = false;
        [SerializeField] [ViewOnly] float timeScale = 1.0f;
        public float TimeScale { get { return timeScale; } set { timeScale = value; } }

        float timer;
        AIInput ai_input;
        [SerializeField] [ViewOnly] AIOutput ai_output;
        ActorAI control;
        NavMeshAgent agent;
        bool IsLocked, AI_Started;
        public delegate void OnDoAIAnything();
        public delegate bool AIPredicateDelegate();
        public event OnDoAIAnything OnInitAIEv, OnTickAIEv;
        public event OnAIStateChangeFunc OnUpdateAI;
        public NavMeshAgent Agent => agent;
        public virtual AIInput AIInput { get { return ai_input; } }
        public AIOutput AIOutput { get { return ai_output; } }
        public AIPredicateDelegate UnlockCondition;

        public virtual void OnInitAI() { }

        private void Awake()
        {
            timeScale = 1.0f;
            agent = this.GetComponentInParent<NavMeshAgent>();
            IsLocked = false;
            UnlockCondition = new AIPredicateDelegate(() => { return true; });
            control = this;
            AI_Started = false;
            ai_input = new AIInput();
            ai_output = new AIOutput();
            if (description != null)
            {
                if (description.InputTasks != null && description.InputTasks.Count > 0)
                {
                    for (int i = 0; i < description.InputTasks.Count; i++)
                    {
                        var task = description.InputTasks[i];
                        if (task == null) { continue; }
                        task.OnInitAI(control);
                    }
                }

                if (description.OutputTasks != null && description.OutputTasks.Count > 0)
                {
                    for (int i = 0; i < description.OutputTasks.Count; i++)
                    {
                        var task = description.OutputTasks[i];
                        if (task == null) { continue; }
                        task.OnInitAI(control);
                    }
                }
            }
            OnInitAIEv?.Invoke();
            AI_Started = true;
            OnInitAI();
        }

        private void OnDestroy()
        {
            if (description != null)
            {
                if (description.InputTasks != null && description.InputTasks.Count > 0)
                {
                    for (int i = 0; i < description.InputTasks.Count; i++)
                    {
                        var task = description.InputTasks[i];
                        if (task == null) { continue; }
                        task.OnCleanupAI(control);
                    }
                }

                if (description.OutputTasks != null && description.OutputTasks.Count > 0)
                {
                    for (int i = 0; i < description.OutputTasks.Count; i++)
                    {
                        var task = description.OutputTasks[i];
                        if (task == null) { continue; }
                        task.OnCleanupAI(control);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (willDebug == false) { return; }

            if (description != null)
            {
                if (description.InputTasks != null && description.InputTasks.Count > 0)
                {
                    for (int i = 0; i < description.InputTasks.Count; i++)
                    {
                        var task = description.InputTasks[i];
                        if (task == null) { continue; }
                        task.DrawDebug(transform);
                    }
                }

                if (description.OutputTasks != null && description.OutputTasks.Count > 0)
                {
                    for (int i = 0; i < description.OutputTasks.Count; i++)
                    {
                        var task = description.OutputTasks[i];
                        if (task == null) { continue; }
                        task.DrawDebug(transform);
                    }
                }
            }
        }

        private void Update()
        {
            if (AI_Started == false || description == null) { return; }
            timer += realtime ? Time.unscaledDeltaTime * timeScale : Time.deltaTime * timeScale;
            if (timer > tickInterval)
            {
                timer = 0f;
                if (IsLocked)
                {
                    if (UnlockCondition != null)
                    {
                        var conditionValue = UnlockCondition.Invoke();
                        if (conditionValue)
                        {
                            IsLocked = false;
                        }
                    }
                    else
                    {
                        IsLocked = false;
                    }
                }

                if (IsLocked == false)
                {
                    if (description.InputTasks != null && description.InputTasks.Count > 0)
                    {
                        for (int i = 0; i < description.InputTasks.Count; i++)
                        {
                            var task = description.InputTasks[i];
                            if (task == null) { continue; }
                            task.GetAIInput();
                        }
                    }
                    if (description.OutputTasks != null && description.OutputTasks.Count > 0)
                    {
                        for (int i = 0; i < description.OutputTasks.Count; i++)
                        {
                            var task = description.OutputTasks[i];
                            if (task == null) { continue; }
                            task.CalculateAIOutput();
                        }
                    }
                    IsLocked = true;
                    OnUpdateAI?.Invoke(ai_output);
                }
                OnTickAIEv?.Invoke();
            }
        }
    }
}