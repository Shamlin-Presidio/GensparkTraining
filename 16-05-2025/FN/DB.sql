-- P H A S E    2

CREATE DATABASE EdTech;


-- Students 
CREATE TABLE students (
    student_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(15)
);

-- Courses 
CREATE TABLE courses (
    course_id SERIAL PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL,
    category VARCHAR(50),
    duration_days INT
);

-- Trainers 
CREATE TABLE trainers (
    trainer_id SERIAL PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    expertise VARCHAR(100)
);

-- Enrollments 
CREATE TABLE enrollments (
    enrollment_id SERIAL PRIMARY KEY,
    student_id INT NOT NULL REFERENCES students(student_id),
    course_id INT NOT NULL REFERENCES courses(course_id),
    enroll_date DATE NOT NULL
);

-- Certificates 
CREATE TABLE certificates (
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INT NOT NULL UNIQUE REFERENCES enrollments(enrollment_id),
    issue_date DATE NOT NULL,
    serial_no VARCHAR(50) UNIQUE NOT NULL
);

-- Course_Trainers 
CREATE TABLE course_trainers (
    course_id INT NOT NULL REFERENCES courses(course_id),
    trainer_id INT NOT NULL REFERENCES trainers(trainer_id),
    PRIMARY KEY (course_id, trainer_id)
);


-- Inset data

-- Students
INSERT INTO students (name, email, phone) VALUES 
('Python kumar', 'kumar@python.com', '9898989898'),
('Data Raj', 'raj@data.com', '9898989898');

-- Courses
INSERT INTO courses (course_name, category, duration_days) VALUES 
('Intro to Python', 'Programming', 30),
('Data Science Basics', 'Data Science', 45);

-- Trainers
INSERT INTO trainers (trainer_name, expertise) VALUES 
('Dr. Nagendran', 'Python'),
('Mr. Data Arumugam', 'Data Science');

-- Course_Trainers (if many-to-many)
INSERT INTO course_trainers (course_id, trainer_id) VALUES 
(1, 1),
(2, 2);

-- Enrollments
INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES 
(1, 1, '2025-01-10'),
(2, 2, '2025-02-15');

-- Certificates
INSERT INTO certificates (enrollment_id, issue_date, serial_no) VALUES 
(1, '2025-03-10', 'CERT-PY-001'),
(2, '2025-04-20', 'CERT-DS-002');

-- Indexes, maked query faster by using b-trees to query

CREATE INDEX idx_students_student_id ON students(student_id);
CREATE INDEX idx_students_email ON students(email);
CREATE INDEX idx_courses_course_id ON courses(course_id);



----- P H A S E    3

--1. List students and the courses they enrolled in

SELECT 
    s.student_id, s.name AS student_name,
    c.course_id, c.course_name
FROM students s
JOIN enrollments e ON s.student_id = e.student_id
JOIN courses c ON e.course_id = c.course_id;


-- 2. Show students who received certificates with trainer names
SELECT 
    s.name AS student_name,
    c.course_name,
    cert.serial_no,
    cert.issue_date,
    t.trainer_name
FROM certificates cert
JOIN 
    enrollments e ON cert.enrollment_id = e.enrollment_id
JOIN 
    students s ON e.student_id = s.student_id
JOIN 
    courses c ON e.course_id = c.course_id
JOIN 
    course_trainers ct ON c.course_id = ct.course_id
JOIN trainers t ON ct.trainer_id = t.trainer_id;


-- 3. Count number of students per course
SELECT 
    c.course_name,
    COUNT(e.student_id) AS student_count
FROM courses c
LEFT JOIN 
    enrollments e ON c.course_id = e.course_id
GROUP BY c.course_id, c.course_name
ORDER BY student_count DESC;


---------- P H A S E    4

-- F U N C T I O N  

CREATE OR REPLACE FUNCTION get_certified_students(p_course_id INT)
RETURNS TABLE (
    student_id INT,
    student_name VARCHAR,
    email VARCHAR,
    certificate_serial_no VARCHAR,
    issue_date DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT s.student_id, s.name, s.email, cert.serial_no, cert.issue_date
    FROM students s
    JOIN enrollments e ON s.student_id = e.student_id
    JOIN certificates cert ON e.enrollment_id = cert.enrollment_id
    WHERE e.course_id = p_course_id;
END;
$$ LANGUAGE plpgsql;


-- S T O R E D   P R O C E D U R E

CREATE OR REPLACE PROCEDURE sp_enroll_student(
    p_student_id INT,
    p_course_id INT,
    completed BOOLEAN
)
LANGUAGE plpgsql AS $$
DECLARE
    new_enrollment_id INT;
BEGIN
    
    INSERT INTO enrollments(student_id, course_id, enroll_date)
    VALUES (p_student_id, p_course_id, CURRENT_DATE)
    RETURNING enrollment_id INTO new_enrollment_id;

    IF completed THEN
        INSERT INTO certificates(enrollment_id, issue_date, serial_no)
        VALUES (
            new_enrollment_id,
            CURRENT_DATE,
            'CERT-' || p_course_id || '-' || new_enrollment_id
        );
    END IF;
END;
$$;




-- P H A S E   5  --- C U R S O R 

DO $$
DECLARE
    rec RECORD;
BEGIN
    FOR rec IN
        SELECT s.name, s.email
        FROM students s
        JOIN enrollments e ON s.student_id = e.student_id
        LEFT JOIN certificates cert ON e.enrollment_id = cert.enrollment_id
        WHERE e.course_id = 1  -- can be any id we want
          AND cert.certificate_id IS NULL
    LOOP
        RAISE NOTICE 'Student: %, Email: %', rec.name, rec.email;
    END LOOP;
END;
$$;



-- P H A S E   6   --- Security & Roles   

/*
1. Create a `readonly_user` role:

   * Can run `SELECT` on `students`, `courses`, and `certificates`
   * Cannot `INSERT`, `UPDATE`, or `DELETE`
*/
CREATE ROLE readonly_user LOGIN PASSWORD 'readonlypass';
-- CAN:
GRANT CONNECT ON DATABASE EdTech TO readonly_user;
GRANT USAGE ON SCHEMA public TO readonly_user;
GRANT SELECT ON TABLE students, courses, certificates TO readonly_user;
--CAN NOT:
REVOKE INSERT, UPDATE, DELETE ON TABLE students, courses, certificates FROM readonly_user;



/*
2. Create a `data_entry_user` role:

   * Can `INSERT` into `students`, `enrollments`
   * Cannot modify certificates directly
*/
-- 2. Create data_entry_user role
CREATE ROLE data_entry_user LOGIN PASSWORD 'dataentrypass';

-- CAN:
GRANT CONNECT ON DATABASE EdTech TO data_entry_user;
GRANT USAGE ON SCHEMA public TO data_entry_user;

GRANT INSERT ON TABLE students, enrollments TO data_entry_user;
-- CAN NOT:
REVOKE UPDATE, DELETE ON TABLE certificates FROM data_entry_user;
REVOKE INSERT ON TABLE certificates FROM data_entry_user;



-- PH A S E   7

DO $$
DECLARE
    new_enrollment_id INT;
    p_student_id INT := 1;
    p_course_id INT := 1;
BEGIN
    BEGIN
        INSERT INTO enrollments(student_id, course_id, enroll_date)
        VALUES (p_student_id, p_course_id, CURRENT_DATE)
        RETURNING enrollment_id INTO new_enrollment_id;

        INSERT INTO certificates(enrollment_id, issue_date, serial_no)
        VALUES (
            new_enrollment_id,
            CURRENT_DATE,
            'CERT-' || p_course_id || '-' || new_enrollment_id
        );

        COMMIT;
    EXCEPTION WHEN OTHERS THEN
        ROLLBACK;
        RAISE NOTICE 'Transaction failed. Rolled back.';
		-- here it will rolled back since we insert an existin id of 1
    END;
END;
$$;

-- rewriting this as a stored procedure

CREATE OR REPLACE PROCEDURE sp_enroll_and_certify(
    p_student_id INT,
    p_course_id INT
)
LANGUAGE plpgsql AS $$
DECLARE
    new_enrollment_id INT;
BEGIN
    
    INSERT INTO enrollments(student_id, course_id, enroll_date)
    VALUES (p_student_id, p_course_id, CURRENT_DATE)
    RETURNING enrollment_id INTO new_enrollment_id;

    
    INSERT INTO certificates(enrollment_id, issue_date, serial_no)
    VALUES (
        new_enrollment_id,
        CURRENT_DATE,
        'CERT-' || p_course_id || '-' || new_enrollment_id
    );
EXCEPTION
    WHEN OTHERS THEN
        RAISE NOTICE 'Transaction failed in procedure. Rolled back.';
        RAISE;  -- Re-raise the error so caller can handle rollback
END;
$$;


-- the required transcation block
BEGIN;
    CALL sp_enroll_and_certify(1, 1);
COMMIT;
