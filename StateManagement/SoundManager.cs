using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;


namespace GameProject5.StateManagement
{
    /// <summary>
    /// Manages sound effects and the background music for the game
    /// </summary>
    public class SoundManager
    {
        // The sound effect to be played when a placement is 'bad'
        private SoundEffect _badPlaceSound;

        // The sound effect to be played when a placement is 'good'
        private SoundEffect _goodPlaceSound;


        /// <summary>
        /// Loads the content for the sound effects and background music
        /// </summary>
        /// <param name="content">The content manager to load with</param>
        public void LoadContent(ContentManager content)
        {
            
            _badPlaceSound = content.Load<SoundEffect>("Bad");
            _goodPlaceSound = content.Load<SoundEffect>("Success");           
        }

        /// <summary>
        /// Plays the bad place sound effect
        /// </summary>
        public void PlayBadPlaceSound()
        {
            _badPlaceSound.Play(GameSettings.SFXVolume, 0, 0);
        }

        /// <summary>
        /// Plays the good place sound effect
        /// </summary>
        public void PlayGoodPlaceSound()
        {
            _goodPlaceSound.Play(GameSettings.SFXVolume, 0, 0);
        }

    }
}
