using UnityEngine;

namespace ForgeLightToolkit.Editor
{
    public class RoadMapRenderer : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            foreach (Transform transformChild in transform)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transformChild.position, 1f);
            }

            Gizmos.color = Color.red;

            for (var i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }

            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }
    }
}