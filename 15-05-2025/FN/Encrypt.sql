CREATE EXTENSION IF NOT EXISTS pgcrypto; 

-- necesssary tables

CREATE TABLE store (
    store_id SERIAL PRIMARY KEY,
    store_name TEXT
);


CREATE TABLE address (
    address_id SERIAL PRIMARY KEY,
    address_line TEXT,
    city TEXT
);


CREATE TABLE customer (
    customer_id SERIAL PRIMARY KEY,
    store_id INT NOT NULL,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    email BYTEA NOT NULL, -- we'll encrypt, so not text
    address_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT fk_store FOREIGN KEY (store_id) REFERENCES store(store_id),
    CONSTRAINT fk_address FOREIGN KEY (address_id) REFERENCES address(address_id)
);



-- insert values

INSERT INTO store (store_name) VALUES ('Puthu kadai');
INSERT INTO address (address_line, city) VALUES ('no6, Dubai kuruku santhu', 'Dubai');




-- procedures

-- 1. Create a stored procedure to encrypt a given text
-- Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an encrypted version using PostgreSQL's pgcrypto extension.
-- Use pgp_sym_encrypt(text, key) from pgcrypto.

CREATE OR REPLACE PROCEDURE sp_encrypt_text(
    IN input_text TEXT,
    IN key TEXT,
    OUT encrypted_output BYTEA
)
LANGUAGE plpgsql
AS $$
BEGIN
    encrypted_output := pgp_sym_encrypt(input_text, key);
END;
$$;


-- 2. Create a stored procedure to compare two encrypted texts
-- Task: Write a procedure sp_compare_encrypted that takes two encrypted values and checks if they decrypt to the same plain text.

CREATE OR REPLACE PROCEDURE sp_compare_encrypted(
    IN enc1 BYTEA,
    IN enc2 BYTEA,
    IN key TEXT,
    OUT are_equal BOOLEAN
)
LANGUAGE plpgsql
AS $$
DECLARE
    dec1 TEXT;
    dec2 TEXT;
BEGIN
    dec1 := pgp_sym_decrypt(enc1, key);
    dec2 := pgp_sym_decrypt(enc2, key);
    are_equal := (dec1 = dec2);
END;
$$;

-- 3. Create a stored procedure to partially mask a given text
-- Task: Write a procedure sp_mask_text that:
 
-- Shows only the first 2 and last 2 characters of the input string
-- Masks the rest with *

CREATE OR REPLACE PROCEDURE sp_mask_text(
    IN input_text TEXT,
    OUT masked_text TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    len INT;
BEGIN
    len := LENGTH(input_text);

    IF len <= 4 THEN
        masked_text := REPEAT('*', len);
    ELSE
        masked_text := SUBSTRING(input_text FROM 1 FOR 2) || REPEAT('*', len - 4) || SUBSTRING(input_text FROM len - 1 FOR 2);
    END IF;
END;
$$;

-- 4. Create a procedure to insert into customer with encrypted email and masked name
-- Task:
 
-- Call sp_encrypt_text for email
-- Call sp_mask_text for first_name
 
--Insert masked and encrypted values into the customer table
--Use any valid address_id and store_id to satisfy FK constraints.

CREATE OR REPLACE PROCEDURE sp_insert_customer(
    IN p_first_name TEXT,
    IN p_last_name TEXT,
    IN p_email TEXT,
    IN p_store_id INT,
    IN p_address_id INT,
    IN p_key TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    encrypted_email BYTEA;
    masked_first TEXT;
BEGIN
    CALL sp_encrypt_text(p_email, p_key, encrypted_email);
    CALL sp_mask_text(p_first_name, masked_first);

    INSERT INTO customer (
        store_id, first_name, last_name,
        email, address_id
    )
    VALUES (
        p_store_id, masked_first, p_last_name,
        encrypted_email, p_address_id
    );
END;
$$;

-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
-- Task:
-- Write sp_read_customer_masked() that:
-- Loops through all rows
-- Decrypts email
-- Displays customer_id, masked first name, and decrypted email

CREATE OR REPLACE PROCEDURE sp_read_customer_masked(
    IN p_key TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    rec RECORD;
BEGIN
    FOR rec IN SELECT customer_id, first_name, email FROM customer LOOP
        RAISE NOTICE 'ID: %, Name: %, Email: %',
            rec.customer_id,
            rec.first_name,
            pgp_sym_decrypt(rec.email, p_key);
    END LOOP;
END;
$$;




CALL sp_insert_customer( 'Operation', 'Keller', 'keller.ops@example.com', 1, 1,'RR44'); -- army level encryption now.... lol :)


SELECT customer_id, first_name, pgp_sym_decrypt(email, 'RR44') AS decrypted_email
FROM customer;

CALL sp_read_customer_masked('RR44');

Select * from Customer;