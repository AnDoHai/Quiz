
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Tms.DataAccess
{

using System;
    using System.Collections.Generic;
    
public partial class RoleModuleAction
{

    public int RoleModuleActionID { get; set; }

    public int RoleID { get; set; }

    public int ModuleActionID { get; set; }



    public virtual ModuleAction ModuleAction { get; set; }

    public virtual Role Role { get; set; }

}

}