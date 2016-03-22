/*
Copyright (c) 2015 Eric Begue (ericbeg@gmail.com)

This source file is part of the Panda BT package, which is licensed under
the Unity's standard Unity Asset Store End User License Agreement ("Unity-EULA").

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Panda
{
    public class PandaBehaviour : BehaviourTree
    {

        #region input tasks

        // Is*
        [Task]
        public void IsKeyDown(string keycode)
        {
            KeyCode k = GetKeyCode( keycode );
            Task.current.Complete( Input.GetKeyDown(k) );
        }

        [Task]
        public void IsKeyUp(string keycode)
        {
            KeyCode k = GetKeyCode( keycode );
            Task.current.Complete( Input.GetKeyUp(k));
        }

        [Task]
        public void IsKeyPressed(string keycode)
        {
            KeyCode k = GetKeyCode( keycode );
            Task.current.Complete(Input.GetKey(k));
        }

        [Task]
        public void IsMouseButtonPressed(int button)
        {
            Task.current.Complete(Input.GetMouseButton(button));
        }


        [Task]
        public void IsMouseButtonUp(int button)
        {
            Task.current.Complete(Input.GetMouseButtonUp(button));
        }

        [Task]
        public void IsMouseButtonDown(int button)
        {
            Task.current.Complete(Input.GetMouseButtonDown(button));
        }

        [Task]
        public void IsButtonUp(string buttonName)
        {
            Task.current.Complete(Input.GetButtonUp(buttonName));
        }

        [Task]
        public void IsButtonDown(string buttonName)
        {
            Task.current.Complete(Input.GetButtonDown(buttonName));
        }

        [Task]
        public void IsButtonPressed(string buttonName)
        {
            Task.current.Complete(Input.GetButton(buttonName));
        }


        // Wait*
        [Task]
        public void WaitKeyDown(string keycode)
        {
            KeyCode k = GetKeyCode( keycode );

            if (Input.GetKeyDown(k))
                Task.current.Succeed();

        }

        [Task]
        public void WaitAnyKeyDown(string keycode)
        {
            var task = Task.current;
            KeyCode k = GetKeyCode(keycode);

            if (!task.isStarting)
            {
                if (Input.anyKeyDown)
                    task.Complete(Input.GetKeyDown(k));
            }

        }

        [Task]
        public void WaitKeyUp(string keycode)
        {
            KeyCode k = GetKeyCode(keycode);
            if (Input.GetKeyUp(k))
                Task.current.Succeed();

        }

        [Task]
        public void WaitMouseButtonUp(int button)
        {
            if (Input.GetMouseButtonUp(button))
                Task.current.Succeed();
        }

        [Task]
        public void WaitMouseButtonDown(int button)
        {
            if (Input.GetMouseButtonDown(button))
                Task.current.Succeed();
        }

        [Task]
        public void WaitButtonUp(string buttonName)
        {
            if (Input.GetButtonUp(buttonName))
                Task.current.Succeed();
        }

        [Task]
        public void WaitButtonDown(string buttonName)
        {
            if (Input.GetButtonUp(buttonName))
                Task.current.Succeed();
        }

        // Hold*
        [Task]
        public void HoldKey(string keycode, float duration)
        {
            var task = Task.current;
            if (task.isStarting)
                task.item = null;

            KeyCode k = GetKeyCode(keycode);

            if (Input.GetKeyDown(k))
                task.item = Time.time;

            if( task.item != null )
            {
                float downTime = (float)task.item;
                float elapsedTime = Time.time - downTime;
                
                if (Input.GetKeyUp(k))
                {
                    task.Complete( elapsedTime >= duration );
                    task.debugInfo = string.Format("Done");

                }
                else
                {
                    if (elapsedTime < duration)
                        task.debugInfo = string.Format("{0:000}%", Mathf.Clamp01(elapsedTime / duration) * 100.0f);
                    else
                        task.debugInfo = string.Format("Waiting for key release.");
                }

            }

        }

        // IsNext*
        [Task]
        public void IsNextKeyDown(string keycode)
        {
            var task = Task.current;
            if (!task.isStarting && Input.anyKeyDown)
            {
                bool isMouseButton = false;
                for( int i=0; i < 3; i++)
                {
                    if(Input.GetMouseButton(i)  )
                    {
                        isMouseButton = true;
                        break;
                    }
                }

                if (!isMouseButton)
                {
                    KeyCode k = GetKeyCode(keycode);
                    task.Complete(Input.GetKeyDown(k));
                }
            }

        }


        #endregion



        #region status tasks

        [Task]
        public void Succeed() { Task.current.Succeed(); }

        [Task]
        public void Fail() { Task.current.Fail(); }

        [Task]
        public void Running() {  }
        #endregion

        #region debug tasks

        [Task]
        public void DebugLog(string message)
        {
            Debug.Log(message);
            Task.current.Succeed();
        }

        [Task]
        public void DebugLogError(string message)
        {
            Debug.LogError(message);
            Task.current.Succeed();
        }

        [Task]
        public void DebugLogWarning(string message)
        {
            Debug.LogWarning(message);
            Task.current.Succeed();
        }

        #endregion

        #region time tasks

        [Task]
        public void Wait(float duration)
        {
            var task = Task.current;
            if (task.isStarting)
            {
                task.item = Time.time;
            }

            float elapsedTime = Time.time - (float)Task.current.item;

            float tta = duration - elapsedTime;
            if (tta < 0.0f) tta = 0.0f;

            task.debugInfo = string.Format("t-{0:0.000}",  tta);

            if (elapsedTime >= duration)
                task.Succeed();
        }

        [Task]
        public void Wait(int ticks)
        {
            var task = Task.current;
            int n = 0;
            if (task.isStarting)
            {
                task.item = n;
            }
            else
            {
                // increment tickcount
                n = (int)task.item;
                ++n;
                task.item = n;
            }

            task.debugInfo = string.Format("n-{0}", ticks-n);

            if ( n >= ticks )
                task.Succeed();

        }


        [Task]
        public void RealtimeWait(float duration)
        {
            var task = Task.current;

            if (task.isStarting)
                task.item = Time.realtimeSinceStartup;

            float elapsedTime = Time.realtimeSinceStartup - (float)Task.current.item;

            float tta = duration - elapsedTime;
            if (tta < 0.0f) tta = 0.0f;

            task.debugInfo = string.Format("t-{0:0.000}", tta);

            if (elapsedTime >= duration)
                task.Succeed();
        }

        #endregion

        public KeyCode GetKeyCode(string keycode)
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), keycode);
        }

        public void Reset()
        {
            if (program != null)
                program.Reset();
        }

    }
}
