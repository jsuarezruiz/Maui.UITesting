﻿namespace Microsoft.Maui.Automation.Driver
{
	public interface IDriver
	{
		string Name { get; }

		IAutomationConfiguration Configuration { get; }

		Task<IDeviceInfo> GetDeviceInfo();

		Task InstallApp(string file, string appId);
		Task RemoveApp(string appId);

		Task LaunchApp(string appId);
		Task StopApp(string appId);

		Task ClearAppState(string appId);

		Task PushFile(string appId, string localFile, string destinationDirectory);
        Task PullFile(string appId, string remoteFile, string localDirectory);

        Task Tap(int x, int y);

		Task LongPress(int x, int y);

		Task KeyPress(char keyCode);

		Task Swipe((int x, int y) start, (int x, int y) end);

		Task Back();

		Task InputText(string text);

		Task OpenUri(string uri);

		Task<string> GetProperty(Platform platform, string elementId, string propertyName);

		Task<IEnumerable<Element>> GetElements(Platform platform);

		Task<IEnumerable<Element>> FindElements(Platform platform, string propertyName, string pattern, bool isExpression = false, string ancestorId = "");
	}
}