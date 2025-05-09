-- List all films with their length and rental rate, sorted by length descending.
-- Columns: title, length, rental_rate
SELECT title, length, rental_rate
FROM film
ORDER BY length DESC;



-- Find the top 5 customers who have rented the most films.
-- Hint: Use the rental and customer tables.
SELECT * FROM customer LIMIT 2;
SELECT * FROM rental LIMIT 2;

SELECT 
    c.customer_id,
    c.first_name,
    c.last_name,
    COUNT(r.rental_id) AS Total_rentals
FROM customer c
JOIN rental r ON c.customer_id = r.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name
ORDER BY total_rentals DESC LIMIT 5;

-- Display all films that have never been rented.
-- Hint: Use LEFT JOIN between film and inventory → rental.

SELECT * FROM film;
SELECT * FROM inventory;
SELECT * FROM rental;



SELECT f.film_id, f.title
FROM film f
LEFT JOIN inventory i ON f.film_id = i.film_id
LEFT JOIN rental r ON i.inventory_id = r.inventory_id
WHERE r.rental_id IS NULL;



-----------------------JOIN Queries-----------------------

-- List all actors who appeared in the film ‘Academy Dinosaur’.
-- Tables: film, film_actor, actor

SELECT a.actor_id, a.first_name, a.last_name
FROM actor a
JOIN film_actor fa ON a.actor_id = fa.actor_id
JOIN film f ON fa.film_id = f.film_id
WHERE f.title = 'Academy Dinosaur';


-- List each customer along with the total number of rentals they made and the total amount paid.
-- Tables: customer, rental, payment

SELECT * FROM customer;
SELECT * FROM rental;
SELECT * FROM payment;

SELECT 
    c.customer_id,
    c.first_name,
    c.last_name,
    COUNT(r.rental_id) AS total_rentals,
    SUM(p.amount) AS total_paid
FROM 
    customer c
LEFT JOIN rental r ON c.customer_id = r.customer_id
LEFT JOIN payment p ON c.customer_id = p.customer_id
GROUP BY 
    c.customer_id, c.first_name, c.last_name
ORDER BY 
    total_paid DESC;



-- Using a CTE, show the top 3 rented movies by number of rentals.
-- Columns: title, rental_count

WITH cte_rental_counts AS (
    SELECT f.title, COUNT(r.rental_id) AS rental_count
    FROM film f
    JOIN inventory i ON f.film_id = i.film_id
    JOIN rental r ON i.inventory_id = r.inventory_id
    GROUP BY f.title
)
SELECT title, rental_count
FROM cte_rental_counts
ORDER BY rental_count DESC
LIMIT 3;

-- Find customers who have rented more than the average number of films.
-- Use a CTE to compute the average rentals per customer, then filter.

-- trying
WITH cte_customer_rentals AS (
    SELECT 
        customer_id,
        COUNT(rental_id) AS rental_count
    FROM rental
    GROUP BY customer_id
),
cte_avg_rentals AS (
    SELECT AVG(rental_count) AS avg_rental_count
    FROM customer_rentals
)
SELECT 
    cr.customer_id,
    cr.rental_count
FROM 
    cte_customer_rentals cr,
    cte_avg_rentals ar
WHERE 
    cr.rental_count > ar.avg_rental_count
ORDER BY 
    cr.rental_count DESC;



-- Write a function that returns the total number of rentals for a given customer ID.
-- Function: get_total_rentals(customer_id INT)

CREATE OR REPLACE FUNCTION get_total_rentals(customer_id INT)
RETURNS INTEGER AS $$
DECLARE
    total_rentals INT;
BEGIN
    SELECT COUNT(*) INTO total_rentals
    FROM rental
    WHERE rental.customer_id = get_total_rentals.customer_id;

    RETURN total_rentals;
END;
$$ LANGUAGE plpgsql;
 
------------------------------NOTES: ------------------------------
 -- $$ IS The delimeter, and @is used only in SSMS.. 
 -- here we use like get_total_rentals.customer_id
 ------------------------------------------------------------------


------------------------------------ Stored Procedure Question------------------------------------
-- Write a stored procedure that updates the rental rate of a film by film ID and new rate.
-- Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)
--------------------------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE proc_update_rental_rate(film_id INT, new_rate NUMERIC)
AS $$
BEGIN
    UPDATE film
    SET rental_rate = update_rental_rate.new_rate
    WHERE film.film_id = update_rental_rate.film_id;
END;
$$ LANGUAGE plpgsql;

CALL proc_update_rental_rate(1, 9.99);

select * FROM film order by film_id;