using Zenject;

namespace CardSorting
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneController>().AsSingle().NonLazy();
        }
    }
}
