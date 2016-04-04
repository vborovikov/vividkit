namespace Toolkit.PresentationModel.Navigation
{
	using System.Collections.Generic;

	public interface ITombstone
	{
		void OnSerializing(IDictionary<string, object> state);
		void OnDeserializing(IDictionary<string, object> state);
	}
}
