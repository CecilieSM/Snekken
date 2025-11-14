CREATE TABLE RESOURCETYPE ( 
	ResourceTypeId int IDENTITY(1,1) PRIMARY KEY,
	Title nvarchar(50) NOT NULL,      
    Unit int NOT NULL,      
    Requirement nvarchar(255)     
);

CREATE TABLE RESOURCE (
    ResourceId int IDENTITY(1,1) PRIMARY KEY,
    Title nvarchar(100) NOT NULL,
    Price decimal (10,2) NOT NULL,
    IsActive bit NOT NULL DEFAULT 1,
	Description nvarchar(300),
    ResourceTypeId int NOT NULL,
    CONSTRAINT FK_Resource_ResourceType
        FOREIGN KEY (ResourceTypeId) REFERENCES RESOURCETYPE(ResourceTypeId)
);

CREATE TABLE PERSON (
	PersonId int IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(100) NOT NULL,
	Email nvarchar(100) NOT NULL,
	Phone nvarchar(20)
);

CREATE TABLE BOOKING (
	BookingId int IDENTITY(1,1) PRIMARY KEY,
	StartTime datetime2 NOT NULL,
	EndTime datetime2 NOT NULL,
	RequirementFulfilled bit NOT NULL DEFAULT 0,
	IsPaid bit NOT NULL DEFAULT 0,
	ResourceId int NOT NULL,
	PersonId int NOT NULL,
	CONSTRAINT FK_Booking_Resource FOREIGN KEY (ResourceId) REFERENCES RESOURCE(ResourceId),
	CONSTRAINT FK_Booking_Person FOREIGN KEY (PersonId) REFERENCES PERSON(PersonId)
);