using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ForgeLightToolkit.Editor.FileTypes.Map
{
    public class NodeRenderer : MonoBehaviour
    {
        public Node Parent;
        public List<Node> EdgeNodes = new();

        private void OnDrawGizmos()
        {
            foreach (var edgeNode in EdgeNodes)
            {
                var edgePosition = edgeNode.Position;
                var parentPosition = Parent.Position;

                edgePosition.Scale(transform.root.localScale);
                parentPosition.Scale(transform.root.localScale);

                Handles.zTest = CompareFunction.LessEqual;
                Handles.DrawLine(edgePosition, parentPosition);
                Handles.DrawWireDisc(edgePosition, Vector3.up, 0.5f);
            }
        }
    }
}