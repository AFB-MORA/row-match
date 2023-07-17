namespace Com.AFBiyik.PopupSystem
{
    /// <summary>
    /// Popup data for popup events.
    /// </summary>
    public struct PopupData
    {
        /// <summary>
        /// Currently active popup
        /// </summary>
        public readonly IPopup activePopup;

        /// <summary>
        /// Closed popup or the popup below
        /// </summary>
        public readonly IPopup previousPopup;

        public PopupData(IPopup activePopup, IPopup previousPopup)
        {
            this.activePopup = activePopup;
            this.previousPopup = previousPopup;
        }
    }
}
