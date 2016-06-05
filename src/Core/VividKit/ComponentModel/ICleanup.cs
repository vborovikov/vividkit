namespace Toolkit.ComponentModel
{
    /// <summary>
    /// Defines a method to reset the state of an object.
    /// </summary>
    public interface ICleanup
    {
        /// <summary>
        /// Resets the state.
        /// </summary>
        void Cleanup();
    }
}