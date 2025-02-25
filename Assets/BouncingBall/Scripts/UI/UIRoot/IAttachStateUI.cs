namespace BouncingBall.UI.Root
{
    public interface IAttachStateUI
    {
        public StateUI StateUI { get; }

        public void AttachStateUI(StateUI sceneUI);
    }
}
