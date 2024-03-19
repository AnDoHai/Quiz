
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/12/2024 16:28:10
-- Generated from EDMX file: D:\Quiz\Tms.DataAccess\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QuizSystem];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Answers__ChoiceI__1F98B2C1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Answers] DROP CONSTRAINT [FK__Answers__ChoiceI__1F98B2C1];
GO
IF OBJECT_ID(N'[dbo].[FK__UserTest__UserId__5812160E]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTest] DROP CONSTRAINT [FK__UserTest__UserId__5812160E];
GO
IF OBJECT_ID(N'[dbo].[FK_Answers_Questions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Answers] DROP CONSTRAINT [FK_Answers_Questions];
GO
IF OBJECT_ID(N'[dbo].[FK_Choices_Questions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Choices] DROP CONSTRAINT [FK_Choices_Questions];
GO
IF OBJECT_ID(N'[dbo].[FK_Contest_Quiz]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contest] DROP CONSTRAINT [FK_Contest_Quiz];
GO
IF OBJECT_ID(N'[dbo].[FK_News_CategoryNews]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[News] DROP CONSTRAINT [FK_News_CategoryNews];
GO
IF OBJECT_ID(N'[dbo].[FK_Questions_Contest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_Questions_Contest];
GO
IF OBJECT_ID(N'[dbo].[FK_Questions_Quiz]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_Questions_Quiz];
GO
IF OBJECT_ID(N'[dbo].[FK_Questions_Section]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_Questions_Section];
GO
IF OBJECT_ID(N'[dbo].[FK_Quiz_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Quiz] DROP CONSTRAINT [FK_Quiz_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleModuleAction_ModuleAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleModuleAction] DROP CONSTRAINT [FK_RoleModuleAction_ModuleAction];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleModuleAction_Roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleModuleAction] DROP CONSTRAINT [FK_RoleModuleAction_Roles];
GO
IF OBJECT_ID(N'[dbo].[FK_Section_Quiz]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Section] DROP CONSTRAINT [FK_Section_Quiz];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRole_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserRole_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRole_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserRole_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTest_Quiz]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTest] DROP CONSTRAINT [FK_UserTest_Quiz];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestQuestion_Contest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestQuestion] DROP CONSTRAINT [FK_UserTestQuestion_Contest];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestQuestion_Questions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestQuestion] DROP CONSTRAINT [FK_UserTestQuestion_Questions];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestQuestion_Section]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestQuestion] DROP CONSTRAINT [FK_UserTestQuestion_Section];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestQuestion_UserTest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestQuestion] DROP CONSTRAINT [FK_UserTestQuestion_UserTest];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTestQuestionAnswers_UserTestQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserTestQuestionAnswers] DROP CONSTRAINT [FK_UserTestQuestionAnswers_UserTestQuestion];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccountToken]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountToken];
GO
IF OBJECT_ID(N'[dbo].[Answers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Answers];
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[CategoryNews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CategoryNews];
GO
IF OBJECT_ID(N'[dbo].[Choices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Choices];
GO
IF OBJECT_ID(N'[dbo].[Configuration]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Configuration];
GO
IF OBJECT_ID(N'[dbo].[Contest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contest];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[EmailTemplate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailTemplate];
GO
IF OBJECT_ID(N'[dbo].[ModuleAction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModuleAction];
GO
IF OBJECT_ID(N'[dbo].[News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[News];
GO
IF OBJECT_ID(N'[dbo].[Questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Questions];
GO
IF OBJECT_ID(N'[dbo].[Quiz]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Quiz];
GO
IF OBJECT_ID(N'[dbo].[RoleModuleAction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleModuleAction];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Section]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Section];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[UserTest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTest];
GO
IF OBJECT_ID(N'[dbo].[UserTestQuestion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTestQuestion];
GO
IF OBJECT_ID(N'[dbo].[UserTestQuestionAnswers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTestQuestionAnswers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AccountTokens'
CREATE TABLE [dbo].[AccountTokens] (
    [AccountTokenId] int IDENTITY(1,1) NOT NULL,
    [AccountId] int  NOT NULL,
    [IsAdminAccountSide] bit  NULL,
    [TokenKey] varchar(500)  NULL,
    [ExpiredDate] datetime  NULL,
    [TokenType] int  NOT NULL
);
GO

-- Creating table 'Answers'
CREATE TABLE [dbo].[Answers] (
    [AnswerID] int IDENTITY(1,1) NOT NULL,
    [AnswerText] nvarchar(max)  NULL,
    [QuestionID] int  NULL,
    [Title] nvarchar(1000)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [Type] int  NULL,
    [Code] nvarchar(1000)  NULL,
    [ChoiceID] int  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [Type] int  NULL,
    [TimeLimit] int  NULL,
    [PassingScore] int  NULL,
    [CertificateImageFont] nvarchar(2500)  NULL,
    [CertificateImageBack] nvarchar(2500)  NULL,
    [PreFix] nvarchar(10)  NULL
);
GO

-- Creating table 'CategoryNews'
CREATE TABLE [dbo].[CategoryNews] (
    [CategoryNewsId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'Choices'
CREATE TABLE [dbo].[Choices] (
    [ChoiceID] int IDENTITY(1,1) NOT NULL,
    [ChoiceText] nvarchar(max)  NULL,
    [QuestionID] int  NULL,
    [Title] nvarchar(1000)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [Type] int  NULL
);
GO

-- Creating table 'Configurations'
CREATE TABLE [dbo].[Configurations] (
    [ConfigurationId] int IDENTITY(1,1) NOT NULL,
    [ExchangeRate] decimal(19,4)  NULL,
    [AdminAcceptanceBalancePercentage] int  NOT NULL,
    [ClientAcceptanceBalancePercentage] int  NOT NULL
);
GO

-- Creating table 'Contests'
CREATE TABLE [dbo].[Contests] (
    [ContestID] int IDENTITY(1,1) NOT NULL,
    [ContestName] nvarchar(1000)  NULL,
    [Title] nvarchar(1000)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [TimeLimit] int  NULL,
    [Type] int  NULL,
    [Order] int  NULL,
    [QuizID] int  NULL,
    [FullScore] int  NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [CountryID] int  NOT NULL,
    [CountryName] varchar(50)  NOT NULL,
    [ISO2] char(2)  NULL,
    [ISO3] char(3)  NULL
);
GO

-- Creating table 'EmailTemplates'
CREATE TABLE [dbo].[EmailTemplates] (
    [EmailTemplateID] int IDENTITY(1,1) NOT NULL,
    [ApplicationID] int  NOT NULL,
    [Placed] varchar(256)  NOT NULL,
    [Cancelled] varchar(256)  NOT NULL,
    [Updated] varchar(256)  NOT NULL,
    [PartialApproval] varchar(256)  NOT NULL,
    [NotApproved] varchar(256)  NOT NULL,
    [Approved] varchar(256)  NOT NULL,
    [IntoReviewOnPlaced] varchar(256)  NOT NULL,
    [Done] varchar(256)  NULL,
    [Paid] varchar(256)  NULL
);
GO

-- Creating table 'ModuleActions'
CREATE TABLE [dbo].[ModuleActions] (
    [ModuleActionID] int IDENTITY(1,1) NOT NULL,
    [Module] varchar(50)  NULL,
    [Action] varchar(50)  NULL,
    [Description] varchar(500)  NULL,
    [OrderIndex] int  NULL,
    [Group] int  NULL
);
GO

-- Creating table 'News'
CREATE TABLE [dbo].[News] (
    [NewsId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(250)  NULL,
    [SortDescription] nvarchar(1000)  NULL,
    [Description] nvarchar(3000)  NULL,
    [IsHot] bit  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Author] nvarchar(50)  NOT NULL,
    [CategoryNewsId] int  NULL,
    [Image] nvarchar(1000)  NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [QuestionID] int IDENTITY(1,1) NOT NULL,
    [QuestionText] nvarchar(max)  NULL,
    [QuizID] int  NULL,
    [AudioUrl] nvarchar(1000)  NULL,
    [Title] nvarchar(1000)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [Point] float  NULL,
    [Type] int  NULL,
    [ContestID] int  NULL,
    [SectionID] int  NULL,
    [Order] int  NULL,
    [Layout] int  NULL,
    [TimeLimit] int  NULL,
    [ImageUrl] nvarchar(1000)  NULL,
    [MaxLengthText] int  NULL,
    [StatusTextbox] bit  NULL,
    [GroupIndex] int  NULL
);
GO

-- Creating table 'Quizs'
CREATE TABLE [dbo].[Quizs] (
    [QuizID] int IDENTITY(1,1) NOT NULL,
    [QuizName] nvarchar(500)  NULL,
    [Title] nvarchar(500)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [CategoryId] int  NULL,
    [TimeLimit] int  NULL,
    [Type] int  NULL,
    [TotalScore] int  NULL
);
GO

-- Creating table 'RoleModuleActions'
CREATE TABLE [dbo].[RoleModuleActions] (
    [RoleModuleActionID] int IDENTITY(1,1) NOT NULL,
    [RoleID] int  NOT NULL,
    [ModuleActionID] int  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Note] nvarchar(300)  NULL,
    [Status] bit  NOT NULL,
    [Type] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [CreatedByUserId] nvarchar(50)  NULL,
    [UpdatedUserId] nvarchar(50)  NULL
);
GO

-- Creating table 'Sections'
CREATE TABLE [dbo].[Sections] (
    [SectionID] int IDENTITY(1,1) NOT NULL,
    [SectionName] nvarchar(500)  NULL,
    [Title] nvarchar(500)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Type] int  NULL,
    [Order] int  NULL,
    [QuizID] int  NULL,
    [ContestID] int  NULL,
    [TimeLimit] nvarchar(200)  NULL,
    [OrderIndex] int  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NULL,
    [Email] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [PasswordSalt] nvarchar(max)  NULL,
    [IsSupperAdmin] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [LastLoginDate] datetime  NULL,
    [LastActivityDate] datetime  NULL,
    [IsLockedOut] bit  NOT NULL,
    [Avatar] nvarchar(max)  NULL,
    [Status] bit  NOT NULL,
    [FullName] nvarchar(150)  NULL,
    [Tel] nvarchar(50)  NULL,
    [FirstName] nvarchar(150)  NULL,
    [LastName] nvarchar(150)  NULL,
    [Gender] int  NULL,
    [DateOfBirth] datetime  NULL,
    [PlaceOfBirth] nvarchar(500)  NULL,
    [Address] nvarchar(500)  NULL,
    [IdentityCardNo] varchar(20)  NULL,
    [PhoneNo] varchar(20)  NULL,
    [ChineseName] nvarchar(100)  NULL,
    [Level] int  NULL,
    [CountryCode] nvarchar(50)  NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [UserRoleId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [CreatedDate] datetime  NULL
);
GO

-- Creating table 'UserTests'
CREATE TABLE [dbo].[UserTests] (
    [UserTestId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NULL,
    [QuizID] int  NULL,
    [Title] nvarchar(250)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [TotalPoint] float  NULL
);
GO

-- Creating table 'UserTestQuestions'
CREATE TABLE [dbo].[UserTestQuestions] (
    [UserTestQuestionId] int IDENTITY(1,1) NOT NULL,
    [UserTestId] int  NULL,
    [QuestionID] int  NULL,
    [Title] nvarchar(500)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(2000)  NULL,
    [ContestID] int  NULL,
    [SectionID] int  NULL
);
GO

-- Creating table 'UserTestQuestionAnswers'
CREATE TABLE [dbo].[UserTestQuestionAnswers] (
    [UserTestQuestionAnswerID] int IDENTITY(1,1) NOT NULL,
    [UserTestQuestionAnswerText] nvarchar(max)  NULL,
    [UserTestQuestionID] int  NULL,
    [Title] nvarchar(1000)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [UpdatedDate] datetime  NULL,
    [UpdatedBy] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Description] nvarchar(1000)  NULL,
    [Type] int  NULL,
    [Code] nvarchar(1000)  NULL,
    [Point] float  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AccountTokenId] in table 'AccountTokens'
ALTER TABLE [dbo].[AccountTokens]
ADD CONSTRAINT [PK_AccountTokens]
    PRIMARY KEY CLUSTERED ([AccountTokenId] ASC);
GO

-- Creating primary key on [AnswerID] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [PK_Answers]
    PRIMARY KEY CLUSTERED ([AnswerID] ASC);
GO

-- Creating primary key on [CategoryId] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [CategoryNewsId] in table 'CategoryNews'
ALTER TABLE [dbo].[CategoryNews]
ADD CONSTRAINT [PK_CategoryNews]
    PRIMARY KEY CLUSTERED ([CategoryNewsId] ASC);
GO

-- Creating primary key on [ChoiceID] in table 'Choices'
ALTER TABLE [dbo].[Choices]
ADD CONSTRAINT [PK_Choices]
    PRIMARY KEY CLUSTERED ([ChoiceID] ASC);
GO

-- Creating primary key on [ConfigurationId] in table 'Configurations'
ALTER TABLE [dbo].[Configurations]
ADD CONSTRAINT [PK_Configurations]
    PRIMARY KEY CLUSTERED ([ConfigurationId] ASC);
GO

-- Creating primary key on [ContestID] in table 'Contests'
ALTER TABLE [dbo].[Contests]
ADD CONSTRAINT [PK_Contests]
    PRIMARY KEY CLUSTERED ([ContestID] ASC);
GO

-- Creating primary key on [CountryID] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([CountryID] ASC);
GO

-- Creating primary key on [EmailTemplateID] in table 'EmailTemplates'
ALTER TABLE [dbo].[EmailTemplates]
ADD CONSTRAINT [PK_EmailTemplates]
    PRIMARY KEY CLUSTERED ([EmailTemplateID] ASC);
GO

-- Creating primary key on [ModuleActionID] in table 'ModuleActions'
ALTER TABLE [dbo].[ModuleActions]
ADD CONSTRAINT [PK_ModuleActions]
    PRIMARY KEY CLUSTERED ([ModuleActionID] ASC);
GO

-- Creating primary key on [NewsId] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [PK_News]
    PRIMARY KEY CLUSTERED ([NewsId] ASC);
GO

-- Creating primary key on [QuestionID] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [PK_Questions]
    PRIMARY KEY CLUSTERED ([QuestionID] ASC);
GO

-- Creating primary key on [QuizID] in table 'Quizs'
ALTER TABLE [dbo].[Quizs]
ADD CONSTRAINT [PK_Quizs]
    PRIMARY KEY CLUSTERED ([QuizID] ASC);
GO

-- Creating primary key on [RoleModuleActionID] in table 'RoleModuleActions'
ALTER TABLE [dbo].[RoleModuleActions]
ADD CONSTRAINT [PK_RoleModuleActions]
    PRIMARY KEY CLUSTERED ([RoleModuleActionID] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [SectionID] in table 'Sections'
ALTER TABLE [dbo].[Sections]
ADD CONSTRAINT [PK_Sections]
    PRIMARY KEY CLUSTERED ([SectionID] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [UserRoleId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([UserRoleId] ASC);
GO

-- Creating primary key on [UserTestId] in table 'UserTests'
ALTER TABLE [dbo].[UserTests]
ADD CONSTRAINT [PK_UserTests]
    PRIMARY KEY CLUSTERED ([UserTestId] ASC);
GO

-- Creating primary key on [UserTestQuestionId] in table 'UserTestQuestions'
ALTER TABLE [dbo].[UserTestQuestions]
ADD CONSTRAINT [PK_UserTestQuestions]
    PRIMARY KEY CLUSTERED ([UserTestQuestionId] ASC);
GO

-- Creating primary key on [UserTestQuestionAnswerID] in table 'UserTestQuestionAnswers'
ALTER TABLE [dbo].[UserTestQuestionAnswers]
ADD CONSTRAINT [PK_UserTestQuestionAnswers]
    PRIMARY KEY CLUSTERED ([UserTestQuestionAnswerID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ChoiceID] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [FK__Answers__ChoiceI__1F98B2C1]
    FOREIGN KEY ([ChoiceID])
    REFERENCES [dbo].[Choices]
        ([ChoiceID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Answers__ChoiceI__1F98B2C1'
CREATE INDEX [IX_FK__Answers__ChoiceI__1F98B2C1]
ON [dbo].[Answers]
    ([ChoiceID]);
GO

-- Creating foreign key on [QuestionID] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [FK_Answers_Questions]
    FOREIGN KEY ([QuestionID])
    REFERENCES [dbo].[Questions]
        ([QuestionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Answers_Questions'
CREATE INDEX [IX_FK_Answers_Questions]
ON [dbo].[Answers]
    ([QuestionID]);
GO

-- Creating foreign key on [CategoryId] in table 'Quizs'
ALTER TABLE [dbo].[Quizs]
ADD CONSTRAINT [FK_Quiz_Category]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Quiz_Category'
CREATE INDEX [IX_FK_Quiz_Category]
ON [dbo].[Quizs]
    ([CategoryId]);
GO

-- Creating foreign key on [CategoryNewsId] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [FK_News_CategoryNews]
    FOREIGN KEY ([CategoryNewsId])
    REFERENCES [dbo].[CategoryNews]
        ([CategoryNewsId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_News_CategoryNews'
CREATE INDEX [IX_FK_News_CategoryNews]
ON [dbo].[News]
    ([CategoryNewsId]);
GO

-- Creating foreign key on [QuestionID] in table 'Choices'
ALTER TABLE [dbo].[Choices]
ADD CONSTRAINT [FK_Choices_Questions]
    FOREIGN KEY ([QuestionID])
    REFERENCES [dbo].[Questions]
        ([QuestionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Choices_Questions'
CREATE INDEX [IX_FK_Choices_Questions]
ON [dbo].[Choices]
    ([QuestionID]);
GO

-- Creating foreign key on [QuizID] in table 'Contests'
ALTER TABLE [dbo].[Contests]
ADD CONSTRAINT [FK_Contest_Quiz]
    FOREIGN KEY ([QuizID])
    REFERENCES [dbo].[Quizs]
        ([QuizID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contest_Quiz'
CREATE INDEX [IX_FK_Contest_Quiz]
ON [dbo].[Contests]
    ([QuizID]);
GO

-- Creating foreign key on [ContestID] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_Questions_Contest]
    FOREIGN KEY ([ContestID])
    REFERENCES [dbo].[Contests]
        ([ContestID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Questions_Contest'
CREATE INDEX [IX_FK_Questions_Contest]
ON [dbo].[Questions]
    ([ContestID]);
GO

-- Creating foreign key on [ContestID] in table 'Sections'
ALTER TABLE [dbo].[Sections]
ADD CONSTRAINT [FK_Section_Quiz]
    FOREIGN KEY ([ContestID])
    REFERENCES [dbo].[Contests]
        ([ContestID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Section_Quiz'
CREATE INDEX [IX_FK_Section_Quiz]
ON [dbo].[Sections]
    ([ContestID]);
GO

-- Creating foreign key on [ContestID] in table 'UserTestQuestions'
ALTER TABLE [dbo].[UserTestQuestions]
ADD CONSTRAINT [FK_UserTestQuestion_Contest]
    FOREIGN KEY ([ContestID])
    REFERENCES [dbo].[Contests]
        ([ContestID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestQuestion_Contest'
CREATE INDEX [IX_FK_UserTestQuestion_Contest]
ON [dbo].[UserTestQuestions]
    ([ContestID]);
GO

-- Creating foreign key on [ModuleActionID] in table 'RoleModuleActions'
ALTER TABLE [dbo].[RoleModuleActions]
ADD CONSTRAINT [FK_RoleModuleAction_ModuleAction]
    FOREIGN KEY ([ModuleActionID])
    REFERENCES [dbo].[ModuleActions]
        ([ModuleActionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleModuleAction_ModuleAction'
CREATE INDEX [IX_FK_RoleModuleAction_ModuleAction]
ON [dbo].[RoleModuleActions]
    ([ModuleActionID]);
GO

-- Creating foreign key on [QuizID] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_Questions_Quiz]
    FOREIGN KEY ([QuizID])
    REFERENCES [dbo].[Quizs]
        ([QuizID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Questions_Quiz'
CREATE INDEX [IX_FK_Questions_Quiz]
ON [dbo].[Questions]
    ([QuizID]);
GO

-- Creating foreign key on [SectionID] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_Questions_Section]
    FOREIGN KEY ([SectionID])
    REFERENCES [dbo].[Sections]
        ([SectionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Questions_Section'
CREATE INDEX [IX_FK_Questions_Section]
ON [dbo].[Questions]
    ([SectionID]);
GO

-- Creating foreign key on [QuestionID] in table 'UserTestQuestions'
ALTER TABLE [dbo].[UserTestQuestions]
ADD CONSTRAINT [FK_UserTestQuestion_Questions]
    FOREIGN KEY ([QuestionID])
    REFERENCES [dbo].[Questions]
        ([QuestionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestQuestion_Questions'
CREATE INDEX [IX_FK_UserTestQuestion_Questions]
ON [dbo].[UserTestQuestions]
    ([QuestionID]);
GO

-- Creating foreign key on [QuizID] in table 'UserTests'
ALTER TABLE [dbo].[UserTests]
ADD CONSTRAINT [FK_UserTest_Quiz]
    FOREIGN KEY ([QuizID])
    REFERENCES [dbo].[Quizs]
        ([QuizID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTest_Quiz'
CREATE INDEX [IX_FK_UserTest_Quiz]
ON [dbo].[UserTests]
    ([QuizID]);
GO

-- Creating foreign key on [RoleID] in table 'RoleModuleActions'
ALTER TABLE [dbo].[RoleModuleActions]
ADD CONSTRAINT [FK_RoleModuleAction_Roles]
    FOREIGN KEY ([RoleID])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleModuleAction_Roles'
CREATE INDEX [IX_FK_RoleModuleAction_Roles]
ON [dbo].[RoleModuleActions]
    ([RoleID]);
GO

-- Creating foreign key on [RoleId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRole_Role]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[Roles]
        ([RoleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole_Role'
CREATE INDEX [IX_FK_UserRole_Role]
ON [dbo].[UserRoles]
    ([RoleId]);
GO

-- Creating foreign key on [SectionID] in table 'UserTestQuestions'
ALTER TABLE [dbo].[UserTestQuestions]
ADD CONSTRAINT [FK_UserTestQuestion_Section]
    FOREIGN KEY ([SectionID])
    REFERENCES [dbo].[Sections]
        ([SectionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestQuestion_Section'
CREATE INDEX [IX_FK_UserTestQuestion_Section]
ON [dbo].[UserTestQuestions]
    ([SectionID]);
GO

-- Creating foreign key on [UserId] in table 'UserTests'
ALTER TABLE [dbo].[UserTests]
ADD CONSTRAINT [FK__UserTest__UserId__5812160E]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__UserTest__UserId__5812160E'
CREATE INDEX [IX_FK__UserTest__UserId__5812160E]
ON [dbo].[UserTests]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRole_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole_User'
CREATE INDEX [IX_FK_UserRole_User]
ON [dbo].[UserRoles]
    ([UserId]);
GO

-- Creating foreign key on [UserTestId] in table 'UserTestQuestions'
ALTER TABLE [dbo].[UserTestQuestions]
ADD CONSTRAINT [FK_UserTestQuestion_UserTest]
    FOREIGN KEY ([UserTestId])
    REFERENCES [dbo].[UserTests]
        ([UserTestId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestQuestion_UserTest'
CREATE INDEX [IX_FK_UserTestQuestion_UserTest]
ON [dbo].[UserTestQuestions]
    ([UserTestId]);
GO

-- Creating foreign key on [UserTestQuestionID] in table 'UserTestQuestionAnswers'
ALTER TABLE [dbo].[UserTestQuestionAnswers]
ADD CONSTRAINT [FK_UserTestQuestionAnswers_UserTestQuestion]
    FOREIGN KEY ([UserTestQuestionID])
    REFERENCES [dbo].[UserTestQuestions]
        ([UserTestQuestionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTestQuestionAnswers_UserTestQuestion'
CREATE INDEX [IX_FK_UserTestQuestionAnswers_UserTestQuestion]
ON [dbo].[UserTestQuestionAnswers]
    ([UserTestQuestionID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------