
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
    
public partial class Choice
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Choice()
    {

        this.Answers = new HashSet<Answer>();

    }


    public int ChoiceID { get; set; }

    public string ChoiceText { get; set; }

    public Nullable<int> QuestionID { get; set; }

    public string Title { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> UpdatedDate { get; set; }

    public string UpdatedBy { get; set; }

    public bool Status { get; set; }

    public string Description { get; set; }

    public Nullable<int> Type { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Answer> Answers { get; set; }

    public virtual Question Question { get; set; }

}

}
