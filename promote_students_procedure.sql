-- Instrukcja MS-SQL tworząca procedurę składowaną promująca studentów na nowy semestr.

DROP PROCEDURE PromoteStudents;
CREATE PROCEDURE PromoteStudents @Studies NVARCHAR(100), @Semester INT
AS
BEGIN
    SET XACT_ABORT ON

    BEGIN TRANSACTION

        DECLARE @IdStudies INT = (SELECT IdStudy FROM Studies WHERE Name = @Studies)
        IF @IdStudies IS NULL
            BEGIN
                ROLLBACK
                RETURN
            END

        DECLARE @IdOldEnrollment INT = (SELECT IdEnrollment FROM Enrollment WHERE Semester = @Semester AND IdStudy = @IdStudies)
        IF @IdOldEnrollment IS NULL
            BEGIN
                ROLLBACK
                RETURN
            END

        DECLARE @IdPromotedEnrollment INT = (SELECT IdEnrollment FROM Enrollment WHERE Semester = @Semester + 1 AND IdStudy = @IdStudies)
        IF @IdPromotedEnrollment IS NULL
            BEGIN
                SET @IdPromotedEnrollment = (SELECT TOP 1 IdEnrollment FROM Enrollment ORDER BY IdEnrollment DESC) + 1
                INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate)
                VALUES (@IdPromotedEnrollment, @Semester + 1, @IdStudies, GETDATE())
            END

        UPDATE Student SET IdEnrollment = @IdPromotedEnrollment WHERE IdEnrollment = @IdOldEnrollment

    COMMIT

    SELECT * FROM Enrollment WHERE IdEnrollment = @IdPromotedEnrollment
END;