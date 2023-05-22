using System;
using System.Collections.Generic;
using System.Text;

namespace CCodeAI.QuickActions.Skills;

public class CodeOptimizationSemanticFunction
{
    public const string CodeOptimize = """
            ```{{$extension}}
            {{$input}}
            ```
            优化代码：
            """;
}
