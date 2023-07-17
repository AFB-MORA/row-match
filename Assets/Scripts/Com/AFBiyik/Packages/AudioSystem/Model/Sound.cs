using System;

namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Playable sound.
    /// </summary>
    [Serializable]
    public struct Sound {
        /// <summary>
        /// Addressable key of the sound.
        /// </summary>
        public string name;

        /// <summary>
        /// Loop the sound.
        /// </summary>
        public bool loop;

        /// <summary>
        /// Volume of the sound. Between 0 and 1.
        /// </summary>
        public float volume;

        /// <summary>
        /// Start time of the sound.
        /// </summary>
        public float startTime;

        /// <summary>
        /// Length of the sound to play after <see cref="startTime"/>
        /// </summary>
        public float length;

        /// <summary>
        /// Auto despawns sound after sound completes.
        /// </summary>
        public bool autoDespawn;

        /// <summary>
        /// Playable sound.
        /// <br/>
        /// If loop is true, autoDespawn is automatically false.
        /// <br/>
        /// Do not set end time and length at the same time.
        /// </summary>
        /// <param name="name">Addressable key of the sound.</param>
        /// <param name="loop">Loop the sound.</param>
        /// <param name="volume">Volume of the sound. Between 0 and 1.</param>
        /// <param name="startTime">Start time of the sound.</param>
        /// <param name="length">Length of the sound to play after <see cref="startTime"/></param>
        /// <param name="endTime">End time of the sound.</param>
        /// <param name="autoDespawn">Auto despawns sound after sound completes.</param>
        public Sound(string name, bool loop = false, float volume = 1f, float startTime = 0, float length = -1, float endTime = -1, bool autoDespawn = true) : this() {
            this.name = name;
            this.loop = loop;
            this.volume = volume;
            this.length = -1;
            this.autoDespawn = autoDespawn;

            if (loop) {
                this.autoDespawn = false;
            }

            if (startTime < 0) {
                startTime = 0;
            }

            this.startTime = startTime;

            if (length > 0) {
                this.length = length;
            }
            else if (endTime > 0) {
                this.length = endTime - startTime;
            }
        }
    }
}
