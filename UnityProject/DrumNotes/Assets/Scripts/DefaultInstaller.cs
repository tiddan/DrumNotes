using UI;
using Zenject;

public class DefaultInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<MainViewModel>()
            .AsSingle();
    }
}