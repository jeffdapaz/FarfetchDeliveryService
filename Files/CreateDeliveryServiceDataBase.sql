CREATE DATABASE FarfetchDeliveryService
GO

USE FarfetchDeliveryService
GO

CREATE TABLE Users
(
    Id       INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Login    VARCHAR(50)  NOT NULL,    
    Password VARCHAR(200) NOT NULL,
    Role     VARCHAR(50)  NOT NULL,
)

INSERT INTO Users (Login, Password, Role) VALUES ('User',  '????n????:b?????\f?]Z????\u0012\u0002\f?:?l?', 'user'); --123456
INSERT INTO Users (Login, Password, Role) VALUES ('Admin', '??~??:m@??@???9?;?????o\u001f?????<G!',        'admin'); --abcdef