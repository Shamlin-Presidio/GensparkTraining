use pubs
go

-- print details of publisher who has never published
SELECT * FROM publishers WHERE pub_id NOT IN 
(SELECT DISTINCT pub_id FROM titles);

-- print details of publisher who has never published
SELECT title, pub_name 
FROM titles RIGHT OUTER JOIN publishers
on titles.pub_id = publishers.pub_id;

-----------------------------------------------------------
-- PRACTICE QUESTIONS
-----------------------------------------------------------
--select author_id for all books, print author_id and book name
SELECT au_id, title FROM 
titleauthor JOIN titles 
ON titleauthor.title_id = titles.title_id;

-- use instances like titleauthor ta, only when we have same column in both tables (ambiguity), as shown below

SELECT CONCAT(au_fname, au_lname) Author_name, title Book_Name from authors a
JOIN titleauthor ta on a.au_id = ta.au_id
JOIN titles t on ta.title_id = t.title_id;


-- print publisher's name, book name and order date of books

SELECT t.title  Book, p.pub_name as Publisher, s.ord_date as Order_Date 
from publishers p 
JOIN  titles t ON t.pub_id = p.pub_id 
JOIN sales s ON s.title_id = t.title_id
ORDER BY 3 DESC;


-- print publisher's name and date of sale for first book for all publishers

SELECT p.pub_name, MIN(s.ord_date) AS First_Book_Sale_Date
FROM publishers p
LEFT OUTER JOIN titles t ON p.pub_id = t.pub_id
LEFT OUTER JOIN sales s ON t.title_id = s.title_id
GROUP BY p.pub_name
ORDER BY 2 DESC;