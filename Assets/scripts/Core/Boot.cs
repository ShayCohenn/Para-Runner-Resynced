using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParaRunner.Core
{
    public class Boot : MonoBehaviour
    {
        private void InitializeServices()
        {
            // Register global services here so any script can access them later
            // Example:
            // ServiceLocator.Register<IAudioService>(new AudioManager());
            // ServiceLocator.Register<ISaveService>(new LocalSaveService());

            Debug.Log("[Boot] All core services registered!");
        }

        private void Awake()
        {
            InitializeServices();
            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            // Once initialized, transition to MainMenu or Gameplay
            SceneManager.LoadScene("MainMenu"); // or "Gameplay"
        }
    }
}