----El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)
select 
	cast(sum(Total) as decimal(12,2)) as Total ,
	count(*) as Cantidad 
from Venta
where Fecha >= DATEADD(D,-30,GETDATE()) 



----El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto)
select top  1
	cast(Total as decimal(12,2)) as Total ,
	Fecha as FechaHora 
from Venta
where Fecha >= DATEADD(D,-30,GETDATE()) 
order by Total desc

----Indicar cuál es el producto con mayor monto total de ventas 
select top 1 
	cast(sum(Total) as decimal(12,2)) as Total ,
	p.Nombre as Producto 
from Venta v
inner join VentaDetalle vd on v.ID_Venta = vd.ID_Venta
inner join Producto p on vd.ID_Producto = p.ID_Producto
where Fecha >= DATEADD(D,-30,GETDATE()) 
group by p.Nombre
order by Total desc

  -- Indicar el local con mayor monto de ventas 

  select  top 1 
  cast(sum(Total) as decimal(12,2)) as Total , 
  l.Nombre 
  from Venta v
  inner join Local l on v.ID_Local = l.ID_Local
  where Fecha >= DATEADD(D,-30,GETDATE()) 
group by l.Nombre
order by Total desc

-- ¿Cuál es la marca con mayor margen de ganancias?

select
top  1 
m.Nombre,
(vd.TotalLinea - (p.Costo_Unitario * vd.Cantidad)  )/ (p.Costo_Unitario * vd.Cantidad)  as Margen
from Venta v
inner join VentaDetalle vd on v.ID_Venta = vd.ID_Venta
inner join Producto p on vd.ID_Producto = p.ID_Producto
inner join Marca m on p.ID_Marca = m.ID_Marca
where Fecha >= DATEADD(D,-30,GETDATE()) 
order by Margen desc


--¿Cómo obtendrías cuál es el producto que más se vende en cada local?

SELECT Local, Nombre, TotalVentas
FROM (
    SELECT local.Nombre as Local, Producto.Nombre as Nombre, COUNT(Cantidad) AS TotalVentas,
           ROW_NUMBER() OVER (PARTITION BY Local.Nombre ORDER BY COUNT(Cantidad) DESC) AS Rank
    FROM VentaDetalle
	inner join venta  on Venta.ID_Venta =  VentaDetalle.ID_Venta
	inner join Producto on VentaDetalle.ID_Producto = Producto.ID_Producto
	inner join Local on venta.ID_Local = Local.ID_Local
    GROUP BY local.Nombre , Producto.Nombre
) AS Subquery
WHERE Rank = 1 order by TotalVentas desc
