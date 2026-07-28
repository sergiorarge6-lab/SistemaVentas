USE SistemaVentas;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Productos_Nombre'
      AND object_id = OBJECT_ID('dbo.Productos')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Productos_Nombre
    ON dbo.Productos(Nombre);
END
GO