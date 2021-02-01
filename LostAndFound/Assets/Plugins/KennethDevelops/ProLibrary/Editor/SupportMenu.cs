using UnityEditor;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Editor{
    
    public static class SupportMenu{
        
        [MenuItem("/KennethDevelops/Support/Online Documentation/ProLibrary")]
        public static void ShowDocumentation(){
            Application.OpenURL("https://github.com/kgazcurra/ProLibraryWiki/wiki");
        }
        
    }
    
}