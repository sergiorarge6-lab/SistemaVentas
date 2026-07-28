USE SistemaVentas;
GO

CREATE TABLE dbo.PedidoDetalle
(
    Id                  INT IDENTITY(1,1) NOT NULL,
    PedidoCabeceraId    INT NOT NULL,
    ProductoId          INT NOT NULL,
    Cantidad            INT NOT NULL,
    PrecioUnitario      DECIMAL(18,2) NOT NULL,
    Subtotal            DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_PedidoDetalle
        PRIMARY KEY(Id),

    CONSTRAINT FK_PedidoDetalle_Pedido
        FOREIGN KEY(PedidoCabeceraId)
        REFERENCES PedidoCabecera(Id),

    CONSTRAINT FK_PedidoDetalle_Producto
        FOREIGN KEY(ProductoId)
        REFERENCES Productos(Id)
);
GO