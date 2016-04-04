namespace Toolkit.PresentationModel.Navigation
{
	/// <summary>
	/// Provides a custom constructor for content URIs and modifies content URIs.
	/// </summary>
	public interface IUriBuilder
	{
		/// <summary>
		/// Builds the content URI.
		/// </summary>
		/// <param name="baseUri">The base URI.</param>
        /// <param name="extraData">The data object.</param>
		/// <returns></returns>
		string BuildUri(string baseUri, object extraData);
	}
}
