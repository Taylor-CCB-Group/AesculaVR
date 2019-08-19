using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AesculaVR.Manipulations
{
    /// <summary>
    /// How is the manlipulatable being manlipulated?
    /// </summary>
    public abstract class Manipulation
    {
        private InputManager primary, secondary;
        private Manipulatable manipulatable;
        private EditorManager editorManager;

        protected InputManager Primary { get { return primary; } }
        protected InputManager Secondary { get { return secondary; } }


        public Manipulatable Manipulatable { get { return manipulatable; } }
        public EditorManager EditorManager { get { return editorManager; } }

        /// <summary>
        /// The Start of the manlipulation.
        /// </summary>
        public Manipulation(InputManager primary, InputManager secondary, Manipulatable manipulatable)

        {
            this.manipulatable = manipulatable;
            this.primary = primary;
            this.secondary = secondary;
            this.editorManager = EditorManager.GetManager();

            this.manipulatable.OnTransformationStarted();
        }

        /// <summary>
        /// The manlipulation is ongoing
        /// </summary>
        public virtual void Update() { this.manipulatable.OnTransformation(); }

        /// <summary>
        /// The end of the manlipulation
        /// </summary>
        public virtual void End() { this.manipulatable.OnTransformationEnded(); }
    }
}