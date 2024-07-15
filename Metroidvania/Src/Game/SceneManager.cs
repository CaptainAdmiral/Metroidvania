using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metroidvania.Src.Game
{
    internal class SceneManager: IUpdatable
    {
        public IScene CurScene;
        public SceneManager() { }

        public void Update(double dTime)
        {
            if (CurScene is IUpdatable)
            {
                ((IUpdatable)CurScene).Update(dTime);
            }
        }
    }
}
