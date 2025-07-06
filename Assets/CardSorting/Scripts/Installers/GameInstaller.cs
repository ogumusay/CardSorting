using CardSorting;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStarter>().AsSingle().NonLazy();
        Container.Bind<BoardController>().AsSingle().NonLazy();
    }
}