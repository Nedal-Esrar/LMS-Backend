IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Majors] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [DepartmentId] int NOT NULL,
    CONSTRAINT [PK_Majors] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Majors_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [WorkId] nvarchar(max) NOT NULL,
    [UserName] nvarchar(256) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Gender] int NOT NULL,
    [BirthDate] date NOT NULL,
    [EducationalLevel] int NOT NULL,
    [MajorId] int NOT NULL,
    [DepartmentId] int NOT NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AspNetUsers_Majors_MajorId] FOREIGN KEY ([MajorId]) REFERENCES [Majors] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] int NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RefreshTokens] (
    [Id] uniqueidentifier NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [Expiration] datetime2 NOT NULL,
    [UserId] int NOT NULL,
    [ApplicationUserId] int NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshTokens_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE INDEX [IX_AspNetUsers_DepartmentId] ON [AspNetUsers] ([DepartmentId]);
GO

CREATE INDEX [IX_AspNetUsers_MajorId] ON [AspNetUsers] ([MajorId]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Majors_DepartmentId] ON [Majors] ([DepartmentId]);
GO

CREATE INDEX [IX_RefreshTokens_ApplicationUserId] ON [RefreshTokens] ([ApplicationUserId]);
GO

CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241102122112_Initial', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'UserName');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [UserName] nvarchar(256) NULL;
GO

ALTER TABLE [AspNetUsers] ADD [FirstName] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [AspNetUsers] ADD [LastName] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [AspNetUsers] ADD [MiddleName] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241104192324_AddDetailedNames', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] ADD [RoleId] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [File] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [ContentType] nvarchar(max) NOT NULL,
    [Extension] nvarchar(max) NOT NULL,
    [Path] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_File] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_AspNetUsers_RoleId] ON [AspNetUsers] ([RoleId]);
GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241109083142_SimplifyRoles', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [File] DROP CONSTRAINT [PK_File];
GO

EXEC sp_rename N'[File]', N'Files';
GO

ALTER TABLE [Files] ADD CONSTRAINT [PK_Files] PRIMARY KEY ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241109092515_EditFileEntityName', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] ADD [ProfilePictureId] uniqueidentifier NULL;
GO

CREATE UNIQUE INDEX [IX_AspNetUsers_ProfilePictureId] ON [AspNetUsers] ([ProfilePictureId]) WHERE [ProfilePictureId] IS NOT NULL;
GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Files_ProfilePictureId] FOREIGN KEY ([ProfilePictureId]) REFERENCES [Files] ([Id]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241109154034_AddProfilePictureToUser', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Departments_DepartmentId];
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Majors_MajorId];
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'MajorId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [MajorId] int NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'DepartmentId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AspNetUsers] ALTER COLUMN [DepartmentId] int NULL;
GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]);
GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Majors_MajorId] FOREIGN KEY ([MajorId]) REFERENCES [Majors] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [RefreshTokens] ADD CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241115204535_NullableMajorDepartmentInApplicationUser', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId];
GO

ALTER TABLE [RefreshTokens] ADD CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241122104912_CascadeDeleteBehaviourOnUserRefreshToken', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Notifications] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Link] nvarchar(max) NULL,
    [IsRead] bit NOT NULL,
    [CreatedAtUtc] datetime2 NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241130185128_AddNotifications', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
GO

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
GO

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_AspNetRoles_RoleId];
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Departments_DepartmentId];
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Files_ProfilePictureId];
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Majors_MajorId];
GO

ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
GO

ALTER TABLE [Majors] DROP CONSTRAINT [FK_Majors_Departments_DepartmentId];
GO

ALTER TABLE [Notifications] DROP CONSTRAINT [FK_Notifications_AspNetUsers_UserId];
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_AspNetUsers_ApplicationUserId];
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId];
GO

ALTER TABLE [Majors] DROP CONSTRAINT [PK_Majors];
GO

ALTER TABLE [Files] DROP CONSTRAINT [PK_Files];
GO

ALTER TABLE [Departments] DROP CONSTRAINT [PK_Departments];
GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [PK_AspNetUsers];
GO

EXEC sp_rename N'[Majors]', N'Major';
GO

EXEC sp_rename N'[Files]', N'File';
GO

EXEC sp_rename N'[Departments]', N'Department';
GO

EXEC sp_rename N'[AspNetUsers]', N'User';
GO

EXEC sp_rename N'[Major].[IX_Majors_DepartmentId]', N'IX_Major_DepartmentId', N'INDEX';
GO

EXEC sp_rename N'[User].[IX_AspNetUsers_RoleId]', N'IX_User_RoleId', N'INDEX';
GO

EXEC sp_rename N'[User].[IX_AspNetUsers_ProfilePictureId]', N'IX_User_ProfilePictureId', N'INDEX';
GO

EXEC sp_rename N'[User].[IX_AspNetUsers_MajorId]', N'IX_User_MajorId', N'INDEX';
GO

EXEC sp_rename N'[User].[IX_AspNetUsers_DepartmentId]', N'IX_User_DepartmentId', N'INDEX';
GO

ALTER TABLE [Major] ADD CONSTRAINT [PK_Major] PRIMARY KEY ([Id]);
GO

ALTER TABLE [File] ADD CONSTRAINT [PK_File] PRIMARY KEY ([Id]);
GO

ALTER TABLE [Department] ADD CONSTRAINT [PK_Department] PRIMARY KEY ([Id]);
GO

ALTER TABLE [User] ADD CONSTRAINT [PK_User] PRIMARY KEY ([Id]);
GO

CREATE TABLE [Course] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ExpectedTimeToFinishHours] float NOT NULL,
    [ExpirationMonths] int NULL,
    [CreatedAtUtc] datetime2 NOT NULL,
    [UpdatedAtUtc] datetime2 NULL,
    [CreatedById] int NOT NULL,
    CONSTRAINT [PK_Course] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Course_User_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [User] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [CourseAssignment] (
    [CourseId] bigint NOT NULL,
    [MajorId] int NOT NULL,
    CONSTRAINT [PK_CourseAssignment] PRIMARY KEY ([CourseId], [MajorId]),
    CONSTRAINT [FK_CourseAssignment_Course_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Course] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseAssignment_Major_MajorId] FOREIGN KEY ([MajorId]) REFERENCES [Major] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [CourseMajor] (
    [AssignedMajorsId] int NOT NULL,
    [CoursesAssignedToId] bigint NOT NULL,
    CONSTRAINT [PK_CourseMajor] PRIMARY KEY ([AssignedMajorsId], [CoursesAssignedToId]),
    CONSTRAINT [FK_CourseMajor_Course_CoursesAssignedToId] FOREIGN KEY ([CoursesAssignedToId]) REFERENCES [Course] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseMajor_Major_AssignedMajorsId] FOREIGN KEY ([AssignedMajorsId]) REFERENCES [Major] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Section] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Index] int NOT NULL,
    [CourseId] bigint NOT NULL,
    CONSTRAINT [PK_Section] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Section_Course_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Course] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserCourse] (
    [UserId] int NOT NULL,
    [CourseId] bigint NOT NULL,
    [Status] int NOT NULL,
    [FinishedAtUtc] datetime2 NULL,
    CONSTRAINT [PK_UserCourse] PRIMARY KEY ([UserId], [CourseId]),
    CONSTRAINT [FK_UserCourse_Course_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Course] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserCourse_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [SectionPart] (
    [Id] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Index] int NOT NULL,
    [SectionId] bigint NOT NULL,
    [MaterialType] int NOT NULL,
    [FileId] uniqueidentifier NULL,
    [Link] nvarchar(max) NULL,
    [PassThresholdPoints] int NULL,
    CONSTRAINT [PK_SectionPart] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SectionPart_File_FileId] FOREIGN KEY ([FileId]) REFERENCES [File] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_SectionPart_Section_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Section] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Question] (
    [Id] bigint NOT NULL IDENTITY,
    [Text] nvarchar(max) NOT NULL,
    [Points] int NOT NULL,
    [SectionPartId] bigint NOT NULL,
    CONSTRAINT [PK_Question] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Question_SectionPart_SectionPartId] FOREIGN KEY ([SectionPartId]) REFERENCES [SectionPart] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserSectionPartDone] (
    [UserId] int NOT NULL,
    [SectionPartId] bigint NOT NULL,
    [IsDone] bit NOT NULL,
    CONSTRAINT [PK_UserSectionPartDone] PRIMARY KEY ([UserId], [SectionPartId]),
    CONSTRAINT [FK_UserSectionPartDone_SectionPart_SectionPartId] FOREIGN KEY ([SectionPartId]) REFERENCES [SectionPart] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserSectionPartDone_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserSectionPartExamState] (
    [UserId] int NOT NULL,
    [SectionPartId] bigint NOT NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_UserSectionPartExamState] PRIMARY KEY ([UserId], [SectionPartId]),
    CONSTRAINT [FK_UserSectionPartExamState_SectionPart_SectionPartId] FOREIGN KEY ([SectionPartId]) REFERENCES [SectionPart] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserSectionPartExamState_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Choice] (
    [Id] bigint NOT NULL IDENTITY,
    [Text] nvarchar(max) NOT NULL,
    [IsCorrect] bit NOT NULL,
    [QuestionId] bigint NOT NULL,
    CONSTRAINT [PK_Choice] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Choice_Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Choice_QuestionId] ON [Choice] ([QuestionId]);
GO

CREATE INDEX [IX_Course_CreatedById] ON [Course] ([CreatedById]);
GO

CREATE INDEX [IX_CourseAssignment_MajorId] ON [CourseAssignment] ([MajorId]);
GO

CREATE INDEX [IX_CourseMajor_CoursesAssignedToId] ON [CourseMajor] ([CoursesAssignedToId]);
GO

CREATE INDEX [IX_Question_SectionPartId] ON [Question] ([SectionPartId]);
GO

CREATE INDEX [IX_Section_CourseId] ON [Section] ([CourseId]);
GO

CREATE INDEX [IX_SectionPart_FileId] ON [SectionPart] ([FileId]);
GO

CREATE INDEX [IX_SectionPart_SectionId] ON [SectionPart] ([SectionId]);
GO

CREATE INDEX [IX_UserCourse_CourseId] ON [UserCourse] ([CourseId]);
GO

CREATE INDEX [IX_UserSectionPartDone_SectionPartId] ON [UserSectionPartDone] ([SectionPartId]);
GO

CREATE INDEX [IX_UserSectionPartExamState_SectionPartId] ON [UserSectionPartExamState] ([SectionPartId]);
GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [AspNetUserTokens] ADD CONSTRAINT [FK_AspNetUserTokens_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Major] ADD CONSTRAINT [FK_Major_Department_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Department] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [RefreshTokens] ADD CONSTRAINT [FK_RefreshTokens_User_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [User] ([Id]);
GO

ALTER TABLE [RefreshTokens] ADD CONSTRAINT [FK_RefreshTokens_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION;
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Department_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Department] ([Id]);
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_File_ProfilePictureId] FOREIGN KEY ([ProfilePictureId]) REFERENCES [File] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Major_MajorId] FOREIGN KEY ([MajorId]) REFERENCES [Major] ([Id]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241223174900_AddCourses', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [CourseMajor];
GO

DROP TABLE [UserSectionPartExamState];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SectionPart]') AND [c].[name] = N'PassThresholdPoints');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [SectionPart] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [SectionPart] DROP COLUMN [PassThresholdPoints];
GO

ALTER TABLE [SectionPart] ADD [ExamId] bigint NULL;
GO

ALTER TABLE [Question] ADD [ExamId] bigint NULL;
GO

ALTER TABLE [Question] ADD [ImageId] uniqueidentifier NULL;
GO

CREATE TABLE [Exam] (
    [Id] bigint NOT NULL IDENTITY,
    [DurationMinutes] int NOT NULL,
    [PassThresholdPoints] int NOT NULL,
    [SectionPartId] bigint NOT NULL,
    CONSTRAINT [PK_Exam] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserExamState] (
    [UserId] int NOT NULL,
    [ExamId] bigint NOT NULL,
    [Status] int NOT NULL,
    [SectionPartId] bigint NULL,
    CONSTRAINT [PK_UserExamState] PRIMARY KEY ([UserId], [ExamId]),
    CONSTRAINT [FK_UserExamState_Exam_ExamId] FOREIGN KEY ([ExamId]) REFERENCES [Exam] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserExamState_SectionPart_SectionPartId] FOREIGN KEY ([SectionPartId]) REFERENCES [SectionPart] ([Id]),
    CONSTRAINT [FK_UserExamState_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_SectionPart_ExamId] ON [SectionPart] ([ExamId]) WHERE [ExamId] IS NOT NULL;
GO

CREATE INDEX [IX_Question_ExamId] ON [Question] ([ExamId]);
GO

CREATE INDEX [IX_Question_ImageId] ON [Question] ([ImageId]);
GO

CREATE INDEX [IX_UserExamState_ExamId] ON [UserExamState] ([ExamId]);
GO

CREATE INDEX [IX_UserExamState_SectionPartId] ON [UserExamState] ([SectionPartId]);
GO

ALTER TABLE [Question] ADD CONSTRAINT [FK_Question_Exam_ExamId] FOREIGN KEY ([ExamId]) REFERENCES [Exam] ([Id]);
GO

ALTER TABLE [Question] ADD CONSTRAINT [FK_Question_File_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [File] ([Id]) ON DELETE SET NULL;
GO

ALTER TABLE [SectionPart] ADD CONSTRAINT [FK_SectionPart_Exam_ExamId] FOREIGN KEY ([ExamId]) REFERENCES [Exam] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241226220948_IntroduceExamEntity', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Question] DROP CONSTRAINT [FK_Question_SectionPart_SectionPartId];
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_User_ApplicationUserId];
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [FK_RefreshTokens_User_UserId];
GO

ALTER TABLE [UserExamState] DROP CONSTRAINT [FK_UserExamState_SectionPart_SectionPartId];
GO

DROP INDEX [IX_UserExamState_SectionPartId] ON [UserExamState];
GO

DROP INDEX [IX_Question_SectionPartId] ON [Question];
GO

ALTER TABLE [RefreshTokens] DROP CONSTRAINT [PK_RefreshTokens];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserExamState]') AND [c].[name] = N'SectionPartId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [UserExamState] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [UserExamState] DROP COLUMN [SectionPartId];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Question]') AND [c].[name] = N'SectionPartId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Question] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Question] DROP COLUMN [SectionPartId];
GO

EXEC sp_rename N'[RefreshTokens]', N'RefreshToken';
GO

EXEC sp_rename N'[RefreshToken].[IX_RefreshTokens_UserId]', N'IX_RefreshToken_UserId', N'INDEX';
GO

EXEC sp_rename N'[RefreshToken].[IX_RefreshTokens_ApplicationUserId]', N'IX_RefreshToken_ApplicationUserId', N'INDEX';
GO

DROP INDEX [IX_Question_ExamId] ON [Question];
DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Question]') AND [c].[name] = N'ExamId');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Question] DROP CONSTRAINT [' + @var6 + '];');
UPDATE [Question] SET [ExamId] = CAST(0 AS bigint) WHERE [ExamId] IS NULL;
ALTER TABLE [Question] ALTER COLUMN [ExamId] bigint NOT NULL;
ALTER TABLE [Question] ADD DEFAULT CAST(0 AS bigint) FOR [ExamId];
CREATE INDEX [IX_Question_ExamId] ON [Question] ([ExamId]);
GO

ALTER TABLE [Question] ADD [Index] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Choice] ADD [Index] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [RefreshToken] ADD CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([Id]);
GO

CREATE TABLE [ExamSession] (
    [Id] uniqueidentifier NOT NULL,
    [ExamId] bigint NOT NULL,
    [UserId] int NOT NULL,
    [StartDateUtc] datetime2 NOT NULL,
    [CheckpointQuestionId] bigint NOT NULL,
    [IsDone] bit NOT NULL,
    CONSTRAINT [PK_ExamSession] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExamSession_Exam_ExamId] FOREIGN KEY ([ExamId]) REFERENCES [Exam] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExamSession_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ExamSessionQuestionChoice] (
    [ExamSessionId] uniqueidentifier NOT NULL,
    [QuestionId] bigint NOT NULL,
    [ChoiceId] bigint NULL,
    CONSTRAINT [PK_ExamSessionQuestionChoice] PRIMARY KEY ([ExamSessionId], [QuestionId]),
    CONSTRAINT [FK_ExamSessionQuestionChoice_Choice_ChoiceId] FOREIGN KEY ([ChoiceId]) REFERENCES [Choice] ([Id]),
    CONSTRAINT [FK_ExamSessionQuestionChoice_ExamSession_ExamSessionId] FOREIGN KEY ([ExamSessionId]) REFERENCES [ExamSession] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExamSessionQuestionChoice_Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_ExamSession_ExamId] ON [ExamSession] ([ExamId]);
GO

CREATE INDEX [IX_ExamSession_UserId] ON [ExamSession] ([UserId]);
GO

CREATE INDEX [IX_ExamSessionQuestionChoice_ChoiceId] ON [ExamSessionQuestionChoice] ([ChoiceId]);
GO

CREATE INDEX [IX_ExamSessionQuestionChoice_QuestionId] ON [ExamSessionQuestionChoice] ([QuestionId]);
GO

ALTER TABLE [RefreshToken] ADD CONSTRAINT [FK_RefreshToken_User_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [User] ([Id]);
GO

ALTER TABLE [RefreshToken] ADD CONSTRAINT [FK_RefreshToken_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241230172635_ImproveExams', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [ExamSession] ADD [Grade] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241231214237_AddGradeToExamSession', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [UserCourse] ADD [StartedAtUtc] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250101213359_AddStartDateForUserCourse', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Course] DROP CONSTRAINT [FK_Course_User_CreatedById];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Course]') AND [c].[name] = N'CreatedById');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Course] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Course] ALTER COLUMN [CreatedById] int NULL;
GO

ALTER TABLE [Course] ADD CONSTRAINT [FK_Course_User_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [User] ([Id]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250108190058_SetCourseCreatedByUserDeleteBrahviorToSetNUll', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Notifications] DROP CONSTRAINT [FK_Notifications_User_UserId];
GO

ALTER TABLE [RefreshToken] DROP CONSTRAINT [FK_RefreshToken_User_ApplicationUserId];
GO

DROP INDEX [IX_RefreshToken_ApplicationUserId] ON [RefreshToken];
GO

ALTER TABLE [Notifications] DROP CONSTRAINT [PK_Notifications];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RefreshToken]') AND [c].[name] = N'ApplicationUserId');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [RefreshToken] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [RefreshToken] DROP COLUMN [ApplicationUserId];
GO

EXEC sp_rename N'[Notifications]', N'Notification';
GO

EXEC sp_rename N'[Notification].[IX_Notifications_UserId]', N'IX_Notification_UserId', N'INDEX';
GO

ALTER TABLE [Notification] ADD CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]);
GO

ALTER TABLE [Notification] ADD CONSTRAINT [FK_Notification_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250112185708_RemoveRedundantApplicationUserIdColumnInRefreshToken', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [User] DROP CONSTRAINT [FK_User_AspNetRoles_RoleId];
GO

DROP TABLE [AspNetRoleClaims];
GO

DROP TABLE [AspNetUserClaims];
GO

DROP TABLE [AspNetUserLogins];
GO

DROP TABLE [AspNetUserRoles];
GO

DROP TABLE [AspNetUserTokens];
GO

ALTER TABLE [AspNetRoles] DROP CONSTRAINT [PK_AspNetRoles];
GO

DROP INDEX [RoleNameIndex] ON [AspNetRoles];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'AccessFailedCount');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [User] DROP COLUMN [AccessFailedCount];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'ConcurrencyStamp');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [User] DROP COLUMN [ConcurrencyStamp];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'EmailConfirmed');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [User] DROP COLUMN [EmailConfirmed];
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'LockoutEnabled');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [User] DROP COLUMN [LockoutEnabled];
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'LockoutEnd');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [User] DROP COLUMN [LockoutEnd];
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[User]') AND [c].[name] = N'TwoFactorEnabled');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [User] DROP COLUMN [TwoFactorEnabled];
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoles]') AND [c].[name] = N'ConcurrencyStamp');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoles] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [AspNetRoles] DROP COLUMN [ConcurrencyStamp];
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoles]') AND [c].[name] = N'NormalizedName');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoles] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [AspNetRoles] DROP COLUMN [NormalizedName];
GO

EXEC sp_rename N'[AspNetRoles]', N'Role';
GO

ALTER TABLE [Role] ADD CONSTRAINT [PK_Role] PRIMARY KEY ([Id]);
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250112205035_CleanupUnusedIdentityTablesAndAttributes', N'8.0.10');
GO

COMMIT;
GO

