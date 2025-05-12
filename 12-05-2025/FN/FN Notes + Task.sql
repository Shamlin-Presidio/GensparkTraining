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


DO $$
BEGIN
    BEGIN
        INSERT INTO customers (name) VALUES ('Alice');

        -- Check condition
        IF EXISTS (SELECT 1 FROM customers WHERE name = 'Alice') THEN
            SAVEPOINT point_a;
        ELSE
            RAISE NOTICE 'Insert failed';
            ROLLBACK;
            RETURN;
        END IF;

        -- another operation
        INSERT INTO orders (customer_id) VALUES (1);

        IF EXISTS (SELECT 1 FROM orders WHERE customer_id = 1) THEN
            COMMIT;
        ELSE
            RAISE NOTICE 'Order insert failed, rolling back to savepoint';
            ROLLBACK TO SAVEPOINT point_a;
            COMMIT;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            RAISE NOTICE 'Unexpected error: %', SQLERRM;
            ROLLBACK;
    END;
END $$;

