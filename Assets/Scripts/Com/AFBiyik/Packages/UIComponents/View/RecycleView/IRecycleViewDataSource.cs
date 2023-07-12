using UnityEngine;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Data source for recycle view
    /// </summary>
    public interface IRecycleViewDataSource
    {
        /// <summary>
        /// Number of items int the data
        /// </summary>
        int NumberOfItems { get; }

        /// <summary>
        /// Item height
        /// </summary>
        float ItemHeight { get; }

        /// <summary>
        /// Creates view
        /// </summary>
        /// <returns></returns>
        GameObject CreateView();

        /// <summary>
        /// Update views
        /// </summary>
        /// <param name="view">View to update</param>
        /// <param name="index">Data index</param>
        /// <returns></returns>
        GameObject SetView(GameObject view, int index);
    }
}
