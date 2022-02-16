using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Neuron
{
    public abstract class ActorAI : MonoBehaviour
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
        bool IsLocked, AI_Started, areTasksValid;
        
        public event OnDoAIAnything onInit, onTick;
        public event OnAIStateChangeFunc onStateChange;
        public NavMeshAgent Agent => agent;
        public virtual AIInput AIInput { get { return ai_input; } }
        public AIOutput AIOutput { get { return ai_output; } }
        public AIPredicateDelegate UnlockCondition;

        protected virtual void OnCleanup() { }
        protected virtual void OnDebugDraw() { }
        protected virtual void OnInit() { }
        protected virtual void OnTick() { }
        protected virtual void OnStateChange(AIOutput state) { }

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

            AI_Started = true;
            onInit?.Invoke();
            OnInit();
            areTasksValid = description != null && description.InputTask != null && description.OutputTask != null;
            if (areTasksValid)
            {
                description.InputTask.OnInitAI(control);
                description.OutputTask.OnInitAI(control);
            }
        }

        private void OnDestroy()
        {
            OnCleanup();
            if (!areTasksValid) { return; }
            description.InputTask.OnCleanupAI(control);
            description.OutputTask.OnCleanupAI(control);
        }

        private void OnDrawGizmos()
        {
            if (willDebug == false) { return; }
            OnDebugDraw();
            if (!areTasksValid) { return; }
            description.InputTask.DrawDebug(transform);
            description.OutputTask.DrawDebug(transform);
        }

        private void Update()
        {
            if (AI_Started == false || !areTasksValid) { return; }
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
                    description.InputTask.GetAIInput();
                    description.OutputTask.CalculateAIOutput();
                    IsLocked = true;
                    onStateChange?.Invoke(ai_output);
                    OnStateChange(ai_output);
                }
                onTick?.Invoke();
                OnTick();
            }
        }
    }
}