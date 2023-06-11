using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
    public sealed class SceneContext : MonoBehaviour, IContext, IContainerFactory
    {
        //Serialized fields

        [SF] private bool autoRun = true;
        [SF] private AbstractInstaller[] installers;

        //Private fields
        
        private DiContainer diContainer;
        
        private float lastTick;
        
        private readonly HashSet<IProcessable> processables = new HashSet<IProcessable>();
        private readonly HashSet<ITickable> tickables = new HashSet<ITickable>();
        private readonly HashSet<IFixedProcessable> fixedProcessables = new HashSet<IFixedProcessable>();

        //Properties
        
        public DiContainer DiContainer => diContainer;
        public AbstractInstaller[] Installers => installers;

        public void CreateContainer(DiContainer overrideContainer = null)
        {
            // Register scene context
            Context.SceneContext = this;
            
            // Creating sub container
            diContainer = Context.DiContainer.CreateSubContainer();
            
            // Getting scene mono behaviours
            var sceneMonoBehaviours = new List<MonoBehaviour>();
            InjectExtensions.GetSceneMonoBehaviours(ref sceneMonoBehaviours);
			
            // Queueing for injection
            foreach (var monoBehaviour in sceneMonoBehaviours)
            {
                DiContainer.QueueForInject(monoBehaviour);
            }
            
            // Installing bindings
            foreach (var installer in installers)
            {
                installer.InstallBindings(diContainer);
            }
            
            foreach (var binding in diContainer.Container)
            {
                if(binding.Value.Instance == null)
                    continue;
                
                if (binding.Value.Instance is IProcessable processable)
                {
                    processables.Add(processable);
                }
                
                if (binding.Value.Instance is IFixedProcessable fixedProcessable)
                {
                    fixedProcessables.Add(fixedProcessable);
                }
                
                if (binding.Value.Instance is ITickable tickable)
                {
                    tickables.Add(tickable);
                }

                if (binding.Value.Instance is IPreInstall initializable)
                {
                    initializable.Initialize();
                }
            }
            
            DiContainer.InjectAll();
            
            Context.TimeTookToInstall = Time.realtimeSinceStartup;
        }

        // MonoBehaviour

        private void Awake()
        {
            if (autoRun)
            {
                CreateContainer();
            }
        }

        private void Update()
        {
            foreach (var processable in processables)
            {
                processable.Process(Time.deltaTime);
            }
            
            if (Time.time - lastTick >= 1)
            {
                lastTick = Time.time;
                
                foreach (var tickable in tickables)
                {
                    tickable.Tick();
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var fp in fixedProcessables)
            {
                fp.FixedProcess(Time.fixedDeltaTime);
            }
        }
    }
}
