﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeSharpenerCryptoAnalyzer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CodeSharpenerCryptoAnalyzer.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Method Parameters Specified are Invalid.
        /// </summary>
        internal static string ConstraintAnalyzerDescription {
            get {
                return ResourceManager.GetString("ConstraintAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Value &apos;{0}&apos; is Invalid. Valid Values are &apos;{1}&apos;.
        /// </summary>
        internal static string ConstraintAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ConstraintAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Method Parameter.
        /// </summary>
        internal static string ConstraintAnalyzerTitle {
            get {
                return ResourceManager.GetString("ConstraintAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event Aggregation Condition not Satisfied.
        /// </summary>
        internal static string EventAggAnalyzerDescription {
            get {
                return ResourceManager.GetString("EventAggAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event &apos;{0}&apos; does not Satisfy Aggregation Condition.
        /// </summary>
        internal static string EventAggAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("EventAggAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event Aggregation Condition Violation.
        /// </summary>
        internal static string EventAggAnalyzerTitle {
            get {
                return ResourceManager.GetString("EventAggAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a Valid Event Signature.
        /// </summary>
        internal static string EventAnalyzerDescription {
            get {
                return ResourceManager.GetString("EventAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Method &apos;{0}&apos; is Invalid. Valid Method Signatures(s) are &apos;{1}&apos;.
        /// </summary>
        internal static string EventAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("EventAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Event Constraint Violation.
        /// </summary>
        internal static string EventAnalyzerTitle {
            get {
                return ResourceManager.GetString("EventAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Events are Invoked in not a Valid Order.
        /// </summary>
        internal static string OrderAnalyzerDescription {
            get {
                return ResourceManager.GetString("OrderAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &apos;{0}&apos; Events are Invoked in an Incorrect Order. Valid Order of Invocation is &apos;{1}&apos;.
        /// </summary>
        internal static string OrderAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("OrderAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Events Order.
        /// </summary>
        internal static string OrderAnalyzerTitle {
            get {
                return ResourceManager.GetString("OrderAnalyzerTitle", resourceCulture);
            }
        }
    }
}
