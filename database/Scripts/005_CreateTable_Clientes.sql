CREATE TABLE dbo.Clientes
(
    Id              INT IDENTITY(1,1) NOT NULL,
    Nombre          VARCHAR(100) NOT NULL,
    Email           VARCHAR(150) NULL,
    Activo          BIT NOT NULL
        CONSTRAINT DF_Clientes_Activo DEFAULT(1),

    CONSTRAINT PK_Clientes
        PRIMARY KEY(Id)
);
GO