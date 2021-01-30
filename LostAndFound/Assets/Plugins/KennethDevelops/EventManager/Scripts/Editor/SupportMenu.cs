using UnityEditor;
using UnityEngine;

public static class SupportMenu {

	[MenuItem("/KennethDevelops/Support/Report a bug")]
	public static void ShowBugReport(){
		Application.OpenURL("https://goo.gl/forms/k2bxEeoUS5XkfZKo2");
	}
	
	[MenuItem("/KennethDevelops/Support/Feedback")]
	public static void ShowCustomerFeedback(){
		Application.OpenURL("https://goo.gl/forms/Bltw82mtyn6pDobf1");
	}
	
	[MenuItem("/KennethDevelops/Support/Online Documentation/EventManager")]
	public static void ShowDocumentation(){
		Application.OpenURL("https://github.com/kgazcurra/EventManagerWiki/wiki");
	}
	
}