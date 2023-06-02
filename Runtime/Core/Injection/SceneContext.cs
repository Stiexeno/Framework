using System;
using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
    public class SceneContext : MonoBehaviour, IContext, IContainerFactory
    {
        //Serialized fields

        [SF] private bool autoRun;
        [SF] private InstallerInfo[] installers;

        //Private fields
        
        private DiContainer diContainer;
        
        private float lastTick;
        
        private readonly HashSet<IProcessable> processables = new HashSet<IProcessable>();
        private readonly HashSet<ITickable> tickables = new HashSet<ITickable>();
        private readonly HashSet<IFixedProcessable> fixedProcessables = new HashSet<IFixedProcessable>();

        //Properties
        
        public DiContainer DiContainer => diContainer;

        public void CreateContainer(DiContainer overrideContainer = null)
        {
            diContainer = Context.DiContainer.CreateSubContainer();

            diContainer.Bind<IContext>().FromInstance(this);

            foreach (var installer in installers)
            {
                var installerInstance = Activator.CreateInstance(installer.TypeValue) as IBindingInstaller;
                installerInstance.InstallBindings(diContainer);
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
            }
            
            this.diContainer.InjectToSceneGameObjects();
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
    
    [Serializable]
    public struct InstallerInfo
    {
        public string name;
        public string path;
        public string typeName;
        public string assembly;

        public Type TypeValue
        {
            get => !string.IsNullOrEmpty(typeName) ? Type.GetType($"{typeName}, {assembly}") : null;
            set => typeName = value != null ? value.AssemblyQualifiedName : string.Empty;
        }
            
        public InstallerInfo(string name, string typeName, string path, string assembly)
        {
            this.name = name;
            this.typeName = typeName;
            this.path = path;
            this.assembly = assembly;
        }
    }
}
