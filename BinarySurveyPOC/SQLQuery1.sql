CREATE PROCEDURE [dbo].[usp_surveys_get_by_lat_lng]
@lat decimal(9,6),
@lng decimal(9,6)
AS
BEGIN
SET NOCOUNT ON
DECLARE @g geography;   
SET @g = geography::Point(@lat, @lng, 4326)  

SELECT * FROM Surveys where @g.STIntersects([Location].STAsText()) <> 0 order by AddDate desc
END