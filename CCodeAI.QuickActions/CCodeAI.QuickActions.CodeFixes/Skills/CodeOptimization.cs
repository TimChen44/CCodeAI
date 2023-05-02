using System;
using System.Collections.Generic;
using System.Text;

namespace CCodeAI.QuickActions.Skills;

internal class CodeOptimizationSemanticFuncation
{
    public const string CodeOptimize = """
            ```{{$extension}}
            {{$input}}
            ```
            优化代码：
            """;
}
