CREATE TABLE dbo.Customers(
	Id uniqueidentifier NOT NULL,
	FirstName nvarchar(100) NOT NULL,
	LastName nvarchar(100) NOT NULL,
	Email nvarchar(100) NOT NULL,
	CONSTRAINT PK_Id PRIMARY KEY (Id),
	CONSTRAINT UK_Email UNIQUE (Email)
);