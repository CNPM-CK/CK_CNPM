SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF OBJECT_ID(N'dbo.AI_RenewalPrediction', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AI_RenewalPrediction
    (
        predictionId BIGINT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_AI_RenewalPrediction PRIMARY KEY,
        snapshotId VARCHAR(100) NOT NULL,
        maKH VARCHAR(15) NOT NULL,
        maHD VARCHAR(15) NOT NULL,
        snapshotDate DATE NOT NULL,
        modelVersion VARCHAR(120) NOT NULL,
        renewalProbability FLOAT NOT NULL,
        predictedRenewal BIT NOT NULL,
        decisionThreshold FLOAT NOT NULL,
        isColdStart BIT NOT NULL,
        evidenceLevel VARCHAR(20) NOT NULL,
        snapshotJson NVARCHAR(MAX) NOT NULL,
        explanation NVARCHAR(MAX) NOT NULL,
        predictedAtUtc DATETIME2(0) NOT NULL
            CONSTRAINT DF_AI_RenewalPrediction_PredictedAtUtc DEFAULT SYSUTCDATETIME(),
        CONSTRAINT UQ_AI_RenewalPrediction_Snapshot_Model
            UNIQUE (snapshotId, modelVersion),
        CONSTRAINT CK_AI_RenewalPrediction_Probability
            CHECK (renewalProbability >= 0 AND renewalProbability <= 1),
        CONSTRAINT CK_AI_RenewalPrediction_Threshold
            CHECK (decisionThreshold >= 0 AND decisionThreshold <= 1),
        CONSTRAINT CK_AI_RenewalPrediction_Evidence
            CHECK (evidenceLevel IN ('low', 'medium', 'high'))
    );

    CREATE INDEX IX_AI_RenewalPrediction_Customer_Date
        ON dbo.AI_RenewalPrediction (maKH, snapshotDate DESC);
    CREATE INDEX IX_AI_RenewalPrediction_Contract_Date
        ON dbo.AI_RenewalPrediction (maHD, snapshotDate DESC);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_AI_Renewal_SavePrediction
    @snapshotId VARCHAR(100),
    @maKH VARCHAR(15),
    @maHD VARCHAR(15),
    @snapshotDate DATE,
    @modelVersion VARCHAR(120),
    @renewalProbability FLOAT,
    @predictedRenewal BIT,
    @decisionThreshold FLOAT,
    @isColdStart BIT,
    @evidenceLevel VARCHAR(20),
    @snapshotJson NVARCHAR(MAX),
    @explanation NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    UPDATE dbo.AI_RenewalPrediction WITH (UPDLOCK, SERIALIZABLE)
    SET maKH = @maKH,
        maHD = @maHD,
        snapshotDate = @snapshotDate,
        renewalProbability = @renewalProbability,
        predictedRenewal = @predictedRenewal,
        decisionThreshold = @decisionThreshold,
        isColdStart = @isColdStart,
        evidenceLevel = @evidenceLevel,
        snapshotJson = @snapshotJson,
        explanation = @explanation,
        predictedAtUtc = SYSUTCDATETIME()
    WHERE snapshotId = @snapshotId
      AND modelVersion = @modelVersion;

    IF @@ROWCOUNT = 0
    BEGIN
        INSERT dbo.AI_RenewalPrediction
        (
            snapshotId, maKH, maHD, snapshotDate, modelVersion,
            renewalProbability, predictedRenewal, decisionThreshold,
            isColdStart, evidenceLevel, snapshotJson, explanation
        )
        VALUES
        (
            @snapshotId, @maKH, @maHD, @snapshotDate, @modelVersion,
            @renewalProbability, @predictedRenewal, @decisionThreshold,
            @isColdStart, @evidenceLevel, @snapshotJson, @explanation
        );
    END;

    COMMIT TRANSACTION;
END;
GO
