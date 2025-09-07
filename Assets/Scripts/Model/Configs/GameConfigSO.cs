using UnityEngine;
using Zenject;

namespace Model.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game")]
    public class GameConfigSO : ScriptableObjectInstaller<GameConfigSO>
    {
        public string gameplayScene;

        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
}