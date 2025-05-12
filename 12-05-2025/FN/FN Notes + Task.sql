/*
    -------------------------------------------
         A C I D    P R O P E R T I E S
    -------------------------------------------

    Atomicity    --> All or none
    Consistency  --> Ensures that a transaction brings the database from one valid state to another, maintaining database rules and constraints
    Isolation    --> Concurrent transactions dont interefere
    Durability   --> Once a transaction is commited, the changes are permanent, even through system crashes

    Basic Transaction Commands:
    Begin
    Commit
    Rollback
    Savepoint
*/


CREATE OR REPLACE PROCEDURE transfer_funds_if_possible(OUT success BOOLEAN)
LANGUAGE plpgsql
AS $$
DECLARE
    current_balance NUMERIC;
BEGIN
    SAVEPOINT before_transfer;

    SELECT balance INTO current_balance
    FROM tbl_bank_accounts
    WHERE account_name = 'Alice';

    IF current_balance >= 4500 THEN
        UPDATE tbl_bank_accounts SET balance = balance - 4500 WHERE account_name = 'Alice';
        UPDATE tbl_bank_accounts SET balance = balance + 4500 WHERE account_name = 'Bob';
        success := TRUE;
        RAISE NOTICE 'Transfer completed.';
    ELSE
        RAISE NOTICE 'Insufficient Funds! Rolling back to savepoint.';
        ROLLBACK TO SAVEPOINT before_transfer;
        success := FALSE;
    END IF;
END;
$$;

-- Begin the transaction and call the procedure, then commit or rollback based on result
DO $$
DECLARE
    transfer_success BOOLEAN;
BEGIN
    BEGIN

        BEGIN TRANSACTION;
        -- Call the procedure and capture OUT parameter
        CALL transfer_funds_if_possible(transfer_success);

        -- Decide based on success flag
        IF transfer_success THEN
            COMMIT;
            RAISE NOTICE 'Transaction committed.';
        ELSE
            ROLLBACK;
            RAISE NOTICE 'Transaction rolled back.';
        END IF;

    EXCEPTION
        WHEN OTHERS THEN
            RAISE NOTICE 'Unexpected error: %, rolling back.', SQLERRM;
            ROLLBACK;
    END;
END;
$$;
