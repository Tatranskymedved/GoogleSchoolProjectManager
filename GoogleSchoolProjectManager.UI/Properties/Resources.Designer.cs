﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoogleSchoolProjectManager.UI.Properties {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GoogleSchoolProjectManager.UI.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folders/files on Google Drive.
        /// </summary>
        public static string APP_GoogleDriveFolderFileList {
            get {
                return ResourceManager.GetString("APP_GoogleDriveFolderFileList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _Exit.
        /// </summary>
        public static string APP_MENU_Exit {
            get {
                return ResourceManager.GetString("APP_MENU_Exit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _File.
        /// </summary>
        public static string APP_MENU_File {
            get {
                return ResourceManager.GetString("APP_MENU_File", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to App Title.
        /// </summary>
        public static string APP_Title {
            get {
                return ResourceManager.GetString("APP_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refresh.
        /// </summary>
        public static string CMD_GetFolderTree {
            get {
                return ResourceManager.GetString("CMD_GetFolderTree", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select folder.
        /// </summary>
        public static string CMD_SelectFileTemplateSource {
            get {
                return ResourceManager.GetString("CMD_SelectFileTemplateSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select folder.
        /// </summary>
        public static string CMD_SelectFolder {
            get {
                return ResourceManager.GetString("CMD_SelectFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select week from date.
        /// </summary>
        public static string DatePicker_KHS_SelectDateFrom {
            get {
                return ResourceManager.GetString("DatePicker_KHS_SelectDateFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select week to date.
        /// </summary>
        public static string DatePicker_KHS_SelectDateTo {
            get {
                return ResourceManager.GetString("DatePicker_KHS_SelectDateTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Contacting server, downloading folders, initializing list..
        /// </summary>
        public static string DIALOG_CMDGetFolderTree_PROGRESS_Message {
            get {
                return ResourceManager.GetString("DIALOG_CMDGetFolderTree_PROGRESS_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Download in progress.
        /// </summary>
        public static string DIALOG_CMDGetFolderTree_PROGRESS_Title {
            get {
                return ResourceManager.GetString("DIALOG_CMDGetFolderTree_PROGRESS_Title", resourceCulture);
            }
        }
    }
}