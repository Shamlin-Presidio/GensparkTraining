/*
Cursors 
Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.

Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.
 
Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.
 
Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.
--------------------------------------------------------------------------
 
Transactions 
Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.
 
Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.
 
Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.
 
Perform a transaction that transfers inventory from one store to another (delete + insert) safely.
 
Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.
----------------------------------------------------------------------------
 
Triggers
Create a trigger to prevent inserting payments of zero or negative amount.
 
Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.
 
Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.
*/



-------------------------- CURSORS----------------------------

-- Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.

CREATE TABLE customer_rental_summary (customer_id INT, name TEXT, rental_count INT);

DO $$
DECLARE
    rec RECORD;
    cur CURSOR FOR
        SELECT c.customer_id, c.first_name || ' ' || c.last_name AS name, COUNT(r.rental_id) AS rental_count
        FROM customer c
        LEFT JOIN rental r ON c.customer_id = r.customer_id
        GROUP BY c.customer_id, name;
BEGIN
    OPEN cur;
    LOOP
        FETCH cur INTO rec;
        EXIT WHEN NOT FOUND;
        INSERT INTO customer_rental_summary VALUES (rec.customer_id, rec.name, rec.rental_count);
    END LOOP;
    CLOSE cur;
END;
$$;

SELECT * from customer_rental_summary;




-- Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.

DO $$
DECLARE
    rec RECORD;
    cur CURSOR FOR
        SELECT f.title, COUNT(*) AS rental_count
        FROM film f
        JOIN inventory i ON f.film_id = i.film_id
        JOIN rental r ON i.inventory_id = r.inventory_id
        JOIN film_category fc ON f.film_id = fc.film_id
        JOIN category c ON fc.category_id = c.category_id
        WHERE c.name = 'Comedy'
        GROUP BY f.title
        HAVING COUNT(*) > 10;
BEGIN
    OPEN cur;
    LOOP
        FETCH cur INTO rec;
        EXIT WHEN NOT FOUND;
        RAISE NOTICE 'Comedy Film: % (rented % times)', rec.title, rec.rental_count;
    END LOOP;
    CLOSE cur;
END;
$$;



-- Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.
CREATE TABLE store_film_report (store_id INT, film_count INT);

DO $$
DECLARE
    rec RECORD;
    cur CURSOR FOR
        SELECT s.store_id, COUNT(DISTINCT i.film_id) AS film_count
        FROM store s
        JOIN inventory i ON s.store_id = i.store_id
        GROUP BY s.store_id;
BEGIN
    OPEN cur;
    LOOP
        FETCH cur INTO rec;
        EXIT WHEN NOT FOUND;
        INSERT INTO store_film_report VALUES (rec.store_id, rec.film_count);
    END LOOP;
    CLOSE cur;
END;
$$;

SELECT * FROM store_film_report;



-- Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.

CREATE TABLE inactive_customers (customer_id INT, name TEXT);

DO $$
DECLARE
    rec RECORD;
    cur CURSOR FOR
        SELECT c.customer_id, c.first_name || ' ' || c.last_name AS name
        FROM customer c
        LEFT JOIN rental r ON c.customer_id = r.customer_id AND r.rental_date > CURRENT_DATE - INTERVAL '6 months'
        GROUP BY c.customer_id, name;
BEGIN
    OPEN cur;
    LOOP
        FETCH cur INTO rec;
        EXIT WHEN NOT FOUND;
        INSERT INTO inactive_customers VALUES (rec.customer_id, rec.name);
    END LOOP;
    CLOSE cur;
END;
$$;

SELECT * FROM inactive_customers;



----------------------------- TRANSACTIONS ----------------------------------


-- Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.

BEGIN

    INSERT INTO customer (store_id, first_name, last_name, email, address_id, active, create_date)
    VALUES (1, 'Shaml', 'lin', 'Sham@lin.com', 1, true, CURRENT_DATE);

    INSERT INTO rental (rental_date, inventory_id, customer_id, staff_id)
    VALUES (CURRENT_TIMESTAMP, 1, 600, 1);

    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (600, 1, currval('rental_rental_id_seq'), 5.99, CURRENT_TIMESTAMP);

COMMIT;



-- Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.
BEGIN;
    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (1, 1, 99999, 5.00, CURRENT_TIMESTAMP);

    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (1, 1, 99999, 10.00, CURRENT_TIMESTAMP);

ROLLBACK;

 
-- Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.
BEGIN;
    SAVEPOINT step1;
    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (1, 1, 1, 10.00, CURRENT_TIMESTAMP);

    SAVEPOINT step2;

    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (1, 1, 2, -5.00, CURRENT_TIMESTAMP); -- let's surpirse them with a negative rental id   :)

    ROLLBACK TO SAVEPOINT step2;


    INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
    VALUES (1, 1, 2, 5.00, CURRENT_TIMESTAMP);

COMMIT;
 
-- Perform a transaction that transfers inventory from one store to another (delete + insert) safely.
BEGIN;
    UPDATE inventory
    SET store_id = 2
    WHERE inventory_id = 1 AND store_id = 1;
COMMIT;
 
-- Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.
BEGIN;
    DELETE FROM payment WHERE customer_id = 600;
    DELETE FROM rental WHERE customer_id = 600;
    DELETE FROM customer WHERE customer_id = 600;
COMMIT;



----------------------------- TRIGGERS ----------------------------------

-- Create a trigger to prevent inserting payments of zero or negative amount.
CREATE OR REPLACE FUNCTION prevent_zero_negative_payment()
RETURNS TRIGGER AS $$
BEGIN
    IF new.amount <= 0 THEN
        RAISE EXCEPTION 'Payment amount must be greater than zero';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_prevent_bad_payment
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_zero_negative_payment();




ROLLBACK;
drop trigger trg_prevent_bad_payment on payment;

 
-- Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.


CREATE OR REPLACE FUNCTION update_last_modified_film()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.title IS DISTINCT FROM OLD.title OR NEW.rental_rate IS DISTINCT FROM OLD.rental_rate THEN
        NEW.last_update_custom := CURRENT_TIMESTAMP;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_film_update
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION update_last_modified_film();




DROP TRIGGER trg_film_update on film;

 
-- Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.

CREATE TABLE rental_log (
    film_id INT,
    log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION log_high_rentals()
RETURNS TRIGGER AS $$
DECLARE
    cnt INT;
BEGIN
    SELECT COUNT(*) INTO cnt
    FROM rental r
    JOIN inventory i ON r.inventory_id = i.inventory_id
    WHERE i.film_id = (SELECT film_id FROM inventory WHERE inventory_id = NEW.inventory_id)
      AND r.rental_date >= CURRENT_DATE - INTERVAL '7 days';

    IF cnt > 3 THEN
        INSERT INTO rental_log(film_id) VALUES (
            (SELECT film_id FROM inventory WHERE inventory_id = NEW.inventory_id)
        );
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_rental_volume
AFTER INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION log_high_rentals();


-- testing this :

INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id)
VALUES 
  (NOW(), 1, 1, NOW() + INTERVAL '1 day', 1),
  (NOW() + INTERVAL '1 minute', 1, 1, NOW() + INTERVAL '1 day 1 minute', 1),
  (NOW() + INTERVAL '2 minutes', 1, 1, NOW() + INTERVAL '1 day 2 minutes', 1),
  (NOW() + INTERVAL '3 minutes', 1, 1, NOW() + INTERVAL '1 day 3 minutes', 1);

SELECT * FROM rental_log;


