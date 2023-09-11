using System;
using System.Collections.Generic;
using UnityEditor;

namespace Framework
{
    public class GraphSaver
    {
        public event EventHandler<string> SaveMessage;
        
        public void SaveCanvas(GraphCanvas canvas)
        {
            //AddNodeAsset(canvas.Root, canvas.Nodes);
            SaveMessage?.Invoke(this, "Saved");
        }

        private void AddNodeAsset(GraphNode root, IReadOnlyList<GraphNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Behaviour == null)
                    continue;
                
                if (AssetDatabase.Contains(node.Behaviour) == false)
                {
                    node.Behaviour.name = node.Behaviour.GetType().Name;
                    AssetDatabase.AddObjectToAsset(node.Behaviour, root.Behaviour);
                }
            }
        }
    }
}