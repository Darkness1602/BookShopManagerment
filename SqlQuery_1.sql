USE BOOKSHOP
GO
CREATE PROC uspGetAllUsers
AS
BEGIN
    SELECT UserID, UserName, UserPhone, UserAddress, Password, Role
    FROM UsersTbl;
END;

-- Stored PROC: Thêm người dùng mới
GO
CREATE PROC uspAddUser
    @UserName NVARCHAR(50),
    @UserPhone NVARCHAR(20),
    @UserAddress NVARCHAR(100),
    @Password NVARCHAR(50),
    @Role NVARCHAR(20)
AS
BEGIN
    INSERT INTO UsersTbl (UserName, UserPhone, UserAddress, Password, Role)
    VALUES (@UserName, @UserPhone, @UserAddress, @Password, @Role);
END;

GO
CREATE PROC uspUpdateUser
    @UserID INT,
    @UserName NVARCHAR(50),
    @UserPhone NVARCHAR(20),
    @UserAddress NVARCHAR(100),
    @Password NVARCHAR(50),
    @Role NVARCHAR(20)
AS
BEGIN
    UPDATE UsersTbl
    SET UserName = @UserName,
        UserPhone = @UserPhone,
        UserAddress = @UserAddress,
        Password = @Password,
        Role = @Role
    WHERE UserID = @UserID;
END;

-- Stored PROC: Xóa người dùng
GO
CREATE PROC uspDeleteUser
    @UserID INT
AS
BEGIN
    DELETE FROM UsersTbl
    WHERE UserID = @UserID;
END;

-- Stored PROC: Kiểm tra đăng nhập
GO
CREATE PROC uspCheckLogin
    @UserName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT UserID, UserName, UserPhone, UserAddress, Role
    FROM UsersTbl
    WHERE UserName = @UserName AND Password = @Password;
END;

-- Stored PROC: Lấy danh sách tất cả sách

GO
CREATE PROC uspGetBookDTOs
AS
BEGIN
    SELECT BookId, Title, Author, CategoryId, Quantity, Price
    FROM BooksTbl;
END;

-- Stored PROC: Thêm sách mới
GO
CREATE PROC uspAddBook
    @Title NVARCHAR(100),
    @Author NVARCHAR(50),
    @CategoryId NVARCHAR(50),
    @Quantity INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    INSERT INTO BooksTbl (Title, Author, CategoryId, Quantity, Price)
    VALUES (@Title, @Author, @CategoryId, @Quantity, @Price);
END;


-- Stored PROC: Cập nhật thông tin sách

GO
CREATE PROC uspUpdateBook
    @BookId INT,
    @Title NVARCHAR(100),
    @Author NVARCHAR(50),
    @CategoryId NVARCHAR(50),
    @Quantity INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    UPDATE BooksTbl
    SET Title = @Title,
        Author = @Author,
        CategoryId = @CategoryId,
        Quantity = @Quantity,
        Price = @Price
    WHERE BookId = @BookId;
END;

-- Stored PROC: Xóa sách
GO
CREATE PROC uspDeleteBook
    @BookId INT
AS
BEGIN
    DELETE FROM BooksTbl
    WHERE BookId = @BookId;
END;

-- Stored PROC: Cập nhật số lượng sách
GO
CREATE PROC uspUpdateBookQuantity
    @BookId INT,
    @NewQuantity INT
AS
BEGIN
    UPDATE BooksTbl
    SET Quantity = @NewQuantity
    WHERE BookId = @BookId;
END;

-- Stored PROC: Lấy tổng số sách
GO
CREATE PROC uspGetTotalBooks
AS
BEGIN
    SELECT SUM(Quantity) FROM BooksTbl;
END;

-- Stored PROC: Thêm hóa đơn mới

GO
CREATE PROCEDURE uspAddBill
    @BillId INT OUTPUT,
    @UserID NVARCHAR(50),
    @ClientName NVARCHAR(100),
    @Amount DECIMAL(18,2)
    
AS
BEGIN
    iNSERT INTO BillsTbl (UserID, ClientName, Amount)
    VALUES (@UserID, @ClientName, @Amount);

    SET @BillId = SCOPE_IDENTITY(); -- Lấy BillId vừa insert
END

GO
CREATE PROC uspGetCustomerByPhone
    @Phone NVARCHAR(20),
    @Name NVARCHAR(100)
AS
BEGIN
    -- Kiểm tra xem khách hàng đã tồn tại chưa
    IF EXISTS (SELECT 1 FROM CustomersTbl WHERE Phone = @Phone)
    BEGIN
        -- Nếu khách hàng tồn tại, lấy thông tin khách hàng
        SELECT CustomerId, Name, Phone, TotalPoints, UsedPoints
        FROM CustomersTbl
        WHERE Phone = @Phone;
    END
    ELSE
    BEGIN
        -- Nếu khách hàng không tồn tại, thêm khách hàng mới với tên từ tham số @Name
        INSERT INTO CustomersTbl (Phone, Name, TotalPoints, UsedPoints)
        VALUES (@Phone, @Name, 0, 0);

        -- Trả về thông tin khách hàng vừa tạo mới
        SELECT CustomerId, @Name AS Name, @Phone AS Phone, 0 AS TotalPoints, 0 AS UsedPoints
        FROM CustomersTbl
        WHERE Phone = @Phone;
    END
END;
GO
CREATE PROC uspAddCustomer
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @TotalPoints INT,
    @UsedPoints INT
AS
BEGIN
    INSERT INTO CustomersTbl (Name, Phone, TotalPoints, UsedPoints)
    VALUES (@Name, @Phone, @TotalPoints, @UsedPoints);

    SELECT SCOPE_IDENTITY() AS CustomerId;
END;

GO
CREATE PROC uspUpdateCustomerPoints
    @Phone NVARCHAR(20),              
    @AmountSpent DECIMAL(18,2),       
    @PointsToUse INT                  
AS
BEGIN
    DECLARE @TotalPoints INT;
    DECLARE @NewTotalPoints INT;
    DECLARE @PointsEarned INT ;

    -- Kiểm tra xem khách hàng có tồn tại không
    SELECT @TotalPoints = TotalPoints 
    FROM CustomersTbl 
    WHERE Phone = @Phone;

    IF @TotalPoints IS NULL
    BEGIN
        RAISERROR(N'Customer not found', 16, 1);
        RETURN;
    END

    -- Kiểm tra số điểm sử dụng có hợp lệ không
    IF @PointsToUse > @TotalPoints
    BEGIN
        RAISERROR(N'Insufficient points', 16, 1);
        RETURN;
    END

    -- Tính lại tổng điểm
    SET @NewTotalPoints = @TotalPoints - @PointsToUse;

    -- Tính điểm tích lũy mới
    SET @PointsEarned = FLOOR(@AmountSpent / 1000);

    -- Cập nhật điểm
    UPDATE CustomersTbl
    SET 
        TotalPoints = @NewTotalPoints + @PointsEarned,
        UsedPoints = UsedPoints + @PointsToUse
    WHERE Phone = @Phone;
END;

GO
CREATE PROC uspAddPoints
    @phone NVARCHAR(20),
    @pointsToAdd INT
AS
BEGIN
    UPDATE CustomersTbl
    SET TotalPoints = TotalPoints + @pointsToAdd
    WHERE Phone = @phone
END

GO
CREATE PROC uspUpdatePointsWithUsedPoints
    @phone NVARCHAR(20),
    @pointsEarned DECIMAL(10, 2) -- Giữ giá trị thập phân ở mức độ chính xác
AS
BEGIN
    -- Làm tròn điểm xuống (có thể thay thế bằng ROUND hoặc CEILING tùy theo yêu cầu)
    DECLARE @roundedPoints INT
    SET @roundedPoints = FLOOR(@pointsEarned) -- Làm tròn xuống

    -- Cập nhật điểm cho khách hàng
    UPDATE CustomersTbl
    SET TotalPoints = @roundedPoints
    WHERE Phone = @phone
END



GO
CREATE PROCEDURE uspUpdateCustomer
    @CustomerID INT,
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @TotalPoints INT,
    @UsedPoints INT
AS
BEGIN
    UPDATE CustomersTbl
    SET 
        Name = @Name,
        Phone = @Phone,
        TotalPoints = @TotalPoints,
        UsedPoints = @UsedPoints
    WHERE CustomerID = @CustomerID
END

GO
CREATE PROCEDURE uspDeleteCustomer
    @CustomerID INT
AS
BEGIN
    DELETE FROM CustomersTbl WHERE CustomerID = @CustomerID
END

GO
CREATE PROCEDURE uspGetTotalCustomers
AS
BEGIN
    SELECT COUNT(*) AS TotalCustomers FROM CustomersTbl
END


Go
drop proc uspUpdateCustomer
go
drop proc uspDeleteCustomer
go
drop proc uspGetTotalCustomers