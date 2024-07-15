using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Game
{
    internal interface IUpdatable
    {
        /// <summary>
        /// intended to be called by one of the main game loops to update an updatable.
        /// </summary>
        /// <param name="dTime">delta time since last game update</param>
        public void DoUpdate(double dTime)
        {
            if (ShouldUpdate()) Update(dTime);
        }

        /// <summary>
        /// overriding this method can effectively sleep an updatable while updates are not required
        /// </summary>
        /// <returns>whether or not to call Update() for this object</returns>
        public virtual bool ShouldUpdate()
        {
            return true;
        }

        /// <param name="dTime">delta time since last game update</param>
        /// <summary>
        /// called once per frame by one of the main game update loops
        /// </summary>
        public void Update(double dTime);
    }
}
