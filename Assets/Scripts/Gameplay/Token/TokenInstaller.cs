using Core.Utilities;
using Zenject;

namespace Gameplay.Token
{
    public class TokenInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPickupModel>().To<TokenModel>().AsSingle();
            Container.Bind<ISpriteAnimatorModel>().To<SpriteAnimatorModel>().AsSingle();
            Container.BindInterfacesTo<SpriteAnimatorService>().AsSingle();
            Container.Bind<ISpriteAnimatorControl>().To<TokenSpriteControl>().AsSingle();
        }
    }
}