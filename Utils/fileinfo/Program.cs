using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace fileinfo
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1) return 1;
            var file = args[0];
            Assembly ass;
            try { ass = Assembly.LoadFrom(file); }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }
            var version = ass.GetName().Version;
            Console.WriteLine(version.ToString());
            return 0;
        }
    }
}
