using System;
using System.Collections.Generic;

namespace BouncingBall.Utilities.Reset
{
    public class ResetManager : IResettable
    {
        private List<IResettable> _resettables = new();

        public void Add(IResettable resettable)
        {
            _resettables.Add(resettable);
        }

        public void Remove(IResettable resettable)
        {
            if (!_resettables.Contains(resettable))
            {
                throw new ArgumentException($"The object {resettable} was not added to the reset list.");
            }

            _resettables.Remove(resettable);
        }

        public void Reset()
        {
            foreach (IResettable resettable in _resettables)
            {
                resettable.Reset();
            }
        }
    }
}
