CREATE TRIGGER PreventOverlap
ON BOOKING
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM BOOKING b
        JOIN inserted i
          ON b.ResourceId = i.ResourceId
         AND b.StartTime < i.EndTime
         AND b.EndTime > i.StartTime
         AND b.BookingId <> i.BookingId   -- avoid self‑match on update
    )
    BEGIN
        RAISERROR ('Booking overlaps with existing booking for this resource', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;