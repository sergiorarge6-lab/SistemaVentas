USE SistemaVentas;
GO

CREATE TABLE dbo.PedidoCabecera
(
    Id              INT IDENTITY(1,1) NOT NULL,
    ClienteId       INT NOT NULL,
    Fecha           DATETIME NOT NULL,
    Total           DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_PedidoCabecera
        PRIMARY KEY(Id),

    CONSTRAINT FK_PedidoCabecera_Clientes
        FOREIGN KEY(ClienteId)
        REFERENCES Clientes(Id)
);
GO