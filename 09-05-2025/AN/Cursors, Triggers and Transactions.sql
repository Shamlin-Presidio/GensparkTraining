------------------------------------------------------------------------------------------------------------------------------------
--                                      C U R S O R S   
------------------------------------------------------------------------------------------------------------------------------------

-- Write a cursor that loops through all films and prints titles longer than 120 minutes.
DO $$
DECLARE
    film_rec RECORD;
BEGIN
    FOR film_rec IN
        SELECT title, length FROM film
    LOOP
        IF film_rec.length > 120 THEN
            RAISE NOTICE 'Long Film: % (% minutes)', film_rec.title, film_rec.length;
        END IF;
    END LOOP;
END;
$$;


-- Create a cursor that iterates through all customers and counts how many rentals each made.

DO $$
DECLARE
    cust_rec RECORD;
    rental_count INT;
BEGIN
    FOR cust_rec IN SELECT customer_id, first_name, last_name FROM customer
    LOOP
        SELECT COUNT(*) INTO rental_count FROM rental
        WHERE customer_id = cust_rec.customer_id;
        RAISE NOTICE 'Customer: % %, Rentals: %', cust_rec.first_name, cust_rec.last_name, rental_count;
    END LOOP;
END;
$$;


-- Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.

DO $$
DECLARE
    film_rec RECORD;
    rental_count INT;
BEGIN
    FOR film_rec IN SELECT film_id, title FROM film
    LOOP
        SELECT COUNT(*) INTO rental_count FROM inventory i
        JOIN rental r ON i.inventory_id = r.inventory_id
        WHERE i.film_id = film_rec.film_id;

        IF rental_count < 5 THEN
            UPDATE film SET rental_rate = rental_rate + 1
            WHERE film_id = film_rec.film_id;
            RAISE NOTICE 'Increased rate for % (ID %)', film_rec.title, film_rec.film_id;
        END IF;
    END LOOP;
END;
$$;


-- Create a function using a cursor that collects titles of all films from a particular category.
CREATE OR REPLACE FUNCTION get_films_by_category(param_category_name VARCHAR)
RETURNS TABLE(film_title VARCHAR) AS $$
DECLARE
    param_film_title VARCHAR;
    film_cursor CURSOR FOR
        SELECT f.title
        FROM film f
        JOIN film_category fc ON f.film_id = fc.film_id
        JOIN category c ON fc.category_id = c.category_id
        WHERE c.name = param_category_name;
BEGIN
    OPEN film_cursor;
    LOOP
        FETCH film_cursor INTO param_film_title;
        EXIT WHEN NOT FOUND;
        film_title := param_film_title;  
        RETURN NEXT; 
    END LOOP;
    CLOSE film_cursor;
    RETURN; 
END;
$$ LANGUAGE plpgsql;

SELECT get_films_by_category('Action')

-- Loop through all stores and count how many distinct films are available in each store using a cursor.

DO $$
DECLARE
    store_rec RECORD;
    film_count INT;
BEGIN
    FOR store_rec IN SELECT store_id FROM store
    LOOP
        SELECT COUNT(DISTINCT film_id) INTO film_count FROM inventory
        WHERE store_id = store_rec.store_id;

        RAISE NOTICE 'Store ID % has % distinct films.', store_rec.store_id, film_count;
    END LOOP;
END;
$$;


------------------------------------------------------------------------------------------------------------------------------------
--                                      T RI G G E R S   
------------------------------------------------------------------------------------------------------------------------------------

-- Write a trigger that logs whenever a new customer is inserted.

CREATE TABLE customer_log (
    customer_id INT,
    full_name TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE FUNCTION log_new_customer()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO customer_log (customer_id, full_name)
    VALUES (NEW.customer_id, CONCAT(NEW.first_name, ' ', NEW.last_name));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_new_customer
AFTER INSERT ON customer
FOR EACH ROW
EXECUTE FUNCTION log_new_customer();


-- Create a trigger that prevents inserting a payment of amount 0.
CREATE FUNCTION prevent_zero_payment()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.amount = 0 THEN
        RAISE EXCEPTION 'Zero amount payments are not allowed.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- creating the triggwr
CREATE TRIGGER trg_prevent_zero_payment
BEFORE INSERT ON payment
FOR EACH ROW EXECUTE FUNCTION prevent_zero_payment();

--Set up a trigger to automatically set last_update on the film table before update.

CREATE OR REPLACE FUNCTION update_last_modified()
RETURNS TRIGGER AS $$
BEGIN
    NEW.last_update := CURRENT_TIMESTAMP;
    RETURN NEW;
END;

$$ LANGUAGE plpgsql;
CREATE TRIGGER trg_update_film_last_modified
BEFORE UPDATE ON film
FOR EACH ROW EXECUTE FUNCTION update_last_modified();


-- Create a trigger to log changes in the inventory table (insert/delete).

CREATE TABLE inventory_log (
    action TEXT,
    inventory_id INT,
    film_id INT,
    store_id INT,
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE FUNCTION log_inventory_changes()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO inventory_log(action, inventory_id, film_id, store_id)
        VALUES ('INSERT', NEW.inventory_id, NEW.film_id, NEW.store_id);
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO inventory_log(action, inventory_id, film_id, store_id)
        VALUES ('DELETE', OLD.inventory_id, OLD.film_id, OLD.store_id);
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_inventory_insert
AFTER INSERT ON inventory
FOR EACH ROW
EXECUTE FUNCTION log_inventory_changes();

CREATE TRIGGER trg_inventory_delete
AFTER DELETE ON inventory
FOR EACH ROW
EXECUTE FUNCTION log_inventory_changes();


-- Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.
CREATE FUNCTION prevent_rental_if_debt()
RETURNS TRIGGER AS $$
DECLARE
    total_due NUMERIC;
BEGIN
    SELECT SUM(amount) INTO total_due
    FROM payment
    WHERE customer_id = NEW.customer_id;

    IF total_due < 50 THEN RAISE EXCEPTION 'Customer owes more than $50. Cannot proceed with rental.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_prevent_debt_rental
BEFORE INSERT ON rental FOR EACH ROW
EXECUTE FUNCTION prevent_rental_if_debt();




------------------------------------------------------------------------------------------------------------------------------------
--                                      T R A N S AC T I O N S   
------------------------------------------------------------------------------------------------------------------------------------




-- Write a transaction that inserts a customer and an initial rental in one atomic operation.

BEGIN;

-- CUstomer
INSERT INTO customer (first_name, last_name, email, active)
VALUES ('John', 'Doe', 'random.mail@mail.com', TRUE);
-- Iitial Rent, like default
INSERT INTO rental (customer_id, inventory_id, rental_date)
VALUES (
    (SELECT customer_id FROM customer WHERE email = 'random.mail@mail.com' LIMIT 1), 
    1, CURRENT_TIMESTAMP
);

COMMIT;


-- Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.

BEGIN;

UPDATE film
SET rental_rate = rental_rate + 1
WHERE film_id = 1;

INSERT INTO inventory (film_id, store_id)
VALUES (1, 1);

-- Simulate an error (for example, an insert with a bad value)
INSERT INTO inventory (film_id, store_id)
VALUES (NULL, 1); -- NUll calue casuses an error, this makes a rollback

COMMIT;



-- Create a transaction that transfers an inventory item from one store to another.
BEGIN;

UPDATE inventory
SET store_id = 2
WHERE store_id = 1 AND film_id = 1;


COMMIT;

-- Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.
BEGIN;

UPDATE payment
SET amount = amount + 10
WHERE customer_id = 1;

-- savepoint 1
SAVEPOINT savepoint1;

-- another change
UPDATE payment
SET amount = amount + 15
WHERE customer_id = 2;

-- Rollback to the savepoint, undoing the second update
ROLLBACK TO SAVEPOINT savepoint1;

COMMIT;


-- Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

BEGIN;

DELETE FROM rental
WHERE customer_id = 1;

DELETE FROM payment
WHERE customer_id = 1;

DELETE FROM customer
WHERE customer_id = 1;

COMMIT;


-- Procedure: get_overdue_rentals() that selects relevant columns.
 
CREATE FUNCTION get_overdue_rentals()
RETURNS TABLE (customer_id smallint, rental_date timestamp without time zone) AS $$
BEGIN
	RETURN QUERY
	SELECT r.customer_id, r.rental_date FROM rental as r
	WHERE r.return_date IS NULL AND r.rental_date < CURRENT_DATE - INTERVAL '7 days';
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_overdue_rentals()