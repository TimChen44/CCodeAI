﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCodeAI.Resources {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CCodeAI.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 Cancel(_C) 的本地化字符串。
        /// </summary>
        public static string CancleBtn {
            get {
                return ResourceManager.GetString("CancleBtn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 ```{{$extension}}
        ///            {{$input}}
        ///            ```
        ///            解释代码： 的本地化字符串。
        /// </summary>
        public static string CodeExplain {
            get {
                return ResourceManager.GetString("CodeExplain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 ```{{$extension}}
        ///            {{$input}}
        ///            ```
        ///            给每一行代码添加注释： 的本地化字符串。
        /// </summary>
        public static string CodeNote {
            get {
                return ResourceManager.GetString("CodeNote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 ```{{$extension}}
        ///            {{$input}}
        ///            ```
        ///            优化代码： 的本地化字符串。
        /// </summary>
        public static string CodeOptimize {
            get {
                return ResourceManager.GetString("CodeOptimize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 ```{{$extension}}
        ///            {{$input}}
        ///            ```
        ///            根据注释续写代码，只需要返回新增的代码，不要返回其他内容： 的本地化字符串。
        /// </summary>
        public static string ContinuationCode {
            get {
                return ResourceManager.GetString("ContinuationCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Inquiry for AI, please wait... 的本地化字符串。
        /// </summary>
        public static string InquiryAI {
            get {
                return ResourceManager.GetString("InquiryAI", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Insert(_I) 的本地化字符串。
        /// </summary>
        public static string InsertBtn {
            get {
                return ResourceManager.GetString("InsertBtn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 When in doubt, ask AI. 的本地化字符串。
        /// </summary>
        public static string WhenDoubtAI {
            get {
                return ResourceManager.GetString("WhenDoubtAI", resourceCulture);
            }
        }
    }
}
