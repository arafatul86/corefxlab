﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.IO.Compression.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class BrotliEx {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BrotliEx() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("System.IO.Compression.Brotli.Resources.BrotliEx", typeof(BrotliEx).GetTypeInfo().Assembly);
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
        ///   Looks up a localized string similar to Decoder instance create fail.
        /// </summary>
        internal static string DecoderInstanceCreate {
            get {
                return ResourceManager.GetString("DecoderInstanceCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encoder instance create fail.
        /// </summary>
        internal static string EncoderInstanceCreate {
            get {
                return ResourceManager.GetString("EncoderInstanceCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected decompress finish.
        /// </summary>
        internal static string FinishDecompress {
            get {
                return ResourceManager.GetString("FinishDecompress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument is invalid.
        /// </summary>
        internal static string InvalidArgument {
            get {
                return ResourceManager.GetString("InvalidArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mode change is not permitted .
        /// </summary>
        internal static string InvalidModeChange {
            get {
                return ResourceManager.GetString("InvalidModeChange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quality and WindowSize is ambitious for Decompress mode.
        /// </summary>
        internal static string QualityAndWinSize {
            get {
                return ResourceManager.GetString("QualityAndWinSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stream disposed or not created.
        /// </summary>
        internal static string StreamDisposed {
            get {
                return ResourceManager.GetString("StreamDisposed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error ReadTimeout exceeded.
        /// </summary>
        internal static string TimeoutRead {
            get {
                return ResourceManager.GetString("TimeoutRead", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error WriteTimeout exceeded.
        /// </summary>
        internal static string TimeoutWrite {
            get {
                return ResourceManager.GetString("TimeoutWrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to unable to decode stream.
        /// </summary>
        internal static string unableDecode {
            get {
                return ResourceManager.GetString("unableDecode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to unable to compress stream.
        /// </summary>
        internal static string unableEncode {
            get {
                return ResourceManager.GetString("unableEncode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong stream mode. Expect: Compress.
        /// </summary>
        internal static string WrongModeCompress {
            get {
                return ResourceManager.GetString("WrongModeCompress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong stream mode. Expect: Decompress.
        /// </summary>
        internal static string WrongModeDecompress {
            get {
                return ResourceManager.GetString("WrongModeDecompress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect value of quality.
        /// </summary>
        internal static string WrongQuality {
            get {
                return ResourceManager.GetString("WrongQuality", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect value of WindowSize.
        /// </summary>
        internal static string WrongWindowSize {
            get {
                return ResourceManager.GetString("WrongWindowSize", resourceCulture);
            }
        }
    }
}
