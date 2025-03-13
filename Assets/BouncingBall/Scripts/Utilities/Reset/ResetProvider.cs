using System;
using System.Collections.Generic;

namespace BouncingBall.Utilities.Reset
{
    public class ResetProvider : IResettable
    {
        private List<IResettable> _resettableObjects = new();

        public void RegisterResettable(IResettable resettable)
        {
            _resettableObjects.Add(resettable);
        }

        public void UnregisterResettable(IResettable resettable)
        {
            if (!_resettableObjects.Contains(resettable))
            {
                throw new ArgumentException($"The object {resettable} was not registered for reset.");
            }

            _resettableObjects.Remove(resettable);
        }

        public void Reset()
        {
            foreach (IResettable resettable in _resettableObjects)
            {
                resettable.Reset();
            }
        }
    }
}
