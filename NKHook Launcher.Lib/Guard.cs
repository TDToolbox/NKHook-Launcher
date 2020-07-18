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
        /// <summary>
        /// Check if a string is valid
        /// </summary>
        /// <param name="text">string to check</param>
        /// <returns>whether or not string is valid</returns>
        public static bool IsStringValid(string text)
        {
            if (text != "" && text != null)
                return true;
            return false;
        }

        /// <summary>
        /// Check if text is valid json
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>Whether or not text is valid json</returns>
        public static bool IsJsonValid(string text)
        {
            if (!IsStringValid(text))
                return false;

            try
            {
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                dynamic result = serializer.DeserializeObject(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if file's text is valid json
        /// </summary>
        /// <param name="file">Read all text from this file and check if it's valid json</param>
        /// <returns>Whether or not json file has valid json</returns>
        public static bool IsJsonValid(FileInfo file)
        {
            if (!File.Exists(file.FullName))
            {
                Log.Output("Unable to validate json for the file: \"" + file.FullName + "\" File does not exist");
                return false;
            }

            return IsJsonValid(File.ReadAllText(file.FullName));
        }
    }
}
