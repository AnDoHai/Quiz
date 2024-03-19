﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class QuizSystemEntities : DbContext
{
    public QuizSystemEntities()
        : base("name=QuizSystemEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<AccountToken> AccountTokens { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryNew> CategoryNews { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Contest> Contests { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<ModuleAction> ModuleActions { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizs { get; set; }

    public virtual DbSet<RoleModuleAction> RoleModuleActions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTestQuestionAnswer> UserTestQuestionAnswers { get; set; }

    public virtual DbSet<UserTestQuestion> UserTestQuestions { get; set; }

    public virtual DbSet<UserTest> UserTests { get; set; }

}

}
