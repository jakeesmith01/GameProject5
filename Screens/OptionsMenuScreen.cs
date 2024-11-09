using GameProject5.Screens;
using System.Drawing.Printing;
using GameProject5.StateManagement;

namespace GameProject5.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {
       

        // The menuentry for sound effects audio options
        private readonly MenuEntry _sfxAudioMenuEntry;

        // The current audio volume
        private static float CurrentMusicVolume = GameSettings.MusicVolume;

        // The current sfx volume
        private static float CurrentSFXVolume = GameSettings.SFXVolume;

        public OptionsMenuScreen() : base("Options")
        {
           
            _sfxAudioMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            
            _sfxAudioMenuEntry.Selected += SFXAudioMenuEntrySelected;
            back.Selected += OnCancel;

            
            MenuEntries.Add(_sfxAudioMenuEntry);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _sfxAudioMenuEntry.Text = $"SFX Volume: {CurrentSFXVolume.ToString("P0")}";
        }

        /// <summary>
        /// Event handler for when the SFXAudio menu entry is selected, increments audio by 0.1f, until 1.0f which it resets to 0.1f
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SFXAudioMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            CurrentSFXVolume += 0.1f;
            GameSettings.SFXVolume = CurrentSFXVolume;

            if (CurrentSFXVolume > 1.01f)
                CurrentSFXVolume = 0.1f;
            GameSettings.SFXVolume = CurrentSFXVolume;
            SetMenuEntryText();
        }
    }
}
