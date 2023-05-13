using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CCodeAI.Models
{
    [ComVisible(true)]
    public  class ConfigOptions : DialogPage
    {
        [Category("CCodeAI")]
        [DisplayName("Endpoint")]
        [Description("Azure OpenAI Endpoint")]
        public string Endpoint { get; set; } 

        [Category("CCodeAI")]
        [DisplayName("AppKey")]
        [Description("Azure OpenAI AppKey")]
        public string AppKey { get; set; } 
    }
}
