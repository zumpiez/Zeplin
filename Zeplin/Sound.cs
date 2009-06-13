//Zeplin Engine - Sound.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Zeplin
{
    /// <summary>
    /// Very basic wrapper for XNA's sound library. 
    /// </summary>
    /// <remarks>Does not support any type besides SoundEffect right now.</remarks>
    public class Sound
    {
        /// <summary>
        /// Constructs a Sound object from the specified resource string
        /// </summary>
        /// <param name="resourceName">The name of a SoundEffect resource</param>
        public Sound(string resourceName)
        {
            soundEffect = Engine.Content.Load<SoundEffect>(resourceName);
        }

        /// <summary>
        /// Plays this sound unless it is already playing.
        /// </summary>
        public void Play()
        {
            if(instance == null || instance.State != SoundState.Playing)
                instance = soundEffect.Play();
        }

        /// <summary>
        /// Plays this sound even if it is already playing.
        /// </summary>
        /// <remarks>A sound played in this way cannot be paused or stopped or otherwise manipulated.</remarks>
        public void PlayWithoutChecking()
        {
            soundEffect.Play();
        }

        /// <summary>
        /// Plays this sound unless it is already playing.
        /// </summary>
        /// <param name="volume">The volume of the sound. Valid values are between 0.0 (silent) and 1.0 (full).</param>
        /// <param name="pitch">Pitch adjustment, ranging from -1.0 (one octave down) to 1.0 (one octave up). 0.0 is </param>
        /// <param name="pan">Panning, ranging from -1.0 (full left) to 1.0 (full right). 0.0 is centered.</param>
        /// <param name="loop">Whether to continue playing the song indefinitely, until stopped or paused manually, or set to no longer loop.</param>
        public void Play(float volume, float pitch, float pan, bool loop)
        {
            if (instance == null || instance.State != SoundState.Playing)
                instance = soundEffect.Play(volume, pitch , pan, loop);
        }

        /// <summary>
        /// Stops the sound.
        /// </summary>
        /// <param name="immediate">Specifies whether to stop playing immediately, or to continue playing until the end of the sound.</param>
        public void Stop(bool immediate)
        {
            if(instance != null)
                instance.Stop(immediate);
        }

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        public void Pause()
        {
            if(instance != null)
                instance.Pause();
        }
        
        SoundEffect soundEffect;
        SoundEffectInstance instance = null;
    }
}
