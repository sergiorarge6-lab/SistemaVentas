USE SistemaVentas;
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        Id                  INT IDENTITY(1,1) NOT NULL,
        Codigo              VARCHAR(20) NOT NULL,
        Nombre              VARCHAR(100) NOT NULL,
        Descripcion         VARCHAR(250) NULL,
        Precio              DECIMAL(18,2) NOT NULL,
        Stock               INT NOT NULL,
        Activo              BIT NOT NULL
            CONSTRAINT DF_Productos_Activo DEFAULT(1),
        FechaCreacion       DATETIME2 NOT NULL
            CONSTRAINT DF_Productos_FechaCreacion DEFAULT(SYSDATETIME()),
        FechaModificacion   DATETIME2 NULL,

        CONSTRAINT PK_Productos
            PRIMARY KEY CLUSTERED (Id),

        CONSTRAINT UQ_Productos_Codigo
            UNIQUE (Codigo)
    );
END
GO