﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Xbim.COBie.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorDescription {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorDescription() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Xbim.COBie.Resources.ErrorDescription", typeof(ErrorDescription).Assembly);
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
        ///   Looks up a localized string similar to Value must be alpha-numeric.
        /// </summary>
        internal static string AlphaNumeric_Value_Expected {
            get {
                return ResourceManager.GetString("AlphaNumeric_Value_Expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be a valid email address.
        /// </summary>
        internal static string Email_Value_Expected {
            get {
                return ResourceManager.GetString("Email_Value_Expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must exist in column {1} of the {0} sheet.
        /// </summary>
        internal static string ForeignKey_Violation {
            get {
                return ResourceManager.GetString("ForeignKey_Violation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be a valid date.
        /// </summary>
        internal static string ISODate_Value_Expected {
            get {
                return ResourceManager.GetString("ISODate_Value_Expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to meter.
        /// </summary>
        internal static string meter {
            get {
                return ResourceManager.GetString("meter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value.
        /// </summary>
        internal static string Null_ForeignKey_Value {
            get {
                return ResourceManager.GetString("Null_ForeignKey_Value", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be a valid number.
        /// </summary>
        internal static string Numeric_Value_Expected {
            get {
                return ResourceManager.GetString("Numeric_Value_Expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must exist in column {1} of the {0} sheet.
        /// </summary>
        internal static string PickList_Violation {
            get {
                return ResourceManager.GetString("PickList_Violation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be unique.
        /// </summary>
        internal static string PrimaryKey_Violation {
            get {
                return ResourceManager.GetString("PrimaryKey_Violation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required value is empty.
        /// </summary>
        internal static string Text_Value_Expected {
            get {
                return ResourceManager.GetString("Text_Value_Expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be under {0} characters.
        /// </summary>
        internal static string Value_Out_of_Bounds {
            get {
                return ResourceManager.GetString("Value_Out_of_Bounds", resourceCulture);
            }
        }
    }
}
