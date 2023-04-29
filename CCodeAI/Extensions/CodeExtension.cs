using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCodeAI.Extensions
{
    public static class CodeExtension
    {
        public static string GetCodeType(string extension)
        {
            return extension switch
            {
                ".cs" => "C#",
                ".js" => "JavaScript",
                ".ts" => "TypeScript",
                ".cpp" => "C++",
                _ => ""
            };
        }
    }
}
