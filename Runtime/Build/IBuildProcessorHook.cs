namespace Varguiniano.Core.Runtime.Build
{
    /// <summary>
    /// Interface that defines a build processor hook.
    /// </summary>
    public interface IBuildProcessorHook
    {
        /// <summary>
        /// Run your hook.
        /// </summary>
        bool RunHook();
    }
}