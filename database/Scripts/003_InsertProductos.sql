USE SistemaVentas;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Productos)
BEGIN
    INSERT INTO dbo.Productos
    (
        Codigo,
        Nombre,
        Descripcion,
        Precio,
        Stock,
        Activo,
        FechaCreacion
    )
    VALUES
    ('P0001','Notebook Dell','Notebook Dell Inspiron',1500000,10,1,GETDATE()),
    ('P0002','Mouse Logitech','Mouse inalámbrico',35000,50,1,GETDATE()),
    ('P0003','Teclado Redragon','Teclado mecánico',85000,20,1,GETDATE());
END
GO