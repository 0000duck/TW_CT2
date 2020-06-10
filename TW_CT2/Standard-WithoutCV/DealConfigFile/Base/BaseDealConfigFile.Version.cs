using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using System.Reflection;

namespace DealConfigFile
{
    [Serializable]
    public class BaseParConfigFile:BaseClass
    {
        #region 版本
        public static VerInfo GetVersion()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyName assemblyName = assembly.GetName();
                Version version = assemblyName.Version;

                VerInfo v = new VerInfo();
                v.Name = assemblyName.Name;
                v.Version = version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + "." + version.Revision.ToString();
                v.PathDll = assembly.Location;
                return v;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion 版本
    }
}
