using CryslData;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CodeSharpenerCryptoAnalysis.Models
{
    public class MethodSignatureModel
    {
        public string MethodName { get; set; }
        public ICollection<ArgumentTypes> Parameters { get; set; }       
    }
}
