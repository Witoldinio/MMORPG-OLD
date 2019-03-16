using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void RaceSelectionMenu()
        {
            SceneManager.LoadScene("RaceSelection");
        }

        public void CharacterSelectionMenu()
        {
            SceneManager.LoadScene("CharacterSelection");
        }

        public void CharacterCreationMenu()
        {
            SceneManager.LoadScene("CharacterCreation");
        }
    }
}
