using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Utilities
{
    public class SceneLoader
    {
        public async UniTask LoadSingleAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
    }
}