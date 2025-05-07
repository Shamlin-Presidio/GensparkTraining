-- https://pgexercises.com/questions/basic/
-- PostgreSQL exercises
 
 -- B A S I C    

 SELECT * FROM cd.facilities;


SELECT name, membercost
FROM cd.facilities;


SELECT * from cd.facilities WHERE membercost>0;


SELECT facid, name, membercost, monthlymaintenance 
FROM cd.facilities 
WHERE membercost > 0 and (membercost < monthlymaintenance/50.0);      

SELECT * FROM cd.facilities 
WHERE name LIKE '%Tennis%';


SELECT * FROM cd.facilities 
WHERE facid in (1,5);   

SELECT name, 
	CASE WHEN (monthlymaintenance > 100) THEN
		'expensive'
	ELSE
		'cheap'
	end as cost
	FROM cd.facilities;


SELECT memid, surname, firstname, joindate 
	FROM cd.members
	WHERE joindate >= '2012-09-01';        


SELECT DISTINCT surname 
FROM cd.members
ORDER BY surname
LIMIT 10;       


SELECT surname AS name_list FROM cd.members
UNION
SELECT name FROM cd.facilities;


SELECT MAX(joindate) AS latest
FROM cd.members;  

SELECT firstname, surname, joindate 
FROM cd.members
WHERE joindate = (SELECT MAX(joindate) FROM cd.members );



-- J O I N S    C A T E G O R Y


SELECT bks.starttime 
FROM 
cd.bookings bks INNER JOIN cd.members mems
ON mems.memid = bks.memid
WHERE mems.firstname='David' AND mems.surname='Farrell';   


SELECT bks.starttime as start, facs.name as name
FROM cd.facilities facs JOIN cd.bookings bks
ON facs.facid = bks.facid
WHERE facs.name like 'Tennis%' AND bks.starttime >= '2012-09-21' AND bks.starttime < '2012-09-22'
ORDER BY bks.starttime;          



SELECT DISTINCT rec.firstname as firstname, rec.surname as surname
FROM cd.members mem JOIN cd.members rec
ON rec.memid = mem.recommendedby
ORDER BY surname, firstname;    

-- ALTERNATIVE APPROACH< WITHOUT JOIN:

SELECT DISTINCT firstname, surname
FROM cd.members
WHERE memid IN (
  SELECT recommendedby
  FROM cd.members
  WHERE recommendedby IS NOT NULL
)
ORDER BY surname, firstname;



SELECT mems.firstname memfname, mems.surname memsname, recs.firstname recfname, recs.surname recsname
FROM cd.members mems LEFT OUTER JOIN cd.members recs
ON recs.memid = mems.recommendedby
ORDER BY memsname, memfname;   


SELECT DISTINCT CONCAT(mems.firstname, ' ', mems.surname) AS member, facs.name AS facility
FROM cd.members mems
JOIN cd.bookings bks ON mems.memid = bks.memid
JOIN cd.facilities facs ON bks.facid = facs.facid
WHERE facs.name LIKE 'Tennis%'
ORDER BY member, facility;

