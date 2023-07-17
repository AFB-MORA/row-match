namespace Com.AFBiyik.AudioSystem {
    /// <summary>
    /// Controls music.
    /// <example>
    /// <code>
    /// [Inject]
    /// private ISoundController2d soundController2d;
    ///
    /// private void FooAction() {
    ///     soundController2d.PlaySound(new Sound(Sounds.Foo_SOUND));
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public interface ISoundController2d : IAudioController {
    }
}
