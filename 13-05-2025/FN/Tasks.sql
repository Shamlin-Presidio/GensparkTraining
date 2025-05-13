
-- 1. Try two concurrent updates to same row → see lock in action.
CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    name TEXT,
    price INT
);

INSERT INTO products (name, price) VALUES ('Laptop', 1000);

-- Session A
BEGIN;
UPDATE products SET price = 1100 WHERE id = 1;

-- Session B
BEGIN;
UPDATE products SET price = 1200 WHERE id = 1;
SELECT * FROM products;

-- Session A

-- committing after some time
COMMIT;

-- 2. Write a query using SELECT...FOR UPDATE and check how it locks row.

-- Session A

BEGIN;
-- Lock the row using FOR UPDATE
SELECT * FROM products WHERE id = 1 FOR UPDATE;
-- Row with id = 1 is now locked



-- Session B

BEGIN;
-- Try to update the same row
UPDATE products SET price = 1300 WHERE id = 1;
-- This is blocked until Transaction A commits or rollbacks

SELECT * FROM products;


-- 3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.

-- Session A
INSERT INTO products (name, price) VALUES ('Phone', 500);  -- id = 2


BEGIN;
UPDATE products SET price = 1100 WHERE id = 1;
-- Holds lock on row id = 1


UPDATE products SET price = 500 WHERE id = 2;
-- Waits for B's lock on row id = 2


-- Session B

BEGIN;
UPDATE products SET price = 600 WHERE id = 2;
-- Holds lock on row id = 2

UPDATE products SET price = 700 WHERE id = 1;
-- Now both A and B wait on each other → PostgreSQL detects deadlock



-- 4. Use pg_locks query to monitor active locks.
SELECT *
FROM pg_locks;

-- 5. Explore about Lock Modes.


-- Learning.....


------------------------------------------------TRANSACTIONS------------------------------------------------

create table audit_log
(audit_id serial primary key,
table_name text,
field_name text,
old_value text,
new_value text,
updated_date Timestamp default current_Timestamp)

create or replace function Update_Audit_log()
returns trigger 
as $$
begin
	Insert into audit_log(table_name,field_name,old_value,new_value,updated_date) 
	values('customer','email',OLD.email,NEW.email,current_Timestamp);
	return new;
end;
$$ language plpgsql


create trigger trg_log_customer_email_Change
before update
on customer
for each row
execute function Update_Audit_log();

drop trigger trg_log_customer_email_Change on customer;
drop table audit_log;
select * from customer order by customer_id

select * from audit_log
update customer set email = 'mary.smith@sakilacustomer.org' where customer_id = 1



--------------------------------------------------------------------------------
--                  N OT E    T H I S 
--------------------------------------------------------------------------------

-- in PostgreSQL, trigger functions cannot accept parameters