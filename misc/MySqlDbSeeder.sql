CREATE TABLE `RoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_RoleClaims` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `Roles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `NormalizedName` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Roles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `User` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `FirstName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `LastName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Nic` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Address` longtext CHARACTER SET utf8mb4 NULL,
    `DateOfBirth` longtext CHARACTER SET utf8mb4 NULL,
    `Discriminator` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Speciality` int NULL,
    `UserName` longtext CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` longtext CHARACTER SET utf8mb4 NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` longtext CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_User` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `UserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_UserClaims` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `UserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_UserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `UserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_UserRoles` PRIMARY KEY (`UserId`, `RoleId`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `UserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_UserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`)
) CHARACTER SET=utf8mb4;


CREATE TABLE `Appointment` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AppointmentDate` datetime(6) NOT NULL,
    `AppointmentTime` time(6) NOT NULL,
    `PatientId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `DoctorId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Appointment` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Appointment_User_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `User` (`Id`),
    CONSTRAINT `FK_Appointment_User_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `User` (`Id`)
) CHARACTER SET=utf8mb4;


CREATE INDEX `IX_Appointment_DoctorId` ON `Appointment` (`DoctorId`);


CREATE INDEX `IX_Appointment_PatientId` ON `Appointment` (`PatientId`);



-- Seed data for Roles table
INSERT INTO Roles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('72c5c746-1b34-423a-b474-58bc9d268279', 'Doctor', 'DOCTOR', 'c914c3ca-87be-4500-956e-85fecf66e52d'),
       ('d40be4cc-b67f-429a-8bc8-483ce89e314e', 'Administrator', 'ADMINISTRATOR', '289ef3a1-9595-4606-bb90-82001c1df934'),
       ('dd8d68cd-786a-47c6-a191-7811c9bcb580', 'Patient', 'PATIENT', 'f559ae1e-3046-488a-8b0d-410e1b2b82ab');


-- Seed data for User table
INSERT INTO User (Id, FirstName, LastName, Password, PhoneNumber, Nic, Discriminator, Speciality, DateOfBirth, Address, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) 
VALUES 
('601cc81c-1811-4e78-81ea-aa3d7c29c137', 'Jane', 'Doe', 'Hemotrack123', '0123456789', '00100', 'Doctor', 1, NULL, NULL, 'Jane', 'JANE', 'janedoe@hemotrack.com', 'JANEDOE@HEMOTRACK.COM', 0, 'AQAAAAEAACcQAAAAEG+jbOgNdrS1tP4gCuzZJZrFs82A0DnrAhv6cNtIH06ANkeShHrJaY9iQI/U6yCe4A==', 'O7423PV7LFDTXTJQ76NHDKROFNLYZXMC', '1ada8938-3a33-48f0-a421-51ad13c65e79', 0, 0, NULL, 1, 0),
('6e32c966-2a53-48f0-be72-d24e309382cd', 'Jane', 'Smith', 'Hemotrack123', '0123456789', '00104', 'Patient', NULL, NULL, NULL, 'JaneSmith', 'JANESMITH', 'janesmith@hemotrack.com', 'JANESMITH@HEMOTRACK.COM', 0, 'AQAAAAEAACcQAAAAED4IFmmau+n1KdIgRMfEVVzTT9yW87/zVYC0yAO143qP5tEhIn3R94mPOO7Zc3lDrQ==', 'LZLT2E66Z2JUAPVMU3IEN2FG6RODR66Z', 'fe2b1c37-c754-448c-9394-68f54acec905', 0, 0, NULL, 1, 0),
('e153c902-e30f-4329-acbd-70e64676f76b', 'John', 'Smith', 'Hemotrack123', '0123456789', '00103', 'Patient', NULL, NULL, NULL, 'JohnSmith', 'JOHNSMITH', 'johnsmith@hemotrack.com', 'JOHNSMITH@HEMOTRACK.COM', 0, 'AQAAAAEAACcQAAAAEJN/U5lXWraeI3QN3nAJbV6Xf1a6BGVSbziMJJxmrmJU0SBHFi1nXqT6zcU8dSEDVA==', '73UFGPPLLBZZFIIBAAECTYYLFT4GROSK', '8f32b967-6b0f-4dda-b4d6-eedd8478cc8b', 0, 0, NULL, 1, 0),
('f8659bd9-7d6f-48a4-816b-b4bd55c975cd', 'John', 'Doe', 'Hemotrack123', '0123456789', '00100', 'Doctor', NULL, NULL, NULL, 'John', 'JOHN', 'johndoe@hemotrack.com', 'JOHNDOE@HEMOTRACK.COM', 0, 'AQAAAAEAACcQAAAAEB/H0oCdDa802A6ewtfpJHVn/FhVuH7TU4KCzJPV1Z6esWwry72anDlRvDENDAHo6Q==', 'VTA2U3O4ALGNC75BYQZK5UBC3IJS3SDZ', 'f24f72f1-a461-4f0d-a4cd-84e07c915ab1', 0, 0, NULL, 1, 0),
('f9150d2c-d8d8-4a86-86d4-4a66fe963087', 'admin', '', 'Hemotrack123', '0000', '0000', 'Administrator', NULL, NULL, NULL, 'admin', 'ADMIN', 'admin@hemotrack.com', 'ADMIN@HEMOTRACK.COM', 0, 'AQAAAAEAACcQAAAAEDgO3+wedlfxFwktrtP4L4AAHMfGoZjd6IynAB33HkkFmtpXSMEyr0+rkB9ym17few==', 'GXUFXY7WZXFIAXC57E4A55RLGB2LGH6K', '9d620d9e-6516-4057-bcbc-d82b4ae65320', 0, 0, NULL, 1, 0);

-- Seed data for Appointment table 
INSERT INTO Appointment (Id, Title, AppointmentDate, AppointmentTime, PatientId, DoctorId) 
VALUES 
(1, 'First Session', '2024-02-29', '14:00:00.0000000', 'e153c902-e30f-4329-acbd-70e64676f76b', 'f8659bd9-7d6f-48a4-816b-b4bd55c975cd'), 
(2, 'Second session', '2024-02-29', '06:00:00.0000000', 'e153c902-e30f-4329-acbd-70e64676f76b', '601cc81c-1811-4e78-81ea-aa3d7c29c137'), 
(3, 'First appointment', '2024-02-29', '10:00:00.0000000', '6e32c966-2a53-48f0-be72-d24e309382cd', 'f8659bd9-7d6f-48a4-816b-b4bd55c975cd'), 
(4, 'Second appointment', '2024-03-01', '10:00:00.0000000', '6e32c966-2a53-48f0-be72-d24e309382cd', '601cc81c-1811-4e78-81ea-aa3d7c29c137');


INSERT INTO UserRoles (UserId,RoleId) VALUES
	 (N'601cc81c-1811-4e78-81ea-aa3d7c29c137',N'72c5c746-1b34-423a-b474-58bc9d268279'),
	 (N'6e32c966-2a53-48f0-be72-d24e309382cd',N'dd8d68cd-786a-47c6-a191-7811c9bcb580'),
	 (N'd1a16a19-891a-437f-8d03-90b2de2fed70',N'd40be4cc-b67f-429a-8bc8-483ce89e314e'),
	 (N'e153c902-e30f-4329-acbd-70e64676f76b',N'dd8d68cd-786a-47c6-a191-7811c9bcb580'),
	 (N'f8659bd9-7d6f-48a4-816b-b4bd55c975cd',N'72c5c746-1b34-423a-b474-58bc9d268279'),
	 (N'f9150d2c-d8d8-4a86-86d4-4a66fe963087',N'd40be4cc-b67f-429a-8bc8-483ce89e314e');