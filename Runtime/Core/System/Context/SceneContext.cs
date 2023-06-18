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
        
        private readonly HashSet<Binding> processables = new HashSet<Binding>();
        private readonly HashSet<Binding> tickables = new HashSet<Binding>();
        private readonly HashSet<Binding> fixedProcessables = new HashSet<Binding>();

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
            
            // Installing scene bootstrap bindings
            ScriptableObject.CreateInstance<SceneInstaller>().InstallBindings(diContainer);

            foreach (var binding in diContainer.Container)
            {
                if (binding.Value.ModifierInterfaces != null)
                {
                    if (binding.Value.ModifierInterfaces.Contains(typeof(IProcessable)))
                    {
                        processables.Add((binding.Value));
                    }
                
                    if (binding.Value.ModifierInterfaces.Contains(typeof(IFixedProcessable)))
                    {
                        fixedProcessables.Add((binding.Value));
                    }
                
                    if (binding.Value.ModifierInterfaces.Contains(typeof(ITickable)))
                    {
                        tickables.Add((binding.Value));
                    }
                }

                if(binding.Value.Instance == null)
                    continue;
                
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
                if (processable.Instance != null)
                {
                    var instnace = processable.Instance as IProcessable;
                    instnace.Process(Time.deltaTime);   
                }
            }
            
            if (Time.time - lastTick >= 1)
            {
                lastTick = Time.time;
                
                foreach (var tickable in tickables)
                {
                    if (tickable.Instance != null)
                    {
                        var instnace = tickable.Instance as ITickable;
                        instnace.Tick();   
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var fp in fixedProcessables)
            {
                if (fp.Instance != null)
                {
                    var instnace = fp.Instance as IFixedProcessable;
                    instnace.FixedProcess(Time.deltaTime);   
                }
            }
        }
    }
}
