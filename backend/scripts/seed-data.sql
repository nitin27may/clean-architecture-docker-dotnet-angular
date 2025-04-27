-- Enable UUID extension if not already enabled
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create tables
CREATE TABLE IF NOT EXISTS "ActivityLog" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "Username" VARCHAR(250) NOT NULL,
    "Email" VARCHAR(250) NOT NULL,
    "Activity" VARCHAR(250) NOT NULL,
    "Endpoint" VARCHAR(250) NOT NULL,
    "HttpMethod" VARCHAR(10) NOT NULL,
    "Timestamp" TIMESTAMP WITH TIME ZONE NOT NULL,
    "IpAddress" VARCHAR(45),
    "UserAgent" VARCHAR(250)
);

CREATE TABLE IF NOT EXISTS "Contacts" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "DateOfBirth" DATE,
    "Mobile" VARCHAR(20) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "City" VARCHAR(100) NOT NULL,
    "PostalCode" VARCHAR(10) NOT NULL,
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "Operations" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(100) NOT NULL,
    "Description" VARCHAR(250),
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "Pages" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(100) NOT NULL,
    "Url" VARCHAR(250) NOT NULL,
    "Order" INT NOT NULL DEFAULT 0,
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "Permissions" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PageId" UUID NOT NULL,
    "OperationId" UUID NOT NULL,
    "Description" VARCHAR(250),
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "Roles" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(100) NOT NULL,
    "Description" VARCHAR(250),
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "RolePermissions" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "RoleId" UUID NOT NULL,
    "PermissionId" UUID NOT NULL,
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "UserRoles" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "RoleId" UUID NOT NULL,
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "FirstName" VARCHAR(250) NOT NULL,
    "LastName" VARCHAR(250) NOT NULL,
    "UserName" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(200) NOT NULL,
    "Mobile" VARCHAR(20) NOT NULL,
    "Password" TEXT NOT NULL,
    "CreatedOn" TIMESTAMP WITH TIME ZONE NOT NULL,
    "CreatedBy" UUID,
    "UpdatedOn" TIMESTAMP WITH TIME ZONE,
    "UpdatedBy" UUID
);

-- Set up foreign key constraints
ALTER TABLE "Permissions" ADD CONSTRAINT "FK_Permissions_Operations" 
    FOREIGN KEY("OperationId") REFERENCES "Operations"("Id");

ALTER TABLE "Permissions" ADD CONSTRAINT "FK_Permissions_Pages" 
    FOREIGN KEY("PageId") REFERENCES "Pages"("Id");

-- Add unique constraint for PageId and OperationId combination
ALTER TABLE "Permissions" ADD CONSTRAINT "UQ_Permission_Page_Operation" 
    UNIQUE ("PageId", "OperationId");

-- Add unique constraint for Page Name
ALTER TABLE "Pages" ADD CONSTRAINT "UQ_Page_Name" 
    UNIQUE ("Name");

-- Add unique constraint for Operation Name
ALTER TABLE "Operations" ADD CONSTRAINT "UQ_Operation_Name" 
    UNIQUE ("Name");

-- Add unique constraint for Role Name
ALTER TABLE "Roles" ADD CONSTRAINT "UQ_Role_Name" 
    UNIQUE ("Name");

-- Add unique constraint for RoleId and PermissionId combination
ALTER TABLE "RolePermissions" ADD CONSTRAINT "UQ_RolePermission_Role_Permission" 
    UNIQUE ("RoleId", "PermissionId");

ALTER TABLE "RolePermissions" ADD CONSTRAINT "FK_RolePermissions_Permissions" 
    FOREIGN KEY("PermissionId") REFERENCES "Permissions"("Id");

ALTER TABLE "RolePermissions" ADD CONSTRAINT "FK_RolePermissions_Roles" 
    FOREIGN KEY("RoleId") REFERENCES "Roles"("Id");

ALTER TABLE "UserRoles" ADD CONSTRAINT "FK_RoleUsers_Roles" 
    FOREIGN KEY("RoleId") REFERENCES "Roles"("Id");

ALTER TABLE "UserRoles" ADD CONSTRAINT "FK_RoleUsers_Users" 
    FOREIGN KEY("UserId") REFERENCES "Users"("Id");

-- Insert data into Operations table
INSERT INTO "Operations" ("Id", "Name", "Description", "CreatedOn", "CreatedBy") VALUES
('cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'Delete', 'Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('dce8d805-df41-4549-be7b-6ed5647b09c3', 'Update', 'Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('7493f274-5007-4e17-9840-88c9a096422f', 'Read', 'Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('09be3f29-6429-4089-a2a9-a17efe46cd7b', 'Create', 'Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');

-- Insert data into Pages table
INSERT INTO "Pages" ("Id", "Name", "Url", "Order", "CreatedOn", "CreatedBy") VALUES
('e4943131-a642-4352-9725-e44ba5972b4e', 'Operations', 'admin/operations', 3, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d4943131-a642-4352-9725-e44ba5972b4d', 'Pages', 'admin/pages', 2, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('a4943131-a642-4352-9725-e44ba5972b47', 'Roles', 'admin/roles', 4, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('b4943131-a642-4352-9725-e44ba5972b46', 'RolePermissionMapping', 'admin/role-permission-mapping', 5, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('c4943131-a642-4352-9725-e44ba5972b4b', 'Users', 'admin/users', 6, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('94943131-a642-4352-9725-e44ba5972b49', 'UserRoles', 'admin/user-roles', 7, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('aa56a391-e880-4ac5-9f6f-6c8aa33454b8', 'Contacts', '/contacts', 1, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('c4943131-a642-4352-9725-e44ba5972b4c', 'ActivityLog', 'admin/activity-logs', 10, NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');

-- Insert data into Roles table
INSERT INTO "Roles" ("Id", "Name", "Description", "CreatedOn", "CreatedBy") VALUES
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'Admin', 'This is Admin Role', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('3a07551f-7473-44a6-a664-e6c7c834902b', 'Reader', 'This is a Reader role', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('104102f5-e0ec-4739-8fda-f05552b677c3', 'Editor', 'This is editor role', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');

-- Insert initial admin user
INSERT INTO "Users" ("Id", "FirstName", "LastName", "UserName", "Email", "Mobile", "Password", "CreatedOn") VALUES
('26402b6c-ebdd-44c3-9188-659a134819cb', 'Nitin', 'Singh', 'nitin27may@gmail.com', 'nitin27may@gmail.com', '+91983336412', 'AQAAAAIAAYagAAAAEC1iNqNI7oqJKNcpJ+kYreWvBzjMxE/FWhfoDXzP5CoV60u6JHm5PwHIb3w7K7lWxw==', TIMESTAMPTZ '2024-08-24 02:56:23.6635113+00');

-- Insert additional users
INSERT INTO "Users" ("Id", "FirstName", "LastName", "UserName", "Email", "Mobile", "Password", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy")
VALUES
    ('424ffb80-05bf-43f8-8814-2772a5de2543', 'Sachin', 'Singh', 'reader@gmail.com', 'reader@gmail.com', '+91983336421', 'AQAAAAIAAYagAAAAEC1iNqNI7oqJKNcpJ+kYreWvBzjMxE/FWhfoDXzP5CoV60u6JHm5PwHIb3w7K7lWxw==', TIMESTAMPTZ '2024-09-05 14:48:17.0399044+00', '00000000-0000-0000-0000-000000000000', NULL, NULL),
    ('3aa35df1-2578-4ed3-a93b-8b8eb955499e', 'Vikram', 'Singh', 'editor@gmail.com', 'editor@gmail.com', '+91983336421', 'AQAAAAIAAYagAAAAEC1iNqNI7oqJKNcpJ+kYreWvBzjMxE/FWhfoDXzP5CoV60u6JHm5PwHIb3w7K7lWxw==', TIMESTAMPTZ '2024-08-28 20:29:02.2362893+00', '00000000-0000-0000-0000-000000000000', NULL, NULL);
-- Assign roles to the new users
INSERT INTO "UserRoles" ("Id", "UserId", "RoleId", "CreatedOn", "CreatedBy") VALUES
    (uuid_generate_v4(), '424ffb80-05bf-43f8-8814-2772a5de2543', '3a07551f-7473-44a6-a664-e6c7c834902b', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),  -- Reader role for Sachin
    (uuid_generate_v4(), '26402b6c-ebdd-44c3-9188-659a134819cb', 'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),  -- Admin role for Nitin
    (uuid_generate_v4(), '3aa35df1-2578-4ed3-a93b-8b8eb955499e', '104102f5-e0ec-4739-8fda-f05552b677c3', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'); -- Editor role for Vikram

-- Insert permissions
INSERT INTO "Permissions" ("Id", "PageId", "OperationId", "Description", "CreatedOn", "CreatedBy") VALUES
('d35daa4e-fd02-4934-98d2-5b06e9b694b8', 'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'Contacts Write', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('82755e66-b743-46e2-b612-efd2db6bcc75', 'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'Contacts Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('a6657401-0d2b-4bf9-9314-26be98878822', 'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', '7493f274-5007-4e17-9840-88c9a096422f', 'Contacts Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d5282999-a12a-45ad-b7d0-2becc27d5e92', 'aa56a391-e880-4ac5-9f6f-6c8aa33454b8', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'Contacts Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('c94c23ad-59d4-4f80-91ee-39316140cb17', 'c4943131-a642-4352-9725-e44ba5972b4b', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'User Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('c94c23ad-59d4-4f80-91ee-39316140db17', 'c4943131-a642-4352-9725-e44ba5972b4b', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'User Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('82755e66-b743-46e2-b612-efd2db6bcd75', 'c4943131-a642-4352-9725-e44ba5972b4b', '7493f274-5007-4e17-9840-88c9a096422f', 'User Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('82755e66-b743-46e2-b612-efd2db6bce75', 'c4943131-a642-4352-9725-e44ba5972b4b', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'User Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694b9', 'c4943131-a642-4352-9725-e44ba5972b4c', '7493f274-5007-4e17-9840-88c9a096422f', 'ActivityLog Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c0', 'd4943131-a642-4352-9725-e44ba5972b4d', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'Pages Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c1', 'd4943131-a642-4352-9725-e44ba5972b4d', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'Pages Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c2', 'd4943131-a642-4352-9725-e44ba5972b4d', '7493f274-5007-4e17-9840-88c9a096422f', 'Pages Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c3', 'd4943131-a642-4352-9725-e44ba5972b4d', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'Pages Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c4', 'e4943131-a642-4352-9725-e44ba5972b4e', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'Operations Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c5', 'e4943131-a642-4352-9725-e44ba5972b4e', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'Operations Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c6', 'e4943131-a642-4352-9725-e44ba5972b4e', '7493f274-5007-4e17-9840-88c9a096422f', 'Operations Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d35daa4e-fd02-4934-98d2-5b06e9b694c7', 'e4943131-a642-4352-9725-e44ba5972b4e', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'Operations Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- Add permissions for Roles page
('635daa4e-fd02-4934-98d2-5b06e9b694e1', 'a4943131-a642-4352-9725-e44ba5972b47', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'Roles Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('635daa4e-fd02-4934-98d2-5b06e9b694e2', 'a4943131-a642-4352-9725-e44ba5972b47', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'Roles Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('635daa4e-fd02-4934-98d2-5b06e9b694e3', 'a4943131-a642-4352-9725-e44ba5972b47', '7493f274-5007-4e17-9840-88c9a096422f', 'Roles Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('635daa4e-fd02-4934-98d2-5b06e9b694e4', 'a4943131-a642-4352-9725-e44ba5972b47', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'Roles Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- Add permissions for RolePermissionMapping page
('735daa4e-fd02-4934-98d2-5b06e9b694e5', 'b4943131-a642-4352-9725-e44ba5972b46', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'RolePermissionMapping Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('735daa4e-fd02-4934-98d2-5b06e9b694e6', 'b4943131-a642-4352-9725-e44ba5972b46', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'RolePermissionMapping Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('735daa4e-fd02-4934-98d2-5b06e9b694e7', 'b4943131-a642-4352-9725-e44ba5972b46', '7493f274-5007-4e17-9840-88c9a096422f', 'RolePermissionMapping Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('735daa4e-fd02-4934-98d2-5b06e9b694e8', 'b4943131-a642-4352-9725-e44ba5972b46', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'RolePermissionMapping Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- Add permissions for UserRoles page
('f35daa4e-fd02-4934-98d2-5b06e9b694d6', '94943131-a642-4352-9725-e44ba5972b49', '09be3f29-6429-4089-a2a9-a17efe46cd7b', 'UserRoles Create', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('f35daa4e-fd02-4934-98d2-5b06e9b694d7', '94943131-a642-4352-9725-e44ba5972b49', 'dce8d805-df41-4549-be7b-6ed5647b09c3', 'UserRoles Update', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('f35daa4e-fd02-4934-98d2-5b06e9b694d8', '94943131-a642-4352-9725-e44ba5972b49', '7493f274-5007-4e17-9840-88c9a096422f', 'UserRoles Read', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('f35daa4e-fd02-4934-98d2-5b06e9b694d9', '94943131-a642-4352-9725-e44ba5972b49', 'cef15d6f-25e4-422b-a7d6-405aaa2de2d5', 'UserRoles Delete', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');

-- Set up admin role permissions
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "CreatedOn", "CreatedBy") VALUES
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'a6657401-0d2b-4bf9-9314-26be98878822', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694b8', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '82755e66-b743-46e2-b612-efd2db6bcc75', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd5282999-a12a-45ad-b7d0-2becc27d5e92', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'c94c23ad-59d4-4f80-91ee-39316140cb17', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'c94c23ad-59d4-4f80-91ee-39316140db17', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '82755e66-b743-46e2-b612-efd2db6bcd75', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '82755e66-b743-46e2-b612-efd2db6bce75', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694b9', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c0', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c1', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c2', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c3', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c4', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c5', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c6', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'd35daa4e-fd02-4934-98d2-5b06e9b694c7', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- Assign UserRoles permissions to admin role
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'f35daa4e-fd02-4934-98d2-5b06e9b694d6', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'f35daa4e-fd02-4934-98d2-5b06e9b694d7', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'f35daa4e-fd02-4934-98d2-5b06e9b694d8', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', 'f35daa4e-fd02-4934-98d2-5b06e9b694d9', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- Assign Roles permissions to admin role
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '635daa4e-fd02-4934-98d2-5b06e9b694e1', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '635daa4e-fd02-4934-98d2-5b06e9b694e2', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '635daa4e-fd02-4934-98d2-5b06e9b694e3', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '635daa4e-fd02-4934-98d2-5b06e9b694e4', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
-- Assign RolePermissionMapping permissions to admin role
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '735daa4e-fd02-4934-98d2-5b06e9b694e5', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Create
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '735daa4e-fd02-4934-98d2-5b06e9b694e6', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Update
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '735daa4e-fd02-4934-98d2-5b06e9b694e7', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Read
('d95d2348-1d79-4b93-96d4-e48e87fcb4b5', '735daa4e-fd02-4934-98d2-5b06e9b694e8', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'); -- Delete

-- Set up editor role permissions (can read, create and update)
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "CreatedOn", "CreatedBy") VALUES
-- Contact permissions for editor
('104102f5-e0ec-4739-8fda-f05552b677c3', 'a6657401-0d2b-4bf9-9314-26be98878822', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Read
('104102f5-e0ec-4739-8fda-f05552b677c3', 'd35daa4e-fd02-4934-98d2-5b06e9b694b8', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Create
('104102f5-e0ec-4739-8fda-f05552b677c3', '82755e66-b743-46e2-b612-efd2db6bcc75', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Update
-- User permissions for editor (only read)
('104102f5-e0ec-4739-8fda-f05552b677c3', '82755e66-b743-46e2-b612-efd2db6bcd75', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'); -- Read

-- Set up reader role permissions (can only read)
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "CreatedOn", "CreatedBy") VALUES
-- Contact permissions for reader
('3a07551f-7473-44a6-a664-e6c7c834902b', 'a6657401-0d2b-4bf9-9314-26be98878822', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'), -- Read
-- User permissions for reader (only read)
('3a07551f-7473-44a6-a664-e6c7c834902b', '82755e66-b743-46e2-b612-efd2db6bcd75', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'); -- Read

-- Assign admin role to admin user
-- INSERT INTO "UserRoles" ("UserId", "RoleId", "CreatedOn", "CreatedBy") VALUES
-- ('26402b6c-ebdd-44c3-9188-659a134819cb', 'd95d2348-1d79-4b93-96d4-e48e87fcb4b5', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');

-- Insert sample contacts
INSERT INTO "Contacts" ("Id", "FirstName", "LastName", "DateOfBirth", "Mobile", "Email", "City", "PostalCode", "CreatedOn", "CreatedBy") VALUES
(uuid_generate_v4(), 'John', 'Smith', '1985-03-15', '+14155552671', 'john.smith@email.com', 'San Francisco', '94105', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Maria', 'Garcia', '1990-07-22', '+34915553492', 'maria.garcia@email.com', 'Madrid', '28001', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'David', 'Chen', '1988-11-30', '+8613911234567', 'david.chen@email.com', 'Shanghai', '200000', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Sarah', 'Johnson', '1992-04-18', '+442075556789', 'sarah.j@email.com', 'London', 'SW1A 1AA', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Raj', 'Patel', '1987-09-25', '+919876543210', 'raj.patel@email.com', 'Mumbai', '400001', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Emma', 'Wilson', '1995-01-12', '+61412345678', 'emma.w@email.com', 'Sydney', '2000', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Hans', 'Schmidt', '1983-06-08', '+491517234567', 'hans.schmidt@email.com', 'Berlin', '10115', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Sophie', 'Dubois', '1993-12-03', '+33612345678', 'sophie.d@email.com', 'Paris', '75001', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Carlos', 'Santos', '1986-08-17', '+5511987654321', 'carlos.s@email.com', 'São Paulo', '01000-000', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb'),
(uuid_generate_v4(), 'Yuki', 'Tanaka', '1991-05-29', '+819012345678', 'yuki.t@email.com', 'Tokyo', '100-0001', NOW(), '26402b6c-ebdd-44c3-9188-659a134819cb');
