
INSERT INTO ResourceType(title, Unit, requirement) 
VALUES('båd', 2, 'Husk: Check bådcertifikat');

INSERT INTO Resource(title, price, isActive, resourceTypeId) 
VALUES('Jolle', 550, 1, 1)

INSERT INTO Person(name, phone, email) 
VALUES('Peter Hansen', '56614525', 'peter.hansen@gmail.com')

INSERT INTO Booking(startTime, endTime, requirementFulfilled, isPaid, resourceId, personId) 
VALUES(current_timeStamp, current_timeStamp, 0, 1, 1, 1)

