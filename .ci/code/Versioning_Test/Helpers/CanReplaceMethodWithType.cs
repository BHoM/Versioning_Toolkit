using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Test.Versioning
{
    public static partial class Helpers
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static bool CanReplaceMethodWithType(string json)
        {
            string customJson = json.Replace(" \"_t\" : \"System.Reflection.MethodBase\", ", "");
            CustomObject customObj = Engine.Serialiser.Convert.FromJson(customJson) as CustomObject;
            if (customObj == null || !customObj.CustomData.ContainsKey("TypeName") || !customObj.CustomData.ContainsKey("MethodName"))
                return false;

            string typeJson = customObj.CustomData["TypeName"] as string;
            string methodName = customObj.CustomData["MethodName"] as string;
            if (typeJson == null || methodName == null)
                return false;

            List<string> split = typeJson.Split(new char[] { '"', ',' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (split.Count < 7)
                return false;
            string typeName = split[6];

            Type matchingType = MatchingType(typeName, methodName);
            return matchingType != null;
        }


        /*************************************/
        /**** Private Methods             ****/
        /*************************************/

        private static Type MatchingType(string typeName, string methodName)
        {
            if (m_TypeDic.Count == 0)
            {
                m_TypeDic = Engine.Reflection.Query.BHoMTypeList()
                    .GroupBy(x => x.Namespace.Split(new char[] { '.' })[2])
                    .ToDictionary(x => x.Key, x => x.ToList());
            }

            if (!typeName.EndsWith("Create"))
                return null;

            string key = typeName.Split(new char[] { '.' })[2];
            List<Type> types = new List<Type>();
            if (m_TypeDic.ContainsKey(key))
                types = m_TypeDic[key];

            Type match = null;
            List<Type> matches = types.Where(x => x.Name == methodName).ToList();
            if (matches.Count == 1)
                match = matches.First();

            if (match == null)
            {
                matches = types.Where(x => x.Name.StartsWith(methodName)).ToList();
                if (matches.Count == 1)
                    match = matches.First();
            }

            return match;
        }


        /*************************************/
        /**** Private Methods             ****/
        /*************************************/

        private static Dictionary<string, List<Type>> m_TypeDic = new Dictionary<string, List<Type>>();

        /*************************************/
    }
}
