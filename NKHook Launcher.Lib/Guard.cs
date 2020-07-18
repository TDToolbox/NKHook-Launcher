using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHook_Launcher.Lib
{
    public class Guard
    {
        public static bool IsStringValid(string text)
        {
            if (text != "" && text != null)
                return true;
            return false;
        }


        public static bool IsJsonValid(string json)
        {
            if (!IsStringValid(json))
                return false;

            try
            {
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                dynamic result = serializer.DeserializeObject(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsJsonValid(FileInfo json)
        {
            if (!File.Exists(json.FullName))
            {
                Log.Output("Unable to validate json for the file: \"" + json.FullName + "\" File does not exist");
                return false;
            }

            return IsJsonValid(File.ReadAllText(json.FullName));
        }
    }
}
