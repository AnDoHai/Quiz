
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
    
public partial class UserTestQuestionAnswer
{

    public int UserTestQuestionAnswerID { get; set; }

    public string UserTestQuestionAnswerText { get; set; }

    public Nullable<int> UserTestQuestionID { get; set; }

    public string Title { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> UpdatedDate { get; set; }

    public string UpdatedBy { get; set; }

    public bool Status { get; set; }

    public string Description { get; set; }

    public Nullable<int> Type { get; set; }

    public string Code { get; set; }

    public Nullable<double> Point { get; set; }



    public virtual UserTestQuestion UserTestQuestion { get; set; }

}

}