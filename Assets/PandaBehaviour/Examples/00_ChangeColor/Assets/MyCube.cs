using UnityEngine;
using System.Collections;
using Panda;

namespace Panda.Examples.ChangeColor
{
    public class MyCube : MonoBehaviour
    {
        /*
         * Set the color to the specified rgb value and succeed.
         */
        [Task] // Attribute used to mark a member usable from a BT script.
        void SetColor(float r, float g, float b)
        {
            this.GetComponent<Renderer>().material.color = new Color(r, g, b);
            Task.current.Succeed();
        }
    }
}
