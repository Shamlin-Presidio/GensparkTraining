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


