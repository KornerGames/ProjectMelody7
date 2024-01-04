using GamePlay.Input;
using NaughtyAttributes;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "New PlaySettings", menuName = "HellRaiser/Create Settings")]
    public class PlaySettings : ScriptableObject
    {

        [System.Serializable]
        public class ColorScheme {

            public Color m_healthGain = Color.white;
            public Color m_healthLoss = Color.white;
            public Color m_damageNull = Color.white;

            [Space]

            public Color m_staminaGain = Color.white;
            public Color m_staminaLoss = Color.white;

            [Space]

            public Color m_moneyGain = Color.white;
            public Color m_moneyLoss = Color.white;

            [Space]

            public Color m_time = Color.white;

            [Space]

            public Color m_missionMainPanel = Color.white;
            public Color m_missionSidePanel = Color.white;

        } //end of ColorScheme class

        [System.Serializable]
        public class AudioTheme {

            [Header("Sound FXs")]

            public AudioClip m_sfxButtonClick;

            [Space]

            public AudioClip m_sfxMissionUpdate;

            [Header("Background Music")]

            public AudioClip m_bgmMissionAccomplished;
            public AudioClip m_bgmMissionFailure;

        }//end of AudioTheme class

        [SerializeField]
        private bool isInputPlatformSpecific;

        [SerializeField]
        [DisableIf("isInputPlatformSpecific")]
        private InputType m_gamePlayInput;
        public InputType GamePlayInput {
            get {
                if (isInputPlatformSpecific)
                {
                    #if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
                        return InputType.Keyboard;
                    #else
                        return InputType.OnScreenButtons;
                    #endif
                }
                return m_gamePlayInput;    
            }
        }

        public ColorScheme m_colorScheme;

        public AudioTheme m_audioTheme;

        //TODO code more play settings variables and usage

    }


}