namespace Toolkit.PresentationModel.Navigation
{
	public interface INavigable
	{
		void OnNavigatedTo(object parameter);
		void OnNavigatedFrom(object parameter);
	}
}
